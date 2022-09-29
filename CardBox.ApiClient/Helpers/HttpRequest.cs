using System.Text;
using Newtonsoft.Json;

namespace CardBox.ApiClient.Helpers;

internal class HttpRequestManager
{
    private readonly HttpClient client = new();

    public HttpRequestManager(string url)
    {
        client.DefaultRequestHeaders.Add("URL", url);
    }

    public string Get(object? obj = null)
    {
        string data = JsonConvert.SerializeObject(obj ?? "", Formatting.Indented);

        var response = Request(HttpMethod.Get, data);

        return response.Content.ReadAsStringAsync().Result;
    }

    public HttpResponseMessage Request(HttpMethod method, string data)
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