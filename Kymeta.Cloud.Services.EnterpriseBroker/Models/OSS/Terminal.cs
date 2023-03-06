using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public struct GeoLoc
{
    public double Longitude { get; set; }

    public double Latitude { get; set; }
    public GeoLoc(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}

public class Terminal
{
    public Guid Id { get; set; }
    public string? SalesforceAssetId { get; set; }
    public string? OracleTerminalSerial { get; set; }
    public Guid? AccountId { get; set; }
    public string? SalesforceAccountId { get; set; }
    public string? AccountName { get; set; }
    public string? Description { get; set; }
    public Guid? BilledAccountId { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? Name { get; set; }
    public string? Serial { get; set; }
    public string? ProductCode { get; set; }
    public string? PartNumber { get; set; }
    public bool? Provisioned { get; set; }
    public DateTime? ProvisionedOn { get; set; }
    public bool? Activated { get; set; }
    public DateTime? ActivatedOn { get; set; }
    public bool? Paused { get; set; }
    public DateTime? PausedOn { get; set; }
    public PauseReason? PauseReason { get; set; }
    public bool? Private { get; set; }
    public DateTime? PrivateOn { get; set; }
    public bool? EnableCellTraffic { get; set; }
    public TerminalHardware? Hardware { get; set; }
    public TerminalSatelliteNetworkConfiguration? SatelliteNetworkConfiguration { get; set; }
    public TerminalSimConfiguration? SimConfiguration { get; set; }
    public TerminalNetworkStatus? NetworkStatus { get; set; }
    public TerminalLocation? Location { get; set; }
    public TerminalSoftware? Software { get; set; }
    public List<ComponentHistoryRecord>? ComponentHistory { get; set; }
    public List<string>? Features { get; set; }
    public string? PublicKey { get; set; }
    public TerminalSubscription? ActiveSubscription { get; set; }
    public List<string>? VisibleToAccounts { get; set; }
    public List<string>? VisibleToUsers { get; set; }
    public long? TotalSatelliteUsage { get; set; }
    public long? TotalCellularUsage { get; set; }
    public bool? Archived { get; set; }

    // Partner API only
    public bool? SyncInProgress { get; set; }
}

public class TerminalSatelliteNetworkConfiguration
{
    public string? Eth0IpAddress { get; set; }
    public string? Eth0SubnetMask { get; set; }
    public string? Sat0IpAddress { get; set; }
    public string? Sat0SubnetMask { get; set; }
    public string? MgmtIpAddress { get; set; }
    public string? MgmtSubnetMask { get; set; }
    public string? DhcpGateway { get; set; }
    public string? DhcpClientIpRangeStart { get; set; }
    public string? DhcpClientIpRangeEnd { get; set; }
}

public class TerminalSimConfiguration
{
    public string? EID { get; set; }
    public string? Msisdn { get; set; }
    public float? CubicLTEIMSI { get; set; }
    public float? Tele2LTEIMSI { get; set; }
    public float? CubicDkIMSI { get; set; }
    public float? CubicFrIMSI { get; set; }
    public string? Aisimsi { get; set; }
    public string? OrFr901IMSI { get; set; }
    public string? Batch { get; set; }
    public string? Profile { get; set; }
    public string? FormFactor { get; set; }
    public string? Grade { get; set; }
    public string? SimVendor { get; set; }
    public string? CheckedOutTo { get; set; }
}

public class TerminalNetworkStatus
{
    public bool? SatelliteOnline { get; set; }
    public bool? CellularOnline { get; set; }
    public double? SatelliteSignal { get; set; }
    public double? CellularSignal { get; set; }
    public bool? Wifi { get; set; }
    public string? Satellite { get; set; }
    public string? Beam { get; set; }
    public DateTime? SatelliteLastUpdated { get; set; }
    public DateTime? CellularLastUpdated { get; set; }
}

public class TerminalSubscription
{
    public Guid? SubscriptionId { get; set; }
    public string? SalesforceSubscriptionId { get; set; }
    public string? Description { get; set; }
    public int? DataLimitInGb { get; set; }
    public List<NetworkIdentifier>? NetworkIdentifiers { get; set; }
    public bool? HasCellular { get; set; }
    public bool? HasSatellite { get; set; }
    public bool? CanActivate { get; set; }
    public bool? CanProvision { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class TerminalLocation
{
    public GeoLoc Coordinates { get; set; }
    public DateTime? LastUpdated { get; set; }
}

public class TerminalSoftware
{
    public string? CurrentVersion { get; set; }
    public string? TargetVersion { get; set; }
    public bool? Locked { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
    public DateTime? LastCheckin { get; set; }
}

public class TerminalHardware
{
    public TerminalHardwareItem? Antenna { get; set; }
    public TerminalHardwareItem? Modem { get; set; }
    public TerminalHardwareItem? Buc { get; set; }
    public TerminalHardwareItem? Lnb { get; set; }
    public TerminalHardwareItem? CellRouter { get; set; }
    public TerminalHardwareItem? Terminal { get; set; }
    public TerminalHardwareItem? Diplexer { get; set; }
}

public class TerminalHardwareItem
{
    public string? Model { get; set; }
    public string? Serial { get; set; }
    public string? AssetId { get; set; }
    public string? IMEI { get; set; }
    public string? ICCID { get; set; }
}

public class ComponentHistoryRecord
{
    public string? Serial { get; set; }
    public string? Type { get; set; }
    public string? AssetId { get; set; }
    public string? Model { get; set; }
    public DateTime? AddedOn { get; set; }
    public DateTime? RemovedOn { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PauseReason
{
    FORCE, // KSN
    MANUAL, // Partner-Api
    SUBSCRIPTION_EXPIRED, // Subscription expired
    OP // Overage Protection
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UnpauseReason
{
    FORCE,
    MANUAL,
    SUBSCRIPTION_STARTED,
    OP_RESET
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NetworkIdentifier
{
    Evolution,
    Velocity,
    Transec,
    Cubic,
    Peplink
}