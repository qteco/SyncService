using SyncService.Core.Interfaces;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Core.Services;
public class ClientService: IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientSiteRepository _clientSiteRepository;
    
    private readonly ISuperopsApiClient _superopsApiClient;
    public ClientService(IClientRepository clientRepository, ISuperopsApiClient superopsApiClient, IClientSiteRepository clientSiteRepository)
    {
        _clientRepository = clientRepository;
        _superopsApiClient = superopsApiClient;
        _clientSiteRepository = clientSiteRepository;
    }
    public async Task<List<Client>> GetDatabaseClients()
    {
        return await _clientRepository.GetExistingClientsAsync();
    }

    public async Task<List<Client>> GetNewClientsAsync()
    {
        return await _clientRepository.GetNewClients(await _superopsApiClient.GetClientListAsync());
    }
    public async Task SyncClients()
    {
        await _clientRepository.SyncClientsFromSuperopsToDatabase(await _superopsApiClient.GetClientListAsync());
    }
    public async Task<string> SyncClientSites()
    {
        var clients = await _superopsApiClient.GetClientListAsync();
        var accountId = "";
        
        foreach (var client in clients)
        {
            accountId = client.AccountId;
            await _clientSiteRepository.SyncClientSitesFromSuperops(await _superopsApiClient.GetClientSiteDataAsync(client.AccountId), accountId);
        }

        return accountId;
    }
    
    public async Task<bool> IsNewClient(Client client)
    {
        return await _clientRepository.IsNewClient(client);
    }
}