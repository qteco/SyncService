using Microsoft.EntityFrameworkCore;
using SyncService.Core;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Models;
using SyncService.Data.DataContext;

namespace SyncService.Data.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly DatabaseContext _context;
    public ClientRepository(DatabaseContext context)
    {
        _context = context;
    }
    public Task<List<Client>> GetExistingClientsAsync()
    {
        return _context.Clients.ToListAsync();
    }
    public async Task<List<Client>> GetNewClients(List<Client> clients)
    {
        var newClients = new List<Client>();

        foreach (var client in clients)
        {
            if (await IsNewClient(client))
            {
                newClients.Add(client);
            }
        }

        return newClients;
    }
    public async Task<bool> IsNewClient(Client client)
    {
        var existingClient = await _context.Clients
            .FirstOrDefaultAsync(c => c.AccountId == client.AccountId);

        return existingClient == null;
    }
    public async Task SyncClientsFromSuperopsToDatabase(List<Client> clients)
    {
        foreach (var client in clients)
        {
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.AccountId == client.AccountId);

            if (existingClient != null)
            {
                existingClient.Name = client.Name;
                existingClient.AccountId = client.AccountId;
                existingClient.Stage = client.Stage;
                existingClient.Status = client.Status;
                existingClient.EmailDomains = client.EmailDomains;
                existingClient.AccountManager = client.AccountManager;
                existingClient.PrimaryContact = client.PrimaryContact;
                existingClient.SecondaryContact = client.SecondaryContact;
                existingClient.HqSite = client.HqSite;
                existingClient.TechnicianGroups = client.TechnicianGroups;
                existingClient.CustomFields = client.CustomFields;
            }
            else
            {
                var newClient = new Client
                {
                    Name = client.Name,
                    AccountId = client.AccountId,
                    Stage = client.Stage,
                    Status = client.Status,
                    EmailDomains = client.EmailDomains,
                    AccountManager = client.AccountManager,
                    PrimaryContact = client.PrimaryContact,
                    SecondaryContact = client.SecondaryContact,
                    HqSite = client.HqSite,
                    TechnicianGroups = client.TechnicianGroups,
                    CustomFields = client.CustomFields,
                    ExactId = ExactIdGenerator.GenerateExactId()
                };
                _context.Clients.Add(newClient);
            }
        }
        
        await _context.SaveChangesAsync();
    }
    public static async Task<Guid> GetClientId(string accountId, DatabaseContext context)
    {
        var client = await context.Clients
            .FirstOrDefaultAsync(c => c.AccountId == accountId);

        return client.Id;
    }
    
    
}
