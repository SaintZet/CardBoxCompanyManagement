using CardBox.ApiClient.Constants;
using CardBox.ApiClient.Contracts;
using CardBox.ApiClient.Helpers;
using CardBox.ApiClient.Models;
using Newtonsoft.Json;

namespace CardBox.ApiClient.Services;

public class CompaniesService : ICompaniesService
{
    private readonly HttpRequestManager _defaultHttpManager = new(CompanyRequests.Company);
    
    public async Task<List<Company>> GetCompaniesAsync()
    {
        HttpRequestManager specificHttpManager = new(CompanyRequests.Companies);
        var response = specificHttpManager.Request(HttpMethod.Get, string.Empty);
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, Company>>(responseBody)!.Select(item => item.Value).ToList();
    }

    public bool Delete(Company company)
    {
        var body = JsonConvert.SerializeObject(company, Formatting.Indented);
        var response = _defaultHttpManager.Request(HttpMethod.Delete, body);
        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public bool Post(Company company)
    {
        var body = JsonConvert.SerializeObject(company, Formatting.Indented);
        var response = _defaultHttpManager.Request(HttpMethod.Post, body);
        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }

    public bool Put(Company company)
    {
        var body = JsonConvert.SerializeObject(company, Formatting.Indented);
        var response = _defaultHttpManager.Request(HttpMethod.Put, body);
        return response.StatusCode == System.Net.HttpStatusCode.OK;
    }
}