namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceQueryObjectModel
    {
        public List<SalesforceQueryRelatedFilesModel> Records { get; set; }
    }

    public class SalesforceQueryRelatedFilesModel
    {
        public string Id { get; set; }
        public string LinkedEntityId { get; set; }
        public string ContentDocumentId { get; set; }
    }
}
