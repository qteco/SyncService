using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
using SyncService.Core.Interfaces.ApiClients;

namespace SyncService.ExternalServices.ApiClients;

public class BroadsoftApiClient : IBroadsoftApiClient
{
    private  string BaseUrl { get; set; }
    private  string UserId { get; set; }
    private  string Password { get; set; }
    private readonly HttpClient HttpClient;

    public BroadsoftApiClient()
    {
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{UserId}:{Password}"))
        );
    }

    public async Task<(string CallId, string PhoneNumber)?> GetLatestCallAsync()
    {
        var response = await HttpClient.GetAsync(BaseUrl);

        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();

        if (!content.Contains("queueEntries"))
            return null;

        XDocument doc = XDocument.Parse(content);
        XNamespace xsi = "http://schema.broadsoft.com/xsi";

        var callIdElement = doc.Descendants(xsi + "callId").FirstOrDefault();
        var nameElement = doc.Descendants(xsi + "name").FirstOrDefault();

        if (callIdElement != null && nameElement != null)
        {
            return (callIdElement.Value, nameElement.Value);
        }

        return null;
    }
}