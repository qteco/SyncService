using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Repositories;

public interface IClientSiteRepository
{
    public Task SyncClientSitesFromSuperops(List<ClientSite> clients, string accountId);
}