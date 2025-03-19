using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Models;

namespace SyncService.Data.DataContext;

public class ClientSiteContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<ClientSite> ClientSites { get; set; }
    public DbSet<BusinessHour> BusinessHours { get; set; }
    private readonly string _databaseConnection;

    public ClientSiteContext(IConfiguration databaseConnection)
    {
        _databaseConnection = databaseConnection["DatabaseConnection"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseConnection);
    }
}