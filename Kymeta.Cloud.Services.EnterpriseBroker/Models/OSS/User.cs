namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class User
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? RoleId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? Enabled { get; set; }
    public string? ContactType { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
