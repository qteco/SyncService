public class Ticket
{
    public string AccountId { get; set; }
    public string Requesterid { get; set; }
    public string Subject { get; set; }
    public string Category { get; set; }
    public string Status { get; set; }
    public string TicketType { get; set; }
    public string Source { get; set; }
    public int TicketId { get; private set; }
    private static int nextTicketId = 1;
    
    public Ticket(string accountId, string requesterid)
    {
        AccountId = accountId;
        Requesterid = requesterid;
        TicketId = nextTicketId++;
        Subject = $"Call Ticket: {TicketId}";
        Category = "Overig";
        Status = "Open";
        TicketType = "INCIDENT";
        Source = "FORM";
    }
}
