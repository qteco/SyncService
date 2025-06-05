using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;
using SyncService.Data.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    //Endpoint to retrieve existing clients from database
    [HttpGet("GetClientsFromDatabase")]
    public async Task <List<Client>> GetClientsFromDatabase()
    {
        return await _clientService.GetDatabaseClients();
    }
    
    /* Endpoint to sync clients from RMM to database.
    Used to store clients and apply queries. */
    [HttpGet("Sync")]
    public async Task SyncClients()
    {
        await _clientService.SyncClients();
    }

    //Endpoint to sync clientSites from RMM to database. 
    [HttpGet("SyncClientSites")]
    public async Task SyncClientSites()
    {
        await _clientService.SyncClientSites();
    }
}