using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SyncService.Core.Interfaces.ApiClients;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace SyncService.ExternalServices.ApiClients;

public class IrmaApiClient : IIrmaApiClient
{
    private string Endpoint { get; set; }
    private string Username { get; set; }
    private string Password { get; set; }
    private HttpClient HttpClient { get; set; }

    public IrmaApiClient() 
    {
        Endpoint = "https://service-accept.grexx.today/interfaces/routit/yav58bpfycatxp6i5291/realtime";
        Username = Environment.GetEnvironmentVariable("IrmaUsername");
        Password = Environment.GetEnvironmentVariable("IrmaPassword");
        HttpClient = new HttpClient();
    }
    public async Task GetInvoices()
    {
        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        try
        {
            var xmlPayload = @"<InvoicesRequest_V1 xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                                </InvoicesRequest_V1>";

            var content = new StringContent(xmlPayload, Encoding.UTF8, "application/xml");
            HttpResponseMessage response = await HttpClient.PostAsync(Endpoint, content);
            
            string rContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(rContent);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
        }

    }
}