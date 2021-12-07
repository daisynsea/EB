namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
public interface IContactBrokerService
{
    Task<CreateContactResponse> CreateContact(CreateContactModel model);
    Task<UpdateContactResponse> UpdateContact(UpdateContactModel model);
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

    public async Task<CreateContactResponse> CreateContact(CreateContactModel model)
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
            Action = ActionType.Create,
            Object = ActionObjectType.Contact,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = JsonSerializer.Serialize(model),
            LastUpdatedOn = DateTime.UtcNow,
            OriginalTransactionId = null, // TODO: Populate this later when hooking up Retry
            TransactionLog = new List<SalesforceActionRecord>()
        };
        // Insert the event into the database, receive the response object and update the existing variable
        salesforceTransaction = await _actionsRepository.InsertActionRecord(salesforceTransaction);
        #endregion

        /*
         * MARSHAL UP RESPONSE
         */
        #region Build initial response object
        var response = new CreateContactResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = StatusType.Skipped // We don't sync Contacts to OSS
        };
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            // TODO: Delete this mock response and hook this up
            Random rnd = new Random();
            response.OracleStatus = StatusType.Successful;
            response.OraclePersonId = $"MockOraclePersonId{rnd.Next(100000, 999999)}";
        }
        #endregion

        return response;
    }

    public async Task<UpdateContactResponse> UpdateContact(UpdateContactModel model)
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
            Action = ActionType.Update,
            Object = ActionObjectType.Contact,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = JsonSerializer.Serialize(model),
            LastUpdatedOn = DateTime.UtcNow,
            OriginalTransactionId = null, // TODO: Populate this later when hooking up Retry
            TransactionLog = new List<SalesforceActionRecord>()
        };
        // Insert the event into the database, receive the response object and update the existing variable
        salesforceTransaction = await _actionsRepository.InsertActionRecord(salesforceTransaction);
        #endregion

        /*
         * MARSHAL UP RESPONSE
         */
        #region Build initial response object
        var response = new UpdateContactResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = StatusType.Skipped
        };
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            // TODO: Delete this mock response and hook this up
            Random rnd = new Random();
            response.OracleStatus = StatusType.Successful;
        }
        #endregion

        return response;
    }
}