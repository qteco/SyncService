using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyncService.Core.Interfaces;
using SyncService.Core.Interfaces.ApiClients;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Services;

namespace SyncService.Tests
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IClientSiteRepository> _mockClientSiteRepository;
        private readonly Mock<ISuperopsApiClient> _mockSuperopsApiClient;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockClientSiteRepository = new Mock<IClientSiteRepository>();
            _mockSuperopsApiClient = new Mock<ISuperopsApiClient>();

            _clientService = new ClientService(
                _mockClientRepository.Object,
                _mockSuperopsApiClient.Object,
                _mockClientSiteRepository.Object
            );
        }
        
    }
}
        
        

        