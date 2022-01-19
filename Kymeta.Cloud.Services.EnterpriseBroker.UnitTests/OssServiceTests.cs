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
            .ReturnsAsync(new Tuple<Account, string>(account, string.Empty));

        // Act
        var result = await ossService.Object.UpdateAccount(model, transaction);

        // Assert
        Assert.NotNull(result.Item1);
        Assert.Equal(result.Item2, string.Empty);
    }
}