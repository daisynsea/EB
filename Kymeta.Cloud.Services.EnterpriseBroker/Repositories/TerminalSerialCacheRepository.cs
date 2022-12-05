namespace Kymeta.Cloud.Services.EnterpriseBroker.Repositories;

public interface ITerminalSerialCacheRepository
{
    Task<IEnumerable<SalesOrderResponse>> GetSalesOrdersByOrderNumbers(IEnumerable<string> orderNumbers);
}

public class TerminalSerialCacheRepository : ITerminalSerialCacheRepository
{
    // TODO: remove this once database exists
    private static List<SalesOrderResponse> _salesOrders = new List<SalesOrderResponse>();

    public TerminalSerialCacheRepository()
    {
        // TODO: Inject a database or http client once available
        // Tracking: https://jira.kymetacorp.com/browse/CLDSRV-14310
        // TODO: Remove this dummy data
        _salesOrders.Add(new SalesOrderResponse
        {
            SalesOrder = "ABC123",
            Terminals = new List<SalesOrderTerminal>
            {
                new SalesOrderTerminal
                {
                    AntennaSerial = "ACG029K220323054",
                    IpAddress = "192.168.44.2",
                    BucSerial = "211500304",
                    HybridRouterIccid = "",
                    HybridRouterImei = "",
                    HybridRouterSerial = "",
                    DescriptionFirstLine = "KYMETA U8 GEO TERMINAL, 20W BUC, IQ 200",
                    SatModem = "1828A022-067868",
                    DescriptionSecondLine = "",
                    DiplexerSerial = "76554092109405",
                    LinkTimestamp = new DateTime(2022, 11, 15, 7, 19, 51),
                    LnbSerial = "10302002200R220202613",
                    OracleTerminalSerial = "ACB003-00107",
                    ProductCode = "U8901-10113-0",
                    TerminalKpn = "805-00016-101-C",
                    TerminalSerial = "ACB000K221111142"
                }
            }
        });
    }

    public async Task<IEnumerable<SalesOrderResponse>> GetSalesOrdersByOrderNumbers(IEnumerable<string> orderNumbers)
    {
        // TODO: Implement this once database available
        var response = new List<SalesOrderResponse>();

        var results = _salesOrders.Where(r => orderNumbers.Contains(r.SalesOrder));

        return results;
    }
}
