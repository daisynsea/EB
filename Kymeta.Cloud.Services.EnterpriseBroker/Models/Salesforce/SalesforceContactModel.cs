namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class SalesforceContactModel : SalesforceActionObject
    {
        public string? ParentAccountId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        //public string? FirstName => Name?.Split(' ')?.FirstOrDefault();
        //public string? LastName => Name?.Split(' ')?.LastOrDefault();
        public bool? IsPrimary { get; set; }
        public string? Role { get; set; }
        public string? Title { get; set; }
    }
}
