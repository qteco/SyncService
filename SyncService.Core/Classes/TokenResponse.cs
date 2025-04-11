namespace SyncService.Core.Classes;

public class TokenResponse 
{ 
    public string Access_Token { get; set; }
    public string Token_Type { get; set; }
    public string Expires_In { get; set; }
    public string Refresh_Token { get; set; }
}