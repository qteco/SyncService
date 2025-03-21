using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncService.Core.Models;

using System;
using System.Collections.Generic;

public class ExactClient
{
    [Key]
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string Address { get; set; }
    public Client Client { get; set; }
}    
