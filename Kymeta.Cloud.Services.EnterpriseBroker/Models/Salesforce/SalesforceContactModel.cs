namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class SalesforceContactModel : SalesforceActionObject
    {
        public long? OraclePartyId { get; set; }
        public string? ParentAccountId { get; set; }
        /// <summary>
        /// This is required to link legacy objects to the correct items in Oracle
        /// </summary>
        public string? ParentOraclePartyId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public bool? IsPrimary { get; set; }
        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? OraclePartyId { get; set; }
    }
}
