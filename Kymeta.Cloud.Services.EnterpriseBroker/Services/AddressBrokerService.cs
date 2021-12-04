namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IAddressBrokerService
{
    Task<CreateAddressResponse> CreateAddress(CreateAddressModel model);
    Task<UpdateAddressResponse> UpdateAddress(UpdateAddressModel model);
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

    public async Task<CreateAddressResponse> CreateAddress(CreateAddressModel model)
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
            Action = ActionType.Create,
            Object = ActionObjectType.Address,
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
        var response = new CreateAddressResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            // TODO: Delete this mock response and hook this up
            Random rnd = new Random();
            response.OracleStatus = StatusType.Successful;
            response.OracleAddressId = $"MockOracleAddressId{rnd.Next(100000, 999999)}";
        }
        #endregion

        #region Send to OSS
        if (syncToOss)
        {
            var addedAddressTuple = await _ossService.UpdateAccountAddress(new UpdateAddressModel { Address1 = model.Address1, Address2 = model.Address2, Country = model.Country }, salesforceTransaction);
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

        return response;
    }

    public async Task<UpdateAddressResponse> UpdateAddress(UpdateAddressModel model)
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
            Action = ActionType.Update,
            Object = ActionObjectType.Address,
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
        var response = new UpdateAddressResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            // TODO: Delete this mock response and hook this up
            Random rnd = new Random();
            response.OracleStatus = StatusType.Successful;
            response.OracleAddressId = $"MockOracleAddressId{rnd.Next(100000, 999999)}";
        }
        #endregion

        #region Send to OSS
        if (syncToOss)
        {
            var updatedAddressTuple = await _ossService.UpdateAccountAddress(new UpdateAddressModel { Address1 = model.Address1, Address2 = model.Address2, Country = model.Country }, salesforceTransaction);
            if (string.IsNullOrEmpty(updatedAddressTuple.Item2)) // No error!
            {
                response.OSSStatus = StatusType.Successful;
            }
            else // Is error, do not EXIT..
            {
                response.OSSStatus = StatusType.Error;
                response.OSSErrorMessage = updatedAddressTuple.Item2;
            }
        }
        #endregion

        return response;
    }
}

