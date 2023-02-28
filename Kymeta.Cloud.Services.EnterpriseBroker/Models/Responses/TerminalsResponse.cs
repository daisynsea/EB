namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;

public class TerminalsResponse
{
    public long Count { get; set; }
    public IEnumerable<Terminal>? Items { get; set; }
}
