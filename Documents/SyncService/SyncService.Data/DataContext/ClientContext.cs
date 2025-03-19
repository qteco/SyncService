using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Models;

namespace SyncService.Data.DataContext;

public class ClientContext : DbContext
{
    public DbSet<Client> Client { get; set; }
    public DbSet<AccountManager> AccountManager { get; set; }
    public DbSet<PrimaryContact> PrimaryContact { get; set; }
    public DbSet<SecondaryContact> SecondaryContact { get; set; }
    public DbSet<HqSite> HqSite { get; set; }

    private string _databaseConnection { get; set; }

    public ClientContext(DbContextOptions<ClientContext> options, IConfiguration configuration) : base(options)
    {
        _databaseConnection = configuration["DatabaseConnection"];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .OwnsOne(c => c.AccountManager);
        modelBuilder.Entity<Client>()
            .OwnsOne(c => c.PrimaryContact);
        modelBuilder.Entity<Client>()
            .OwnsOne(c => c.SecondaryContact);
        modelBuilder.Entity<Client>()
            .OwnsOne(c => c.HqSite);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_databaseConnection);
        }
    }
}