using SyncService.Core.Classes;
using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Repositories;

public interface IExactRepository
{ 
    public bool IsClientInExact(string code);
    public Task SyncNewClients(List<ExactClientDTO> exactTransferList);
    public List<ExactClientDTO> CreateExactTransferList(List<Client> clients, List<ClientSite> sites);
    
}