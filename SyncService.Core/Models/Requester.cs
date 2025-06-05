using System.ComponentModel.DataAnnotations;
using SyncService.Core.Models;

public class Requester
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string ContactNumber { get; set; }
    public string AccountId { get; set; }  // Foreign key
    public Client Client { get; set; }     // Navigation property
}