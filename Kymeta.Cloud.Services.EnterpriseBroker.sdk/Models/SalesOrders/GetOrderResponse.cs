using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
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

        public Item FindLatestRevision()
        {
            return Items.OrderByDescending(item => item.LastUpdateDate).First();
        }
    }

    public class Item
    {
        public long HeaderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? SourceTransactionNumber { get; set; }
        public string? SourceTransactionSystem { get; set; }
        public string? SourceTransactionId { get; set; }
        public long? BusinessUnitId { get; set; }
        public string? BusinessUnitName { get; set; }
        public DateTime TransactionOn { get; set; }
        public long? BuyingPartyId { get; set; }
        public string? BuyingPartyName { get; set; }
        public string? BuyingPartyNumber { get; set; }
        public string? BuyingPartyPersonFirstName { get; set; }
        public string? BuyingPartyPersonLastName { get; set; }
        public string? BuyingPartyPersonMiddleName { get; set; }
        public string? BuyingPartyPersonNameSuffix { get; set; }
        public string? BuyingPartyPersonTitle { get; set; }
        public long? BuyingPartyContactId { get; set; }
        public string? BuyingPartyContactName { get; set; }
        public string? BuyingPartyContactNumber { get; set; }
        public string? BuyingPartyContactFirstName { get; set; }
        public string? BuyingPartyContactLastName { get; set; }
        public string? BuyingPartyContactMiddleName { get; set; }
        public string? BuyingPartyContactNameSuffix { get; set; }
        public string? BuyingPartyContactTitle { get; set; }
        public long? PreferredSoldToContactPointId { get; set; }
        public string? CustomerPONumber { get; set; }
        public string? TransactionTypeCode { get; set; }
        public string? TransactionType { get; set; }
        public bool? SubstituteAllowedFlag { get; set; }
        public string? PackingInstructions { get; set; }
        public string? ShippingInstructions { get; set; }
        public bool? ShipsetFlag { get; set; }
        public bool? PartialShipAllowedFlag { get; set; }
        public DateTime? RequestedShipDate { get; set; }
        public DateTime? RequestedArrivalDate { get; set; }
        public DateTime? LatestAcceptableShipDate { get; set; }
        public DateTime? LatestAcceptableArrivalDate { get; set; }
        public DateTime? EarliestAcceptableShipDate { get; set; }
        public string? ShipmentPriorityCode { get; set; }
        public string? ShipmentPriority { get; set; }
        public string? ShippingCarrierId { get; set; }
        public string? ShippingCarrier { get; set; }
        public string? ShippingServiceLevelCode { get; set; }
        public string? ShippingServiceLevel { get; set; }
        public string? ShippingModeCode { get; set; }
        public string? ShippingMode { get; set; }
        public string? FOBPointCode { get; set; }
        public string? FOBPoint { get; set; }
        public string? DemandClassCode { get; set; }
        public string? DemandClass { get; set; }
        public string? FreightTermsCode { get; set; }
        public string? FreightTerms { get; set; }
        public long? RequestedFulfillmentOrganizationId { get; set; }
        public string? RequestedFulfillmentOrganizationCode { get; set; }
        public string? RequestedFulfillmentOrganizationName { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierSiteId { get; set; }
        public int? PaymentTermsCode { get; set; }
        public string? PaymentTerms { get; set; }
        public string? SalespersonId { get; set; }
        public string? Salesperson { get; set; }
        public string? PricedOn { get; set; }
        public string? TransactionalCurrencyCode { get; set; }
        public string? TransactionalCurrencyName { get; set; }
        public string? AppliedCurrencyCode { get; set; }
        public string? CurrencyConversionDate { get; set; }
        public string? CurrencyConversionRate { get; set; }
        public string? CurrencyConversionType { get; set; }
        public string? PricingSegmentCode { get; set; }
        public string? PricingSegment { get; set; }
        public long? PricingStrategyId { get; set; }
        public string? PricingStrategyName { get; set; }
        public bool? AllowCurrencyOverrideFlag { get; set; }
        public string? SegmentExplanationMessageName { get; set; }
        public string? PricingSegmentExplanation { get; set; }
        public string? StrategyExplanationMessageName { get; set; }
        public string? PricingStrategyExplanation { get; set; }
        public string? SalesChannelCode { get; set; }
        public string? SalesChannel { get; set; }
        public string? Comments { get; set; }
        public string? StatusCode { get; set; }
        public string? Status { get; set; }
        public bool? OnHoldFlag { get; set; }
        public bool? CanceledFlag { get; set; }
        public string? CancelReasonCode { get; set; }
        public string? CancelReason { get; set; }
        public DateTime? RequestedCancelDate { get; set; }
        public long? RequestingBusinessUnitId { get; set; }
        public string? RequestingBusinessUnitName { get; set; }
        public long? RequestingLegalEntityId { get; set; }
        public string? RequestingLegalEntity { get; set; }
        public bool FreezePriceFlag { get; set; }
        public bool FreezeShippingChargeFlag { get; set; }
        public bool FreezeTaxFlag { get; set; }
        public bool SubmittedFlag { get; set; }
        public string? SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string? TransactionDocumentTypeCode { get; set; }
        public bool? PreCreditCheckedFlag { get; set; }
        public string? RevisionSourceSystem { get; set; }
        public int? SourceTransactionRevisionNumber { get; set; }
        public string? TradeComplianceResultCode { get; set; }
        public string? TradeComplianceResult { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? OpenFlag { get; set; }
        public string? OrigSystemDocumentReference { get; set; }
        public string? OrderKey { get; set; }
        public string? AppliedCurrencyName { get; set; }
        public string? SupplierSiteName { get; set; }
        public string? MessageText { get; set; }
        public string? AgreementHeaderId { get; set; }
        public string? AgreementNumber { get; set; }
        public string? AgreementVersionNumber { get; set; }
        public Link[] links { get; set; }
    }
}