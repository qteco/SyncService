using System.ComponentModel.DataAnnotations;

namespace SyncService.Core.Models;
public class ExactClient
{
    [Key]
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Address { get; set; }
    public Client Client { get; set; }
}
