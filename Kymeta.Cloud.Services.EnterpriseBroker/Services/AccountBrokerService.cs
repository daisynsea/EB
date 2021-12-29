using System.Text.Json;
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
            // There is a plethora of possible exceptions in this flow, so we're going to catch any of them and ensure the logs are written
            try
            {
                #region Organization
                // We have to create 5 entities in Oracle: Organization, Location(s), PartySite, CustomerAccount, CustomerAccountProfile
                var organizationResult = await _oracleService.GetOrganizationBySalesforceAccountId(model.Name, model.ObjectId, salesforceTransaction);
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
                    var updatedOrganization = await _oracleService.UpdateOrganization(organizationResult.Item2, model, salesforceTransaction);
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
                var partySites = new List<OraclePartySite>();

                // verify we have addresses
                if (model.Addresses != null && model.Addresses.Count > 0)
                {
                    List<Task<Tuple<OracleLocationModel, string>>> createLocationTasks = new();
                    // create a Location for each address
                    foreach (var address in model.Addresses)
                    {
                        // check the Organization PartySites (Locations) with the address to see if it has been created already
                        var orgPartySite = organization.PartySites?.FirstOrDefault(s => s.OrigSystemReference == address.ObjectId);
                        if (orgPartySite == null)
                        {
                            // create Location & OrgPartySite as a list of tasks to run async (outside of the loop)
                            createLocationTasks.Add(_oracleService.CreateLocation(address, salesforceTransaction));
                        }
                        else
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
                                // TODO: what action should we take here? Alert of some sort?
                                // create location failed for some reason
                                Console.WriteLine($"[DEBUG] Error: {result.Item2}");
                            }
                            else
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
                        var createPartySitesResult = await _oracleService.CreateOrganizationPartySites(organization.PartyId, partySitesToCreate, salesforceTransaction);
                        if (createPartySitesResult.Item1 == null)
                        {
                            // create PartySites failed for some reason
                            Console.WriteLine($"[DEBUG] Error: {createPartySitesResult.Item2}");
                        }
                        else
                        {
                            partySites.AddRange(createPartySitesResult.Item1);
                        }
                    }
                }
                #endregion

                #region Person
                // create empty list of persons to track new additions
                var persons = new List<OraclePersonObject>();

                // verify we have contacts
                if (model.Contacts != null && model.Contacts.Count > 0)
                {
                    // create a Person for each contact
                    foreach (var contact in model.Contacts)
                    {
                        // check the Organization Contacts with the contact to see if it has been created already
                        var orgContact = organization.Contacts?.FirstOrDefault(s => s.OrigSystemReference == contact.ObjectId);
                        if (orgContact == null)
                        {
                            // create Person requests as a list of tasks to run async (outside of the loop)
                            // unable to use Task.WhenAll because Oracle is complaining... response from Oracle:
                            // JBO-26092: Failed to lock the record in table HZ_ORGANIZATION_PROFILES with key oracle.jbo.Key[300000100251313 ], another user holds the lock.
                            var addedPersonResult = await _oracleService.CreatePerson(contact, organization.PartyId.ToString(), salesforceTransaction);
                            if (addedPersonResult.Item1 == null)
                            {
                                // TODO: what action should we take here? Alert of some sort?
                                Console.WriteLine($"[DEBUG] Error: {addedPersonResult.Item2}");
                            }
                            else
                            {
                                // Person was created successfully... add it to the list so we can check it against the Customer Account Contacts
                                persons.Add(addedPersonResult.Item1);
                            }
                        }
                        else
                        {
                            // TODO: update Person? do nothing?
                            // TODO: add to `persons` so we can check Customer Account?
                        }
                    }
                }
                #endregion

                #region Customer Account
                // search for existing Customer Account records based on Name and Salesforce Id
                var existingCustomerAccount = await _oracleService.GetCustomerAccountBySalesforceAccountId(model.ObjectId, salesforceTransaction);
                if (!existingCustomerAccount.Item1)
                {
                    // TODO: fatal error occurred when sending request to oracle... return badRequest here?
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = existingCustomerAccount.Item3;
                    return response;
                }

                // init our simplified model
                var customerAccount = new OracleCustomerAccount();
                // If Customer Account does not exist, create it
                if (existingCustomerAccount.Item2 == null)
                {
                    var addedCustomerAccount = await _oracleService.CreateCustomerAccount(organization.PartyId, model, partySites, persons, salesforceTransaction);
                    customerAccount = addedCustomerAccount.Item1;
                    oracleCustomerAccountId = addedCustomerAccount.Item1.CustomerAccountId;
                }
                else // Otherwise, update it
                {
                    // TODO: include Persons... check for existing Customer Account Contacts before updating Customer Account
                    // TODO: Need a corresponding Id here?
                    var updatedCustomerAccount = await _oracleService.UpdateCustomerAccount(model, salesforceTransaction);
                    oracleCustomerAccountId = existingCustomerAccount.Item2.CustomerAccountId;
                }
                #endregion

                #region Customer Profile
                // if no Customer Profile exists, this request will return a 500 result (Internal Server Error)... which is super lame.
                // we are accounting for it in the WebException that is thrown within OracleService to determine if the record does not exist
                // so we are handling it (somewhat) gracefully
                var existingCustomerAccountProfile = await _oracleService.GetCustomerProfileByAccountNumber(customerAccount.AccountNumber?.ToString(), salesforceTransaction);
                if (!existingCustomerAccountProfile.Item1)
                {
                    // TODO: fatal error occurred when sending request to oracle... return badRequest here?
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = existingCustomerAccountProfile.Item3;
                    return response;
                }
                // If Customer Account does not exist, create it
                if (existingCustomerAccountProfile.Item2 == null)
                {
                    var addedCustomerAccountProfile = await _oracleService.CreateCustomerAccountProfile(customerAccount.PartyId, (uint)customerAccount.AccountNumber, salesforceTransaction);
                    oracleCustomerAccountProfileId = addedCustomerAccountProfile.Item1?.PartyId?.ToString();
                }
                else
                {
                    // TODO: do nothing? Customer Profile already exists
                }
                #endregion

                response.OracleStatus = StatusType.Successful;
                response.OracleCustomerAccountId = oracleCustomerAccountId;
                response.OracleOrganizationId = oracleOrganizationId;
                response.OracleCustomerProfileId = oracleCustomerAccountProfileId;

                // Update OSS with Oracle Id if need be
                if (syncToOss && !string.IsNullOrEmpty(oracleCustomerAccountId)) // TODO: Is the customer account Id actually what we want?
                {
                    await _ossService.UpdateAccountOracleId(model, oracleCustomerAccountId, salesforceTransaction);
                }
            } catch (Exception ex)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing to Oracle due to an exception: {ex.Message}";

                if (salesforceTransaction.TransactionLog == null) salesforceTransaction.TransactionLog = new List<SalesforceActionRecord>();
                // Add the salesforce transaction record
                var actionRecord = new SalesforceActionRecord
                {
                    ObjectType = ActionObjectType.Account,
                    Action = salesforceTransaction.TransactionLog.OrderByDescending(s => s.Timestamp).FirstOrDefault()?.Action ?? SalesforceTransactionAction.Default,
                    Status = StatusType.Error,
                    Timestamp = DateTime.UtcNow,
                    ErrorMessage = $"Error syncing to Oracle due to an exception: {ex.Message}",
                    EntityId = model.ObjectId
                };
                salesforceTransaction.TransactionLog?.Add(actionRecord);
                await _actionsRepository.AddTransactionRecord(salesforceTransaction.Id, salesforceTransaction.Object.ToString() ?? "Unknown", actionRecord);
            }
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