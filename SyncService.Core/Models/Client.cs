using System.ComponentModel.DataAnnotations.Schema;

namespace SyncService.Core.Models;
public class Client
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public required string AccountId { get; set; }
    public string? Stage { get; set; }
    public string? Status { get; set; }
    public List<string>? EmailDomains { get; set; }
    public AccountManager? AccountManager { get; set; }
    public PrimaryContact? PrimaryContact { get; set; }
    public SecondaryContact? SecondaryContact { get; set; }
    public HqSite? HqSite { get; set; }
    
    [NotMapped]
    public List<object>? TechnicianGroups { get; set; }
    
    [NotMapped]
    public object? CustomFields { get; set; }
    public List<ClientSite>? ClientSites { get; set; }
    public required string ExactId { get; set; } 
}

public class AccountManager
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
}

public class PrimaryContact
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
}

public class SecondaryContact
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
}

public class HqSite
{
    public string Id { get; set; }
    public string Name { get; set; }
}
   
