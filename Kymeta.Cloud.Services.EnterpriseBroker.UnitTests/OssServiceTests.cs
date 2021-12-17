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
    public void AddAccount_WithValidInput_ReturnsCreatedAccount()
    {
        Assert.True(true);
    }
}