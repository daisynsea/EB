﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IAccountBrokerService
{
    /// <summary>
    /// Process account action
    /// </summary>
    /// <param name="model">Incoming payload</param>
    /// <returns>Processed action</returns>
    Task<AccountResponse> ProcessAccountAction(SalesforceAccountModel model);
}

public class AccountBrokerService : IAccountBrokerService
{
    private readonly IActionsRepository _actionsRepository;
    private readonly IOracleService _oracleService;
    private readonly IOssService _ossService;

    public AccountBrokerService(IActionsRepository actionsRepository, IOracleService oracleService, IOssService ossService)
    {
        _actionsRepository = actionsRepository;
        _oracleService = oracleService;
        _ossService = ossService;
    }

    public async Task<AccountResponse> ProcessAccountAction(SalesforceAccountModel model)
    {
        /*
         * DETERMINE WHERE TO SYNC
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
            Object = ActionObjectType.Account,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = body,
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
        var response = new AccountResponse
        {
            SalesforceObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        /*
         * 1. Find Oracle Organization by SF Account Id
         * 2. If Organization does not exist, create it in Oracle
         * 3. If Organization exists, update the organization in Oracle
         * 4. Find Customer Account by SF Account Id
         * 5. If Customer Account does not exist, create it in Oracle
         * 6. If Customer Account exists, update the customer account in Oracle
         * 7. Find Customer Account Profile by SF Account Id
         * 8. If Customer Account Profile does not exist, create it in Oracle
         * 9. If Customer Account Profile exists, update the customer account profile in Oracle
         * 10. If model.Addresses is not null and has more than 0 entities, this request is an account Create request, process each address via the AddressService
         * 11. If model.Contacts is not null and has more than 0 entities, this request is an account Create request, process each contact via the ContactService
         * 12. Gather up all the Ids from any created or updated entities, update the statuses, and send back the response to SF
         */

        /*
         * INITIALIZE THE IDS
         */
        string? ossAccountId = null;
        string? oracleOrganizationId = null;
        string? oracleCustomerAccountId = null;
        string? oracleCustomerAccountProfileId = null;

        /*
        * FETCH ORGANIZATION
        */


        #region Process Account Create
        /*
         * SEND TO OSS IF REQUIRED
         */
        #region Send to OSS
        if (syncToOss)
        {
            // First, fetch to see if the account exists in OSS. If it does, we do an update. Otherwise we add.
            var existingAccount = await _ossService.GetAccountBySalesforceId(model.ObjectId);
            // Set initial OSSStatus response value to Successful. It will get overwritten if there is an error.
            response.OSSStatus = StatusType.Successful;

            // If the account exists, it's an update
            if (existingAccount != null)
            {
                // If the account exists, we can set the Id early
                response.OssAccountId = existingAccount?.Id?.ToString();
                ossAccountId = existingAccount?.Id.ToString();
                // Update the account
                var updatedAccountTuple = await _ossService.UpdateAccount(model, salesforceTransaction);
                // Item1 is the account object -- if it's null, we have a problem
                if (updatedAccountTuple.Item1 == null)
                {
                    response.OSSErrorMessage = updatedAccountTuple.Item2;
                    response.OSSStatus = StatusType.Error;
                }
            } else
            {
                // Keep in mind, when adding, we do not fill in the Oracle Id here -- we update it after all the Oracle creation is finished
                var addedAccountTuple = await _ossService.AddAccount(model, salesforceTransaction);
                if (string.IsNullOrEmpty(addedAccountTuple.Item2)) // No error!
                {
                    response.OssAccountId = addedAccountTuple.Item1.Id.ToString();
                    ossAccountId = addedAccountTuple.Item1?.Id?.ToString();
                }
                else // Is error, do not EXIT..
                {
                    response.OSSStatus = StatusType.Error;
                    response.OSSErrorMessage = addedAccountTuple.Item2;
                }
            }
        }
        #endregion

        /*
         * SEND TO ORACLE IF REQUIRED
         */
        #region Send to Oracle
        if (syncToOracle)
        {
            #region Organization
            // We have to create 5 entities in Oracle: Organization, Location(s), PartySite, CustomerAccount, CustomerAccountProfile
            var organizationResult = await _oracleService.GetOrganizationBySalesforceAccountId(model.Name, model.ObjectId);
            if (!organizationResult.Item1)
            {
                // TODO: fatal error occurred when sending request to oracle... return badRequest here?
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = organizationResult.Item3;
                return response;
            }
            var organization = new OracleOrganization();
            // If Organization does not exist, create it
            if (organizationResult.Item2 == null)
            {
                var addedOrganization = await _oracleService.CreateOrganization(model, salesforceTransaction);
                if (addedOrganization.Item1 == null)
                {
                    // fatal error occurred
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = addedOrganization.Item2;
                    return response;
                }
                organization = addedOrganization.Item1;
                oracleOrganizationId = addedOrganization.Item1.PartyNumber;
            }
            else // Otherwise, update it
            {
                // TODO: Party Number here?
                var updatedOrganization = await _oracleService.UpdateOrganization(organizationResult.Item2.PartyNumber, model, salesforceTransaction);
                if (updatedOrganization.Item1 == null)
                {
                    // fatal error occurred
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = updatedOrganization.Item2;
                    return response;
                }
                organization = updatedOrganization.Item1;
                oracleOrganizationId = organizationResult.Item2.PartyNumber;
            }
            #endregion

            #region Location & OrgPartySite
            // verify we have addresses
            if (model.Addresses != null && model.Addresses.Count > 0)
            {
                List<Task<Tuple<OracleLocationModel, string>>> createLocationTasks = new();
                // TODO: create a Location for each address
                foreach (var address in model.Addresses)
                {
                    // check the Organization PartySites (Locations) with the address to see if it has been created already
                    var orgPartySite = organizationResult.Item2?.PartySites?.FirstOrDefault(s => s.OrigSystemReference == address.ObjectId);
                    if (orgPartySite == null)
                    {
                        // create Location & OrgPartySite as a list of tasks to run async (outside of the loop)
                        createLocationTasks.Add(_oracleService.CreateLocation(address));
                    } else
                    {
                        // TODO: update Location? do nothing?
                    }
                }

                // check to see if we are creating any Locations
                if (createLocationTasks.Count > 0)
                {
                    var partySitesToCreate = new List<OraclePartySite>();
                    // execute requests to create Locations in async fashion
                    await Task.WhenAll(createLocationTasks);
                    // get the response data
                    var createLocationResults = createLocationTasks.Select(t => t.Result).ToList();
                    // iterate through the results for the create Location requests
                    for (int i = 0; i < createLocationResults.Count; i++)
                    {
                        var result = createLocationResults[i];
                        // acquire the matching Location so we can set the SiteUseType below
                        var address = model.Addresses.FirstOrDefault(a => a.ObjectId == result.Item1.OrigSystemReference);

                        if (result.Item1 == null)
                        {
                            // create location failed for some reason
                            Console.WriteLine("[DEBUG] Error: {result.Item2}");
                        } else
                        {
                            // Location was created successfully... so add to the list so we can create a Party Site record for it
                            partySitesToCreate.Add(new OraclePartySite
                            {
                                LocationId = result.Item1.LocationId,
                                OrigSystemReference = result.Item1.OrigSystemReference,
                                SiteUseType = address.Type
                            });
                        }
                    }

                    // create Organization PartySite (batched into a single request for all Locations)
                    var partySites = await _oracleService.CreateOrganizationPartySites(organization.PartyId, partySitesToCreate);
                }
            }
            #endregion


            #region Customer Account
            var existingCustomerAccount = await _oracleService.GetCustomerAccountBySalesforceAccountId(model.ObjectId);
            if (!existingCustomerAccount.Item1)
            {
                // TODO: fatal error occurred when sending request to oracle... return badRequest here?
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = existingCustomerAccount.Item3;
                return response;
            }
            // If Customer Account does not exist, create it
            if (existingCustomerAccount.Item2 == null)
            {
                var addedCustomerAccount = await _oracleService.CreateCustomerAccount(oracleOrganizationId, model, salesforceTransaction);
                oracleCustomerAccountId = addedCustomerAccount.Item1;
            } else // Otherwise, update it
            {
                // TODO: Need a corresponding Id here?
                var updatedCustomerAccount = await _oracleService.UpdateCustomerAccount(model, salesforceTransaction);
                oracleCustomerAccountId = existingCustomerAccount.Item2.PartyNumber;
            }
            #endregion



            var existingCustomerAccountProfile = await _oracleService.GetCustomerAccountProfileBySalesforceAccountId(model.ObjectId);
            // If Customer Account does not exist, create it
            if (existingCustomerAccountProfile == null)
            {
                var addedCustomerAccountProfile = await _oracleService.CreateCustomerAccountProfile(oracleCustomerAccountId, model, salesforceTransaction);
                oracleCustomerAccountProfileId = addedCustomerAccountProfile.Item1;
            } else // Otherwise, update it
            {
                // TODO: Need a corresponding Id here?
                var updatedCustomerAccountProfile = await _oracleService.UpdateCustomerAccountProfile(model, salesforceTransaction);
                oracleCustomerAccountProfileId = existingCustomerAccountProfile.Item1.PartyNumber;
            }

            response.OracleStatus = StatusType.Successful;
            response.OracleCustomerAccountId = oracleCustomerAccountId;
            response.OracleOrganizationId = oracleOrganizationId;
            response.OracleCustomerProfileId = oracleCustomerAccountProfileId;

            // TODO: Need to create the contacts and addresses, but how will we return the Ids from these child entities back to Salesforce??
        }
        #endregion
        #endregion

        response.CompletedOn = DateTime.UtcNow;

        // Attach the response to the action log item
        salesforceTransaction.Response = response;
        await _actionsRepository.UpdateActionRecord(salesforceTransaction);

        return response;
    }
}