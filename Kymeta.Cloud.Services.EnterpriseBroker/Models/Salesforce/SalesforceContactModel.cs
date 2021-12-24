namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class SalesforceContactModel : SalesforceActionObject
    {
        public string? ParentAccountId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? FirstName => Name?.Split(' ')?.FirstOrDefault();
        public string? LastName => Name?.Split(' ', 1)?.LastOrDefault();
    }
}
