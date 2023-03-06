﻿using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

public class SalesforceClient2
{
    public SalesforceClient2(HttpClient client, ILoggerFactory loggerFactory)
    {
        client.NotNull();
        loggerFactory.NotNull();

        Events = new SalesforceEventApi(client, loggerFactory.CreateLogger<SalesforceEventApi>());
        Invoice = new SalesforceInvoiceApi(client, loggerFactory.CreateLogger<SalesforceInvoiceApi>());
    }

    public SalesforceEventApi Events { get; }
    public SalesforceInvoiceApi Invoice { get; }
}
