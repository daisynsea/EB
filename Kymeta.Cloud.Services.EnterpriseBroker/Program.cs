using Kymeta.Cloud.Commons.AspNet.ApiVersion;
using Kymeta.Cloud.Commons.AspNet.DistributedConfig;
using Kymeta.Cloud.Commons.AspNet.Health;
using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Logging;
using Kymeta.Cloud.Services.EnterpriseBroker;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using System.Text.Json.Serialization;

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
builder.Services.AddCosmosDb(builder.Configuration.GetConnectionString("AzureCosmosDB"));
builder.Services.AddScoped<IActionsRepository, ActionsRepository>();
builder.Services.AddScoped<IOssService, OssService>();
builder.Services.AddScoped<IAccountBrokerService, AccountBrokerService>();
builder.Services.AddScoped<IAddressBrokerService, AddressBrokerService>();
builder.Services.AddScoped<IContactBrokerService, ContactBrokerService>();
builder.Services.AddScoped<IOracleService, OracleService>();
builder.Services.AddScoped<ISalesforceProductsRepository, SalesforceProductsRepository>();
builder.Services.AddRedisClient(new RedisClientOptions
{
    ConnectionString = builder.Configuration.GetConnectionString("RedisCache")
});
builder.Services.AddHttpClient<ISalesforceClient, SalesforceClient>();

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
                    options.Authority = builder.Configuration["Api:ApiAccess"] ?? "https://access.kymeta.io";
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
// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();
app.UseApiVersionPathMiddleware();
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