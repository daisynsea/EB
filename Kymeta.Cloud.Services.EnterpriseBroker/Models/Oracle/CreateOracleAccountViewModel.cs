namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class CreateOracleAccountViewModel
{
    public string AccountName { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
}

public class CreateOracleCustomerAccountViewModel : CreateOracleAccountViewModel
{
    public string OrganizationPartyId { get; set; }
    public string OrganizationPartySiteId { get; set; }
    public string AccountType { get; set; }
    public string AccountSubType { get; set; }
    public string TaxId { get; set; }
    public string SalesforceId { get; set; }
    public string OssId { get; set; }
}

public class UpdateOracleCustomerAccountViewModel : CreateOracleAccountViewModel
{
    public string CustomerAccountId { get; set; }
    public string CustomerAccountPartyId { get; set; }
    public string AccountType { get; set; }
    public string AccountSubType { get; set; }
    public string SalesforceId { get; set; }
    public string OssId { get; set; }
}