using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
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

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests;

public class AccountBrokerServiceTests : IClassFixture<TestFixture>
{
    private TestFixture _fixture;

    public AccountBrokerServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_WithSyncToOSS_NotSyncToOracle_ValidModel_Exists_ReturnsSuccess()
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
            .ReturnsAsync(new Tuple<Account, string>(accountFromOss, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Skipped, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Null(result.OracleCustomerAccountId);
        Assert.Null(result.OracleCustomerProfileId);
        Assert.Null(result.OracleOrganizationId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_WithSyncToOSS_NotSyncToOracle_ValidModel_NotExists_ReturnsSuccess()
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
            .ReturnsAsync((Account)null); // Mock a null return!
        // Because the account is not found above, it means we're adding, so mock that here
        _fixture.OssService
            .Setup(oss => oss.AddAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<Account, string>(accountFromOss, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Skipped, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Null(result.OracleCustomerAccountId);
        Assert.Null(result.OracleCustomerProfileId);
        Assert.Null(result.OracleOrganizationId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_WithSyncToOSS_AndSyncToOracle_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(true, false);
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
            .Setup(oss => oss.AddAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<Account, string>(accountFromOss, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, new OracleOrganization(), string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Equal("123", result.OracleOrganizationId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_NoSyncToOSS_AndSyncToOracle_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = Helpers.BuildSalesforceAccountModel(true, true);
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
            .Setup(oss => oss.AddAccount(It.IsAny<SalesforceAccountModel>(), It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(new Tuple<Account, string>(accountFromOss, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.GetOrganizationBySalesforceAccountId(It.IsAny<string>(), It.IsAny<string>(), transaction))
            .ReturnsAsync(new Tuple<bool, OracleOrganization, string>(true, new OracleOrganization(), string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountAction(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.SalesforceObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.OssAccountId);
        Assert.Equal("123", result.OracleOrganizationId);
    }
}