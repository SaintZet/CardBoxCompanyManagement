using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure
{
    public class HttpRequest
    {
        private readonly HttpClient client = new();

        public HttpRequest(string url)
        {
            client.DefaultRequestHeaders.Add("URL", url);
        }

        public string Get(object? obj = null)
        {
            string data = JsonConvert.SerializeObject(obj ?? "", Formatting.Indented);
            var response = Request(HttpMethod.Get, data);

            return response.Content.ReadAsStringAsync().Result;
        }

        public void Put(object obj) => Request(HttpMethod.Put, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public void Post(object obj) => Request(HttpMethod.Post, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public void Delete(object obj) => Request(HttpMethod.Delete, JsonConvert.SerializeObject(obj, Formatting.Indented));

        private HttpResponseMessage Request(HttpMethod method, string data)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri("https://club.microinvest.net/CardBoxProxyService"),
                Content = new StringContent(data, Encoding.UTF8, "application/json")
            };

            return client.SendAsync(request).Result;
        }
    }
}