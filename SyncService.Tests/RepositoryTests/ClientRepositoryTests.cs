using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Models;
using SyncService.Data.DataContext;
using SyncService.Data.Repositories;
using System;

namespace SyncService.Tests.RepositoryTests
{
    public class ClientRepositoryTests
    {
        private readonly DatabaseContext _context;
        private readonly ClientRepository _clientRepository;

        public ClientRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ClientContext")
                .Options;

            _context = new DatabaseContext(options);
            _context.Database.EnsureCreated();
            _clientRepository = new ClientRepository(_context);
        }

        private string GenerateUniqueExactId()
        {
            var random = new Random();
            return random.Next(100000000, 1000000000).ToString(); 
        }

        [Fact]
        public async Task GetExistingClientsAsync_ShouldReturnClients()
        {
            _context.Clients.Add(new Client { Name = "Client1", AccountId = "1", ExactCode = GenerateUniqueExactId() });
            _context.Clients.Add(new Client { Name = "Client2", AccountId = "2", ExactCode = GenerateUniqueExactId() });
            await _context.SaveChangesAsync();

            var result = await _clientRepository.GetExistingClientsAsync();

            Assert.Contains(result, client => client.Name == "Client1");
            Assert.Contains(result, client => client.Name == "Client2");
        }
        
        [Fact]
        public async Task GetNewClients_ShouldReturnNewClients()
        {
            List<Client> existingClients = new List<Client>();
            existingClients.Add(new Client { Name = "Client1", AccountId = "1", ExactCode = GenerateUniqueExactId() });
            existingClients.Add(new Client { Name = "Client2", AccountId = "2", ExactCode = GenerateUniqueExactId() });
            existingClients.Add(new Client { Name = "Client3", AccountId = "3", ExactCode = GenerateUniqueExactId() });
            _context.Clients.AddRange(existingClients);

            List<Client> newClients = new List<Client>();
            newClients.Add(new Client { Name = "Client4", AccountId = "51255", ExactCode = GenerateUniqueExactId() });
            
            List<Client> result = await _clientRepository.GetNewClients(newClients);
            
            Assert.IsType<List<Client>>(result);
            Assert.Contains(result, client => client.Name == "Client4");
        }

        [Fact]
        public async Task CanDetectNewClient()
        {
            var newClient = new Client { Name = "Client2516", AccountId = "2516", ExactCode = GenerateUniqueExactId() };
            var existingClient = new Client { Name = "Client1", AccountId = "1", ExactCode = GenerateUniqueExactId() };

            var canAddNewClient = await _clientRepository.IsNewClient(newClient);
            var canAddExistingClient = await _clientRepository.IsNewClient(existingClient);
            
            Assert.True(canAddNewClient);
            Assert.False(canAddExistingClient);
        }
        
        [Fact]
        public async Task CanSyncClientsFromSuperops()
        {
            List<Client> superopsClients = new List<Client>();

            var superopsClient1 = new Client { Name = "Client1", AccountId = "1", ExactCode = GenerateUniqueExactId() };
            var superopsClient2 = new Client { Name = "Client888", AccountId = "888", ExactCode = GenerateUniqueExactId() };
            superopsClients.Add(superopsClient1);
            superopsClients.Add(superopsClient2);
            
            await _clientRepository.SyncClientsFromSuperopsToDatabase(superopsClients);

            var updatedClients = await _context.Clients.ToListAsync();
            Assert.Contains(updatedClients, c => c.AccountId == "1");   
            Assert.Contains(updatedClients, c => c.AccountId == "888");  
        }
    }
}