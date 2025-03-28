using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Services;

public interface IExactService
{
    public bool IsClientInExact(string code);
    public Task SyncNewClients();
}