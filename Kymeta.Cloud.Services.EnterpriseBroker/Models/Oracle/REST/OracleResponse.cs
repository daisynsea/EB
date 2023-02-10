using System.Net;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST
{
    public class OracleResponse
    {
        public OracleResponse(HttpStatusCode statusCode, string? message = null, object? data = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public HttpStatusCode StatusCode { get; init; }
        public string? Message { get; init; }
        public object? Data { get; init; }

        public async Task<OracleResponse> ProcessHttpResponse(HttpResponseMessage response, CancellationToken token)
        {
            string content = await response.Content.ReadAsStringAsync(token);
            return new OracleResponse(response.StatusCode, response.ReasonPhrase, content);
        }
    }
}
