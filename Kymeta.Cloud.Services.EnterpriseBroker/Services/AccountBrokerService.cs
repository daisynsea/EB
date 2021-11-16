using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IAccountBrokerService
{
    /// <summary>
    /// Process Salesforce actions received by the EnterpriseBroker
    /// </summary>
    /// <param name="account">Account model</param>
    /// <returns>Added account</returns>
    Task<SalesforceProcessResponse> ProcessSalesforceAction(SalesforceActionObject model);
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

    public async Task<SalesforceProcessResponse> ProcessSalesforceAction(SalesforceActionObject model)
    {
        /*
        * DETERMINE WHERE TO SYNC
        */
        var syncToOss = false;
        var syncToOracle = false;

        // verify if model.ObjectValues contains keys 'syncToOss' and 'syncToOracle' and parse them
        if (model.ObjectValues.ContainsKey("syncToOss"))
        {
            bool.TryParse(model.ObjectValues.GetValueOrDefault("syncToOss").ToString(), out syncToOss);
        }
        if (model.ObjectValues.ContainsKey("syncToOracle"))
        {
            bool.TryParse(model.ObjectValues.GetValueOrDefault("syncToOracle").ToString(), out syncToOracle);
        }

        /*
         * LOG THE ENTERPRISE APPLICATION BROKER ACTION
         */
        #region Log the Enterprise Action
        // Serialize the body coming in
        string body = JsonSerializer.Serialize(model.ObjectValues, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
        // Create the action record object
        var salesforceTransaction = new SalesforceActionTransaction
        {
            Id = Guid.NewGuid(),
            Action = model.ActionType,
            Object = model.ObjectType,
            ObjectId = model.ObjectId,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = JsonSerializer.Serialize(model.ObjectValues),
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
        var response = new SalesforceProcessResponse
        {
            ObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        string oracleAccountId = null;

        #region Process Account Create
        // If it's an account
        if (model.ObjectType == ActionObjectType.Account && model.ActionType == ActionType.Create)
        {
            /*
             * SEND TO ORACLE IF REQUIRED
             */
            #region Send to Oracle
            if (syncToOracle)
            {
                // first string is the oracle account id
                var addedAccountTuple = await _oracleService.AddAccount(model, salesforceTransaction);
                if (string.IsNullOrEmpty(addedAccountTuple.Item2)) // No error!
                {
                    response.OracleStatus = StatusType.Successful;
                    oracleAccountId = addedAccountTuple.Item1; // accountId
                    response.AddedOracleAccountId = addedAccountTuple.Item1;
                }
                else // Is error, do not EXIT..
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = addedAccountTuple.Item2;
                }
            }
            #endregion

            /*
             * SEND TO OSS IF REQUIRED
             */
            #region Send to OSS
            if (syncToOss)
            {
                var addedAccountTuple = await _ossService.AddAccount(model, oracleAccountId, salesforceTransaction);
                if (string.IsNullOrEmpty(addedAccountTuple.Item2)) // No error!
                {
                    response.OSSStatus = StatusType.Successful;
                    response.AddedOssAccountId = addedAccountTuple.Item1.Id.ToString();
                }
                else // Is error, do not EXIT..
                {
                    response.OSSStatus = StatusType.Error;
                    response.OSSErrorMessage = addedAccountTuple.Item2;
                }
            }
            #endregion
        }
        #endregion

        #region Process Account Update
        if (model.ObjectType == ActionObjectType.Account && model.ActionType == ActionType.Update)
        {
            #region Send to Oracle
            if (syncToOracle)
            {
                var updatedAccount = await _oracleService.UpdateAccount(model, salesforceTransaction);
                if (string.IsNullOrEmpty(updatedAccount.Item2)) // No error!
                {
                    response.OracleStatus = StatusType.Successful;
                }
                else // Is error, do not EXIT.. continue to Oracle
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = updatedAccount.Item2;
                }
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
        }
        #endregion

        // TODO: is this even required for CLDSRV? Only needed for Oracle perhaps?
        #region Process Address Create
        //// If it's an account
        //if (model.ObjectType == ActionObject.Address && model.ActionType == ActionType.Create)
        //{
        //    await _actionsRepository.UpdateActionRecord(new SalesforceActionRecord { Id = actionRecord.Id, OssStatus = StatusType.Skipped, OracleStatus = StatusType.Processing });
        //    /*
        //     * SEND TO ORACLE
        //     */
        //    #region Send to Oracle
        //    // first string is the oracle account id
        //    var addedAddressTuple = await _oracleRepository.AddAddress(model);
        //    if (string.IsNullOrEmpty(addedAddressTuple.Item2)) // No error!
        //    {
        //        response.OracleStatus = StatusType.Successful;
        //        oracleAccountId = addedAddressTuple.Item1; // accountId
        //        response.AddedOracleAccountId = addedAddressTuple.Item1;
        //        await _actionsRepository.UpdateActionRecord(new SalesforceActionRecord { Id = actionRecord.Id, OracleStatus = StatusType.Successful });
        //    }
        //    else // Is error, do not EXIT..
        //    {
        //        response.OracleStatus = StatusType.Error;
        //        response.OracleErrorMessage = addedAddressTuple.Item2;
        //        await _actionsRepository.UpdateActionRecord(new SalesforceActionRecord { Id = actionRecord.Id, OracleStatus = StatusType.Error, OracleErrorMessage = addedAddressTuple.Item2 });
        //    }
        //    #endregion
        //}
        #endregion

        return response;
    }
}