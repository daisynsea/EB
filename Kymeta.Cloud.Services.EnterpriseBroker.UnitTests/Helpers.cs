using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class Helpers
    {
        public static SalesforceAccountModel BuildSalesforceAccountModel(bool syncOracle = true, bool syncOss = true, string name = "Trident Ltd")
        {
            return new SalesforceAccountModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "sfc2000118938",
                AccountType = "Marine",
                Name = name,
                SubType = "Coast Goard",
                TaxId = "1004381"
            };
        }

        public static SalesforceActionTransaction BuildSalesforceTransaction()
        {
            return new SalesforceActionTransaction();
        }
    }
}
