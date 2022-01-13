﻿using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class AccountBrokerNegativeTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;

        public AccountBrokerNegativeTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOssTests")]
        public async void PSA_SyncToOSS_ValidModel_Exists_UpdateFailed_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(false, true);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            var accountFromOss = new Account { Id = Guid.NewGuid() };
            _fixture.OssService
                .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
                .ReturnsAsync(accountFromOss);
            // Because the account is found above, we're doing an update
            _fixture.OssService
                .Setup(oss => oss.UpdateAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(new Tuple<Account, string>(null, $"Error when updating"));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Skipped, result.OracleStatus);
            Assert.Equal(StatusType.Error, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Equal("Error when updating", result.OSSErrorMessage);
            Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
            Assert.Null(result.OracleCustomerAccountId);
            Assert.Null(result.OracleCustomerProfileId);
            Assert.Null(result.OracleOrganizationId);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOssTests")]
        public async void PSA_SyncToOSS_ValidModel_NotExists_AddFailed_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(false, true);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            _fixture.OssService
                .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
                .ReturnsAsync((Account)null);
            _fixture.OssService
                .Setup(oss => oss.AddAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(new Tuple<Account, string>(null, $"Error when adding"));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Skipped, result.OracleStatus);
            Assert.Equal(StatusType.Error, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Equal("Error when adding", result.OSSErrorMessage);
            Assert.Null(result.OssAccountId);
            Assert.Null(result.OracleCustomerAccountId);
            Assert.Null(result.OracleCustomerProfileId);
            Assert.Null(result.OracleOrganizationId);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOracleTests")]
        [Trait("Category", "AccountBrokerOracleOrganizationTests")]
        public async void PSA_SyncToOracle_ValidModel_OrganizationFindFailure_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // Mock Oracle portion of the request
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(false, null, $"There was an error finding the Organization in Oracle: Epic Fail."));      

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"There was an error finding the Organization in Oracle: Epic Fail.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
            Assert.Null(result.OssAccountId);
            Assert.Null(result.OracleOrganizationId);
            Assert.Null(result.OracleCustomerProfileId);
            Assert.Null(result.OracleCustomerAccountId);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOracleTests")]
        [Trait("Category", "AccountBrokerOracleOrganizationTests")]
        public async void PSA_SyncToOracle_ValidModel_UpdateOrganizationFailure_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // Mock Oracle portion of the request
            var oracleOrg = new OracleOrganization
            {
                PartyId = 001,
                PartyNumber = "30001",
                OrigSystemReference = "acc30001",
                PartySites = new List<OraclePartySite>
                {
                    new OraclePartySite
                    {
                        OrigSystemReference = "add30001",
                        LocationId = 30001
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
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, oracleOrg, string.Empty));
            _fixture.OracleService
                .Setup(ors => ors.UpdateOrganization(It.IsAny<OracleOrganization>(), It.IsAny<SalesforceAccountModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleOrganization, string>(null, "There was an error updating the account in Oracle: Epic Fail"));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"There was an error updating the account in Oracle: Epic Fail", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
            Assert.Null(result.OssAccountId);
            Assert.Null(result.OracleOrganizationId);
            Assert.Null(result.OracleCustomerProfileId);
            Assert.Null(result.OracleCustomerAccountId);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOracleTests")]
        [Trait("Category", "AccountBrokerOracleOrganizationTests")]
        public async void PSA_SyncToOracle_ValidModel_CreateOrganizationFailure_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // Mock Oracle portion of the request
            var oracleOrg = new OracleOrganization
            {
                PartyId = 001,
                PartyNumber = "30001",
                OrigSystemReference = "acc30001",
                PartySites = new List<OraclePartySite>
                {
                    new OraclePartySite
                    {
                        OrigSystemReference = "add30001",
                        LocationId = 30001
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
            // returns null here, means create
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, null, string.Empty));
            _fixture.OracleService
                .Setup(ors => ors.CreateOrganization(It.IsAny<SalesforceAccountModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleOrganization, string>(null, "There was an error adding the account to Oracle: Epic Fail"));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"There was an error adding the account to Oracle: Epic Fail", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
            Assert.Null(result.OssAccountId);
            Assert.Null(result.OracleOrganizationId);
            Assert.Null(result.OracleCustomerProfileId);
            Assert.Null(result.OracleCustomerAccountId);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOracleTests")]
        [Trait("Category", "AccountBrokerOracleLocationsTests")]
        public async void PSA_SyncToOracle_ValidModel_GetLocationsFailure_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // Mock Oracle portion of the request
            var oracleOrg = new OracleOrganization
            {
                PartyId = 001,
                PartyNumber = "30001",
                OrigSystemReference = "acc30001",
                PartySites = new List<OraclePartySite>
                {
                    new OraclePartySite
                    {
                        OrigSystemReference = "add30001",
                        LocationId = 30001
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
            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
                .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, null, string.Empty));
            _fixture.OracleService
                .Setup(ors => ors.CreateOrganization(It.IsAny<SalesforceAccountModel>(), transaction))
                .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrg, string.Empty));
            _fixture.OracleService
                .Setup(ors => ors.GetLocationsBySalesforceAddressId(It.IsAny<List<string>>(), transaction))
                .ReturnsAsync(new Tuple<bool, IEnumerable<OracleLocationModel>, string>(false, null, $"There was an error finding the Locations in Oracle: Epic Fail."));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Equal($"There was an error finding the Locations in Oracle: Epic Fail.", result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
            Assert.Null(result.OssAccountId);
            Assert.Equal("30001", result.OracleOrganizationId); // we created the org, so we should have the Id for it
            Assert.Null(result.OracleCustomerProfileId);
            Assert.Null(result.OracleCustomerAccountId);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOssTests")]
        public async void PSA_SyncToOss_OssThrowsAnException_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(false, true);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            var accountFromOss = new Account { Id = Guid.NewGuid() };
            _fixture.OssService
                .Setup(oss => oss.GetAccountBySalesforceId(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Explosions!"));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Skipped, result.OracleStatus);
            Assert.Equal(StatusType.Error, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Equal("Error syncing to OSS due to an exception: Explosions!", result.OSSErrorMessage);
        }

        [Fact]
        [Trait("Category", "AccountBrokerTests")]
        [Trait("Category", "AccountBrokerNegativeTests")]
        [Trait("Category", "AccountBrokerOracleTests")]
        public async void PSA_SyncToOracle_OracleThrowsAnException_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(true, false);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            _fixture.OracleService
                .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
                .ThrowsAsync(new Exception("Explosions!"));

            // Act
            var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
            var result = await svc.ProcessAccountAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Error, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Null(result.OSSErrorMessage);
            Assert.Equal("Error syncing to Oracle due to an exception: Explosions!", result.OracleErrorMessage);
        }
    }
}
