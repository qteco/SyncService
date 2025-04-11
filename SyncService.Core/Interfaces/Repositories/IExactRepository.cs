using SyncService.Core.Classes;
using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Repositories;

public interface IExactRepository
{
    public Task<bool> IsClientInDatabase(string code);
    public Task PostClientInDatabase(ExactClientDTO clientDTO);
    public Task UpdateClientInDatabase(ExactClientDTO clientDTO);
    public Task SyncClientsToDatabase(List<ExactClientDTO> exactTransferList);
    public Task<List<ExactClientDTO>> CreateExactTransferList(List<Client> clients, List<ClientSite> sites);
    public Task<string> GetClientIdFromCode(string code);
    public Task<string> GetClientCodeFromGuid(string guid);
}