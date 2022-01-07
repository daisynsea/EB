﻿using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public class UpdateLocationEnvelope
{
    /// <remarks/>
    public UpdateLocationEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public UpdateLocationEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class UpdateLocationEnvelopeHeader
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
public class UpdateLocationEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/")]
    public updateLocationResponse updateLocationResponse { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/applicationModule/types/", IsNullable = false)]
public class updateLocationResponse
{
    /// <remarks/>
    public updateLocationResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/", TypeName = "LocationResult")]
public class updateLocationResponseResult
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/")]
    public UpdateLocationValue Value { get; set; }
}

/// <remarks/>
[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/locationService/", IsNullable = false)]
public class UpdateLocationValue
{
    /// <remarks/>
    public ulong LocationId { get; set; }

    /// <remarks/>
    public System.DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public System.DateTime CreationDate { get; set; }

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
    public string OrigSystemReference { get; set; }

    /// <remarks/>
    public string Country { get; set; }

    /// <remarks/>
    public string Address1 { get; set; }

    /// <remarks/>
    public string Address2 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address3 { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Address4 { get; set; }

    /// <remarks/>
    public string City { get; set; }

    /// <remarks/>
    public uint? PostalCode { get; set; }

    /// <remarks/>
    public string State { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Province { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object County { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressStyle { get; set; }

    /// <remarks/>
    public bool ValidatedFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressLinesPhonetic { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PostalPlus4Code { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Position { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LocationDirections { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressEffectiveDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object AddressExpirationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ClliCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Language { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ShortDescription { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Description { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SalesTaxGeocode { get; set; }

    /// <remarks/>
    public byte SalesTaxInsideCityLimits { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FaLocationId { get; set; }

    /// <remarks/>
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public string GeometryStatusCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ValidationStatusCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DateValidated { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DoNotValidateFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object HouseType { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "date")]
    public System.DateTime EffectiveDate { get; set; }
}

