namespace SyncService.Core.Models;
public class ClientSite
{
    public required string? Id {get; set;}
    public required string? Name {get; set;}
    public required string? City {get; set;}
    public required string? PostalCode {get; set;}
    public string? CountryCode {get; set;}
    public string? ContactNumber {get; set;}
    public string? StateCode {get; set;}
    public string? Line1 {get; set;}
    public string? Line2 {get; set;}
    public string? Line3 {get; set;}
    public List<BusinessHour>? BusinessHour {get; set;}
    public List<string>? HolidayList {get; set;}
    public string? TimezoneCode { get; set; }
    public bool? Working24x7  { get; set;}
    public string? ClientId {get; set;}
}

public class BusinessHour
{
    public Guid Id {get; set;}
    public string? Day {get; set;}
    public string? Start {get; set;}
    public string? End {get; set;}
    public string AccountId {get; set;} 
    public ClientSite ClientSite {get; set;}
}