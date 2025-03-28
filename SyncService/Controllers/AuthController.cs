using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly HttpClient _httpClient;
    public AuthController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        var clientId = Environment.GetEnvironmentVariable("ExactClientId");
        var redirectUri = "https://qtecotest.nl:7198/api/Auth/callback"; 
        var responseType = "code"; 
        
        var authUrl = $"https://start.exactonline.nl/api/oauth2/auth?" +
                      $"client_id={clientId}" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&response_type={responseType}";

        return Redirect(authUrl); 
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest("Authorization code is missing.");
        }

        var tokenUrl = "https://start.exactonline.nl/api/oauth2/token"; 

        var requestData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", Environment.GetEnvironmentVariable("ExactClientId")),
            new KeyValuePair<string, string>("client_secret", Environment.GetEnvironmentVariable("ExactClientSecret")),
            new KeyValuePair<string, string>("redirect_uri", "https://qtecotest.nl:7198/api/Auth/callback"),
            new KeyValuePair<string, string>("code", code.Replace("%21", "!")), 
        });

        try
        {
            var response = await _httpClient.PostAsync(tokenUrl, requestData);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Ok(tokenResponse);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error fetching access token: {ex.Message}");
        }
    }
    
    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken)
    {
        var tokenUrl = "https://start.exactonline.nl/api/oauth2/token"; 

        var requestData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
            new KeyValuePair<string, string>("client_id", Environment.GetEnvironmentVariable("ExactClientId")),
            new KeyValuePair<string, string>("client_secret", Environment.GetEnvironmentVariable("ExactClientSecret")),
        });

        try
        {
            var response = await _httpClient.PostAsync(tokenUrl, requestData);
            response.EnsureSuccessStatusCode();
            
            return Ok(response.Content.ReadAsStringAsync().Result);
        }

        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error fetching refresh token: {ex.Message}");
        }
    }
}
public class TokenResponse 
{ 
    public string Access_Token { get; set; }
    public string Token_Type { get; set; }
    public string Expires_In { get; set; }
    public string Refresh_Token { get; set; }
}

public class RefreshToken
{
    public string Grant_type { get; set; }
    public string Refresh_token { get; set; }
    public string Client_id { get; set; }
    public string Client_secret { get; set; }
}