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
    private List<Client> Services { get; set; }
    private List<ClientSite> ClientSites { get; set; }
    private GraphQLHttpClient GraphQlClient { get; }
    private string _apiToken { get; }

    private readonly string _uri = "https://api.superops.ai/msp";

    private readonly string _subDomain = "qtecobv";

    public SuperopsApiClient()
    {
        GraphQlClient = new GraphQLHttpClient(_uri, new NewtonsoftJsonSerializer());
        GraphQlClient.HttpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                Environment.GetEnvironmentVariable("SuperopsApiToken"));
        GraphQlClient.HttpClient.DefaultRequestHeaders.Add("CustomerSubDomain", _subDomain);
    }

    //Gets the clients from the RMM platform -> used to sync to database.
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
            Services = JsonConvert.DeserializeObject<List<Client>>(clientsJson);

            return Services;
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return Services;
        }

    }

    //Gets the clientSite data from the RMM platform
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

    public async Task<ServiceItem> GetServiceItems(string id)
    {
        ServiceItem service = new ServiceItem();
        var query = @"query getServiceItem($input: ServiceItemIdentifierInput!) {
                    getServiceItem(input: $input) {
                        itemId
                        name
                        description
                        category {
                            ...ServiceCategoryFragment
                        }
                    }
                }";

        var request = new GraphQLRequest
        {
            Query = query,
            Variables = new
            {
                input = new
                {
                    ServiceItemIdentifierInput = id
                }
            }
        };

        try
        {
            var response = await GraphQlClient.SendQueryAsync<dynamic>(request);
            var serviceData = response.Data?.getServiceItem;

            if (serviceData != null)
            {
                var serviceJson = JsonConvert.SerializeObject(serviceData);
                service = JsonConvert.DeserializeObject<ServiceItem>(serviceJson);
                Console.WriteLine($"Deserialized ServiceItem: {serviceJson}");
                Console.WriteLine($"Deserialized ServiceItem: {service}");

            }
            else
            {
                Console.WriteLine("getServiceItem is null in response.");
            }

            return service;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
        }

        return service;
    }

    public async Task<List<ServiceItem>> GetServiceItemList()
    {
        var request = new GraphQLRequest
        {
            Query = @"
                query getServiceItemList($input: ListInfoInput!) {
                      getServiceItemList(input: $input) {
                        items {
                          ...ServiceItemFragment
                        }
                      }
                    }

                fragment ServiceItemFragment on ServiceItem {
                  itemId
                  name
                  description
                  quantityType
                  useAsWorklogItem
                  unitPrice
                  businessHoursUnitPrice
                  afterHoursUnitPrice
                  roundUpValue
                  quantity
                  amount
                  adjustBlockItemAgainstAllItems
                  salesTaxEnabled
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
            var serviceJson = JsonConvert.SerializeObject(response.Data.getServiceItemList.items);
            List<ServiceItem> Services = new List<ServiceItem>(); 
            Services = JsonConvert.DeserializeObject<List<ServiceItem>>(serviceJson);
            var rawResponse = JsonConvert.SerializeObject(response);
            Console.WriteLine(rawResponse);

            return Services;
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return new List<ServiceItem>();
        }
    }

public async void CreateTicket(Ticket ticket){
        
        var request = new GraphQLRequest{
            Query = @"mutation createTicket($input: CreateTicketInput!) {
                    createTicket(input: $input) {
                    subject
                    client
                    requester
                    status
                    ticketType
                    source }}",

            Variables = new
            {
                input = new
                {
                    subject = ticket.Subject,
                    client = new
                    {
                        accountId = ticket.AccountId
                    },
                    requester = new
                    {
                        userId = ticket.Requesterid
                    },
                    status = ticket.Status,
                    ticketType = ticket.TicketType,
                    source = ticket.Source
                }
            }
        };

        var response = await GraphQlClient.SendQueryAsync<dynamic>(request);
        Console.WriteLine(response);
    }
    
}