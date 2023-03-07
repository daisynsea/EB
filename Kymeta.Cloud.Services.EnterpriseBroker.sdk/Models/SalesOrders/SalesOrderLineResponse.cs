
public class SalesOrderLineResponse
{
    public int TotalSize { get; set; }
    public bool Done { get; set; }
    public IEnumerable<SalesOrderLineItems> Records { get; set; }
}

public class SalesOrderLineItems
{
    public Attributes Attributes { get; set; }
    public string Id { get; set; }
    public string Product_Code__c { get; set; }
    public float Quantity { get; set; }
    public string OrderItemNumber { get; set; }
    public string OrderId { get; set; }
    public bool NEO_Sync_to_Oracle__c { get; set; }
    public float UnitPrice { get; set; }
}

public class Attributes
{
    public string Type { get; set; }
    public string Url { get; set; }
}
