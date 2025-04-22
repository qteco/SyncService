using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Services;

public interface IClientService
{ 
    public Task<List<Client>> GetDatabaseClients();
    public Task SyncClients();
    public Task<string> SyncClientSites();
}