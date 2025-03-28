using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Classes;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Controllers;

public class ExactController : Controller
{
    private readonly IExactService _exactService;
    public readonly IExactRepository _exactRepository;
    public readonly IClientService _clientService;
    public readonly IClientSiteService _clientSiteService;



    public ExactController(IExactService exactService, IExactRepository exactRepository, IClientService clientService, IClientSiteService clientSiteService)
    {
        _exactService = exactService;
        _exactRepository = exactRepository;
        _clientService = clientService;
        _clientSiteService = clientSiteService;
    }

    [HttpGet("IsClientInExact")]
    public bool IsNewClient(string code)
    { 
        return _exactService.IsClientInExact(code);
    }
    
    [HttpGet("SyncNewClients")]
    public async Task SyncNewClients()
    {
        await _exactService.SyncNewClients();
    }
    
}