using Kymeta.Cloud.Services.Toolbox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{

    public record OracleUpdateOrder
    {
        public string? SourceTransactionNumber { get; set; }
        public string? SourceTransactionId { get; set; }
        public string? OrderKey { get; set; }
        public string? SourceTransactionSystem { get; set; }
        public string? BusinessUnitName { get; set; }
        public string? BuyingPartyName { get; set; }
        public string? BuyingPartyNumber { get; set; }
        public string? BuyingPartyContactNumber { get; set; }
        public string? TransactionType { get; set; }
        public DateTime? RequestedShipDate { get; set; }
        public string? PaymentTerms { get; set; }
        public string? TransactionalCurrencyCode { get; set; }
        public string? RequestingBusinessUnitName { get; set; }
        public bool FreezePriceFlag { get; set; }
        public bool FreezeShippingChargeFlag { get; set; }
        public bool FreezeTaxFlag { get; set; }
        public bool SubmittedFlag { get; set; }
        public string? SourceTransactionRevisionNumber { get; set; }

        public bool IsValid()
        {
            return SourceTransactionId.IsNotEmpty();
        }
    }
}