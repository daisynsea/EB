namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator
{
    public class ConfiguratorMetadata
    {
        public string? Rules { get; set; }
        public IEnumerable<SpecSheetRow> OspreySpecSheet { get; set; }
    }

    public class SpecSheetRow
    {
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
        public string? Field3 { get; set; }
    }
}
