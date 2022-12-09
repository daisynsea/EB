namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface ITerminalSerialCacheRepository
{
    Task<string> GetSalesOrdersByOrderNumbers(IEnumerable<string> orderNumbers);
}

public class TerminalSerialCacheRepository : ITerminalSerialCacheRepository
{
    private IManufacturingProxyClient _mfgProxyClient;

    public TerminalSerialCacheRepository(IManufacturingProxyClient manufacturingProxyClient)
    {
        _mfgProxyClient = manufacturingProxyClient;
    }

    public async Task<string> GetSalesOrdersByOrderNumbers(IEnumerable<string> orderNumbers)
    {
        return await _mfgProxyClient.GetSalesOrdersByNumbers(orderNumbers);        
    }
}
