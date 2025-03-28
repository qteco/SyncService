using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Services;

public interface IClientSiteService
{
    public Task<List<ClientSite>> GetExistingClientSitesAsync();
}