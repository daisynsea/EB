//using System.Xml.Serialization;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


//// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
//[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
//public class FindOrganizationEnvelope
//{
//    /// <remarks/>
//    public FindOrganizationEnvelopeHeader Header { get; set; }

//    /// <remarks/>
//    public FindOrganizationEnvelopeBody Body { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
//public class FindOrganizationEnvelopeHeader
//{
//    /// <remarks/>
//    [XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
//    public string Action { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
//    public string MessageID { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
//public class FindOrganizationEnvelopeBody
//{
//    /// <remarks/>
//    [XmlElement("findOrganizationResponse", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/")]
//    public findOrganizationResponse findOrganizationResponse { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/")]
//[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/", IsNullable = false)]
//public class findOrganizationResponse
//{
//    /// <remarks/>
//    public findOrganizationResponseResult result { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/", TypeName = "OrganizationPartyResult")]
//public class findOrganizationResponseResult
//{
//    /// <remarks/>
//    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
//    public FindOrganizationValue Value { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
//[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/", IsNullable = false)]
//public class FindOrganizationValue
//{
//    /// <remarks/>
//    public uint PartyNumber { get; set; }

//    /// <remarks/>
//    public ulong PartyId { get; set; }

//    /// <remarks/>
//    public string PartyName { get; set; }

//    /// <remarks/>
//    public FindOrganizationValueOriginalSystemReference OriginalSystemReference { get; set; }

//    /// <remarks/>
//    [XmlElement("PartySite")]
//    public FindOrganizationValuePartySite[] PartySite { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
//public class FindOrganizationValueOriginalSystemReference
//{
//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public ulong OrigSystemReferenceId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public string OrigSystem { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string OrigSystemReference { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string OwnerTableName { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public ulong OwnerTableId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string Status { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object ReasonCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object OldOrigSystemReference { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
//    public System.DateTime StartDateActive { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
//    public System.DateTime EndDateActive { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CreatedBy { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public System.DateTime CreationDate { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string LastUpdatedBy { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public System.DateTime LastUpdateDate { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string LastUpdateLogin { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public byte ObjectVersionNumber { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CreatedByModule { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public ulong PartyId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object RequestId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public FindOrganizationSourceSystemRefInformation SourceSystemRefInformation { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = false)]
//public class FindOrganizationSourceSystemRefInformation
//{
//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/")]
//    public ulong OrigSystemRefId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/", IsNullable = true)]
//    public object @__FLEX_Context { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/", IsNullable = true)]
//    public object @__FLEX_Context_DisplayValue { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/sourceSystemRef/")]
//    public byte _FLEX_NumOfSegments { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
//public class FindOrganizationValuePartySite
//{
//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public ulong PartySiteId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public ulong PartyId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public ulong LocationId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public System.DateTime LastUpdateDate { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public uint PartySiteNumber { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string LastUpdatedBy { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public System.DateTime CreationDate { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CreatedBy { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string LastUpdateLogin { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object RequestId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public string OrigSystem { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string OrigSystemReference { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
//    public System.DateTime StartDateActive { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
//    public System.DateTime EndDateActive { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Mailstop { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public bool IdentifyingAddressFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Language { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string Status { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PartySiteName { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Addressee { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CreatedByModule { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object GlobalLocationNumber { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object DUNSNumberC { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Comments { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PartySiteType { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PartyNameDba { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PartyNameDivision { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PartyNameLegal { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object RelationshipId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PartyUsageCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object UsageCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string FormattedAddress { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string Country { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string Address1 { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Address2 { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Address3 { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Address4 { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string City { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public uint PostalCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object State { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Province { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object County { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object AddressStyle { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public bool ValidatedFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object AddressLinesPhonetic { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object PostalPlus4Code { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Position { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object LocationDirections { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object AddressExpirationDate { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object LocationLanguage { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object ValidationStatusCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object DateValidated { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object DoNotValidateFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object HouseType { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string FormattedMultilineAddress { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string Country1 { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public byte ObjectVersionNumber1 { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public bool ContactPreferenceExistFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object FormattedLocaleAddress { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CurrencyCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CorpCurrencyCode { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public string CurcyConvRateType { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public bool InternalFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public bool OverallPrimaryFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object EmailAddress { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Latitude { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
//    public object Longitude { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public FindOrganizationPartySiteUse PartySiteUse { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//    public FindOrganizationPartySiteInformation PartySiteInformation { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = false)]
//public class FindOrganizationPartySiteUse
//{
//    /// <remarks/>
//    public ulong PartySiteUseId { get; set; }

//    /// <remarks/>
//    [XmlElement(DataType = "date")]
//    public System.DateTime BeginDate { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Comments { get; set; }

//    /// <remarks/>
//    [XmlElement(DataType = "date")]
//    public System.DateTime EndDate { get; set; }

//    /// <remarks/>
//    public ulong PartySiteId { get; set; }

//    /// <remarks/>
//    public System.DateTime LastUpdateDate { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object RequestId { get; set; }

//    /// <remarks/>
//    public string LastUpdatedBy { get; set; }

//    /// <remarks/>
//    public System.DateTime CreationDate { get; set; }

//    /// <remarks/>
//    public string CreatedBy { get; set; }

//    /// <remarks/>
//    public string LastUpdateLogin { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object IntegrationKey { get; set; }

//    /// <remarks/>
//    public string SiteUseType { get; set; }

//    /// <remarks/>
//    public string PrimaryPerType { get; set; }

//    /// <remarks/>
//    public string Status { get; set; }

//    /// <remarks/>
//    public byte ObjectVersionNumber { get; set; }

//    /// <remarks/>
//    public string CreatedByModule { get; set; }
//}

///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
//[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = false)]
//public class FindOrganizationPartySiteInformation
//{
//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/")]
//    public ulong PartySiteId { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/", IsNullable = true)]
//    public object @__FLEX_Context { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/", IsNullable = true)]
//    public object @__FLEX_Context_DisplayValue { get; set; }

//    /// <remarks/>
//    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/flex/partySite/")]
//    public byte _FLEX_NumOfSegments { get; set; }
//}

