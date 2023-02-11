using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests
{
    internal class TestApplication
    {
        private static bool _initialized = false;
        private static object _lock = new object();

        private static WebApplicationFactory<Program> _host = null!;

        public static void StartHost()
        {
            lock (_lock)
            {
                if (_initialized) return;


                _host = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.UseEnvironment("Development");

                    });

                _initialized = true;
            }
        }

        public static T GetRequiredService<T>() where T : notnull
        {
            StartHost();
            return _host.Services.GetRequiredService<T>();
        }

    }
}
