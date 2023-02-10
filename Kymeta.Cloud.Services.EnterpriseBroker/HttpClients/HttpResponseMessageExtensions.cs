using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<OracleResponse> ProcessResponseFromOracle(this HttpResponseMessage response, CancellationToken token)
        {
            string content = await response.Content.ReadAsStringAsync(token);

            return new OracleResponse(response.StatusCode, response.ReasonPhrase, content);
        }
    }
}
