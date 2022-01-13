namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IAddressBrokerService
{
    Task<AddressResponse> ProcessAddressAction(SalesforceAddressModel model);
}

public class AddressBrokerService : IAddressBrokerService
{
    private readonly IActionsRepository _actionsRepository;
    private readonly IOssService _ossService;
    private readonly IOracleService _oracleService;

    public AddressBrokerService(IActionsRepository actionsRepository, IOssService ossService, IOracleService oracleService)
    {
        _actionsRepository = actionsRepository;
        _ossService = ossService;
        _oracleService = oracleService;
    }

    public async Task<AddressResponse> ProcessAddressAction(SalesforceAddressModel model)
    {
        /*
        * WHERE TO SYNC
        */
        var syncToOss = model.SyncToOss.GetValueOrDefault();
        var syncToOracle = model.SyncToOracle.GetValueOrDefault();

        /*
         * LOG THE ENTERPRISE APPLICATION BROKER ACTION
         */
        #region Log the Enterprise Action
        // Serialize the body coming in
        string body = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
        // Create the action record object
        var salesforceTransaction = new SalesforceActionTransaction
        {
            Id = Guid.NewGuid(),
            Object = ActionObjectType.Address,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = JsonSerializer.Serialize(model),
            LastUpdatedOn = DateTime.UtcNow,
            TransactionLog = new List<SalesforceActionRecord>()
        };
        // Insert the event into the database, receive the response object and update the existing variable
        salesforceTransaction = await _actionsRepository.InsertActionRecord(salesforceTransaction);
        #endregion

        /*
         * MARSHAL UP RESPONSE
         */
        #region Build initial response object
        var response = new AddressResponse
        {
            SalesforceObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        #region Send to OSS
        if (syncToOss)
        {
            var addedAddressTuple = await _ossService.UpdateAccountAddress(new UpdateAddressModel { ParentAccountId = model.ParentAccountId, Address1 = model.Address1, Address2 = model.Address2, Country = model.Country }, salesforceTransaction);
            if (string.IsNullOrEmpty(addedAddressTuple.Item2)) // No error!
            {
                response.OSSStatus = StatusType.Successful;
            }
            else // Is error, do not EXIT..
            {
                response.OSSStatus = StatusType.Error;
                response.OSSErrorMessage = addedAddressTuple.Item2;
            }
        }
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            // Get Organization by Salesforce Account Id
            var organizationResult = await _oracleService.GetOrganizationBySalesforceAccountId(model.ParentAccountName, model.ParentAccountId, salesforceTransaction);
            if (!organizationResult.Item1)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Address to Oracle: Organization object with SF reference Id {model.ParentAccountId} was not found.";
                return response;
            }
            var organization = organizationResult.Item2;

            // Get customer account by Salesforce Account Id
            var customerAccountResult = await _oracleService.GetCustomerAccountBySalesforceAccountId(model.ParentAccountId);
            if (!customerAccountResult.Item1)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Address to Oracle: Customer Account object with SF reference Id {model.ParentAccountId} was not found.";
                return response;
            }
            var customerAccount = customerAccountResult.Item2;


            // re-map Salesforce values to Oracle models
            var siteUseTypes = Helpers.RemapAddressTypeToOracleSiteUse(model);
            // for the Organization
            var partySitesToCreate = new List<OraclePartySite>();
            // for the Customer Account
            var accountSites = new List<OracleCustomerAccountSite>();

            // search for existing location
            var locationsResult = await _oracleService.GetLocationsBySalesforceAddressId(new List<string> { model.ObjectId });
            if (!locationsResult.Item1)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Address to Oracle: {locationsResult.Item3}";
                return response;
            }
            if (locationsResult == null || locationsResult.Item2.Count() == 0)
            {
                // create new location
                var createLocationResult = await _oracleService.CreateLocation(model, salesforceTransaction);
                if (createLocationResult.Item1 == null)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: Error creating Location: {createLocationResult.Item2}.";
                    return response;
                }
                var createdLocation = createLocationResult.Item1;
                response.OracleAddressId = createdLocation.LocationId?.ToString();

                // Location was created successfully... so add to the list so we can create a Party Site record for it
                partySitesToCreate.Add(new OraclePartySite
                {
                    LocationId = createLocationResult.Item1.LocationId,
                    OrigSystemReference = createLocationResult.Item1.OrigSystemReference,
                    SiteUses = siteUseTypes
                });
            }
            else
            {
                // update the location
                var updateLocationResult = await _oracleService.UpdateLocation(model, salesforceTransaction);
                if (updateLocationResult.Item1 == null)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: Error updating Location: {updateLocationResult.Item2}.";
                    return response;
                }
                var updatedLocation = updateLocationResult.Item1;
                response.OracleAddressId = updatedLocation.LocationId?.ToString();

                // validate the that PartySite exists for the Organization (if not, create)
                var orgPartySite = organization.PartySites.FirstOrDefault(ps => ps.OrigSystemReference == model.ObjectId);
                if (orgPartySite == null)
                {
                    partySitesToCreate.Add(new OraclePartySite
                    {
                        LocationId = updatedLocation.LocationId,
                        OrigSystemReference = updatedLocation.OrigSystemReference,
                        SiteUses = siteUseTypes
                    });
                } 
                else
                {
                    // append to the list of accountSites so we can verify the Customer Account has the necessary objects for the Location(s)
                    accountSites.Add(new OracleCustomerAccountSite
                    {
                        OrigSystemReference = orgPartySite.OrigSystemReference,
                        PartySiteId = orgPartySite.PartySiteId,
                        SiteUses = orgPartySite.SiteUses?.Select(su => new OracleCustomerAccountSiteUse
                        {
                            SiteUseCode = su.SiteUseType
                        }).ToList()
                    });
                }
            }

            // check to see if we need to create any PartySites for the Organization & Locations
            if (partySitesToCreate.Count > 0)
            {
                // create Organization PartySite (batched into a single request for all Locations)
                var createPartySitesResult = await _oracleService.CreateOrganizationPartySites(organization.PartyId, partySitesToCreate, salesforceTransaction);
                if (createPartySitesResult.Item1 == null)
                {
                    // create PartySites failed for some reason
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: Failed to create Organization Party Sites: {createPartySitesResult.Item2}.";
                    return response;
                }
                else
                {
                    // map created PartySites to list of OracleCustomerAccountSites to create below
                    var sites = createPartySitesResult.Item1.Select(cpr => new OracleCustomerAccountSite
                    {
                        PartySiteId = cpr.PartySiteId,
                        OrigSystemReference = cpr.OrigSystemReference,
                        SiteUses = cpr.SiteUses?.Select(su => new OracleCustomerAccountSiteUse
                        {
                            SiteUseCode = su.SiteUseType
                        }).ToList()
                    });
                    accountSites.AddRange(sites);
                }
            }

            // validate that the CustomerAccountSite exists for the Customer Account (if not, create)
            var accountSite = customerAccount.Sites?.FirstOrDefault(s => s.OrigSystemReference == model.ObjectId);
            if (accountSite == null)
            {
                // merge/update the existing Account to add the Customer Account Site
                var customerAccountUpdateResult = await _oracleService.UpdateCustomerAccountChildren(customerAccount, salesforceTransaction, accountSites, null);
                if (customerAccountUpdateResult.Item1 == null)
                {
                    // failed to update Customer Account
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: Error adding CustomerAccountSite: {customerAccountUpdateResult.Item2}.";
                    return response;
                }
            }

            // indicate successful request
            response.OracleStatus = StatusType.Successful;
        }
        #endregion

        return response;
    }
}

