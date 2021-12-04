﻿namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class CreateContactModel : SalesforceActionObject
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
