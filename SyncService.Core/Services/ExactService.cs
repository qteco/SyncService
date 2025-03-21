using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Core.Services;

public class ExactService : IExactService
{
    private IClientRepository _clientRepository;

    public ExactService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task PostClientInExact(Client client)
    {
        List<Client> existingClients = await _clientRepository.GetExistingClientsAsync();
        List<Client> newClients = await _clientRepository.GetNewClients(existingClients);
        
        throw new NotImplementedException();
    }
}