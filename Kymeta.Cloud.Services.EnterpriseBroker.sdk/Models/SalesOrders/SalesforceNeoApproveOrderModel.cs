using Kymeta.Cloud.Services.Toolbox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record SalesforceNeoApproveOrderModel
{
    public SalesforceNeoApproveOrderData Data { get; init; } = null!;
    public string Channel { get; init; } = null!;

    public OracleUpdateOrder MapToOracleUpdateOrder(Item latestRevision)
    {
        return Data.Payload.MapToOracleUpdateOrder(latestRevision);
    }

    public OracleCreateOrder MapToOracleCreateOrder()
    {
        return Data.Payload.MapToOracleCreateOrder();
    }

    public OracleCreateOrder MapToOracleCreateOrderWithLines(IEnumerable<SalesOrderLineItems> salesforceOrderLines)
    {
        OracleCreateOrder order = Data.Payload.MapToOracleCreateOrder();
        order.Lines = MapToOracleLines(salesforceOrderLines);
        return order;
    }

    private IEnumerable<OrderLines> MapToOracleLines(IEnumerable<SalesOrderLineItems> salesforceOrderLines)
    {
        return salesforceOrderLines.Select(x => new OrderLines
        {
            ProductNumber = x.Product_Code__c,
            OrderedQuantity = x.Quantity, 
            SourceTransactionLineId = x.Id,
            SourceTransactionLineNumber = x.OrderItemNumber,
            SourceTransactionScheduleId = x.Id,
            SourceScheduleNumber = x.OrderItemNumber,
            TransactionCategoryCode = "ORDER",
            TransactionLineType = "Buy",
            OrderedUOM = "EA",
            // AdditionalInformation = new AdditionalInformation()
            // {
            //     FulfillLineEffBSFDCprivateVO = new FullfillLine(){ContextCode = "SFDC", sfOrderProduct = x.Id}
            // },
            // ManualPriceAdjustments = new ManualPriceAdjustments()
            // {
            //     AdjustmentAmount = x.UnitPrice,
            //     SourceManualPriceAdjustmentId = x.Id
            // }
        });
    }
}

public record SalesforceNeoApproveOrderData
{
    public string? Schmea { get; init; }
    public SalesforceNeoApproveOrderPayload Payload { get; init; } = null!;
    public SalesorderEventModel Event { get; init; } = null!;
}



public record SalesforceNeoApproveOrderPayload
{
    public string? NEO_Oracle_Bill_to_Address_ID__c { get; init; }
    public string? NEO_Id__c { get; init; }
    public string? NEO_Preferred_Contract_Method__c { get; init; }
    public string? NEO_Account_Name__c { get; init; }
    public string? NEO_Ship_to_Name__c { get; init; }
    public string? NEO_Deleted_Item_Id__c { get; init; }
    public string? CreatedById { get; init; }
    public string? NEO_Internal_Company__c { get; init; }
    public string? NEO_Event_Type__c { get; init; }
    public string? NEO_Sales_Representative__c { get; init; }
    public string? NEO_OrderNumbrer__c { get; init; }
    public string? NEO_Oracle_Ship_to_Address_ID__c { get; init; }
    public string? NEO_Oracle_Integration_Status__c { get; init; }
    public string? Neo_Oracle_Sync_Status__c { get; init; }
    public string? NEO_PO_Number__c { get; init; }
    public string? NEO_Primary_Contract__c { get; init; }
    public DateOnly? NEO_PO_Date__c { get; init; }
    public string? NEO_Order_Type_Oracle_Sync__c { get; init; }
    public string? NEO_Oracle_SO__c { get; init; }
    public string? NEO_ApprovalStatus__c { get; init; }
    public string? NEO_Oracle_Account_ID__c { get; init; }
    public string? NEO_Oracle_Primary_Contact_ID__c { get; init; }
    public string? NEO_Order_Status__c { get; init; }
    public string? CreatedDate { get; init; }
    public string? NEO_Oracle_Bill_to_Contact_ID__c { get; init; }
    public string? NEO_Bill_To_Name__c { get; init; }

    public string? NEO_Payment_Term__c { get; set; }
    public string? NEO_Currency__c { get; set; }
    public DateTime? NEO_Requested_Ship_Date__c { get; set; }
    public string? NEO_Oracle_Ship_to_Contact_ID__c { get; set; }

    public bool IsValid()
    {
        return NEO_Id__c.IsNotEmpty();

    }

    public OracleCreateOrder MapToOracleCreateOrder()
    {
        return new OracleCreateOrder
        {
            SourceTransactionNumber = NEO_Id__c,
            SourceTransactionId = NEO_OrderNumbrer__c,
            OrderKey = $"OPS:{NEO_OrderNumbrer__c}",
            SourceTransactionSystem = "OPS",
            BusinessUnitName = NEO_Internal_Company__c,
            BuyingPartyName = NEO_Bill_To_Name__c,
            TransactionType = NEO_Order_Type_Oracle_Sync__c,
            FreezePriceFlag = false,
            FreezeShippingChargeFlag = false,
            FreezeTaxFlag = false,
            SubmittedFlag = true,
            BuyingPartyContactNumber = NEO_Oracle_Bill_to_Contact_ID__c,
            PaymentTerms = NEO_Payment_Term__c,
            BuyingPartyNumber = NEO_Oracle_Account_ID__c,
            RequestedShipDate = NEO_Requested_Ship_Date__c,
            RequestingBusinessUnitName = NEO_Internal_Company__c,
            TransactionalCurrencyCode = NEO_Currency__c,
            BillToCustomer = new List<CustomerBill> { new CustomerBill() { AccountNumber = NEO_Oracle_Account_ID__c, ContactNumber = NEO_Oracle_Bill_to_Contact_ID__c } },
            ShipToCustomer = new List<CustomerShip> { new CustomerShip() {  ContactNumber = NEO_Oracle_Ship_to_Contact_ID__c } } 
        };
    }


    public OracleUpdateOrder MapToOracleUpdateOrder(Item latestRevision)
    {
        return new OracleUpdateOrder()

        {
            SourceTransactionNumber = NEO_Id__c,
            SourceTransactionId = NEO_OrderNumbrer__c,
            OrderKey = $"OPS:{NEO_OrderNumbrer__c}",
            SourceTransactionSystem = "OPS",
            BusinessUnitName = NEO_Internal_Company__c,
            BuyingPartyName = NEO_Bill_To_Name__c,
            TransactionType = NEO_Order_Type_Oracle_Sync__c,
            FreezePriceFlag = latestRevision.FreezePriceFlag,
            FreezeShippingChargeFlag = latestRevision.FreezeShippingChargeFlag,
            FreezeTaxFlag = latestRevision.FreezeTaxFlag,
            SubmittedFlag = true, //check about this
            BuyingPartyContactNumber = NEO_Oracle_Bill_to_Contact_ID__c,
            PaymentTerms = NEO_Payment_Term__c,
            BuyingPartyNumber = NEO_Oracle_Account_ID__c,
            RequestedShipDate = NEO_Requested_Ship_Date__c,
            RequestingBusinessUnitName = NEO_Internal_Company__c,
            SourceTransactionRevisionNumber = latestRevision.SourceTransactionNumber,
            TransactionalCurrencyCode = NEO_Currency__c,
        };
    }
}

