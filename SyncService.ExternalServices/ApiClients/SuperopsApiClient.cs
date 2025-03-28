using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using SyncService.Core.Interfaces;
using SyncService.Core.Interfaces.ApiClients;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Models;

namespace SyncService.ExternalServices.ApiClients;
public class SuperopsApiClient : ISuperopsApiClient
{
    private List<Client> Clients { get; set; }
    private List<ClientSite> ClientSites { get; set; }
    private GraphQLHttpClient GraphQlClient { get; }
    private string _apiToken { get; }
    
    private readonly string _uri = "https://api.superops.ai/msp";

    private readonly string _subDomain = "qtecobv";

    public SuperopsApiClient()
    {
        GraphQlClient = new GraphQLHttpClient(_uri, new NewtonsoftJsonSerializer());
        GraphQlClient.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("SuperopsApiToken"));
        GraphQlClient.HttpClient.DefaultRequestHeaders.Add("CustomerSubDomain", _subDomain);
    }
    public async Task<List<Client>> GetClientListAsync()
    {
        var request = new GraphQLRequest
        {
            Query = @"query getClientList($input: ListInfoInput!) {
                    getClientList(input: $input) {
                     clients {
                            ...ClientFragment } 
                    }}

                    fragment ClientFragment on Client {
                        accountId
                        name
                        stage
                        status
                        emailDomains
                        accountManager
                        primaryContact
                        secondaryContact
                        hqSite
                        technicianGroups
                        customFields
                    }",

            Variables = new
            {
                input = new
                {
                    page = 1,
                    pageSize = 1000
                }
            }
        };

        try
        {
            var response = await GraphQlClient.SendQueryAsync<dynamic>(request);
            var clientsJson = JsonConvert.SerializeObject(response.Data.getClientList.clients);
            Clients = JsonConvert.DeserializeObject<List<Client>>(clientsJson);

            return Clients;
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return Clients;
        }
        
    }

    public async Task<List<ClientSite>> GetClientSiteDataAsync(string clientId)
    {
        var query = @"query getClientSites($input: GetClientSiteListInput!) {
                getClientSiteList(input: $input) {
                    sites {
                        ...ClientSiteFragment}
                    }}

                fragment ClientSiteFragment on ClientSite {
                    id
                    name
                    line1
                    stateCode
                    city
                    postalCode
                    timezoneCode
                    line2
                    line3
                    businessHour {
                        day
                        start
                        end
                    }
                    timezoneCode
                    working24x7
                }";

        var request = new GraphQLRequest
        {
            Query = query,
            Variables = new
            {
                input = new
                {
                    clientId
                }
            }
        };

        try
        {
            var response = await GraphQlClient.SendQueryAsync<dynamic>(request);
            if (response.Data?.getClientSiteList?.sites != null)
            {
                var clientsJson = JsonConvert.SerializeObject(response.Data.getClientSiteList.sites);
                ClientSites = JsonConvert.DeserializeObject<List<ClientSite>>(clientsJson);
            }
            else
            {
                Console.WriteLine("No site data returned.");
            }

            if (response.Errors != null)
            {
                Console.WriteLine($"Failed to connect to API. Errors: {response.Errors}");
                foreach (var error in response.Errors)
                {
                    Console.WriteLine(error.Message);
                }
            }

            //System.Console.WriteLine(response.Data);

            return ClientSites;
        }
        
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return ClientSites;
        }
    }
}
