using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Core.Services;

public class ClientSiteService : IClientSiteService
{
    private readonly IClientSiteRepository _clientSiteRepository;

    public ClientSiteService(IClientSiteRepository clientSiteRepository)
    {
        _clientSiteRepository = clientSiteRepository;
    }
    
    //Gets the clientSite data from the RMM system.
    public async Task<List<ClientSite>> GetExistingClientSitesAsync()
    {
        return await _clientSiteRepository.GetExistingClientSitesAsync();
    }
}