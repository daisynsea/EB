namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class CreateOracleAccountViewModel
{
    public string OrganizationName { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
}

public class CreateOracleCustomerAccountViewModel : CreateOracleAccountViewModel
{
    public string PartyId { get; set; }
    public string AccountType { get; set; }
    public string AccountSubType { get; set; }
    public string TaxId { get; set; }
    public string SalesforceId { get; set; }
    public string OssId { get; set; }
}