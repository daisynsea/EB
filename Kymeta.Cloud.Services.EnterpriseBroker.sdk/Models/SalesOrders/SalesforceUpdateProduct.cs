using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public class SalesforceUpdateProduct
    {
        public string OracleItemId { get; set; }
        public string ItemLink { get; set; }
        public string IntegrationStatus { get; set; }
        public string IntegrationError { get; set; }
        public DateTime LastSyncDate { get; set; }
    }
}
