public class ServiceItem
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string QuantityType { get; set; } 
    public ServiceCategory Category { get; set; }
    public bool UseAsWorklogItem { get; set; }
    public string UnitPrice { get; set; } 
    public string BusinessHoursUnitPrice { get; set; }
    public string AfterHoursUnitPrice { get; set; }
    public int RoundUpValue { get; set; }
    public string Quantity { get; set; }
    public string Amount { get; set; }
    public bool AdjustBlockItemAgainstAllItems { get; set; }
    public List<ServiceItem> BlockItemAdjustedItems { get; set; }
    public string BlockItemUsedIn { get; set; } 
    public bool SalesTaxEnabled { get; set; }
}
public class ServiceCategory
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Tax
{
    public string TaxId { get; set; }
    public string Name { get; set; }
    public string Rates { get; set; }
    public string TotalRate { get; set; }
}
