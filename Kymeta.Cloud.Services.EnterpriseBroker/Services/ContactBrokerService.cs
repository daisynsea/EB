namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;
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
            // Get customer account by Salesforce Account Id
            var customerAccount = await _oracleService.GetCustomerAccountBySalesforceAccountId(model.ParentAccountId);
            if (customerAccount == null)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Contact to Oracle: Customer Account object with SF reference Id {model.ParentAccountId} was not found.";
            }

            // TODO: Figure out how to get an existing person from the customer account

            // If exists, update
            // Otherwise, create

            // TODO: Delete this mock response and hook this up
            response.OracleStatus = StatusType.Successful;
        }
        #endregion

        return response;
    }
}