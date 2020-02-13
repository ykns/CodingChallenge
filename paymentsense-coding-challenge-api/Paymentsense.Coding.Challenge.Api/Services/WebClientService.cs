using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Extensions;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public interface IWebClientService
    {
        Task<HttpResponseMessage> GetResponseAsync(string requestUri);
    }

    public class WebClientService : IWebClientService
    {
        private readonly IHttpClientFactory httpClientFactory;
        public WebClientService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetResponseAsync(string requestUri)
        {
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue ("application/json"));
            
            var response = await client.GetAsync(requestUri);
            return response;
        }
    }
}