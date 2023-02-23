using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients
{
    public class GetOrderResponse : IOracleResponsePayload
    {

        public IEnumerable<Item> Items { get; set; }
        public int Count { get; set; }
        public bool HasMore { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public IEnumerable<Link> Links { get; set; }
        public bool IsSuccessfulResponse()
        {
            return Items.Any();
        }
    }

}

public class Item
{
    public long HeaderId { get; set; }
    public string OrderNumber { get; set; }
    public string SourceTransactionNumber { get; set; }
    public string SourceTransactionSystem { get; set; }
    public string SourceTransactionId { get; set; }
    public long BusinessUnitId { get; set; }
    public string BusinessUnitName { get; set; }
}

public class Link
{
    public string Rel { get; set; }
    public string Href { get; set; }
    public string Name { get; set; }
    public string Kind { get; set; }
}
