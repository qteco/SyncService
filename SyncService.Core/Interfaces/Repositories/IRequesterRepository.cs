using SyncService.Core.Models;

namespace SyncService.Core.Interfaces.Repositories;

public interface IRequesterRepository
{
    public Task<Requester> GetRequester(string ContactNumber);
}