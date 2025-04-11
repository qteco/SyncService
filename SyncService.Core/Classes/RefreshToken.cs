namespace SyncService.Core.Classes;

public class RefreshToken
{
    public string Grant_type { get; set; }
    public string Refresh_token { get; set; }
    public string Client_id { get; set; }
    public string Client_secret { get; set; }
}