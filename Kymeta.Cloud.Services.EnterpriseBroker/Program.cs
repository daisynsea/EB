using Kymeta.Cloud.Commons.AspNet.ApiVersion;
using Kymeta.Cloud.Commons.AspNet.DistributedConfig;
using Kymeta.Cloud.Commons.AspNet.Health;
using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Logging;
using Kymeta.Cloud.Logging.Activity;
using Kymeta.Cloud.Services.EnterpriseBroker;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Default connection limit is 100 seconds, make it a lot longer just in case Oracle sucks
builder.WebHost.UseKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
    // if dev env
    var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    if (isDevelopment)
    {
        options.ListenAnyIP(5098);
        options.ListenAnyIP(5099, configure => configure.UseHttps());
    }
    else // not dev env
    {
        // if we're in kubernetes, use a real pfx
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IsKubernetes")) && Environment.GetEnvironmentVariable("IsKubernetes")?.ToLower() == "true")
        {
            var cert = new X509Certificate2("Certificate.pfx", "", X509KeyStorageFlags.MachineKeySet);
            options.ListenAnyIP(80);
            options.ListenAnyIP(443, configure => configure.UseHttps(cert));
        }
    }
});
// Setup configuration
builder.Configuration.AddGrapevineConfiguration(new GrapevineConfigurationOptions
{
    ClientId = builder.Configuration["Configuration:ClientId"],
    ConfigSourceUrl = builder.Configuration["ServiceHealthUrl"],
    Secret = builder.Configuration["Configuration:Secret"]
}, new CancellationTokenSource().Token);
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IsKubernetes")) && Environment.GetEnvironmentVariable("IsKubernetes")?.ToLower() == "true")
{
    builder.Configuration.AddJsonFile("appsettings.Kube.json", optional: true, reloadOnChange: true);
}
if (builder.Environment.IsDevelopment()) builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
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
builder.Services.AddHttpClient<IActivityLoggerClient, ActivityLoggerClient>();
builder.Services.AddHttpClient<IFileStorageClient, FileStorageClient>();
builder.Services.AddCosmosDb(builder.Configuration.GetConnectionString("AzureCosmosDB"));
builder.Services.AddScoped<IActionsRepository, ActionsRepository>();
builder.Services.AddScoped<IOssService, OssService>();
builder.Services.AddScoped<IAccountBrokerService, AccountBrokerService>();
builder.Services.AddScoped<IAddressBrokerService, AddressBrokerService>();
builder.Services.AddScoped<IContactBrokerService, ContactBrokerService>();
builder.Services.AddScoped<IOracleService, OracleService>();
builder.Services.AddScoped<ISalesforceProductsRepository, SalesforceProductsRepository>();
builder.Services.AddScoped<IQuotesRepository, QuotesRepository>();
builder.Services.AddScoped<ICacheRepository, CacheRepository>();
builder.Services.AddScoped<IConfiguratorQuoteRequestService, ConfiguratorQuoteRequestService>();
builder.Services.AddScoped<IProductsBrokerService, ProductsBrokerService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<ITerminalSerialCacheRepository, TerminalSerialCacheRepository>();
// add background operation services
builder.Services.Configure<HostOptions>(hostOptions =>
{
    // prevent host crash if background operation encounters an Exception
    hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
builder.Services.AddHostedService<BackgroundOperationService>();
builder.Services.AddScoped<ISalesforceProcessingService, SalesforceProcessingService>();
// configure redis
builder.Services.AddRedisClient(new RedisClientOptions
{
    ConnectionString = builder.Configuration.GetConnectionString("RedisCache")
});
builder.Services.AddHttpClient<ISalesforceClient, SalesforceClient>();
// Activity logger
builder.Services.AddScoped<IActivityLogger>(provider => new ActivityLogger(new ActivityLoggerConfigurationOptions { ConnectionString = builder.Configuration.GetConnectionString("ActivityQueue") }));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
    {
        opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        opts.LoginPath = "/Auth/Login";
        //opts.LogoutPath = "/auth/logout";
        opts.ClaimsIssuer = "kymetacloudservices";
        opts.ExpireTimeSpan = TimeSpan.FromHours(24);
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.ClientId = "enterprisebroker";
        options.ClientSecret = builder.Configuration["Authentication:OidcSecret"] ?? "secret";
        options.Authority = builder.Configuration["Authentication:OidcAuthority"] ?? "https://access.kymeta.io";
        options.ResponseType = "code";
        options.SignedOutCallbackPath = "/signout-callback-openid";
        options.SignedOutRedirectUri = "~/";
        options.SaveTokens = true;
        options.Scope.Clear();
        options.Scope.Add("email");
        options.Scope.Add("openid");
        options.Scope.Add("enterprisebroker");
        options.Events.OnAuthenticationFailed = ctx =>
        {
            ctx.HandleResponse();
            ctx.Response.Redirect("Unauthorized");
            return Task.FromResult(0);
        };
        options.Events.OnRemoteFailure = ctx =>
        {
            ctx.HandleResponse();
            ctx.Response.Redirect("Error");
            return Task.FromResult(0);
        };
    });

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Add Razor Pages
builder.Services.AddRazorPages();

// END: ConfigureServices

// START: Configure
var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Request.Scheme = "https";
    await next.Invoke();
});
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseApiVersionPathMiddleware();
// after version path, before the api key middleware
app.UseHealthChecks("/health");

app.UseAuthKeyMiddleware();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapHealthChecks("/health");
});

app.Run();