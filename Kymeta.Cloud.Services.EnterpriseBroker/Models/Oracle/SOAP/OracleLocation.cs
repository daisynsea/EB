namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP
{
    public class OracleLocation
    {
        /// <summary>
        /// Randomly generated unique value (Guid)
        /// </summary>
        public string OrigSystemReference { get; set; }
        public string Country { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
