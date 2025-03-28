using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;
using SyncService.Data.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ClientSiteController : ControllerBase
{
    private readonly IClientSiteService _clientSiteService;

    public ClientSiteController(IClientSiteService clientSiteService)
    {
        _clientSiteService = clientSiteService;
    }
    
    [HttpGet("GetClientSitesFromDatabase")]
    public async Task <List<ClientSite>> GetClientSitesFromDatabase()
    {
        return await _clientSiteService.GetExistingClientSitesAsync();
    }
    
}