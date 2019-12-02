using OutboundAdapter.Interfaces.PmsClients;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SiloHost.Clients
{
    public class OperaFactory
    {
        public const string OperaUrl = "https://ove-osb.microsdc.us:9015";

        public static Task<string> ProcessRequest(string content)
        {
            var random = new Random();
            var randomTimeToProcess = random.Next(1, 1000);

            System.Threading.Thread.Sleep(randomTimeToProcess);
            //Task.Delay(randomTimeToProcess);

            return Task.FromResult(content);
        }
    }

    public class OperaHTNG2008BServiceClient : IOperaHTNG2008BServiceClient
    {
        private readonly HttpClient _client;
        private const string endpoint = "/OPERA9OSB/opera/OperaHTNG2008B/HTNG2008BService";

        public OperaHTNG2008BServiceClient()
        {
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(OperaFactory.OperaUrl);
            //client.DefaultRequestHeaders.Add("Content-Type", "text/xml");

            //_client = client;
        }

        async Task<string> IOperaHTNG2008BServiceClient.HTNG2008BService(string content)
        {
            return await OperaFactory.ProcessRequest(content);

            //var response = await _client.PostAsync(endpoint, new StringContent(content));

            //response.EnsureSuccessStatusCode();

            //return await response.Content.ReadAsStringAsync();
        }
    }

    public class OperaHTNG_EXT2008BWebServicesClient : IOperaHTNG_EXT2008BWebServicesClient
    {
        private readonly HttpClient _client;
        private const string Endpoint = "/OPERA9OSB/opera/OperaHTNG_EXT2008BWebServices";

        public OperaHTNG_EXT2008BWebServicesClient()
        {
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(OperaFactory.OperaUrl);
            //client.DefaultRequestHeaders.Add("Content-Type", "text/xml");

            //_client = client;
        }

        async Task<string> IOperaHTNG_EXT2008BWebServicesClient.OperaHTNG_EXT2008BWebServices(string content)
        {
            return await OperaFactory.ProcessRequest(content);

            //var response = await _client.PostAsync(Endpoint, new StringContent(content));

            //response.EnsureSuccessStatusCode();

            //return await response.Content.ReadAsStringAsync();
            ////return await JsonSerializer.DeserializeAsync<ResponseObject>(responseStream);
        }
    }
}
