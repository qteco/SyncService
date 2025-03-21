using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SyncService.Core.Interfaces.Services;
using SyncService.Core.Models;

namespace SyncService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly HttpClient _httpClient;
    
    public AuthController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest("Authorization code not provided.");
        }

        var tokenRequest = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", "8cd00572-1569-46a7-8858-4cf1a2b4110f"),
            new KeyValuePair<string, string>("client_secret", "PbOCYfnKnoQn"),
            new KeyValuePair<string, string>("redirect_uri", "https://localhost:7198/api/Auth/callback"),
            new KeyValuePair<string, string>("code", code)
        });

        var tokenResponse = await _httpClient.PostAsync("https://start.exactonline.nl/api/oauth2/token", tokenRequest);
        var tokenJson = await tokenResponse.Content.ReadAsStringAsync();

        if (!tokenResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)tokenResponse.StatusCode, tokenJson);
        }

        var tokenData = JsonSerializer.Deserialize<TokenResponse>(tokenJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Console.WriteLine(tokenData.AccessToken);
        Console.WriteLine(tokenResponse);
        Console.WriteLine(tokenJson);

        return Ok(new
        {
            AccessToken = tokenData.AccessToken,
            RefreshToken = tokenData.RefreshToken,
            ExpiresIn = tokenData.ExpiresIn
        });
    }
}

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}