using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class FindCustomerAccountEnvelope
{
    /// <remarks/>
    public FindCustomerAccountEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public FindCustomerAccountEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FindCustomerAccountEnvelopeHeader
{
    /// <remarks/>
    [XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string Action { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string MessageID { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class FindCustomerAccountEnvelopeBody
{
    /// <remarks/>
    [XmlElement("findCustomerAccountResponse", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/")]
    public findCustomerAccountResponse findCustomerAccountResponse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/applicationModule/types/", IsNullable = false)]
public class findCustomerAccountResponse
{
    /// <remarks/>
    public findCustomerAccountResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/", TypeName = "CustomerAccountResult")]
public class findCustomerAccountResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
    public FindCustomerAccountValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/", IsNullable = false)]
public class FindCustomerAccountValue
{
    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public uint AccountNumber { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object OrigSystem { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string CustomerType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CustomerClassCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxHeaderLevelFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxRoundingRule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CoterminateDayMonth { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime AccountEstablishedDate { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime AccountTerminationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object HeldBillExpirationDate { get; set; }

    /// <remarks/>
    public bool HoldBillFlag { get; set; }

    /// <remarks/>
    public string AccountName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DepositRefundMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object NpaNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DateTypePreference { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ArrivalsetsIncludeLinesFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object StatusUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AutopayFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LastBatchId { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SellingPartyId { get; set; }

    /// <remarks/>
    [XmlElement("CustomerAccountContact")]
    public ValueCustomerAccountContact[] CustomerAccountContact{ get; set; }

    /// <remarks/>
    [XmlElement("CustomerAccountSite")]
    public ValueCustomerAccountSite[] CustomerAccountSite { get; set; }

    /// <remarks/>
    public ValueCustAcctInformation CustAcctInformation { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountContact
{
    /// <remarks/>
    public ulong CustomerAccountRoleId { get; set; }

    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CustomerAccountSiteId { get; set; }

    /// <remarks/>
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    public string RoleType { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceCode { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public ulong RelationshipId { get; set; }

    /// <remarks/>
    public ulong ContactPersonId { get; set; }

    /// <remarks/>
    public ValueCustomerAccountContactCustomerAccountContactRole CustomerAccountContactRole { get; set; }

    /// <remarks/>
    public ValueCustomerAccountContactCustAcctContactInformation CustAcctContactInformation { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountContactCustomerAccountContactRole
{
    /// <remarks/>
    public ulong ResponsibilityId { get; set; }

    /// <remarks/>
    public ulong CustomerAccountRoleId { get; set; }

    /// <remarks/>
    public string ResponsibilityType { get; set; }

    /// <remarks/>
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public string StatusCode { get; set; }

    /// <remarks/>
    public ValueCustomerAccountContactCustomerAccountContactRoleCustAcctContactRoleInformation CustAcctContactRoleInformation { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountContactCustomerAccountContactRoleCustAcctContactRoleInformation
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContactRole/")]
    public ulong ResponsibilityId { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountContactCustAcctContactInformation
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/")]
    public ulong CustAccountRoleId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/", IsNullable = true)]
    public object @__FLEX_Context { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/", IsNullable = true)]
    public object @__FLEX_Context_DisplayValue { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountContact/")]
    public byte _FLEX_NumOfSegments { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountSite
{
    /// <remarks/>
    public ulong CustomerAccountSiteId { get; set; }

    /// <remarks/>
    public ulong CustomerAccountId { get; set; }

    /// <remarks/>
    public ulong PartySiteId { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string BillToIndicator { get; set; }

    /// <remarks/>
    public string MarketIndicator { get; set; }

    /// <remarks/>
    public string ShipToIndicator { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CustomerCategoryCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Language { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object KeyAccountFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TpHeaderId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object EceTpLocationCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TranslatedCustomerName { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public ulong SetId { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime StartDate { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime EndDate { get; set; }

    /// <remarks/>
    public string SetCode { get; set; }

    /// <remarks/>
    public ValueCustomerAccountSiteCustomerAccountSiteUse CustomerAccountSiteUse { get; set; }

    /// <remarks/>
    public ValueCustomerAccountSiteCustAcctSiteInformation CustAcctSiteInformation { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountSiteCustomerAccountSiteUse
{
    /// <remarks/>
    public ulong SiteUseId { get; set; }

    /// <remarks/>
    public ulong CustomerAccountSiteId { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string SiteUseCode { get; set; }

    /// <remarks/>
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public uint Location { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object BillToSiteUseId { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SICCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PaymentTermId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object GSAIndicator { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TerritoryId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxReference { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SortPriority { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SecondLastAccrueChargeDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LastAccrueChargeDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LastUnaccrueChargeDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SecondLastUnaccrueChrgDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxHeaderLevelFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FinchrgReceivablesTrxId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxRoundingRule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TaxClassification { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public ulong SetId { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime EndDate { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public DateTime StartDate { get; set; }

    /// <remarks/>
    public ValueCustomerAccountSiteCustomerAccountSiteUseCustAcctSiteUseInformation CustAcctSiteUseInformation { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountSiteCustomerAccountSiteUseCustAcctSiteUseInformation
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/")]
    public ulong SiteUseId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/", IsNullable = true)]
    public object @__FLEX_Context { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/", IsNullable = true)]
    public object @__FLEX_Context_DisplayValue { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSiteUse/")]
    public byte _FLEX_NumOfSegments { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustomerAccountSiteCustAcctSiteInformation
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/")]
    public ulong CustAcctSiteId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/", IsNullable = true)]
    public object @__FLEX_Context { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/", IsNullable = true)]
    public object @__FLEX_Context_DisplayValue { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccountSite/")]
    public byte _FLEX_NumOfSegments { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/customerAccountService/")]
public class ValueCustAcctInformation
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public ulong CustAccountId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public string salesforceId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public string ksnId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/", IsNullable = true)]
    public object @__FLEX_Context { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/", IsNullable = true)]
    public object @__FLEX_Context_DisplayValue { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/custAccount/")]
    public byte _FLEX_NumOfSegments { get; set; }
}

