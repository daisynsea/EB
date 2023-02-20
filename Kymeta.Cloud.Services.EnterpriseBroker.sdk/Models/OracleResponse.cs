using Newtonsoft.Json;
using System.Net;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST
{
    public class OracleResponse<T> where T : IOracleResponsePayload
    {
       
        public OracleResponse(HttpStatusCode statusCode, string? message = null, string? data = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public HttpStatusCode StatusCode { get; init; }
        public string? Message { get; init; }

        public string? Data { get; init; }

        public T? Payload
        {
            get
            {
                if (string.IsNullOrEmpty(Data)) return default(T); 
                return JsonConvert.DeserializeObject<T>(Data);
            }
        }
    }
}
