namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceActionRecord
{
    /// <summary>
    /// Event Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Timestamp when the request occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// User name (email) of the Salesforce user performing the action
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// Action performed on the Object (ie. Create, Update)
    /// </summary>
    public ActionType? Action { get; set; }
    /// <summary>
    /// Object type (ie. Account, Contact)
    /// </summary>
    public ActionObjectType? Object { get; set; }
    /// <summary>
    /// Id of the Salesforce object
    /// </summary>
    public string ObjectId { get; set; }
    /// <summary>
    /// Datetime the EnterpriseAction record was last updated on
    /// </summary>
    public DateTime? LastUpdatedOn { get; set; }
    /// <summary>
    /// Body of the request that came in. Can be null.
    /// </summary>
    public string? Body { get; set; }
    /// <summary>
    /// Status of the syncing to Oracle
    /// </summary>
    public StatusType? OracleStatus { get; set; }
    /// <summary>
    /// Status of the syncing to OSS
    /// </summary>
    public StatusType? OssStatus { get; set; }
    public string? OracleErrorMessage { get; set; }
    public string? OssErrorMessage { get; set; }
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
    Processing,
    Successful,
    Error
}
