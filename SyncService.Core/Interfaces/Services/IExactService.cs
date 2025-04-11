using SyncService.Core.Classes;
using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Services;

public interface IExactService
{
    public Task<bool> IsClientInDatabase(string code);
    public Task SyncClientsToDatabase();
    public Task<List<string>> GetClientCodes();
    public Task SyncClientsToExact();
    public Task<bool> IsClientInExact(string guid);
    public Task<HttpResponseMessage> PutClientAsync(ExactClientDTO updatedClient);
}