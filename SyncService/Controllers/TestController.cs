using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Interfaces.ApiClients;
using Microsoft.AspNetCore.Http.HttpResults;


namespace SyncService.Controllers;

public class TestController : ControllerBase
{
    private readonly ISuperopsApiClient _superopsApiClient;
    private readonly IIrmaApiClient _irmaApiClient;
    
    public TestController(ISuperopsApiClient superopsApiClient, IIrmaApiClient irmaApiClient)
    {
        _superopsApiClient = superopsApiClient;
        _irmaApiClient = irmaApiClient;
    }
    
    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        return Ok(await _superopsApiClient.GetServiceItems("4"));
    }
    
    [HttpGet("Test2")]
    public async Task<IActionResult> Test2()
    {
        return Ok(await _superopsApiClient.GetServiceItemList());
    }
    
    [HttpGet("Test3")]
    public async Task<IActionResult> Test3()
    {
        await _irmaApiClient.GetInvoices();
        return Ok();
    }
}