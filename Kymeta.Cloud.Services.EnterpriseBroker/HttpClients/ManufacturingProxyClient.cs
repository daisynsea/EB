﻿namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;

public interface IManufacturingProxyClient
{
    Task<string> GetSalesOrdersByNumbers(IEnumerable<string> orderNumbers);
}

public class ManufacturingProxyClient : IManufacturingProxyClient
{
    private HttpClient _client;
    private IConfiguration _config;
    private ILogger<ManufacturingProxyClient> _logger;

    public ManufacturingProxyClient(HttpClient client, IConfiguration config, ILogger<ManufacturingProxyClient> logger)
    {
        client.BaseAddress = new Uri(config["Api:ManufacturingProxy"]);
        client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

        _client = client;
        _config = config;
        _logger = logger;
    }

    public async Task<string> GetSalesOrdersByNumbers(IEnumerable<string> orderNumbers)
    {
        var response = await _client.PostAsJsonAsync($"v1/getSalesOrdersByNumbers", orderNumbers);
        string data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"Failed to fetch sales orders from Manufacturing Proxy. HTTP call: {(int)response.StatusCode} | {data}");
            return null;
        }

        return data;
    }
}
