namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceAddressModel : SalesforceActionObject
{
    /// <summary>
    /// Account__c
    /// </summary>
    public string? ParentAccountId { get; set; }
    /// <summary>
    /// Parent Account Business Unit
    /// </summary>
    public string? ParentAccountBusinessUnit { get; set; }
    /// <summary>
    /// This is required to link the site to the customer account?
    /// </summary>
    public string? ParentOracleAccountId { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string? SiteName { get; set; }
    /// <summary>
    /// Address__c
    /// This field is two lines and needs to be split on newline
    /// </summary>
    public string? Address { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    /// <summary>
    /// City__c
    /// </summary>
    public string? City { get; set; }
    /// <summary>
    /// Country__c
    /// </summary>
    public string? Country { get; set; }
    /// <summary>
    /// State_Province__c
    /// </summary>
    public string? StateProvince { get; set; }
    /// <summary>
    /// Postal_Code__c
    /// </summary>
    public string? PostalCode { get; set; }
    /// <summary>
    /// Type__c (picklist -> 'Billing & Shipping' || 'Shipping')
    /// </summary>
    public string? Type { get; set; }
}