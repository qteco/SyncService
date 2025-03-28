using SyncService.Core.Models;

namespace SyncService.Core.Classes;

public class ExactClientDTO
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<ExactClientDTO> CreateExactTransferList(List<Client> clients, List<ClientSite> sites)
    {
        List<ExactClientDTO> exactTransferList = new List<ExactClientDTO>();
        
        for (int i = 0; i < clients.Count; i++)
        {
            for (int j = 0; j < sites.Count; j++)
            {
                ExactClientDTO clientDTO = new ExactClientDTO
                {
                    Code = clients[i].ExactId,
                    Name = clients[i].Name,
                    Address = sites[j].Line1
                };
                exactTransferList.Add(clientDTO);
            }
        }
        return exactTransferList;
    }
}