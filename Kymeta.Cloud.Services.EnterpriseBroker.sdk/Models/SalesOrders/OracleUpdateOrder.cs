using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public class OracleUpdateOrder
    {
        public string? OrderKey { get; set; }
        public string PackingInstructions { get; set; }
        public string ShippingInstructions { get; set; }
        public string FOBPointCode { get; set; }

    }
}
