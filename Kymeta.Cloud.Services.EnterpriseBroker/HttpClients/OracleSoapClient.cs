using Kymeta.Cloud.Services.EnterpriseBroker.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;


public interface IOracleSoapClient
{

    Task<Tuple<XDocument, string, string>> SendSoapRequest(string soapEnvelope, string oracleServiceUrl, string? contentType = null);
    Task<Tuple<OracleOrganizationResponse, string>> UpdateOrganization(UpdateOracleOrganizationModel model, ulong partyNumber);
}

public class OracleSoapClient : IOracleSoapClient
{
    private const string UserName = "Oracle:Username";
    private const string Password = "Oracle:Password";
    private HttpClient _client;
    private ILogger<OracleSoapClient> _logger;
    private IConfiguration _config;

    public OracleSoapClient(HttpClient client, IConfiguration configuration, ILogger<OracleSoapClient> logger)
    {
        client.BaseAddress = new Uri(configuration.GetValue<string>("Oracle:Endpoint"));
        // custom BasicAuthenticationHeaderValue class to wrap the encoding 
        client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(configuration.GetValue<string>(UserName), configuration.GetValue<string>(Password));
        _client = client;
        _config = configuration;

        _logger = logger;
    }

    /// <summary>
    /// Marshal a SOAP request to the designated Oracle URL.
    /// </summary>
    /// <param name="soapEnvelope"></param>
    /// <param name="oracleServiceUrl"></param>
    /// <returns></returns>
    public async Task<Tuple<XDocument, string, string>> SendSoapRequest(string soapEnvelope, string oracleServiceUrl, string? contentType = null)
    {
        try
        {
            // check for empty content
            if (string.IsNullOrEmpty(soapEnvelope)) return new Tuple<XDocument, string, string>(null, $"soapEnvelope is required.", null);
            if (string.IsNullOrEmpty(oracleServiceUrl)) return new Tuple<XDocument, string, string>(null, $"oracleServiceUrl is required.", null);


            // TODO: check to see if we can re-use BasicAuthenticationHeaderValue (below) instead of encoding above
            //var creds = new BasicAuthenticationHeaderValue(_config.GetValue<string>("Oracle:Username"), _config.GetValue<string>("Oracle:Password"));

            HttpWebRequest request = CreateRequest(oracleServiceUrl, contentType, soapEnvelope);

            XDocument doc = await ReadResponse(request);
            Console.WriteLine(doc);
            return new Tuple<XDocument, string, string>(doc, null, null);
        }
        catch (WebException wex)
        {
            using var stream = wex.Response?.GetResponseStream();
            using var reader = new StreamReader(stream);

            try
            {
                // deserialize the xml response envelope
                XmlSerializer serializer = new(typeof(FaultEnvelope));
                var result = (FaultEnvelope)serializer.Deserialize(reader);
                var faultMessage = result?.Body.Fault.faultstring;

                return new Tuple<XDocument, string, string>(null, wex.Message, faultMessage);
            }
            catch (Exception ex)
            {
                // additional catch-all in case the fault cannot be deserialized
                return new Tuple<XDocument, string, string>(null, ex.Message, null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Tuple<XDocument, string, string>(null, ex.Message, null);
        }

        
    }

    public async Task<Tuple<OracleOrganizationResponse, string>> UpdateOrganization(UpdateOracleOrganizationModel model, ulong partyNumber)
    {
        var serialized = JsonSerializer.Serialize(model, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
        var content = new StringContent(serialized, Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync($"crmRestApi/resources/latest/accounts/{partyNumber}", content);

        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed UpdateAccount Oracle HTTP call: {(int)response.StatusCode} | {data} | Model sent: {JsonSerializer.Serialize(model)}");
            return new Tuple<OracleOrganizationResponse, string>(null, $"{(int)response.StatusCode} | {data}");
        }

        var deserializedObject = JsonSerializer.Deserialize<OracleOrganizationResponse>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return new Tuple<OracleOrganizationResponse, string>(deserializedObject, null);
    }

    private string GetBasicAuthString()
    {
        // construct the base 64 encoded string used as credentials for the service call
        byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(_config[UserName] + ":" + _config[Password]);
        string credentials = Convert.ToBase64String(toEncodeAsBytes);
        return credentials;
    }

    private HttpWebRequest CreateRequest(string oracleServiceUrl, string? contentType, string soapEnvelope)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(soapEnvelope);
        // create HttpWebRequest connection to the service
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(oracleServiceUrl);

        // configure request headers
        request.Method = "POST";
        request.ContentType = contentType ?? "text/xml;charset=UTF-8";
        request.ContentLength = byteArray.Length;

        // configure the request to use basic authentication, with base64 encoded user name and password
        request.Headers.Add("Authorization", $"Basic {GetBasicAuthString()}");

        // write the xml payload to the request
        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        return request;
    }

    private static async Task<XDocument> ReadResponse(HttpWebRequest request)
    {
        var doc = new XDocument();
        // get the response and process it (print out the response XDocument doc)
        using (WebResponse response = await request.GetResponseAsync())
        {
            using Stream stream = response.GetResponseStream();
            doc = XDocument.Load(stream);
        }

        return doc;
    }

   
}