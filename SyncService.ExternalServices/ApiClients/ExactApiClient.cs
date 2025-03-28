using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Models;

namespace SyncService.ExternalServices.ApiClients;

public class ExactApiClient : IExactApiClient
{
    private IClientRepository _clientRepository;
    private ISuperopsApiClient _superopsApiClient;

    public ExactApiClient(IClientRepository clientRepository, ISuperopsApiClient superopsApiClient)
    {
        _clientRepository = clientRepository;
        _superopsApiClient = superopsApiClient;
    }

    public async Task PostClientInExact()
    {
        List<Client> clients = await _clientRepository.GetExistingClientsAsync();

        
    }

    public bool CheckIfClientExists(string code)
    {
        throw new NotImplementedException();
    }
}