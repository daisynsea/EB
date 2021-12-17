using Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;
using Kymeta.Cloud.Services.EnterpriseBroker.Repositories;
using Kymeta.Cloud.Services.EnterpriseBroker.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests
{
    public class TestFixture : IDisposable
    {
        public Mock<IAccountsClient> AccountsClient = new Mock<IAccountsClient>();
        public Mock<IUsersClient> UsersClient = new Mock<IUsersClient>();
        public Mock<IOracleClient> OracleClient = new Mock<IOracleClient>();
        public Mock<IActionsRepository> ActionsRepository = new Mock<IActionsRepository>();
        public Mock<IOssService> OssService = new Mock<IOssService>();
        public Mock<IOracleService> OracleService = new Mock<IOracleService>();

        public void Dispose()
        {
            // Cleanup here
        }
    }
}
