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
    
    [HttpGet("GetClientsFromDatabase")]
    public async Task <List<Client>> GetClientsFromDatabase()
    {
        return await _clientService.GetDatabaseClients();
    }
    
    [HttpGet("Sync")]
    public async Task SyncClients()
    {
        await _clientService.SyncClients();
    }
    
    [HttpGet("SyncClientSites")]
    public async Task SyncClientSites()
    {
        await _clientService.SyncClientSites();
    }
}