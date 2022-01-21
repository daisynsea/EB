using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
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
    public class OracleServiceNegativeTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;
        private Mock<OracleService> _oracleService;

        public OracleServiceNegativeTests(TestFixture fixture)
        {
            _fixture = fixture;
            _oracleService = new Mock<OracleService>(_fixture.OracleClient.Object, _fixture.Configuration, _fixture.ActionsRepository.Object);
            _oracleService.CallBase = true;
            _oracleService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
                          .Verifiable();
        }

        [Fact]
        [Trait("Category", "OracleServiceTests")]
        [Trait("Category", "OracleServiceNegativeTests")]
        public async void CreateOrganization_ClientError_ReturnsError()
        {
            var account = Helpers.BuildSalesforceAccountModel();
            var transaction = Helpers.BuildSalesforceTransaction();

            var oracleOrganization = new OracleOrganization();
            _fixture.OracleClient.Setup(x => x.CreateOrganization(It.IsAny<CreateOracleOrganizationModel>()))
                .ReturnsAsync(new Tuple<OracleOrganization, string>(null, "Explosions"));

            var response = await _oracleService.Object.CreateOrganization(account, transaction);

            Assert.NotNull(response);
            Assert.Null(response.Item1);
            Assert.Equal($"There was an error adding the account to Oracle: Explosions", response.Item2);
        }
    }
}
