using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Repositories;

public interface IClientRepository
{
    public Task<List<Client>> GetExistingClientsAsync();
    public Task SyncClientsFromSuperopsToDatabase(List<Client> clients);
    public Task<List<Client>> GetNewClients(List<Client> clients);
    public Task<bool> IsNewClient(Client client);
}