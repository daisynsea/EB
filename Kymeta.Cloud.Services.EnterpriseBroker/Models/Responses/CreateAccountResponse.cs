namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses
{
    public class CreateAccountResponse : SalesforceProcessResponse
    {
        public string? OssAccountId { get; set; }
        public string? OracleCustomerAccountId { get; set; } = null;
        public string? OracleCustomerProfileId { get; set; } = null;
        public string? OracleOrganizationId { get; set; } = null;
    }
}
