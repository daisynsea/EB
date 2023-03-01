using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.IntegrationTests
{
    public class EventMappingSmokeTests
    {
        [Fact]
        public void NeoApproveOrderEvent_MapToOracleCreateOrder_CreatesValidOracleCreateOrder()
        {
            SalesforceNeoApproveOrderPayload eventPayload = new()
            {
                NEO_Id__c = "id123",
                NEO_OrderNumbrer__c = "11111",
                NEO_Internal_Company__c = "intCompany",
                NEO_Bill_To_Name__c = "bill name",
                NEO_Order_Type_Oracle_Sync__c = "order type",
                NEO_Oracle_Bill_to_Contact_ID__c = "oracle bill to contact id",
                NEO_Payment_Term__c = "payment term",
                NEO_Oracle_Account_ID__c = "oracle account id",
                NEO_Requested_Ship_Date__c = new DateTime(2022, 2, 15),
                NEO_Currency__c = "USD"
            };

            OracleCreateOrder createdOrder = eventPayload.MapToOracleCreateOrder();

            createdOrder.OrderKey.Should().Be($"OPS:{eventPayload.NEO_OrderNumbrer__c}");
            createdOrder.SourceTransactionNumber.Should().Be(eventPayload.NEO_Id__c);
            createdOrder.SourceTransactionId.Should().Be(eventPayload.NEO_OrderNumbrer__c);
            createdOrder.SourceTransactionSystem.Should().Be("OPS");
            createdOrder.BusinessUnitName.Should().Be(eventPayload.NEO_Internal_Company__c);
            createdOrder.BuyingPartyName.Should().Be(eventPayload.NEO_Bill_To_Name__c);
            createdOrder.TransactionType.Should().Be(eventPayload.NEO_Order_Type_Oracle_Sync__c);
            createdOrder.FreezePriceFlag.Should().BeFalse();
            createdOrder.FreezeShippingChargeFlag.Should().BeFalse();
            createdOrder.FreezeTaxFlag.Should().BeFalse();
            createdOrder.SubmittedFlag.Should().BeTrue();
            createdOrder.BuyingPartyContactNumber.Should().Be(eventPayload.NEO_Oracle_Bill_to_Contact_ID__c);
            createdOrder.PaymentTerms.Should().Be(eventPayload.NEO_Payment_Term__c);
            createdOrder.BuyingPartyNumber.Should().Be(eventPayload.NEO_Oracle_Account_ID__c);
            createdOrder.RequestedShipDate.Should().Be(eventPayload.NEO_Requested_Ship_Date__c);
            createdOrder.RequestingBusinessUnitName.Should().Be(eventPayload.NEO_Internal_Company__c);
            createdOrder.TransactionalCurrencyCode.Should().Be(eventPayload.NEO_Currency__c);
        }

        [Fact]
        public void NeoApproveOrderEvent_MapToOracleUpdateOrder_CreatesValidOracleUpdateOrder()
        {
            SalesforceNeoApproveOrderPayload eventPayload = new()
            {
                NEO_Id__c = "id123",
                NEO_OrderNumbrer__c = "11111",
                NEO_Internal_Company__c = "intCompany",
                NEO_Bill_To_Name__c = "bill name",
                NEO_Order_Type_Oracle_Sync__c = "order type",
                NEO_Oracle_Bill_to_Contact_ID__c = "oracle bill to contact id",
                NEO_Payment_Term__c = "payment term",
                NEO_Oracle_Account_ID__c = "oracle account id",
                NEO_Requested_Ship_Date__c = new DateTime(2022, 2, 15),
                NEO_Currency__c = "USD"
            };

            Item item = new()
            {
                FreezePriceFlag = true,
                FreezeShippingChargeFlag = true,
                FreezeTaxFlag = true,
                SourceTransactionNumber = "11234"
            };

            OracleUpdateOrder updatedOrder = eventPayload.MapToOracleUpdateOrder(item);

            updatedOrder.OrderKey.Should().Be($"OPS:{eventPayload.NEO_OrderNumbrer__c}");
            updatedOrder.SourceTransactionNumber.Should().Be(eventPayload.NEO_Id__c);
            updatedOrder.SourceTransactionId.Should().Be(eventPayload.NEO_OrderNumbrer__c);
            updatedOrder.SourceTransactionSystem.Should().Be("OPS");
            updatedOrder.BusinessUnitName.Should().Be(eventPayload.NEO_Internal_Company__c);
            updatedOrder.BuyingPartyName.Should().Be(eventPayload.NEO_Bill_To_Name__c);
            updatedOrder.TransactionType.Should().Be(eventPayload.NEO_Order_Type_Oracle_Sync__c);
            updatedOrder.FreezePriceFlag.Should().Be(item.FreezePriceFlag);
            updatedOrder.FreezeShippingChargeFlag.Should().Be(item.FreezeShippingChargeFlag);
            updatedOrder.FreezeTaxFlag.Should().Be(item.FreezeTaxFlag);
            updatedOrder.SubmittedFlag.Should().BeTrue();
            updatedOrder.BuyingPartyContactNumber.Should().Be(eventPayload.NEO_Oracle_Bill_to_Contact_ID__c);
            updatedOrder.PaymentTerms.Should().Be(eventPayload.NEO_Payment_Term__c);
            updatedOrder.BuyingPartyNumber.Should().Be(eventPayload.NEO_Oracle_Account_ID__c);
            updatedOrder.RequestedShipDate.Should().Be(eventPayload.NEO_Requested_Ship_Date__c);
            updatedOrder.RequestingBusinessUnitName.Should().Be(eventPayload.NEO_Internal_Company__c);
            updatedOrder.TransactionalCurrencyCode.Should().Be(eventPayload.NEO_Currency__c);
            updatedOrder.SourceTransactionRevisionNumber.Should().Be(item.SourceTransactionNumber);
        }
    }
}
