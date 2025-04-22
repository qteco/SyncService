using SyncService.Core.Classes;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Core.Services;
public class ExactService : IExactService
{
    private readonly IExactRepository _exactRepository;
    private readonly IClientService _clientService;
    private readonly IClientSiteService _clientSiteService;
    private readonly IExactApiClient _exactApiClient;
    private ExactClientDTO NewClient { get; set; }
    private bool IsInExact { get; set; }
    private List<ExactClient> ExactGuids { get; set; }
    public ExactService(IExactRepository exactRepository, IClientService clientService, IClientSiteService clientSiteService, IExactApiClient exactApiClient)
    {
        _exactRepository = exactRepository;
        _clientService = clientService;
        _clientSiteService = clientSiteService;
        _exactApiClient = exactApiClient;
    }
    public async Task<bool> IsClientInDatabase(string code)
    {
        return await _exactRepository.IsClientInDatabase(code);
    }
    public async Task SyncClientsToDatabase()
    { 
        List<Client> clients = await _clientService.GetDatabaseClients();
        List<ClientSite> sites = await _clientSiteService.GetExistingClientSitesAsync();
        
        await _exactRepository.SyncClientsToDatabase(await CreateExactTransferList(clients, sites));
    }
    public async Task SyncClientsToExact()
    {
        List<Client> clients = await _clientService.GetDatabaseClients();
        List<ClientSite> sites = await _clientSiteService.GetExistingClientSitesAsync();
        List<ExactClientDTO> exactTransferList = await CreateExactTransferList(clients, sites);
        ExactGuids = await _exactApiClient.GetAccountGuids();
        
        for (int i = 0; i < exactTransferList.Count; i++)
        {
            var code = exactTransferList[i].Code;
            IsInExact = await IsClientInExact(exactTransferList[i].Id);
            
            NewClient = new ExactClientDTO()
            {
                Code = exactTransferList[i].Code,
                Id = exactTransferList[i].Id,
                Name = exactTransferList[i].Name,
                AddressLine1 = exactTransferList[i].AddressLine1,
                City = exactTransferList[i].City,
                Country = exactTransferList[i].Country,
                Postcode = exactTransferList[i].Postcode,
                Status = exactTransferList[i].Status,
                Website = exactTransferList[i].Website,
            }; 

            if (!IsInExact)
            {
                Console.WriteLine($"Client {exactTransferList[i].Name} has not been synced to Exact. Syncing client {code}...");
                await PostClientAsync(NewClient);
            }
            else
            {
                Console.WriteLine($"Client {exactTransferList[i].Name} updating client in Exact {code}...");
                await PutClientAsync(NewClient);
            }
        }
    }
    public async Task<List<ExactClientDTO>> CreateExactTransferList(List<Client> clients, List<ClientSite> sites)
    {
        return await _exactRepository.CreateExactTransferList(clients, sites);
    }
    public async Task<List<string>> GetClientCodes()
    {
        List<ExactClient> clients = await _exactApiClient.GetAccountGuids();
        List<string> existingAccountCodes = [];

        foreach (ExactClient client in clients)
        {
            var clientDto = await _exactApiClient.GetClientCode(client.Id);
            if (clientDto != null && !string.IsNullOrWhiteSpace(clientDto.Code))
            {
                existingAccountCodes.Add(clientDto.Code);
            }
        }

        return existingAccountCodes;
    }
    public async Task<bool> IsClientInExact(string guid)
    {
        return ExactGuids.Any(c => c.Id == guid);
    }
    public async Task PostClientAsync(ExactClientDTO newClient)
    { 
        await _exactApiClient.PostClientAsync(newClient);
    }
    public async Task<HttpResponseMessage> PutClientAsync(ExactClientDTO newClient)
    {
        return await _exactApiClient.PutClientAsync(newClient);
    }
    

}