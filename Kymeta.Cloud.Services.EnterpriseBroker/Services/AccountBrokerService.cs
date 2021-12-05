using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IAccountBrokerService
{
    /// <summary>
    /// Process account being created for the first time, after approval
    /// </summary>
    /// <param name="model">Create payload</param>
    /// <returns>Added account</returns>
    Task<CreateAccountResponse> ProcessAccountCreate(CreateAccountModel model);
    /// <summary>
    /// Process account being updated
    /// </summary>
    /// <param name="model">Update payload</param>
    /// <returns>Updated account response</returns>
    Task<UpdateAccountResponse> ProcessAccountUpdate(UpdateAccountModel model);
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

    public async Task<CreateAccountResponse> ProcessAccountCreate(CreateAccountModel model)
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
            Object = ActionObjectType.Account,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = body,
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
        var response = new CreateAccountResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        string oracleCustomerAccountId = null;

        #region Process Account Create
        /*
         * SEND TO ORACLE IF REQUIRED
         */
        #region Send to Oracle
        if (syncToOracle)
        {
            // TODO: Actually hook this up

            // first string is the oracle account id
            //var addedAccountTuple = await _oracleService.AddAccount(model, salesforceTransaction);
            //if (string.IsNullOrEmpty(addedAccountTuple.Item2)) // No error!
            //{
            //    response.OracleStatus = StatusType.Successful;
            //    oracleAccountId = addedAccountTuple.Item1; // accountId
            //    response.AddedOracleAccountId = addedAccountTuple.Item1;
            //}
            //else // Is error, do not EXIT..
            //{
            //    response.OracleStatus = StatusType.Error;
            //    response.OracleErrorMessage = addedAccountTuple.Item2;
            //}

            // TODO: Delete this mock response
            Random rnd = new Random();
            response.OracleStatus = StatusType.Successful;
            oracleCustomerAccountId = $"MockCustomerAccountId{rnd.Next(100000, 999999)}";
            var oracleCustomerProfileId = $"MockCustomerProfileId{rnd.Next(100000, 999999)}";
            var oracleOrganizationId = $"MockOrganizationId{rnd.Next(100000, 999999)}";
            response.OracleCustomerAccountId = oracleCustomerAccountId;
            response.OracleOrganizationId = oracleOrganizationId;
            response.OracleCustomerProfileId = oracleCustomerProfileId;
        }
        #endregion

        /*
         * SEND TO OSS IF REQUIRED
         */
        #region Send to OSS
        if (syncToOss)
        {
            var addedAccountTuple = await _ossService.AddAccount(model, oracleCustomerAccountId, salesforceTransaction);
            if (string.IsNullOrEmpty(addedAccountTuple.Item2)) // No error!
            {
                response.OSSStatus = StatusType.Successful;
                response.OssAccountId = addedAccountTuple.Item1.Id.ToString();
            }
            else // Is error, do not EXIT..
            {
                response.OSSStatus = StatusType.Error;
                response.OSSErrorMessage = addedAccountTuple.Item2;
            }
        }
        #endregion
        #endregion

        response.CompletedOn = DateTime.UtcNow;
        return response;
    }
    
    public async Task<UpdateAccountResponse> ProcessAccountUpdate(UpdateAccountModel model)
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
            Object = ActionObjectType.Account,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = body,
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
        var response = new UpdateAccountResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        #region Send to Oracle
        if (syncToOracle)
        {
            response.OracleStatus = StatusType.Successful;
            // TODO: Bring this back when implemented correctly

            //var updatedAccount = await _oracleService.UpdateAccount(model, salesforceTransaction);
            //if (string.IsNullOrEmpty(updatedAccount.Item2)) // No error!
            //{
            //    response.OracleStatus = StatusType.Successful;
            //}
            //else // Is error, do not EXIT.. continue to Oracle
            //{
            //    response.OracleStatus = StatusType.Error;
            //    response.OracleErrorMessage = updatedAccount.Item2;
            //}
        }
        #endregion

        #region Send to OSS
        if (syncToOss)
        {
            var updatedAccount = await _ossService.UpdateAccount(model, salesforceTransaction);
            if (string.IsNullOrEmpty(updatedAccount.Item2)) // No error!
            {
                response.OSSStatus = StatusType.Successful;
            }
            else // Is error, do not EXIT.. continue to Oracle
            {
                response.OSSStatus = StatusType.Error;
                response.OSSErrorMessage = updatedAccount.Item2;
            }
        }
        #endregion

        return response;
    }
}