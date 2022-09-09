using System.Text;
using Newtonsoft.Json;

namespace CardBoxCompanyManagement.Infrastructure;

internal class HttpRequest
{
    private readonly HttpClient client = new();

    public HttpRequest(string url)
    {
        client.DefaultRequestHeaders.Add("URL", url);
    }

    public string Get(object? obj = null)
    {
        string data = JsonConvert.SerializeObject(obj ?? "", Formatting.Indented);

        var response = Request(HttpMethod.Get, data).Result;

        return response.Content.ReadAsStringAsync().Result;
    }

    public bool Put(object obj)
    {
        string body = JsonConvert.SerializeObject(obj, Formatting.Indented);

        var response = Request(HttpMethod.Put, body).Result;

        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public bool Post(object obj)
    {
        string body = JsonConvert.SerializeObject(obj, Formatting.Indented);

        var response = Request(HttpMethod.Post, body).Result;

        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public bool Delete(object obj)
    {
        string body = JsonConvert.SerializeObject(obj, Formatting.Indented);

        var response = Request(HttpMethod.Delete, body).Result;

        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    private async Task<HttpResponseMessage> Request(HttpMethod method, string data)
    {
        var request = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri("https://club.microinvest.net/CardBoxProxyService"),
            Content = new StringContent(data, Encoding.UTF8, "application/json")
        };

        return await client.SendAsync(request);
    }
}