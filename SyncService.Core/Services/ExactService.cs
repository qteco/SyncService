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


    public ExactService(IExactRepository exactRepository, IClientService clientService, IClientSiteService clientSiteService)
    {
        _exactRepository = exactRepository;
        _clientService = clientService;
        _clientSiteService = clientSiteService;
    }

    public bool IsClientInExact(string code)
    {
        return _exactRepository.IsClientInExact(code);
    }

    public async Task SyncNewClients()
    { 
        List<Client> clients = await _clientService.GetDatabaseClients();
        List<ClientSite> sites = await _clientSiteService.GetExistingClientSitesAsync();
        
        await _exactRepository.SyncNewClients(CreateExactTransferList(clients, sites));
    }
    
    public List<ExactClientDTO> CreateExactTransferList(List<Client> clients, List<ClientSite> sites)
    {
        return _exactRepository.CreateExactTransferList(clients, sites);
    }
}