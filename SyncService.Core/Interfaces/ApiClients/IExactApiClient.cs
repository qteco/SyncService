using SyncService.Core.Classes;
using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.ApiClients;

public interface IExactApiClient
{
    public Task<string> GetAccessToken();
    //public Task GetExactClients();
    public Task<List<ExactClient>> GetAccountGuids();
    public Task<ExactClientDTO> GetClientCode(string guid);
    public Task<HttpResponseMessage> PostClientAsync(ExactClientDTO newClient);
    public Task<HttpResponseMessage> PutClientAsync(ExactClientDTO updatedClient);
}