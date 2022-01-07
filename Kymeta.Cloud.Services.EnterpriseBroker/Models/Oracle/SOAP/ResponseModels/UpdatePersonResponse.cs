using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class UpdatePersonEnvelope
{
    /// <remarks/>
    public UpdatePersonEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public UpdatePersonEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class UpdatePersonEnvelopeHeader
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
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class UpdatePersonEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
    public updatePersonResponse updatePersonResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public class updatePersonResponse
{
    /// <remarks/>
    public updatePersonResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", TypeName = "PersonResult")]
public class updatePersonResponseResult
{
    /// <remarks/>
    [XmlElement("Value", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
    public UpdatePersonValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public class UpdatePersonValue
{
    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public uint PartyNumber { get; set; }

    /// <remarks/>
    public string PartyName { get; set; }

    /// <remarks/>
    public string PartyType { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string PersonFirstName { get; set; }

    /// <remarks/>
    public string PersonLastName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonMiddleName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonTitle { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Country { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address2 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address1 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address4 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address3 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PostalCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object City { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Province { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object State { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object County { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string EmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CategoryCode { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public uint PrimaryPhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactPersonId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceSystem { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceSystemReferenceValue { get; set; }

    /// <remarks/>
    public UpdatePersonValueEmail Email { get; set; }

    /// <remarks/>
    public UpdatePersonValuePersonProfile PersonProfile { get; set; }

    /// <remarks/>
    public UpdatePersonValuePhone Phone { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public class UpdatePersonValueEmail
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string ContactPointType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string Status { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PartyUsageCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object OrigSystem { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string EmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PartyName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool OverallPrimaryFlag { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public class UpdatePersonValuePersonProfile
{
    /// <remarks/>
    public ulong PersonProfileId { get; set; }

    /// <remarks/>
    public ulong PartyId { get; set; }

    /// <remarks/>
    public string PersonName { get; set; }

    /// <remarks/>
    public string PersonFirstName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public string PersonMiddleName { get; set; }

    /// <remarks/>
    public string PersonLastName { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public string OrigSystem { get; set; }

    /// <remarks/>
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public uint PartyNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressLine1 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressLine2 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressLine3 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressLine4 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressCity { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressCountry { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressCounty { get; set; }

    /// <remarks/>
    public string PrimaryEmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryFormattedAddress { get; set; }

    /// <remarks/>
    public string PrimaryFormattedPhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryAddressPostalCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredName { get; set; }

    /// <remarks/>
    public ulong PrimaryEmailId { get; set; }

    /// <remarks/>
    public uint PrimaryPhoneNumber { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public class UpdatePersonValuePhone
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string ContactPointType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string Status { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OwnerTableName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong OwnerTableId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PartyUsageCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public string OrigSystem { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ushort PhoneAreaCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public byte PhoneCountryCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PhoneExtension { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PhoneLineType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string RawPhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string FormattedPhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PartyName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object TimezoneCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool OverallPrimaryFlag { get; set; }
}

