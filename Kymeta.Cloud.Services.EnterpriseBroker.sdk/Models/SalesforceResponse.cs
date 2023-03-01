using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Newtonsoft.Json;
using System.Net;

public class SalesforceResponse<T> where T : ISalesforceResponsePayload
{
    public SalesforceResponse(HttpStatusCode status, string? reasonPhase, string? conent)
    {
        StatusCode = status;
        ReasonPhrase = reasonPhase;
        Content = conent;
        SetPayload();
    }

    public string? ReasonPhrase { get; private set; }
    public string? Content { get; private set; }
    public T? Payload { get; private set; }

    public string? ErrorMessage { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }

    public bool IsSuccessStatusCode()
    {
        if (StatusCode.IsSuccess())
        {
            return Payload != null;
        }
        return StatusCode.IsSuccess();
    }

    private void SetPayload()
    {
        if (!StatusCode.IsSuccess() || string.IsNullOrEmpty(Content))
        {
            ErrorMessage = Content;
            Payload = default(T);
        }
        else
        {
            ErrorMessage = "";
            Payload = JsonConvert.DeserializeObject<T>(Content);
        }

    }

}
