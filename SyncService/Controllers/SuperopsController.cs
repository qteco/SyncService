using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

[ApiController]
[Route("api/[controller]")]
public class SuperopsController : ControllerBase
{
    private readonly ISuperopsApiClient _superopsApiClient;

    public SuperopsController(IClientService clientService, ISuperopsApiClient superopsApiClient)
    {
        _superopsApiClient = superopsApiClient;
    }

    [HttpGet("GetClients")]
    public async Task<List<Client>> GetClients()
    {
        return await _superopsApiClient.GetClientListAsync();
    }
    
    [HttpGet("GetSiteListByAccountId")]
    public async Task<List<ClientSite>> GetClientSiteData(string accountId)
    {
        return await _superopsApiClient.GetClientSiteDataAsync(accountId);
    }
    
}