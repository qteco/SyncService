using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SyncService.Core.Models;

namespace SyncService.Data.DataContext;

public class ExactClientContext :DbContext
{
    public DbSet<ExactClient> ExactClients { get; set; }
    
    private readonly string _databaseConnection;

    public ExactClientContext(IConfiguration databaseConnection)
    {
        _databaseConnection = databaseConnection["DatabaseConnection"];
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseConnection);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Ignore<AccountManager>(); 
        modelBuilder.Ignore<PrimaryContact>(); 
        modelBuilder.Ignore<SecondaryContact>(); 
        modelBuilder.Ignore<HqSite>();  
        modelBuilder.Entity<ExactClient>()
            .HasOne(ec => ec.Client)  // ExactClient has one Client
            .WithOne()  // Client has one ExactClient (one-to-one relationship)
            .HasForeignKey<ExactClient>(ec => ec.Code)  // Use Code as foreign key
            .HasPrincipalKey<Client>(c => c.ExactId);  // Adjust delete behavior if needed

        // Make sure EF does not try to create a new table for Client
        modelBuilder.Entity<Client>().ToTable("Client"); 

    }
}