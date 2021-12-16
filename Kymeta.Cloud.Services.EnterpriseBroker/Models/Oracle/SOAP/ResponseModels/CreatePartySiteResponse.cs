using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class PartySiteEnvelope
{
    /// <remarks/>
    public PartySiteEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public PartySiteEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class PartySiteEnvelopeHeader
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
[XmlType("mergeOrganizationResponse", AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class PartySiteEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/")]
    public mergeOrganizationResponse mergeOrganizationResponse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/applicationModule/types/", IsNullable = false)]
public partial class mergeOrganizationResponse
{
    /// <remarks/>
    public mergeOrganizationResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/", TypeName = "OrganizationPartyResult")]
public partial class mergeOrganizationResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
    public PartySiteValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/", IsNullable = false)]
public partial class PartySiteValue
{
    /// <remarks/>
    public uint PartyNumber { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public string PartyType { get; set; }

    /// <remarks/>
    public string PartyName { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ValidatedFlag { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    public System.DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object HQBranchIndicator { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DUNSNumberC { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SICCode { get; set; }

    /// <remarks/>
    public string JgzzFiscalCode { get; set; }

    /// <remarks/>
    public string Address1 { get; set; }

    /// <remarks/>
    public string Country { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address3 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object GroupType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address2 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address4 { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string City { get; set; }

    /// <remarks/>
    public uint PostalCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object County { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object YearEstablished { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Province { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object State { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object URL { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AnalysisFy { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SICCodeType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object EmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FiscalYearendMonth { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object EmployeesTotal { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CurrentFiscalYearPotentialRevenueAmount { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object NextFyPotentialRevenueAmount { get; set; }

    /// <remarks/>
    public bool GSAIndicatorFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CategoryCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LanguageName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object MissionStatement { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ThirdPartyFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object HomeCountry { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CertificationLevel { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CertReasonCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhonePurpose { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneLineType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneCountryCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneAreaCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneExtension { get; set; }

    /// <remarks/>
    public ulong IdenAddrLocationId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryURLContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryEmailContactPointId { get; set; }

    /// <remarks/>
    public ulong IdenAddrPartySiteId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonSecondLastName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredNameId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object TradingPartnerIdentifier { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactPersonId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredFunctionalCurrency { get; set; }

    /// <remarks/>
    public bool InternalFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CeoName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrincipalName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object OrganizationSize { get; set; }

    /// <remarks/>
    public string PartyUniqueName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceSystem { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceSystemReferenceValue { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceSystemUpdateDate { get; set; }

    /// <remarks/>
    public PartySiteValuePartySite PartySite { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/organizationService/")]
public partial class PartySiteValuePartySite
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong PartySiteId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong PartyId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong LocationId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public uint PartySiteNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public System.DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CreatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object OrigSystem { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
    public System.DateTime StartDateActive { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", DataType = "date")]
    public System.DateTime EndDateActive { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Mailstop { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public bool IdentifyingAddressFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Language { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string Status { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartySiteName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Addressee { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object GlobalLocationNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object DUNSNumberC { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartySiteType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartyNameDba { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartyNameDivision { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartyNameLegal { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PartyUsageCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string UsageCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string FormattedAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string Country { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string Address1 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Address2 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Address3 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Address4 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string City { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public uint PostalCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object State { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Province { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object County { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object AddressStyle { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public bool ValidatedFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object AddressLinesPhonetic { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object PostalPlus4Code { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Position { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object LocationDirections { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object AddressExpirationDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object LocationLanguage { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object ValidationStatusCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object DateValidated { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object DoNotValidateFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object HouseType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string FormattedMultilineAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string Country1 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public byte ObjectVersionNumber1 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public bool ContactPreferenceExistFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object FormattedLocaleAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CurrencyCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CorpCurrencyCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public string CurcyConvRateType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public bool InternalFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public bool OverallPrimaryFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object EmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Latitude { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = true)]
    public object Longitude { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
    public PartySitePartySiteUse PartySiteUse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/partyService/", IsNullable = false)]
public partial class PartySitePartySiteUse
{
    /// <remarks/>
    public ulong PartySiteUseId { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public System.DateTime BeginDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public System.DateTime EndDate { get; set; }

    /// <remarks/>
    public ulong PartySiteId { get; set; }

    /// <remarks/>
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public System.DateTime CreationDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    public string IntegrationKey { get; set; }

    /// <remarks/>
    public string SiteUseType { get; set; }

    /// <remarks/>
    public string PrimaryPerType { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }
}

