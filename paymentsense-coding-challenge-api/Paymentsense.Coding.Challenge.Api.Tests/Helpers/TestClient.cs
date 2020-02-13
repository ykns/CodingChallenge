using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Paymentsense.Coding.Challenge.Api.Tests.Helpers
{
    public static class TestHarness
    {
        public static TestServer CreateServer()
        {
            var testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            return testServer;
        } 

        public static HttpClient CreateClient()
        {   
            var client = CreateServer().CreateClient();
            return client;
        }
    }
}