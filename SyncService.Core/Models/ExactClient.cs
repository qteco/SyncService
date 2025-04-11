using System.ComponentModel.DataAnnotations;

namespace SyncService.Core.Models;
public class ExactClient
{
    [Key]
    public required string Code { get; set; } 
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostCode { get; set; }
    public string? Status { get; set; }
    public string? Website { get; set; }
    public Client Client { get; set; }
}
