using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.ApiClients;

public interface IExactApiClient
{
    public Task PostClientInExact();
    public bool CheckIfClientExists(string code);
}