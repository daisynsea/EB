using Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests;

public class OssServiceTests : IClassFixture<TestFixture>
{
    private TestFixture _fixture;

    public OssServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public async void AddAccount_WithValidInput_ReturnsCreatedAccount()
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
            .ReturnsAsync(new Tuple<Account, string>(new Account(), string.Empty));

        // Act
        var result = await ossService.Object.AddAccount(model, transaction);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(result.Item2, string.Empty);
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public async void UpdateAccount_WithValidInput_ReturnsUpdatedAccount()
    {
        var model = Helpers.BuildSalesforceAccountModel();
        model.ParentId = "abc123";
        var transaction = Helpers.BuildSalesforceTransaction();
        var account = new Account { Id = Guid.NewGuid(), ParentId = Guid.NewGuid() };

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
            .ReturnsAsync(new Tuple<Account, string>(account, string.Empty));

        // Act
        var result = await ossService.Object.UpdateAccount(model, transaction);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(result.Item2, string.Empty);
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public async void UpdateAccountOracleId_WithValidInput_ReturnsUpdatedAccount()
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
            .ReturnsAsync(new Tuple<Account, string>(account, string.Empty));

        // Act
        var result = await ossService.Object.UpdateAccountOracleId(model, newOracleId, transaction);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(result.Item2, string.Empty);
    }


    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public void GetDiscountTierFromSalesforceAPIValue_ReturnsCorrectValue_WithCorrectInput()
    {
        // Arrange
        string validInput = "Tier 3(101-250)";
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);

        // Act
        var result = ossService.Object.GetDiscountTierFromSalesforceAPIValue(validInput);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result);
    }

    [Fact]
    [Trait("Category", "OssTests")]
    [Trait("Category", "OssServiceTests")]
    public void GetDiscountTierFromSalesforceAPIValue_Returns1_WithInvalidInput()
    {
        // Arrange
        var ossService = new Mock<OssService>(_fixture.Configuration, _fixture.AccountsClient.Object, _fixture.UsersClient.Object, _fixture.ActionsRepository.Object);

        // Act
        var result1 = ossService.Object.GetDiscountTierFromSalesforceAPIValue("Tier 3)101-250)");
        var result2 = ossService.Object.GetDiscountTierFromSalesforceAPIValue(null);
        var result3 = ossService.Object.GetDiscountTierFromSalesforceAPIValue("abcdefg");
        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.NotNull(result3);

        Assert.Equal(1, result1);
        Assert.Equal(1, result2);
        Assert.Equal(1, result3);
    }
}