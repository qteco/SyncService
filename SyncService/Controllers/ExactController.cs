using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SyncService.Core.Classes;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Controllers;

public class ExactController : Controller
{
    private readonly IExactService _exactService;
    private readonly IExactApiClient _exactApiClient;

    public ExactController(IExactService exactService, IExactApiClient exactApiClient)
    {
        _exactService = exactService;
        _exactApiClient = exactApiClient;
    }

    [HttpGet("IsClientInExact")]
    public async Task<bool> IsNewClient(string code)
    {
        return await _exactService.IsClientInDatabase(code);
    }

    [HttpGet("SyncClientsToDatabase")]
    public async Task SyncClientsToDatabase()
    {
        await _exactService.SyncClientsToDatabase();
    }

    [HttpGet("GetAccountGuids")]
    public async Task<IActionResult> GetAccountGuids()
    {
        return Ok(await _exactApiClient.GetAccountGuids());
    }
    
    [HttpGet("GetClientCodes")]
    public async Task<IActionResult> GetClientCodes()
    {
        return Ok(await _exactService.GetClientCodes());
    }

    [HttpGet("SyncClients")]
    public async Task<IActionResult> SyncClients()
    {
        await _exactService.SyncClientsToExact();
        return Ok();
    }
    
    [HttpGet("TestPut")]
    public async Task<IActionResult> PutTest()
    {
        ExactClientDTO clientDTO = new ExactClientDTO()
        {
            Id = "9cdf84f5-1c38-4ef8-a604-01cb6037ffc0",
            Code = "003215857",
            Name = "Qteco",
            AddressLine1 = "AddressNew",
            City = "CityNew",
            Postcode = "12345 DB"
        };
    
        var response = await _exactService.PutClientAsync(clientDTO);
        var content = await response.Content.ReadAsStringAsync();

        return Content(content, "application/json");
    } 
    
}
