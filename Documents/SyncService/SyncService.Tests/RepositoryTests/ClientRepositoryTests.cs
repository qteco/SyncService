using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Models;
using SyncService.Data.DataContext;
using SyncService.Data.Repositories;

namespace SyncService.Tests.RepositoryTests
{
    public class ClientRepositoryTests
    {
        private readonly ClientContext _context;
        private readonly ClientRepository _clientRepository;

        public ClientRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ClientContext>()
                .UseInMemoryDatabase(databaseName: "ClientContext")
                .Options;

            var configuration = new ConfigurationBuilder()
                .Build();

            _context = new ClientContext(options, configuration);
            _context.Database.EnsureCreated();
            _clientRepository = new ClientRepository(_context);
        }

        [Fact]
        public async Task GetExistingClientsAsync_ShouldReturnClients()
        {
            _context.Client.Add(new Client { Name = "Client1", AccountId = "1" });
            _context.Client.Add(new Client { Name = "Client2", AccountId = "2" });
            await _context.SaveChangesAsync();

            var result = await _clientRepository.GetExistingClientsAsync();

            Assert.Contains(result, client => client.Name == "Client1");
            Assert.Contains(result, client => client.Name == "Client2");
        }
        
        [Fact]
        public async Task GetNewClients_ShouldReturnNewClients()
        {
            List<Client> existingClients = new List<Client>();
            existingClients.Add(new Client { Name = "Client1", AccountId = "1" });
            existingClients.Add(new Client { Name = "Client2", AccountId = "2" });
            existingClients.Add(new Client { Name = "Client3", AccountId = "3" });
            _context.Client.AddRange(existingClients);

            List<Client> newClients = new List<Client>();
            newClients.Add(new Client { Name = "Client4", AccountId = "51255" });
            
            List<Client> result = await _clientRepository.GetNewClients(newClients);
            
            Assert.IsType<List<Client>>(result);
            Assert.Contains(result, client => client.Name == "Client4");
        }

        [Fact]
        public async Task CanDetectNewClient()
        {
            var newClient = new Client { Name = "Client2516", AccountId = "2516" };
            var existingClient = new Client { Name = "Client1", AccountId = "1" };

            var canAddNewClient = await _clientRepository.IsNewClient(newClient);
            var canAddExistingClient = await _clientRepository.IsNewClient(existingClient);
            
            Assert.True(canAddNewClient);
            Assert.False(canAddExistingClient);
        }
        
        [Fact]
        public async Task CanSyncClientsFromSuperops()
        {
            List<Client> superopsClients = new List<Client>();

            var superopsClient1 = new Client { Name = "Client1", AccountId = "1" };
            var superopsClient2 = new Client { Name = "Client888", AccountId = "888" };
            superopsClients.Add(superopsClient1);
            superopsClients.Add(superopsClient2);
            
            await _clientRepository.SyncClientsFromSuperopsToDatabase(superopsClients);

            var updatedClients = await _context.Client.ToListAsync();
            Assert.Contains(updatedClients, c => c.AccountId == "1");   
            Assert.Contains(updatedClients, c => c.AccountId == "888");  
        }

        
    }
} 