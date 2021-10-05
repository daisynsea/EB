using Kymeta.Cloud.Services.EnterpriseBroker.HttpClients;
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

        public void Dispose()
        {
            // Cleanup here
        }
    }
}
