using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TestDemo
{
    [TestClass]
    public  class UserTest
    {
        private HttpClient _httpClient;
        private WebApplicationFactory<Program> webApplicationFactory;
        public UserTest()
        {

            webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("test");
                Environment.SetEnvironmentVariable("test", "test");
            });
        }
        [TestMethod]
        public async Task TestUser()
        {
            _httpClient = webApplicationFactory.CreateDefaultClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer y");
            var response = await _httpClient.GetAsync("api/v1/User");
            Assert.IsTrue((int)response.StatusCode == 401);

            _httpClient = webApplicationFactory.CreateDefaultClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer x");
             response = await _httpClient.GetAsync("api/v1/User");
            Assert.IsTrue((int)response.StatusCode == 200);
        }
    }
}
