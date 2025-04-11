using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using SyncService.Core.Interfaces.ApiClients;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SyncService.Core.Classes;
using SyncService.Core.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SyncService.ExternalServices.ApiClients;

public class ExactApiClient : IExactApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache; 
    private string BaseUrl {get; set;}
    private string Endpoint { get; set; } 
    private string AccessToken {get; set;}
    private string RequestUrl { get; set; }
    public ExactApiClient(IMemoryCache cache, HttpClient httpClient)
    {
        BaseUrl = "https://start.exactonline.nl/";
        _cache = cache;
        _httpClient = httpClient;
    }
    public async Task<string> GetAccessToken()
    {
        string accessToken = _cache.Get("AccessToken")?.ToString();
        string refreshToken = _cache.Get("RefreshToken")?.ToString();

        if (string.IsNullOrEmpty(accessToken))
        {
            string requestUrl = $"https://localhost:7198/api/Auth/refreshToken?refreshToken={refreshToken}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    _cache.Set("AccessToken", tokenResponse.Access_Token, TimeSpan.FromSeconds(int.Parse(tokenResponse.Expires_In)));
                    _cache.Set("RefreshToken", tokenResponse.Refresh_Token);

                    Console.WriteLine($"New access token obtained: {tokenResponse.Access_Token}");
                    return tokenResponse.Access_Token;
                }
                else
                {
                    Console.WriteLine($"Failed to refresh token: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        Console.WriteLine($"Returning existing access token: {accessToken}");
        return accessToken;
    }
    public async Task<List<ExactClient>> GetAccountGuids() 
    { 
        var accountClients = new List<ExactClient>(); 
        string endpoint = "api/v1/4046101/crm/Accounts";
        string requestUrl = BaseUrl + endpoint;
        string accessToken = await GetAccessToken();

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            while (!string.IsNullOrEmpty(requestUrl))
            {
                var response = await _httpClient.GetAsync(requestUrl);
                var responseData = await response.Content.ReadAsStringAsync();

                XDocument xmlDoc = XDocument.Parse(responseData);
                XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
                XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                var entries = xmlDoc.Descendants(XName.Get("entry", "http://www.w3.org/2005/Atom"));

                foreach (var entry in entries)
                {
                    var idElement = entry.Descendants(d + "ID").FirstOrDefault();
                    var nameElement = entry.Descendants(d + "Name").FirstOrDefault();
                    var codeElement = entry.Descendants(d + "Code").FirstOrDefault();

                    if (idElement != null && nameElement != null && codeElement != null)
                    {
                        accountClients.Add(new ExactClient
                        {
                            Id = idElement.Value.Trim(),
                            Name = nameElement.Value.Trim(),
                            Code = codeElement.Value.Trim()
                        });

                    }
                }

                var nextLinkElement = xmlDoc.Descendants(XName.Get("link", "http://www.w3.org/2005/Atom"))
                    .FirstOrDefault(l => (string)l.Attribute("rel") == "next");

                if (nextLinkElement != null)
                {
                    requestUrl = nextLinkElement.Attribute("href")?.Value;
                    if (!requestUrl.StartsWith("http"))
                    {
                        requestUrl = BaseUrl.TrimEnd('/') + "/" + requestUrl.TrimStart('/');
                    }
                }
                else
                {
                    requestUrl = null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return accountClients;
    }


    public async Task<ExactClientDTO> GetClientCode(string guid)
    {
        Endpoint = "api/v1/4046101/crm/Accounts";
        RequestUrl = $"{BaseUrl}{Endpoint}?$filter=ID eq guid'{guid}'&$select=Code";
        AccessToken = await GetAccessToken();

        try
        { 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUrl);
            response.EnsureSuccessStatusCode();

            string responseData = await response.Content.ReadAsStringAsync();

            XDocument xmlDoc = XDocument.Parse(responseData);
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            string code = xmlDoc.Descendants(m + "properties")
                .Elements(d + "Code")
                .FirstOrDefault()?.Value.Trim();

            return new ExactClientDTO { Code = code}; 
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching client", ex);
        }
    }
    public async Task<HttpResponseMessage> PostClientAsync(ExactClientDTO newClient)
    {
        string endpoint = "api/v1/4046101/crm/Accounts";
        string accessToken = await GetAccessToken();
        string requestUrl = BaseUrl + endpoint;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var payload = new 
        {
            Code = newClient.Code,
            Name = newClient.Name,
            AddressLine1 = newClient.AddressLine1,
            City = newClient.City,
            Postcode = newClient.Postcode,
            Status = "C",
            Website = newClient.Website,
        };
        
        string jsonString = JsonConvert.SerializeObject(payload);

        Console.WriteLine("JSON Payload: \n" + jsonString);

        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);
        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine("Response Body: " + responseContent);
        Console.WriteLine(JsonConvert.SerializeObject(newClient, Formatting.Indented));

        return response;
    }
    public async Task<HttpResponseMessage> PutClientAsync(ExactClientDTO updatedClient)
    {
        string endpoint = $"api/v1/4046101/crm/Accounts(guid'{updatedClient.Id}')";
        string accessToken = await GetAccessToken();
        string requestUrl = BaseUrl + endpoint;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var payload = new 
        {
            Code = updatedClient.Code,
            Name = updatedClient.Name,
            AddressLine1 = updatedClient.AddressLine1,
            City = updatedClient.City,
            Postcode = updatedClient.Postcode,
            Status = "C",
            Website = updatedClient.Website,
        };

        string jsonString = JsonConvert.SerializeObject(payload);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(requestUrl, content);
        return response;
    }
}


