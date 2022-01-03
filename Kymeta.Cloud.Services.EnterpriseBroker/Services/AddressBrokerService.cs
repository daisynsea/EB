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
            // Get customer account by Salesforce Account Id
            var customerAccount = await _oracleService.GetCustomerAccountBySalesforceAccountId(model.ParentAccountId, salesforceTransaction);
            if (customerAccount == null)
            {
                response.OracleStatus = StatusType.Error;
                response.OracleErrorMessage = $"Error syncing Address to Oracle: Customer Account object with SF reference Id {model.ParentAccountId} was not found.";
            }

            // TODO: Figure out how to get an existing location from the customer account

            // If exists, update
            // Otherwise, create

            // TODO: Delete this mock response and hook this up
            response.OracleStatus = StatusType.Successful;
        }
        #endregion

        return response;
    }
}

