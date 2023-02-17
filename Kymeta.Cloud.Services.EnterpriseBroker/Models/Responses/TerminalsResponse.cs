namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;

public class TerminalsResponse<T>
{
    public long Count { get; set; }
    public IEnumerable<T>? Items { get; set; }
}
