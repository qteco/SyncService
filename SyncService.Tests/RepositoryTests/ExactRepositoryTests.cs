using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Interfaces;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Services;
using SyncService.Data.DataContext;
using SyncService.Data.Repositories;

namespace SyncService.Tests
{
    public class ClientRepositoryTests
    {
        private readonly DatabaseContext _context;
        private readonly ExactRepository _clientRepository;

        public ClientRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ClientContext")
                .Options;

            var configuration = new ConfigurationBuilder()
                .Build();

            _context = new DatabaseContext(options);
            _context.Database.EnsureCreated();
            _clientRepository = new ExactRepository(_context);
        }
    }
}