using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestDemo
{
    [TestClass]
    public class UnitTest1
    {
        private HttpClient _httpClient;

        public UnitTest1()
        {

            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("test");
                Environment.SetEnvironmentVariable("test", "test");
            });

            _httpClient = application.CreateDefaultClient();
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}