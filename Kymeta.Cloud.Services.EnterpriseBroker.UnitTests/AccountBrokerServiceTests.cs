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
    public async void PSA_WithSyncToOSS_NotSyncToOracle_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = new SalesforceActionObject
        {
            ActionType = ActionType.Create,
            SalesforceOriginUri = "https://salesforce.clouddev.test",
            UserName = "Unit McTester",
            ObjectId = "sfc2000118938",
            ObjectType = ActionObjectType.Account,
            ObjectValues = new Dictionary<string, object>
            {
                { "syncToOss", true },
                { "syncToOracle", false },
                { "name", "Unit Test Account" }
            }
        };

        // Mock successful addition of record to the actions repository
        _fixture.ActionsRepository
            .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(true);
        _fixture.ActionsRepository
            .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(true);
        var accountFromOss = new Account { Id = Guid.NewGuid() };
        _fixture.OssService
            .Setup(oss => oss.AddAccount(It.IsAny<SalesforceActionObject>(), It.IsAny<string>()))
            .ReturnsAsync(new Tuple<Account, string>(accountFromOss, string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountCreate(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.ObjectId);
        Assert.Equal(StatusType.Skipped, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.AddedOssAccountId);
        Assert.Null(result.AddedOracleAccountId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_WithSyncToOSS_AndSyncToOracle_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = new SalesforceActionObject
        {
            ActionType = ActionType.Create,
            SalesforceOriginUri = "https://salesforce.clouddev.test",
            UserName = "Unit McTester",
            ObjectId = "sfc2000118938",
            ObjectType = ActionObjectType.Account,
            ObjectValues = new Dictionary<string, object>
            {
                { "syncToOss", true },
                { "syncToOracle", true },
                { "name", "Unit Test Account" }
            }
        };

        // Mock successful addition of record to the actions repository
        _fixture.ActionsRepository
            .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(true);
        _fixture.ActionsRepository
            .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(true);
        var accountFromOss = new Account { Id = Guid.NewGuid() };
        _fixture.OssService
            .Setup(oss => oss.AddAccount(It.IsAny<SalesforceActionObject>(), It.IsAny<string>()))
            .ReturnsAsync(new Tuple<Account, string>(accountFromOss, string.Empty));
        _fixture.OracleService
            .Setup(ors => ors.AddAccount(It.IsAny<SalesforceActionObject>()))
            .ReturnsAsync(new Tuple<string, string>("123", string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountCreate(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.ObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Successful, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal(accountFromOss.Id?.ToString(), result.AddedOssAccountId);
        Assert.Equal("123", result.AddedOracleAccountId);
    }

    [Fact]
    [Trait("Category", "AccountBrokerTests")]
    public async void PSA_NoSyncToOSS_AndSyncToOracle_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = new SalesforceActionObject
        {
            ActionType = ActionType.Create,
            SalesforceOriginUri = "https://salesforce.clouddev.test",
            UserName = "Unit McTester",
            ObjectId = "sfc2000118938",
            ObjectType = ActionObjectType.Account,
            ObjectValues = new Dictionary<string, object>
            {
                { "syncToOss", false },
                { "syncToOracle", true },
                { "name", "Unit Test Account" }
            }
        };

        // Mock successful addition of record to the actions repository
        _fixture.ActionsRepository
            .Setup(ar => ar.InsertActionRecord(It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(true);
        _fixture.ActionsRepository
            .Setup(ar => ar.UpdateActionRecord(It.IsAny<SalesforceActionTransaction>()))
            .ReturnsAsync(true);
        _fixture.OracleService
            .Setup(ors => ors.AddAccount(It.IsAny<SalesforceActionObject>()))
            .ReturnsAsync(new Tuple<string, string>("123", string.Empty));

        // Act
        var svc = new AccountBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object, _fixture.OssService.Object);
        var result = await svc.ProcessAccountCreate(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.ObjectId, result.ObjectId);
        Assert.Equal(StatusType.Successful, result.OracleStatus);
        Assert.Equal(StatusType.Skipped, result.OSSStatus);
        Assert.Null(result.OracleErrorMessage);
        Assert.Null(result.OSSErrorMessage);
        Assert.Equal("123", result.AddedOracleAccountId);
    }
}