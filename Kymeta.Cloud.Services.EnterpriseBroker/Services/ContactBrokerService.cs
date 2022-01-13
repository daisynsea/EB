namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using System.Text.Json;
using System.Text.Json.Serialization;
public interface IContactBrokerService
{
    Task<ContactResponse> ProcessContactAction(SalesforceContactModel model);
}

public class ContactBrokerService : IContactBrokerService
{
    private readonly IActionsRepository _actionsRepository;
    private readonly IOracleService _oracleService;

    public ContactBrokerService(IActionsRepository actionsRepository, IOracleService oracleService)
    {
        _actionsRepository = actionsRepository;
        _oracleService = oracleService;
    }

    public async Task<ContactResponse> ProcessContactAction(SalesforceContactModel model)
    {
        /*
        * WHERE TO SYNC
        */
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
            Object = ActionObjectType.Contact,
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
        var response = new ContactResponse
        {
            SalesforceObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = StatusType.Skipped // We don't sync Contacts to OSS
        };
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            if (string.IsNullOrEmpty(model.Role))
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Contact to Oracle: Contact with SF reference Id {model.ObjectId} does not have a Contact Role assigned.";
                return response;
            }
            // Get Organization by Salesforce Account Id
            var organizationResult = await _oracleService.GetOrganizationBySalesforceAccountId(model.ParentAccountName, model.ParentAccountId, salesforceTransaction);
            if (!organizationResult.Item1)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Contact to Oracle: Organization object with SF reference Id {model.ParentAccountId} was not found.";
                return response;
            }
            var organization = organizationResult.Item2;

            // Get customer account by Salesforce Account Id
            var customerAccountResult = await _oracleService.GetCustomerAccountBySalesforceAccountId(model.ParentAccountId, salesforceTransaction);
            if (!customerAccountResult.Item1)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Contact to Oracle: Customer Account object with SF reference Id {model.ParentAccountId} was not found.";
                return response;
            }
            var customerAccount = customerAccountResult.Item2;

            var accountContacts = new List<OracleCustomerAccountContact>();
            // fetch Person by Salesforce Id
            var contactIds = new List<string> { model.ObjectId };
            var personsResult = await _oracleService.GetPersonsBySalesforceContactId(contactIds.ToList(), salesforceTransaction);
            if (!personsResult.Item1)
            {
                // fatal error occurred when sending request to oracle
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = personsResult.Item3;
                return response;
            }

            var responsibilityType = OracleSoapTemplates.GetResponsibilityType(model.Role);
            if (personsResult.Item2 == null || personsResult.Item2?.Count() == 0)
            {
                // Person does not exist, so create them
                var createPersonResult = await _oracleService.CreatePerson(model, organization.PartyId, salesforceTransaction);
                if (createPersonResult.Item1 == null)
                {
                    // error while trying to create the Person record
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = createPersonResult.Item2;
                    return response;
                }
                var createdPerson = createPersonResult.Item1;
                response.OraclePersonId = createdPerson.PartyId.ToString();

                // append to list for Account Contacts
                accountContacts.Add(new OracleCustomerAccountContact
                {
                    ContactPersonId = createdPerson.PartyId,
                    OrigSystemReference = createdPerson.OrigSystemReference,
                    ResponsibilityType = responsibilityType,
                    RelationshipId = createdPerson.RelationshipId,
                    IsPrimary = createdPerson.IsPrimary ?? false
                });
            }
            else
            {
                // Person exists, perform update
                var existingPerson = personsResult.Item2.FirstOrDefault();
                var updatePersonResult = await _oracleService.UpdatePerson(model, existingPerson, salesforceTransaction);
                if (updatePersonResult.Item1 == null)
                {
                    // error while trying to update the Person record
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = updatePersonResult.Item2;
                    return response;
                }
                var updatedPerson = updatePersonResult.Item1;
                response.OraclePersonId = updatedPerson.PartyId.ToString();

                // append to list for Account Contacts
                accountContacts.Add(new OracleCustomerAccountContact
                {
                    ContactPersonId = updatedPerson.PartyId,
                    OrigSystemReference = updatedPerson.OrigSystemReference,
                    ResponsibilityType = responsibilityType,
                    RelationshipId = updatedPerson.RelationshipId,
                    IsPrimary = updatedPerson.IsPrimary ?? false
                });
            }

            // verify that the Customer Account has the necessary Contact objects
            var customerAccountContact = customerAccount.Contacts?.FirstOrDefault(c => c.OrigSystemReference == model.ObjectId);
            if (customerAccountContact == null)
            {
                // update the customer account to add the contact
                var customerAccountUpdateResult = await _oracleService.UpdateCustomerAccountChildren(customerAccount, salesforceTransaction, null, accountContacts);
                if (customerAccountUpdateResult.Item1 == null)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = customerAccountUpdateResult.Item2;
                    return response;
                }
            }

            response.OracleStatus = StatusType.Successful;
        }
        #endregion

        return response;
    }
}