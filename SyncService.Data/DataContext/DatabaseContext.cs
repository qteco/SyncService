using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Models;

namespace SyncService.Data.DataContext;

public class DatabaseContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<AccountManager> AccountManagers { get; set; }
    public DbSet<PrimaryContact> PrimaryContacts { get; set; }
    public DbSet<SecondaryContact> SecondaryContacts { get; set; }
    public DbSet<HqSite> HqSites { get; set; }
    public DbSet<ClientSite> ClientSites { get; set; }
    public DbSet<BusinessHour> BusinessHours { get; set; }
    public DbSet<Core.Models.ExactClient> ExactClients { get; set; }
    
    public DbSet<Requester> Requesters { get; set; } 
    public DatabaseContext(DbContextOptions options) : base(options) 
    {
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

        modelBuilder.Entity<Client>()
            .HasMany(c => c.ClientSites)
            .WithOne(cs => cs.Client)
            .HasForeignKey(cs => cs.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.AccountId)
            .IsUnique(); // Ensure uniqueness for relationship

        modelBuilder.Entity<Requester>()
            .HasOne(r => r.Client)
            .WithMany()
            .HasForeignKey(r => r.AccountId)
            .HasPrincipalKey(c => c.AccountId); // important!
        
        modelBuilder.Entity<Core.Models.ExactClient>()
            .HasOne(ec => ec.Client) 
            .WithOne() 
            .HasForeignKey<Core.Models.ExactClient>(ec => ec.Code)  
            .HasPrincipalKey<Client>(c => c.ExactCode);
    }
}