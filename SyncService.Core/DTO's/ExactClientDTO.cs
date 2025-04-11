using Newtonsoft.Json;

public class ExactClientDTO
{
    [JsonProperty("Code")]
    public string Code { get; set; }
    
    [JsonProperty("Id")]
    public string Id { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("AddressLine1")] 
    public string AddressLine1 { get; set; }
    
    [JsonProperty("Email")]
    public string Email {get; set;}
    
    [JsonProperty("City")]
    public string? City { get; set; }
    
    [JsonProperty("Country")]
    public string? Country { get; set; }
    
    [JsonProperty("Postcode")]
    public string? Postcode { get; set; }
    
    [JsonProperty("Status")]
    public string? Status { get; set; }
    
    [JsonProperty("Website")]
    public string? Website { get; set; }
}