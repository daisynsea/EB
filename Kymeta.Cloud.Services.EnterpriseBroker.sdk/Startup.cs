using System.Net.Http.Headers;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk;

public static class Startup
{
    public static readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    static Startup()
    {
        _retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .RetryAsync(5);
    }

    public static IServiceCollection AddEnterpriseBrokerServices(this IServiceCollection services)
    {
        services.NotNull();

        services.AddTransient<SalesforceAccessTokenHandler>();

        services.AddSingleton<ReplayIdStoreService>();
        services.AddSingleton<SalesforceClient2>();

        services.AddOrchestrationServices(builder =>
        {
            builder.AddTaskOrchestrations<SalesOrderOrchestration>();
            builder.AddTaskActivities<GetOracleSalesOrderActivity>();
            builder.AddTaskActivities<GetSalesOrderLinesActivity>();
            builder.AddTaskActivities<UpdateOracleSalesOrderActivity>();
            builder.AddTaskActivities<SetSalesOrderWithOracleActivity>();

            builder.AddTaskOrchestrations<TestOrchestration>();
            builder.AddTaskActivities<Step2_TestActivity>();
            builder.AddTaskActivities<Step3_TestActivity>();
            builder.AddTaskActivities<Step4_TestActivity>();

            builder.MapChannel((services, map) =>
            {
                ServiceOption option = services.GetRequiredService<ServiceOption>();

                map.Map<SalesOrderOrchestration>(option.Salesforce.PlatformEvents.Channels.NeoApproveOrder);
//                map.Map<InvoiceCreateOrchestration>(option.Salesforce.PlatformEvents.Channels.NeoInvoicePosted);
                map.Map<TestOrchestration>("testChannel");
            });
        });

        services.AddHttpClient<SalesforceAuthClient>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();
            httpClient.BaseAddress = new Uri(option.Salesforce.LoginEndpoint);
        })
        .AddPolicyHandler(_retryPolicy);

        services.AddHttpClient<IOracleRestClient,OracleRestClient>();

        services.AddHttpClient<SalesforceClient2>((services, httpClient) =>
        {
            var option = services.GetRequiredService<ServiceOption>();
            var authClient = services.GetRequiredService<SalesforceAuthClient>();

            SalesforceAuthenticationResponse authDetails = authClient.GetAuthToken(CancellationToken.None).Result.NotNull();
            httpClient.BaseAddress = new Uri(authDetails.InstanceUrl + option.Salesforce.BasePath);
        })
        .AddPolicyHandler(_retryPolicy)
        .AddHttpMessageHandler<SalesforceAccessTokenHandler>();


        return services;
    }
}
