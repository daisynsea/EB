using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IProductsBrokerService
{
    Task<List<SalesforceProductObjectModel>> GetSalesforceProductReport();
}

public class ProductsBrokerService : IProductsBrokerService
{
    private readonly ISalesforceClient _sfClient;

    public ProductsBrokerService(ISalesforceClient salesforceClient)
    {
        _sfClient = salesforceClient;
    }

    public async Task<List<SalesforceProductObjectModel>> GetSalesforceProductReport()
    {

        var productReport = await _sfClient.GetProductReportFromSalesforce();
        var rowDataCells = productReport?.factMap?.TT?.rows?.ToList();
        var reportColumns = productReport?.reportMetadata?.detailColumns?.ToList();
        // find index to get labels and values for rowDataCells
        var indexOfProductCode          = reportColumns?.FindIndex(x => x == "CUSTOMER_PRODUCT_ID");
        var indexOfStage                = reportColumns?.FindIndex(x => x == "Product2.Stage__c");
        var indexOfProductName          = reportColumns?.FindIndex(x => x == "PRODUCT_NAME");
        var indexOfProductGen           = reportColumns?.FindIndex(x => x == "Product2.Product_Gen__c");
        var indexOfProductType          = reportColumns?.FindIndex(x => x == "Product2.ProductType__c");
        var indexOfProductFamily        = reportColumns?.FindIndex(x => x == "FAMILY");
        var indexOfTerminalCategory     = reportColumns?.FindIndex(x => x == "Product2.Terminal_Category__c");
        var indexOfPriceBookName        = reportColumns?.FindIndex(x => x == "NAME");
        var indexOfListPrice            = reportColumns?.FindIndex(x => x == "UNIT_PRICE");
        var indexOfItemDetail           = reportColumns?.FindIndex(x => x == "Product2.ItemDetails__c");
        var indexOfProductDesc          = reportColumns?.FindIndex(x => x == "Product2.cpqProductDescription__c");

        var reportData = new List<SalesforceReportViewModel>();
        for (int i = 0; i < rowDataCells?.Count; i++)
        {
            var row = rowDataCells[i];
            var productCode         = row?.dataCells[indexOfProductCode.GetValueOrDefault()]?.label;      
            var stage               = row?.dataCells[indexOfStage.GetValueOrDefault()]?.label;            
            var productName         = row?.dataCells[indexOfProductName.GetValueOrDefault()]?.label;      
            var productGen          = row?.dataCells[indexOfProductGen.GetValueOrDefault()]?.label;       
            var productType         = row?.dataCells[indexOfProductType.GetValueOrDefault()]?.label;      
            var productFamily       = row?.dataCells[indexOfProductFamily.GetValueOrDefault()]?.label;    
            var terminalCategory    = row?.dataCells[indexOfTerminalCategory.GetValueOrDefault()]?.label; 
            var priceBookName       = row?.dataCells[indexOfPriceBookName.GetValueOrDefault()]?.label;    
            var listPrice           = row?.dataCells[indexOfListPrice.GetValueOrDefault()]?.label;        
            var itemDetail          = row?.dataCells[indexOfItemDetail.GetValueOrDefault()]?.label;       
            var productDesc         = row?.dataCells[indexOfProductDesc.GetValueOrDefault()]?.label;      

            var listPriceObj = row?.dataCells[indexOfListPrice.GetValueOrDefault()]?.value;

            listPrice = Convert.ToString(((JValue)((JProperty)((JContainer)listPriceObj).First).Value).Value);
            reportData.Add(new SalesforceReportViewModel
            {
                ProductCode         = Convert.ToString(productCode),
                Stage               = Convert.ToString(stage),
                ProductName         = Convert.ToString(productName),
                ProductGen          = Convert.ToString(productGen),
                ProductType         = Convert.ToString(productType),
                ProductFamily       = Convert.ToString(productFamily),
                TerminalCategory    = Convert.ToString(terminalCategory),
                PriceBookName       = Convert.ToString(priceBookName),
                ListPrice           = Convert.ToString(listPrice),
                ItemDetail          = Convert.ToString(itemDetail),
                ProductDesc         = Convert.ToString(productDesc)
            });

        }

        var accessories = reportData?.Where(r => r.ProductFamily == "Accessories")?.ToList();
        var distinctAccessories = accessories?.DistinctBy(a => a.ProductCode)?.ToList();
        var products = new List<SalesforceProductObjectModel>();
        for (int i = 0; i < distinctAccessories?.Count; i++)
        {
            SalesforceReportViewModel? reportProduct = distinctAccessories[i];
            // there will be two records for product code, with Wholesale price, MSRP price
            var wholesalePrice = accessories?.Where(a => a?.PriceBookName == "Wholesale" && a?.ProductCode == reportProduct?.ProductCode)?.Select(a => a?.ListPrice)?.FirstOrDefault();
            var isWholesalePriceInt = int.TryParse(wholesalePrice, out int wholesalePriceInt);
            var msrpPrice = accessories?.Where(a => a?.PriceBookName == "MSRP" && a?.ProductCode == reportProduct?.ProductCode)?.Select(a => a?.ListPrice)?.FirstOrDefault();
            var isMsrpPriceInt = int.TryParse(msrpPrice, out int msrpPriceInt);
            var kitStr = accessories?.Where(a => a?.ProductCode == reportProduct?.ProductCode)?.Select(a => a?.ProductDesc)?.FirstOrDefault();
            products.Add(new SalesforceProductObjectModel
            {
                Id = reportProduct?.ProductCode,
                Name = reportProduct?.ProductName,
                WholesalePrice = wholesalePriceInt,
                MsrpPrice = msrpPriceInt,
                ProductType = ProductType.accessory,
                Kit = kitStr?.Split(",")
            });
        }

        return products;
    }
}