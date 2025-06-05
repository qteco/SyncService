using Microsoft.EntityFrameworkCore;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Models;
using SyncService.Data.DataContext;

namespace SyncService.Data.Repositories;

public class RequesterRepository : IRequesterRepository
{
    private readonly DatabaseContext _context;
    public RequesterRepository(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<Requester> GetRequester(string contactNumber)
    {
        return await _context.Requesters
            .Include(r => r.Client)
            .FirstOrDefaultAsync(r => r.ContactNumber == contactNumber);
    }
}