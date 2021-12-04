namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class UpdateAddressModel : SalesforceActionObject
    {
        /// <summary>
        /// Salesforce Account Id
        /// </summary>
        public string? ParentAccountId { get; set; }
        /// <summary>
        /// Oracle Account Id
        /// </summary>
        public string? ParentAccountOracleId { get; set; }
        /// <summary>
        /// Oracle Address Id
        /// </summary>
        public string? AddressOracleId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Country { get; set; }
    }
}
