using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
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
                BusinessUnit = "Commercial",
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

        public static SalesforceAddressModel BuildSalesforceAddressModel(bool syncOracle = true, bool syncOss = true)
        {
            return new SalesforceAddressModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "add30001",
                ParentAccountId = "acc30001",
                ParentAccountBusinessUnit = "Commercial",
                City = "Redmond",
                Country = "US",
                PostalCode = "98052",
                StateProvince = "WA",
                SiteName = "ShipStuffHereLol",
                Type = "Billing & Shipping",
                Address = $"12345 SE 100th Street{Environment.NewLine}Ste 100",
                Address1 = "12345 SE 100th Street"
            };
        }

        public static SalesforceContactModel BuildSalesforceContactModel(bool syncOracle = true, bool syncOss = true)
        {
            return new SalesforceContactModel
            {
                EnterpriseOriginUri = "https://salesforce.clouddev.test",
                SyncToOracle = syncOracle,
                SyncToOss = syncOss,
                UserName = "Unit McTester",
                ObjectId = "con30001",
                Email = "primary@company.com",
                Name = "Primary Contact",
                IsPrimary = true,
                ParentAccountId = "acc30001",
                Role = "Bill To Contact"
            };
        }

        public static SalesforceActionTransaction BuildSalesforceTransaction()
        {
            return new SalesforceActionTransaction
            {
                TransactionLog = new List<SalesforceActionRecord>()
            };
        }

        public static OracleOrganization BuildOracleOrganization()
        {
            return new OracleOrganization
            {
                PartyId = 001,
                PartyNumber = 30001,
                OrigSystemReference = "acc30001",
                PartySites = new List<OraclePartySite>
                {
                    new OraclePartySite
                    {
                        OrigSystemReference = "add30001",
                        LocationId = 30001,
                        SiteUses = new List<OraclePartySiteUse>
                        {
                            new OraclePartySiteUse
                            {
                                PartySiteUseId = 20001,
                                SiteUseType = "Shipping"
                            }
                        }
                    },
                    new OraclePartySite
                    {
                        OrigSystemReference = "add30002",
                        LocationId = 30002
                    }
                },
                Contacts = new List<OracleOrganizationContact>
                {
                    new OracleOrganizationContact
                    {
                        OrigSystemReference = "con30001",
                        ContactPartyId = 30001
                    }
                }
            };
        }
        public static OracleCustomerAccount BuildOracleCustomerAccount()
        {
            return new OracleCustomerAccount
            {
                CustomerAccountId = 30001,
                PartyId = 30001,
                OrigSystemReference = "acc30001",
                Contacts = new List<OracleCustomerAccountContact>()
            };
        }
    }
}
