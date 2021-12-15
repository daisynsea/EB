using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class CreatePersonEnvelope
{
    /// <remarks/>
    public CreatePersonEnvelopeHeader Header { get; set; }

    /// <remarks/>
    public CreatePersonEnvelopeBody Body { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class CreatePersonEnvelopeHeader
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
public partial class CreatePersonEnvelopeBody
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
    public createPersonResponse createPersonResponse { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[XmlRoot("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public partial class createPersonResponse
{
    /// <remarks/>
    public createPersonResponseResult result { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
public partial class createPersonResponseResult
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
    public Value Value { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public partial class Value
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
    [XmlElement(IsNullable = true)]
    public object ValidatedFlag { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SICCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object JgzzFiscalCode { get; set; }

    /// <remarks/>
    public string PersonFirstName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonPreNameAdjunct { get; set; }

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
    public object PersonNameSuffix { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonPreviousLastName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonAcademicTitle { get; set; }

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
    [XmlElement(IsNullable = true)]
    public object URL { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SICCodeType { get; set; }

    /// <remarks/>
    public string EmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object GSAIndicatorFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object LanguageName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object MissionStatement { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CategoryCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ThirdPartyFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Salutation { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CertReasonCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CertificationLevel { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhonePurpose { get; set; }

    /// <remarks/>
    public ulong PrimaryPhoneContactPTId { get; set; }

    /// <remarks/>
    public byte PrimaryPhoneCountryCode { get; set; }

    /// <remarks/>
    public string PrimaryPhoneLineType { get; set; }

    /// <remarks/>
    public uint PrimaryPhoneNumber { get; set; }

    /// <remarks/>
    public ushort PrimaryPhoneAreaCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactMethod { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryPhoneExtension { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object IdenAddrLocationId { get; set; }

    /// <remarks/>
    public ulong PrimaryEmailContactPTId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object IdenAddrPartySiteId { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonLastNamePrefix { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PrimaryURLContactPTId { get; set; }

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
    public object PreferredContactPersonId { get; set; }

    /// <remarks/>
    public bool InternalFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredFunctionalCurrency { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Gender { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object MaritalStatus { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DateOfBirth { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object UserGUID { get; set; }

    /// <remarks/>
    public string PartyUniqueName { get; set; }

    /// <remarks/>
    public string SourceSystem { get; set; }

    /// <remarks/>
    public string SourceSystemReferenceValue { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SourceSystemUpdateDate { get; set; }

    /// <remarks/>
    public ValueEmail Email { get; set; }

    ///// <remarks/>
    //public ValuePersonProfile PersonProfile { get; set; }

    /// <remarks/>
    public ValuePhone Phone { get; set; }

    /// <remarks/>
    public ValueRelationship Relationship { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class ValueEmail
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
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public string ContactPointPurpose { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PrimaryByPurpose { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public DateTime StartDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public DateTime EndDate { get; set; }

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
    public object EmailFormat { get; set; }

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


#region ValuePersonProfile
///// <remarks/>
//[Serializable]
//[System.ComponentModel.DesignerCategory("code")]
//[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
//public partial class ValuePersonProfile
//{

//    private ulong personProfileIdField;

//    private ulong partyIdField;

//    private string personNameField;

//    private DateTime lastUpdateDateField;

//    private string lastUpdatedByField;

//    private DateTime creationDateField;

//    private string createdByField;

//    private string lastUpdateLoginField;

//    private object requestIdField;

//    private object personPreNameAdjunctField;

//    private string personFirstNameField;

//    private object personMiddleNameField;

//    private string personLastNameField;

//    private object personNameSuffixField;

//    private object personTitleField;

//    private object personAcademicTitleField;

//    private object personPreviousLastNameField;

//    private object personInitialsField;

//    private object jgzzFiscalCodeField;

//    private object dateOfBirthField;

//    private object placeOfBirthField;

//    private object dateOfDeathField;

//    private object genderField;

//    private object declaredEthnicityField;

//    private object maritalStatusField;

//    private object maritalStatusEffectiveDateField;

//    private object personalIncomeAmountField;

//    private object rentOwnIndField;

//    private object lastKnownGPSField;

//    private DateTime effectiveStartDateField;

//    private DateTime effectiveEndDateField;

//    private bool internalFlagField;

//    private string statusField;

//    private string createdByModuleField;

//    private bool deceasedFlagField;

//    private object commentsField;

//    private object personLastNamePrefixField;

//    private object personSecondLastNameField;

//    private object preferredFunctionalCurrencyField;

//    private object origSystemField;

//    private ulong origSystemReferenceField;

//    private byte effectiveSequenceField;

//    private object headOfHouseholdFlagField;

//    private object householdIncomeAmountField;

//    private object householdSizeField;

//    private string effectiveLatestChangeField;

//    private bool suffixOverriddenFlagField;

//    private object uniqueNameSuffixField;

//    private string corpCurrencyCodeField;

//    private string curcyConvRateTypeField;

//    private string currencyCodeField;

//    private uint partyNumberField;

//    private object salutationField;

//    private object certReasonCodeField;

//    private object certificationLevelField;

//    private object preferredContactMethodField;

//    private object preferredContactPersonIdField;

//    private object primaryAddressLine1Field;

//    private object primaryAddressLine2Field;

//    private object primaryAddressLine3Field;

//    private object primaryAddressLine4Field;

//    private object aliasField;

//    private object primaryAddressCityField;

//    private object primaryAddressCountryField;

//    private object primaryAddressCountyField;

//    private string primaryEmailAddressField;

//    private object primaryFormattedAddressField;

//    private string primaryFormattedPhoneNumberField;

//    private object primaryLanguageField;

//    private string partyUniqueNameField;

//    private object primaryAddressPostalCodeField;

//    private object preferredContactEmailField;

//    private object preferredContactNameField;

//    private object preferredContactPhoneField;

//    private object preferredContactURLField;

//    private object preferredNameField;

//    private object preferredNameIdField;

//    private ulong primaryEmailIdField;

//    private ushort primaryPhoneAreaCodeField;

//    private ulong primaryPhoneIdField;

//    private byte primaryPhoneCountryCodeField;

//    private object primaryPhoneExtensionField;

//    private string primaryPhoneLineTypeField;

//    private uint primaryPhoneNumberField;

//    private object primaryPhonePurposeField;

//    private object primaryWebIdField;

//    private object pronunciationField;

//    private object primaryAddressProvinceField;

//    private object primaryAddressStateField;

//    private object primaryURLField;

//    private object validatedFlagField;

//    private object primaryAddressLatitudeField;

//    private object primaryAddressLongitudeField;

//    private object primaryAddressLocationIdField;

//    private bool favoriteContactFlagField;

//    private object distanceField;

//    private object salesAffinityCodeField;

//    private object salesBuyingRoleCodeField;

//    private object departmentCodeField;

//    private object departmentField;

//    private object jobTitleCodeField;

//    private object jobTitleField;

//    private bool doNotCallFlagField;

//    private bool doNotContactFlagField;

//    private bool doNotEmailFlagField;

//    private bool doNotMailFlagField;

//    private object lastContactDateField;

//    private ulong primaryCustomerIdField;

//    private ulong primaryCustomerRelationshipIdField;

//    private string primaryCustomerNameField;

//    private DateTime lastSourceUpdateDateField;

//    private string lastUpdateSourceSystemField;

//    private object dataCloudStatusField;

//    private object lastEnrichmentDateField;

//    /// <remarks/>
//    public ulong PersonProfileId { get; set; }

//    /// <remarks/>
//    public ulong PartyId { get; set; }

//    /// <remarks/>
//    public string PersonName { get; set; }

//    /// <remarks/>
//    public DateTime LastUpdateDate { get; set; }

//    /// <remarks/>
//    public string LastUpdatedBy { get; set; }

//    /// <remarks/>
//    public DateTime CreationDate { get; set; }

//    /// <remarks/>
//    public string CreatedBy { get; set; }

//    /// <remarks/>
//    public string LastUpdateLogin { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object RequestId { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonPreNameAdjunct { get; set; }

//    /// <remarks/>
//    public string PersonFirstName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonMiddleName { get; set; }

//    /// <remarks/>
//    public string PersonLastName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonNameSuffix { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonTitle { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonAcademicTitle { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonPreviousLastName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonInitials { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object JgzzFiscalCode { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object DateOfBirth { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PlaceOfBirth { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object DateOfDeath { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Gender { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object DeclaredEthnicity { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object MaritalStatus { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object MaritalStatusEffectiveDate { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonalIncomeAmount { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object RentOwnInd { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object LastKnownGPS { get; set; }

//    /// <remarks/>
//    [XmlElement(DataType = "date")]
//    public DateTime EffectiveStartDate { get; set; }

//    /// <remarks/>
//    [XmlElement(DataType = "date")]
//    public DateTime EffectiveEndDate { get; set; }

//    /// <remarks/>
//    public bool InternalFlag { get; set; }

//    /// <remarks/>
//    public string Status { get; set; }

//    /// <remarks/>
//    public string CreatedByModule { get; set; }

//    /// <remarks/>
//    public bool DeceasedFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Comments { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonLastNamePrefix { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PersonSecondLastName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredFunctionalCurrency { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object OrigSystem { get; set; }

//    /// <remarks/>
//    public ulong OrigSystemReference { get; set; }

//    /// <remarks/>
//    public byte EffectiveSequence { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object HeadOfHouseholdFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object HouseholdIncomeAmount { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object HouseholdSize { get; set; }

//    /// <remarks/>
//    public string EffectiveLatestChange { get; set; }

//    /// <remarks/>
//    public bool SuffixOverriddenFlag { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object UniqueNameSuffix { get; set; }

//    /// <remarks/>
//    public string CorpCurrencyCode { get; set; }

//    /// <remarks/>
//    public string CurcyConvRateType { get; set; }

//    /// <remarks/>
//    public string CurrencyCode { get; set; }

//    /// <remarks/>
//    public uint PartyNumber { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Salutation { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object CertReasonCode { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object CertificationLevel { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredContactMethod { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredContactPersonId { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressLine1 { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressLine2 { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressLine3 { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressLine4 { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Alias { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressCity { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressCountry { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressCounty { get; set; }

//    /// <remarks/>
//    public string PrimaryEmailAddress { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryFormattedAddress { get; set; }

//    /// <remarks/>
//    public string PrimaryFormattedPhoneNumber { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryLanguage { get; set; }

//    /// <remarks/>
//    public string PartyUniqueName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressPostalCode { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredContactEmail { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredContactName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredContactPhone { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredContactURL { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredName { get; set; }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PreferredNameId { get; set; }

//    /// <remarks/>
//    public ulong PrimaryEmailId { get; set; }

//    /// <remarks/>
//    public ushort PrimaryPhoneAreaCode
//    {
//        get
//        {
//            return this.primaryPhoneAreaCodeField;
//        }
//        set
//        {
//            this.primaryPhoneAreaCodeField = value;
//        }
//    }

//    /// <remarks/>
//    public ulong PrimaryPhoneId
//    {
//        get
//        {
//            return this.primaryPhoneIdField;
//        }
//        set
//        {
//            this.primaryPhoneIdField = value;
//        }
//    }

//    /// <remarks/>
//    public byte PrimaryPhoneCountryCode
//    {
//        get
//        {
//            return this.primaryPhoneCountryCodeField;
//        }
//        set
//        {
//            this.primaryPhoneCountryCodeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryPhoneExtension
//    {
//        get
//        {
//            return this.primaryPhoneExtensionField;
//        }
//        set
//        {
//            this.primaryPhoneExtensionField = value;
//        }
//    }

//    /// <remarks/>
//    public string PrimaryPhoneLineType
//    {
//        get
//        {
//            return this.primaryPhoneLineTypeField;
//        }
//        set
//        {
//            this.primaryPhoneLineTypeField = value;
//        }
//    }

//    /// <remarks/>
//    public uint PrimaryPhoneNumber
//    {
//        get
//        {
//            return this.primaryPhoneNumberField;
//        }
//        set
//        {
//            this.primaryPhoneNumberField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryPhonePurpose
//    {
//        get
//        {
//            return this.primaryPhonePurposeField;
//        }
//        set
//        {
//            this.primaryPhonePurposeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryWebId
//    {
//        get
//        {
//            return this.primaryWebIdField;
//        }
//        set
//        {
//            this.primaryWebIdField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Pronunciation
//    {
//        get
//        {
//            return this.pronunciationField;
//        }
//        set
//        {
//            this.pronunciationField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressProvince
//    {
//        get
//        {
//            return this.primaryAddressProvinceField;
//        }
//        set
//        {
//            this.primaryAddressProvinceField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressState
//    {
//        get
//        {
//            return this.primaryAddressStateField;
//        }
//        set
//        {
//            this.primaryAddressStateField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryURL
//    {
//        get
//        {
//            return this.primaryURLField;
//        }
//        set
//        {
//            this.primaryURLField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object ValidatedFlag
//    {
//        get
//        {
//            return this.validatedFlagField;
//        }
//        set
//        {
//            this.validatedFlagField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressLatitude
//    {
//        get
//        {
//            return this.primaryAddressLatitudeField;
//        }
//        set
//        {
//            this.primaryAddressLatitudeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object PrimaryAddressLongitude
//    {
//        get
//        {
//            return this.primaryAddressLongitudeField;
//        }
//        set
//        {
//            this.primaryAddressLongitudeField = value;
//        }
//    }

//    /// <remarks/>
//    public object PrimaryAddressLocationId
//    {
//        get
//        {
//            return this.primaryAddressLocationIdField;
//        }
//        set
//        {
//            this.primaryAddressLocationIdField = value;
//        }
//    }

//    /// <remarks/>
//    public bool FavoriteContactFlag
//    {
//        get
//        {
//            return this.favoriteContactFlagField;
//        }
//        set
//        {
//            this.favoriteContactFlagField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Distance
//    {
//        get
//        {
//            return this.distanceField;
//        }
//        set
//        {
//            this.distanceField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object SalesAffinityCode
//    {
//        get
//        {
//            return this.salesAffinityCodeField;
//        }
//        set
//        {
//            this.salesAffinityCodeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object SalesBuyingRoleCode
//    {
//        get
//        {
//            return this.salesBuyingRoleCodeField;
//        }
//        set
//        {
//            this.salesBuyingRoleCodeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object DepartmentCode
//    {
//        get
//        {
//            return this.departmentCodeField;
//        }
//        set
//        {
//            this.departmentCodeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object Department
//    {
//        get
//        {
//            return this.departmentField;
//        }
//        set
//        {
//            this.departmentField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object JobTitleCode
//    {
//        get
//        {
//            return this.jobTitleCodeField;
//        }
//        set
//        {
//            this.jobTitleCodeField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object JobTitle
//    {
//        get
//        {
//            return this.jobTitleField;
//        }
//        set
//        {
//            this.jobTitleField = value;
//        }
//    }

//    /// <remarks/>
//    public bool DoNotCallFlag
//    {
//        get
//        {
//            return this.doNotCallFlagField;
//        }
//        set
//        {
//            this.doNotCallFlagField = value;
//        }
//    }

//    /// <remarks/>
//    public bool DoNotContactFlag
//    {
//        get
//        {
//            return this.doNotContactFlagField;
//        }
//        set
//        {
//            this.doNotContactFlagField = value;
//        }
//    }

//    /// <remarks/>
//    public bool DoNotEmailFlag
//    {
//        get
//        {
//            return this.doNotEmailFlagField;
//        }
//        set
//        {
//            this.doNotEmailFlagField = value;
//        }
//    }

//    /// <remarks/>
//    public bool DoNotMailFlag
//    {
//        get
//        {
//            return this.doNotMailFlagField;
//        }
//        set
//        {
//            this.doNotMailFlagField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object LastContactDate
//    {
//        get
//        {
//            return this.lastContactDateField;
//        }
//        set
//        {
//            this.lastContactDateField = value;
//        }
//    }

//    /// <remarks/>
//    public ulong PrimaryCustomerId
//    {
//        get
//        {
//            return this.primaryCustomerIdField;
//        }
//        set
//        {
//            this.primaryCustomerIdField = value;
//        }
//    }

//    /// <remarks/>
//    public ulong PrimaryCustomerRelationshipId
//    {
//        get
//        {
//            return this.primaryCustomerRelationshipIdField;
//        }
//        set
//        {
//            this.primaryCustomerRelationshipIdField = value;
//        }
//    }

//    /// <remarks/>
//    public string PrimaryCustomerName
//    {
//        get
//        {
//            return this.primaryCustomerNameField;
//        }
//        set
//        {
//            this.primaryCustomerNameField = value;
//        }
//    }

//    /// <remarks/>
//    public DateTime LastSourceUpdateDate
//    {
//        get
//        {
//            return this.lastSourceUpdateDateField;
//        }
//        set
//        {
//            this.lastSourceUpdateDateField = value;
//        }
//    }

//    /// <remarks/>
//    public string LastUpdateSourceSystem
//    {
//        get
//        {
//            return this.lastUpdateSourceSystemField;
//        }
//        set
//        {
//            this.lastUpdateSourceSystemField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object DataCloudStatus
//    {
//        get
//        {
//            return this.dataCloudStatusField;
//        }
//        set
//        {
//            this.dataCloudStatusField = value;
//        }
//    }

//    /// <remarks/>
//    [XmlElement(IsNullable = true)]
//    public object LastEnrichmentDate
//    {
//        get
//        {
//            return this.lastEnrichmentDateField;
//        }
//        set
//        {
//            this.lastEnrichmentDateField = value;
//        }
//    }
//}
#endregion

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class ValuePhone
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
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public string ContactPointPurpose { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PrimaryByPurpose { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public DateTime StartDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public DateTime EndDate { get; set; }

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
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PhoneCallingCalendar { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object LastContactDtTime { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ushort PhoneAreaCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public byte PhoneCountryCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public uint PhoneNumber { get; set; }

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
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PagerTypeCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string FormattedPhoneNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong TransposedPhoneNumber { get; set; }

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

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class ValueRelationship
{
    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong RelationshipRecId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong RelationshipId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong SubjectId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string SubjectType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string SubjectTableName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong ObjectId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string ObjectType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string ObjectTableName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string RelationshipCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string RelationshipType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string Role { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", DataType = "date")]
    public DateTime StartDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", DataType = "date")]
    public DateTime EndDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string Status { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CreatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CreatedByModule { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation1 { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string DirectionCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object PercentageOwnership { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object ObjectUsageCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object SubjectUsageCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public bool PreferredContactFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string ObjectPartyName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string PartyName { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CurrencyCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CurcyConvRateType { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CorpCurrencyCode { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public bool PrimaryCustomerFlag { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string SubjectEmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object ObjectEmailAddress { get; set; }

    /// <remarks/>
    [XmlElement(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public OrganizationContact OrganizationContact { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[XmlRoot(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class OrganizationContact
{
    /// <remarks/>
    public ulong OrgContactId { get; set; }

    /// <remarks/>
    public ulong PartyRelationshipId { get; set; }

    /// <remarks/>
    public ulong ContactPartyId { get; set; }

    /// <remarks/>
    public string PersonFirstName { get; set; }

    /// <remarks/>
    public string PersonLastName { get; set; }

    /// <remarks/>
    public string ContactName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonPreNameAdjunct { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonMiddleName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonNameSuffix { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonPreviousLastName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonAcademicTitle { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Salutation { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonLastNamePrefix { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonSecondLastName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonLanguageName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonTitle { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonCertificationLevel { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PersonCertReasonCode { get; set; }

    /// <remarks/>
    public ulong CustomerPartyId { get; set; }

    /// <remarks/>
    public string CustomerUniqueName { get; set; }

    /// <remarks/>
    public string CustomerName { get; set; }

    /// <remarks/>
    public uint CustomerPartyNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FormattedPhoneNumber { get; set; }

    /// <remarks/>
    public object EmailAddress { get; set; }

    /// <remarks/>
    public object WebUrl { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Comments { get; set; }

    /// <remarks/>
    public uint ContactNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object DepartmentCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Department { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object JobTitle { get; set; }

    /// <remarks/>
    public bool DecisionMakerFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object JobTitleCode { get; set; }

    /// <remarks/>
    public bool ReferenceUseFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object Rank { get; set; }

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
    [XmlElement(IsNullable = true)]
    public object PartySiteId { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }

    /// <remarks/>
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PartySiteName { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SalesAffinityCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SalesAffinityComments { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SalesBuyingRoleCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object SalesInfluenceLevelCode { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object FormattedAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object PreferredContactMethod { get; set; }

    /// <remarks/>
    public string CurrencyCode { get; set; }

    /// <remarks/>
    public string CurcyConvRateType { get; set; }

    /// <remarks/>
    public string CorpCurrencyCode { get; set; }

    /// <remarks/>
    public bool PreferredContactFlag { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ContactFormattedAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object ContactFormattedMultilineAddress { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object CustomerEmailAddress { get; set; }

    /// <remarks/>
    public bool PrimaryCustomerFlag { get; set; }

    /// <remarks/>
    public OrganizationContactOrganizationContactRole OrganizationContactRole { get; set; }
}

/// <remarks/>
[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
public partial class OrganizationContactOrganizationContactRole
{

    private ulong orgContactRoleIdField;

    private ulong origSystemReferenceField;

    private string createdByField;

    private string roleTypeField;

    private ulong orgContactIdField;

    private DateTime creationDateField;

    private object roleLevelField;

    private bool primaryFlagField;

    private DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private string lastUpdateLoginField;

    private string primaryContactPerRoleTypeField;

    private object requestIdField;

    private string statusField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    /// <remarks/>
    public ulong OrgContactRoleId { get; set; }

    /// <remarks/>
    public ulong OrigSystemReference { get; set; }

    /// <remarks/>
    public string CreatedBy { get; set; }

    /// <remarks/>
    public string RoleType { get; set; }

    /// <remarks/>
    public ulong OrgContactId { get; set; }

    /// <remarks/>
    public DateTime CreationDate { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RoleLevel { get; set; }

    /// <remarks/>
    public bool PrimaryFlag { get; set; }

    /// <remarks/>
    public DateTime LastUpdateDate { get; set; }

    /// <remarks/>
    public string LastUpdatedBy { get; set; }

    /// <remarks/>
    public string LastUpdateLogin { get; set; }

    /// <remarks/>
    public string PrimaryContactPerRoleType { get; set; }

    /// <remarks/>
    [XmlElement(IsNullable = true)]
    public object RequestId { get; set; }

    /// <remarks/>
    public string Status { get; set; }

    /// <remarks/>
    public byte ObjectVersionNumber { get; set; }

    /// <remarks/>
    public string CreatedByModule { get; set; }
}

