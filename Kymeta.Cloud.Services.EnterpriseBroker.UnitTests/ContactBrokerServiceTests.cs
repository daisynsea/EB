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
    public class ContactBrokerServiceTests : IClassFixture<TestFixture>
    {
        private TestFixture _fixture;

        public ContactBrokerServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("Category", "ContacBrokerTests")]
        [Trait("Category", "ContacBrokerOssTests")]
        public async void SyncToOracle_UpdateContact_ReturnsSuccess()
        {
            // Arrange
            var model = Helpers.BuildSalesforceContactModel(false, true);
            var transaction = Helpers.BuildSalesforceTransaction();
            Helpers.MockActionRepository(_fixture.ActionsRepository, transaction);

            // TODO: Setup Oracle service
            
            // Act
            var svc = new ContactBrokerService(_fixture.ActionsRepository.Object, _fixture.OracleService.Object);
            var result = await svc.ProcessContactAction(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.ObjectId, result.SalesforceObjectId);
            Assert.Equal(StatusType.Successful, result.OracleStatus);
            Assert.Equal(StatusType.Skipped, result.OSSStatus);
            Assert.Null(result.OracleErrorMessage);
            Assert.Null(result.OSSErrorMessage);
        }
    }
}
