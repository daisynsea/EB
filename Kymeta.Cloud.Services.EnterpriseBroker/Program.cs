using Kymeta.Cloud.Commons.AspNet.ApiVersion;
using Kymeta.Cloud.Commons.AspNet.DistributedConfig;
using Kymeta.Cloud.Commons.AspNet.Health;
using Kymeta.Cloud.Logging;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
// Use a custom port (TODO: This doesn't work right now, known issue, fixed in RC2)
// builder.WebHost.UseUrls("http://*:5098");
// Default connection limit is 100 seconds, make it a lot longer just in case Oracle sucks
//builder.WebHost.UseKestrel(options =>
//{
//    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
//});
// Setup configuration
builder.Configuration.AddGrapevineConfiguration(new GrapevineConfigurationOptions
{
    ClientId = builder.Configuration["Configuration:ClientId"],
    ConfigSourceUrl = builder.Configuration["ServiceHealthUrl"],
    Secret = builder.Configuration["Configuration:Secret"]
}, new CancellationTokenSource().Token);
if (builder.Environment.IsDevelopment()) builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
// Setup logging
string? instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
string pid = String.Format("{0}", Process.GetCurrentProcess().Id);
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddKymetaConsole(builder.Configuration.GetSection("KymetaLogging"));
}
else
{
    builder.Logging.AddKymetaAzureTableStorage(builder.Configuration.GetSection("KymetaLogging"), instanceId ?? "0", pid);
}
// Add health
builder.Services.AddHealthChecks();
// Add services
builder.Services.AddHttpClient<IAccountsClient, AccountsClient>();
builder.Services.AddHttpClient<IOracleClient, OracleClient>();
builder.Services.AddHttpClient<IUsersClient, UsersClient>();
builder.Services.AddScoped<IOssService, OssService>();
// TODO: Add more here

// Add health client
builder.Services.AddHealthClient(new HealthServiceOptions
{
    HealthServiceUrl = builder.Configuration["ServiceHealthUrl"],
    ClientId = builder.Configuration["Configuration:ClientId"],
    Secret = builder.Configuration["Configuration:Secret"]
});
// Add API versioning
builder.Services.AddApiVersioning();

// Add services to the container.
builder.Services.AddControllers();

// END: ConfigureServices
// START: Configure
var app = builder.Build();
// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();
app.UseApiVersionPathMiddleware();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/health");
app.Run();