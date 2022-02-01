namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

/// <summary>
/// This model represents the SF REST response when querying Account objects
/// </summary>
public class SalesforceAccountObjectModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? ParentId { get; set; }
    public string? CreatedDate { get; set; }
    public bool? IsPartner { get; set; }
    public string? AccountType__c { get; set; }
    public string? Sub_Type__c { get; set; }
    public string? Type_of_Company__c { get; set; }
    public string? Business_Unit__c { get; set; }
    public string? Oracle_Acct__c { get; set; }
    public string? KSN_Acct_ID__c { get; set; }
    public string? Lepton_AccID__c { get; set; }
    public string? OSSSyncStatus__c { get; set; }
    public string? OracleSyncStatus__c { get; set; }
    public string? Sync_Instructions__c { get; set; }
    public string? Tax_ID__c { get; set; }
    public string? Sync_Status__c { get; set; }
    public string? OSS_Error_Message__c { get; set; }
    public string? Oracle_Error_Message__c { get; set; }
    public bool? Approved__c { get; set; }
}