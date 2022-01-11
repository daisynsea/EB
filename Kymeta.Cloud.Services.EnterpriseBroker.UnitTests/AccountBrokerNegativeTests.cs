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
        public async void PSA_WithSyncToOSS_NotSyncToOracle_ValidModel_Exists_UpdateFailed_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(false, true);
            var transaction = Helpers.BuildSalesforceTransaction();

            // Mock successful addition of record to the actions repository
            _fixture.ActionsRepository
                .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
            _fixture.ActionsRepository
                .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
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
        public async void PSA_WithSyncToOSS_NotSyncToOracle_ValidModel_NotExists_AddFailed_ReturnsError()
        {
            // Arrange
            var model = Helpers.BuildSalesforceAccountModel(false, true);
            var transaction = Helpers.BuildSalesforceTransaction();

            // Mock successful addition of record to the actions repository
            _fixture.ActionsRepository
                .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
            _fixture.ActionsRepository
                .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
                .ReturnsAsync(transaction);
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
    }
}
