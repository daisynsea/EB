using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;
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
    public class OracleServiceTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;
        private Mock<OracleService> _oracleService;

        public OracleServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
            _oracleService = new Mock<OracleService>(_fixture.OracleClient.Object, _fixture.Configuration, _fixture.ActionsRepository.Object);
            _oracleService.CallBase = true;
            _oracleService.Setup(x => x.LogAction(It.IsAny<SalesforceActionTransaction>(), It.IsAny<SalesforceTransactionAction>(), It.IsAny<ActionObjectType>(), It.IsAny<StatusType>(), It.IsAny<string>(), It.IsAny<string>()))
                          .Verifiable();


        }

        [Fact]
        [Trait("Category", "OracleServiceTests")]
        public async void CreateOrganization_WithValidPayload_ReturnsCreatedOrganization()
        {
            var account = Helpers.BuildSalesforceAccountModel();
            var transaction = Helpers.BuildSalesforceTransaction();

            var oracleOrganization = new OracleOrganization();
            _fixture.OracleClient.Setup(x => x.CreateOrganization(It.IsAny<CreateOracleOrganizationModel>()))
                .ReturnsAsync(new Tuple<OracleOrganization, string>(oracleOrganization, string.Empty));

            var response = await _oracleService.Object.CreateOrganization(account, transaction);

            Assert.NotNull(response);
            Assert.NotNull(response.Item1);
            Assert.Equal(string.Empty, response.Item2);
        }

        [Fact]
        [Trait("Category", "OracleServiceTests")]
        public async void UpdateOrganization_WithValidPayload_ReturnsUpdatedOrganization()
        {
            var account = Helpers.BuildSalesforceAccountModel();
            var transaction = Helpers.BuildSalesforceTransaction();

            var oracleOrganizationResponse = new OracleOrganizationUpdateResponse
            {
                PartyNumber = "123",
                PartyId = 456
            };
            var oracleOrganization = new OracleOrganization();
            _fixture.OracleClient.Setup(x => x.UpdateOrganization(It.IsAny<UpdateOracleOrganizationModel>(), It.IsAny<ulong>()))
                .ReturnsAsync(new Tuple<OracleOrganizationUpdateResponse, string>(oracleOrganizationResponse, string.Empty));

            var response = await _oracleService.Object.UpdateOrganization(oracleOrganization, account, transaction);

            Assert.NotNull(response);
            Assert.NotNull(response.Item1);
            Assert.Equal(string.Empty, response.Item2);
        }
    }
}
