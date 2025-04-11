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
    public async Task<bool> IsClientInDatabase(string code)
    {
        return await _context.ExactClients.AnyAsync(c => c.Code == code);
    }

    public async Task<List<ExactClientDTO>> CreateExactTransferList(List<Client> clients, List<ClientSite> sites)
    {
        List<ExactClientDTO> exactTransferList = new List<ExactClientDTO>();
        
        for (int i = 0; i < clients.Count; i++)
        {
            ExactClientDTO clientDTO = new ExactClientDTO
            {
                Code = clients[i].ExactCode,
                Id = _context.ExactClients
                    .Where(c => c.Code == clients[i].ExactCode)
                    .Select(c => c.Id)
                    .FirstOrDefault(),
                Name = clients[i].Name,
                AddressLine1 = _context.ClientSites
                    .Where(c => c.ClientId == clients[i].Id)
                    .Select(c => c.Line1)
                    .FirstOrDefault() ?? "null", 
                Email = _context.Clients
                    .Where(c => c.Id == clients[i].Id)
                    .Select(c => c.EmailDomains)
                    .FirstOrDefault().FirstOrDefault() ?? "null",
                Postcode = _context.ClientSites.Where(c => c.ClientId == clients[i].Id)
                    .Select(c => c.PostalCode)
                    .FirstOrDefault() ?? "null",
                Status = "C",
                Website = _context.Clients
                    .Where(c => c.Id == clients[i].Id)
                    .Select(c => c.EmailDomains)
                    .FirstOrDefault().FirstOrDefault() ?? "null",

            };
            
            exactTransferList.Add(clientDTO);
        }
        return exactTransferList;
    }

    public async Task PostClientInDatabase(ExactClientDTO clientDTO)
    {
        ExactClient client = new ExactClient()
        {
            Code = clientDTO.Code,
            Id = clientDTO.Id,
            Name = clientDTO.Name,
            Address = clientDTO.AddressLine1,
            Email = clientDTO.Email,
            City = clientDTO.City,
            Country = clientDTO.Country,
            PostCode = clientDTO.Postcode,
            Status = "C",
            Website = clientDTO.Website,
        };
        await _context.ExactClients.AddAsync(client);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateClientInDatabase(ExactClientDTO clientDTO)
    {
        var client = await _context.ExactClients.FirstOrDefaultAsync(c => c.Code == clientDTO.Code);
        if (client != null)
        {
            client.Name = clientDTO.Name;
            client.Address = clientDTO.AddressLine1;
            client.Email = clientDTO.Email;
            client.City = clientDTO.City;
            client.Country = clientDTO.Country;
            client.PostCode = clientDTO.Postcode;
            client.Status = "C";
            client.Website = clientDTO.Website;
            await _context.SaveChangesAsync();
        }
    }
    public async Task SyncClientsToDatabase(List<ExactClientDTO> exactTransferList)
    { 
        foreach (var client in exactTransferList)
        { 
            if (await IsClientInDatabase(client.Code))
            { 
                await UpdateClientInDatabase(client);
            }

            else if (!await IsClientInDatabase(client.Code))
            {
                await PostClientInDatabase(client); 
            };
        }
    }
    
    public async Task<string> GetClientIdFromCode(string code)
    {
        return await _context.ExactClients
            .Where(c => c.Code == code)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<string> GetClientCodeFromGuid(string guid)
    {
        string code = _context.ExactClients.Where(c => c.Id == guid)
            .Select(c => c.Code).FirstOrDefault();

        return code;
    }
    
}
