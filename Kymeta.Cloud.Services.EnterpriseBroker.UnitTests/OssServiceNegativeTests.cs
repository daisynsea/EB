using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests;

public class OssNegativeServiceTests : IClassFixture<TestFixture>
{
    private TestFixture _fixture;

    public OssNegativeServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    [Trait("Category", "OssServiceNegativeTests")]
    public async void AddAccount_WithExistingAccount_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync(new Account());
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = await ossService.Object.AddAccount(model, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"Account with Salesforce ID {model.ObjectId} already exists in the system.");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    [Trait("Category", "OssServiceNegativeTests")]
    public async void AddAccount_AddFails_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync((Account?)null);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        _fixture.UsersClient
            .Setup(x => x.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User { Email = "primary@email.com" });
        _fixture.AccountsClient
            .Setup(x => x.AddAccount(It.IsAny<Account>()))
            .ReturnsAsync(new Tuple<Account, string>(null, "Explosions"));

        // Act
        var result = await ossService.Object.AddAccount(model, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"There was an error adding the account to OSS: Explosions");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    [Trait("Category", "OssServiceNegativeTests")]
    public async void AddAccount_AddThrowsAnException_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync((Account?)null);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        _fixture.UsersClient
            .Setup(x => x.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User { Email = "primary@email.com" });
        _fixture.AccountsClient
            .Setup(x => x.AddAccount(It.IsAny<Account>()))
            .ThrowsAsync(new Exception("Explosions"));

        // Act
        var result = await ossService.Object.AddAccount(model, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"There was an error calling the OSS Accounts service: Explosions");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    [Trait("Category", "OssServiceNegativeTests")]
    public async void UpdateAccount_AccountNotFound_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid() };

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync((Account?)null);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = await ossService.Object.UpdateAccount(model, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    [Trait("Category", "OssServiceNegativeTests")]
    public async void UpdateAccount_UpdateFails_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid() };

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync(account);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        _fixture.UsersClient
           .Setup(x => x.GetUserByEmail(It.IsAny<string>()))
           .ReturnsAsync(new User { Email = "primary@email.com" });
        _fixture.AccountsClient
            .Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<Account>()))
            .ReturnsAsync(new Tuple<Account, string>(null, "Explosions"));

        // Act
        var result = await ossService.Object.UpdateAccount(model, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"There was an error updating the account in OSS: Explosions");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    [Trait("Category", "OssServiceNegativeTests")]
    public async void UpdateAccount_UpdateThrowsException_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid() };

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync(account);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        _fixture.UsersClient
           .Setup(x => x.GetUserByEmail(It.IsAny<string>()))
           .ReturnsAsync(new User { Email = "primary@email.com" });
        _fixture.AccountsClient
            .Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<Account>()))
            .ThrowsAsync(new Exception("Explosions"));

        // Act
        var result = await ossService.Object.UpdateAccount(model, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"There was an error calling the OSS Accounts service: Explosions");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public async void UpdateAccountOracleId_AccountNotFound_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid() };
        string newOracleId = "123";

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync((Account?)null);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = await ossService.Object.UpdateAccountOracleId(model, newOracleId, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public async void UpdateAccountOracleId_AccountUpdateFails_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid() };
        string newOracleId = "123";

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync(account);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        _fixture.UsersClient
            .Setup(x => x.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User { Email = "primary@email.com" });
        _fixture.AccountsClient
            .Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<Account>()))
            .ReturnsAsync(new Tuple<Account, string>(null, "Explosions"));

        // Act
        var result = await ossService.Object.UpdateAccountOracleId(model, newOracleId, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"There was an error updating the account oracle Id in OSS: Explosions");
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public async void UpdateAccountOracleId_ThrowsException_ReturnsError()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid() };
        string newOracleId = "123";

        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);
        ossService.CallBase = true;
        ossService.Setup(x => x.GetAccountBySalesforceId(It.IsAny<string>())).ReturnsAsync(account);
        ossService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();
        _fixture.UsersClient
            .Setup(x => x.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User { Email = "primary@email.com" });
        _fixture.AccountsClient
            .Setup(x => x.UpdateAccount(It.IsAny<Guid>(), It.IsAny<Account>()))
            .ThrowsAsync(new Exception("Explosions"));

        // Act
        var result = await ossService.Object.UpdateAccountOracleId(model, newOracleId, transaction);

        // Assert
        Assert.Null(result.Item1);
        Assert.Equal(result.Item2, $"There was an error calling the OSS Accounts service: Explosions");
    }
}