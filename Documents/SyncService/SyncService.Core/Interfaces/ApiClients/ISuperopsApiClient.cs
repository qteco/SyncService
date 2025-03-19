using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.ApiClients
{
    public interface ISuperopsApiClient
    {
        public Task<List<Client>> GetClientListAsync();
        public Task<List<ClientSite>> GetClientSiteDataAsync(string accountId);
    }
}

