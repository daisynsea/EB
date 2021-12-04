using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceActionTransaction
{
    /// <summary>
    /// Event Id
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }
    /// <summary>
    /// Timestamp when the request occurred.
    /// </summary>
    [JsonProperty("createdOn")]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// User name (email) of the Salesforce user performing the action
    /// </summary>
    [JsonProperty("userName")]
    public string? UserName { get; set; }
    /// <summary>
    /// Action performed on the Object (ie. Create, Update)
    /// </summary>
    [JsonProperty("action")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ActionType? Action { get; set; }
    /// <summary>
    /// Object type (ie. Account, Contact)
    /// </summary>
    [JsonProperty("object")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ActionObjectType? Object { get; set; }
    /// <summary>
    /// Id of the Salesforce object
    /// </summary>
    [JsonProperty("objectId")]
    public string? ObjectId { get; set; }
    /// <summary>
    /// Datetime the EnterpriseAction record was last updated on
    /// </summary>
    [JsonProperty("lastUpdatedOn")]
    public DateTime? LastUpdatedOn { get; set; }
    /// <summary>
    /// Body of the request that came in. Can be null.
    /// </summary>
    [JsonProperty("serializedObjectValues")]
    public string SerializedObjectValues { get; set; }
    /// <summary>
    /// (Optional) If this transaction is a retry of a previous transaction, this field will be populated.
    /// </summary>
    [JsonProperty("originalTransactionId")]
    public string? OriginalTransactionId { get; set; }
    /// <summary>
    /// Log of actions in this transaction
    /// </summary>
    [JsonProperty("transactionLog")]
    public List<SalesforceActionRecord>? TransactionLog { get; set; }

    [JsonIgnore]
    public StatusType? OssStatus
    {
        get
        {
            var ossStatuses = TransactionLog?.Where(a => a.Action.ToString().ToLower().Contains("oss")).OrderByDescending(t => t.Timestamp);
            return ossStatuses?.FirstOrDefault()?.Status ?? StatusType.Skipped;
        }
    }

    [JsonIgnore]
    public StatusType? OracleStatus
    {
        get
        {
            var oracleStatuses = TransactionLog?.Where(a => a.Action.ToString().ToLower().Contains("oracle")).OrderByDescending(t => t.Timestamp);
            return oracleStatuses?.FirstOrDefault()?.Status ?? StatusType.Skipped;
        }
    }
}

public class SalesforceActionRecord
{
    [JsonConverter(typeof(StringEnumConverter))]
    public SalesforceTransactionAction Action { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public StatusType Status { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? ErrorMessage { get; set; }
    public string? EntityId { get; set; }
}

public enum SalesforceTransactionAction
{
    CreateAccountInOss,
    CreateOrganizationInOracle,
    CreateCustomerAccountInOracle,
    CreateCustomerProfileInOracle,
    CreateCustomerAccountSiteInOracle,
    UpdateAccountInOss,
    UpdateOrganizationInOracle,
    UpdateCustomerAccountInOracle,
    UpdateCustomerProfileInOracle,
    UpdateCustomerAccountSiteInOracle,
    UpdateAddressInOss,
    CreateSiteInOracle,
    UpdateSiteInOracle
}

public enum ActionType
{
    Create,
    Update
}

public enum ActionObjectType
{
    Account,
    Contact,
    Address
}

public enum StatusType
{
    Skipped,
    Started,
    Successful,
    Error
}
