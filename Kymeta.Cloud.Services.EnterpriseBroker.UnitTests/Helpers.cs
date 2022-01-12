using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Repositories;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class Helpers
    {
        public static void MockActionRepository(Mock<IActionsRepository> repository, SalesforceActionTransaction transaction)
        {
            // Mock successful addition of record to the actions repository
            repository
                .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
            repository
                .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
        }

        public static SalesforceAccountModel BuildSalesforceAccountModel(bool syncOracle = true, bool syncOss = true, string name = "Trident Ltd")
        {
            return new SalesforceAccountModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "acc30001",
                AccountType = "Marine",
                Name = name,
                SubType = "Coast Goard",
                TaxId = "1004381",
                Addresses = new List<SalesforceAddressModel>
                {
                    new SalesforceAddressModel
                    {
                        ObjectId = "add30001",
                        Type = "Billing & Shipping",
                        ParentAccountId = "acc30001"
                    },
                    new SalesforceAddressModel
                    {
                        ObjectId = "add30002",
                        Type = "Shipping",
                        ParentAccountId = "acc30001"
                    }
                },
                Contacts = new List<SalesforceContactModel>
                {
                    new SalesforceContactModel
                    {
                        ObjectId = "con30001",
                        Email = "primary@company.com",
                        Name = "Primary Contact",
                        IsPrimary = true,
                        ParentAccountId = "acc30001",
                        Role = "Bill To Contact"
                    }
                }
            };
        }

        public static SalesforceActionTransaction BuildSalesforceTransaction()
        {
            return new SalesforceActionTransaction();
        }
    }
}
