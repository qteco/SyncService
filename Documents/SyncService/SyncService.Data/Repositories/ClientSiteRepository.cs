using Microsoft.EntityFrameworkCore;
using SyncService.Core.Interfaces;
using SyncService.Core.Interfaces.Repositories;
using SyncService.Core.Models;
using SyncService.Data.DataContext;
using SyncService.Data.Repositories;

namespace SyncService.Data;

public class ClientSiteRepository : IClientSiteRepository
{
    private readonly ClientSiteContext _context;
    private readonly ClientContext _clientContext;

    public ClientSiteRepository(ClientSiteContext context, ClientContext clientContext)
    {
        _context = context;
        _clientContext = clientContext;
    }
    public async Task SyncClientSitesFromSuperops(List<ClientSite> clientSites, string accountId)
    {
        foreach (var clientSite in clientSites)
        {
            var existingClient = await _context.ClientSites
                .FirstOrDefaultAsync(c => c.Id == clientSite.Id);

            if (existingClient != null)
            {
                existingClient.Id = clientSite.Id;
                existingClient.Name = clientSite.Name;
                existingClient.City = clientSite.City;
                existingClient.PostalCode = clientSite.PostalCode;
                existingClient.CountryCode = clientSite.CountryCode;
                existingClient.ContactNumber = clientSite.ContactNumber;
                existingClient.StateCode = clientSite.StateCode;
                existingClient.Line1 = clientSite.Line1;
                existingClient.Line2 = clientSite.Line2;
                existingClient.Line3 = clientSite.Line3;
                existingClient.BusinessHour = clientSite.BusinessHour;
                existingClient.HolidayList = clientSite.HolidayList;
                existingClient.TimezoneCode = clientSite.TimezoneCode;
                existingClient.Working24x7 = clientSite.Working24x7;
                existingClient.ClientId = ClientRepository.GetClientId(accountId, _clientContext);
                existingClient.BusinessHour = clientSite.BusinessHour?.Select(bh => new BusinessHour
                {
                    Day = bh.Day,
                    Start = bh.Start,
                    End = bh.End,
                    AccountId = clientSite.Id
                }).ToList();
            }
            else
            {
                var newClientSite = new ClientSite
                {
                    Id = clientSite.Id,
                    Name = clientSite.Name,
                    City = clientSite.City,
                    PostalCode = clientSite.PostalCode,
                    CountryCode = clientSite.CountryCode,
                    ContactNumber = clientSite.ContactNumber,
                    StateCode = clientSite.StateCode,
                    Line1 = clientSite.Line1,
                    Line2 = clientSite.Line2,
                    Line3 = clientSite.Line3,
                    HolidayList = clientSite.HolidayList,
                    TimezoneCode = clientSite.TimezoneCode,
                    Working24x7 = clientSite.Working24x7,
                    ClientId = ClientRepository.GetClientId(accountId, _clientContext),
                    BusinessHour = clientSite.BusinessHour?.Select(bh => new BusinessHour
                    {
                        Day = bh.Day,
                        Start = bh.Start,
                        End = bh.End,
                        AccountId = clientSite.Id 
                    }).ToList(),
                };
                _context.ClientSites.Add(newClientSite);
            }
        }
        await _context.SaveChangesAsync();
    }
}