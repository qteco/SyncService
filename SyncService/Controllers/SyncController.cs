using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IExactService _exactService;


    public SyncController(IClientService clientService, IExactService exactService)
    {
        _clientService = clientService;
        _exactService = exactService;
    }
    
    /* Syncs the following data:
     1. Clients from the RMM platform to the database
     2. Clientsites from the RMM platform to the database
     3. Copies the data from CRM to the database
     4. Syncs the clients to CRM.
     */
    
    [HttpGet("FullSync")]
    public async Task<IActionResult> FullSync()
    {
        await _clientService.SyncClients();
        await _clientService.SyncClientSites();
        await _exactService.SyncClientsToDatabase();
        await _exactService.SyncClientsToExact();
        return Ok();
    }
   
}