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
using System.Xml.Linq;
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
            var organization = Helpers.BuildOracleOrganization();
            var partySites = organization.PartySites;

            // simulate XML response object with sample XML
            XDocument xDoc = XDocument.Load("TestFiles\\create-organization-soap-response.xml");

            _fixture.OracleClient
                .Setup(oc => oc.SendSoapRequest(It.IsAny<string>(), It.IsAny<string>(), null))
                .ReturnsAsync(new Tuple<XDocument, string, string>(xDoc, null, null));
            _fixture.OracleService.Setup(os => os.CreateOrganization(It.IsAny<SalesforceAccountModel>(), It.IsAny<List<OraclePartySite>>(), transaction))
                .ReturnsAsync(new Tuple<OracleOrganization, string>(organization, string.Empty));

            var response = await _oracleService.Object.CreateOrganization(account, partySites, transaction);

            Assert.NotNull(response);
            Assert.NotNull(response.Item1);
            Assert.Equal(string.Empty, response.Item2);
            Assert.Equal("Unit Test Alpha", response.Item1.OrganizationName);
            Assert.Equal("300001", response.Item1.PartyNumber.ToString());
            Assert.Equal("UNIT_TEST_REF", response.Item1.OrigSystemReference);
            Assert.NotNull(response.Item1.PartySites);
            Assert.NotEmpty(response.Item1.PartySites);
            Assert.Equal(1, response.Item1.PartySites.Count());
        }

        [Fact]
        [Trait("Category", "OracleServiceTests")]
        public async void UpdateOrganization_WithValidPayload_ReturnsUpdatedOrganization()
        {
            var account = Helpers.BuildSalesforceAccountModel();
            var transaction = Helpers.BuildSalesforceTransaction();

            var oracleOrganizationResponse = new OracleOrganizationResponse
            {
                PartyNumber = "123",
                PartyId = 456
            };
            _fixture.OracleClient.Setup(x => x.UpdateOrganization(It.IsAny<UpdateOracleOrganizationModel>(), It.IsAny<ulong>()))
                .ReturnsAsync(new Tuple<OracleOrganizationResponse, string>(oracleOrganizationResponse, string.Empty));

            var oracleOrganization = new OracleOrganization();
            var response = await _oracleService.Object.UpdateOrganization(oracleOrganization, account, transaction);

            Assert.NotNull(response);
            Assert.NotNull(response.Item1);
            Assert.Equal(string.Empty, response.Item2);
        }
    }
}
