namespace SyncService.Core.Interfaces.ApiClients;

public interface IBroadsoftApiClient
{
    public Task<(string CallId, string PhoneNumber)?> GetLatestCallAsync();
}