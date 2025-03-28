using Microsoft.EntityFrameworkCore;
using SyncService.Core.Classes;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Models;
using SyncService.Data.DataContext;

namespace SyncService.Data.Repositories;

public class ExactRepository : IExactRepository
{
    private readonly DatabaseContext _context;

    public ExactRepository(DatabaseContext context)
    {
        _context = context;
    }

    public bool IsClientInExact(string code)
    {
        return _context.ExactClients.FirstOrDefault(c => c.Code == code) != null;
    }

    public List<ExactClientDTO> CreateExactTransferList(List<Client> clients, List<ClientSite> sites)
    {
        List<ExactClientDTO> exactTransferList = new List<ExactClientDTO>();
        
        for (int i = 0; i < clients.Count; i++)
        {
            ExactClientDTO clientDTO = new ExactClientDTO
            {
                Code = clients[i].ExactId,
                Name = clients[i].Name,
                Address = _context.ClientSites
                    .Where(c => c.ClientId == clients[i].Id)
                    .Select(c => c.Line1)
                    .FirstOrDefault() ?? "null"
            };
            
            exactTransferList.Add(clientDTO);
        }
        return exactTransferList;
    }
    
    public async Task SyncNewClients(List<ExactClientDTO> exactTransferList)
    {
        foreach (var client in exactTransferList)
        {
            var existingClient = await _context.ExactClients
                .FirstOrDefaultAsync(c => c.Code == client.Code);

            if (IsClientInExact(client.Code))
            {
                existingClient.Code = client.Code;
                existingClient.Name = client.Name;
                existingClient.Address = client.Address;
            }

            else if (!IsClientInExact(client.Code))
            {
                var ExactClient = new ExactClient
                {
                    Code = client.Code,
                    Name = client.Name,
                    Address = client.Address,
                };
                _context.Add(ExactClient);
            }
        }
        await _context.SaveChangesAsync();
    }
    
}