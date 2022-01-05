using System.ComponentModel;
using System.Xml.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels;

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[System.Xml.Serialization.XmlRootAttribute("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
public partial class CreatePersonEnvelope
{

    private CreatePersonEnvelopeHeader headerField;

    private CreatePersonEnvelopeBody bodyField;

    /// <remarks/>
    public CreatePersonEnvelopeHeader Header
    {
        get
        {
            return this.headerField;
        }
        set
        {
            this.headerField = value;
        }
    }

    /// <remarks/>
    public CreatePersonEnvelopeBody Body
    {
        get
        {
            return this.bodyField;
        }
        set
        {
            this.bodyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class CreatePersonEnvelopeHeader
{

    private string actionField;

    private string messageIDField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string Action
    {
        get
        {
            return this.actionField;
        }
        set
        {
            this.actionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2005/08/addressing")]
    public string MessageID
    {
        get
        {
            return this.messageIDField;
        }
        set
        {
            this.messageIDField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public partial class CreatePersonEnvelopeBody
{

    private createPersonResponse createPersonResponseField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModu" +
        "le/types/")]
    public createPersonResponse createPersonResponse
    {
        get
        {
            return this.createPersonResponseField;
        }
        set
        {
            this.createPersonResponseField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/")]
[System.Xml.Serialization.XmlRootAttribute("result", Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/applicationModule/types/", IsNullable = false)]
public partial class createPersonResponse
{

    private createPersonResponseResult resultField;

    /// <remarks/>
    public createPersonResponseResult result
    {
        get
        {
            return this.resultField;
        }
        set
        {
            this.resultField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", TypeName = "PersonResult")]
public partial class createPersonResponseResult
{

    private CreatePersonValue valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
    public CreatePersonValue Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/", IsNullable = false)]
public partial class CreatePersonValue
{

    private ulong partyIdField;

    private uint partyNumberField;

    private string partyNameField;

    private string partyTypeField;

    private object validatedFlagField;

    private string lastUpdatedByField;

    private string lastUpdateLoginField;

    private System.DateTime creationDateField;

    private object requestIdField;

    private System.DateTime lastUpdateDateField;

    private string createdByField;

    private string origSystemReferenceField;

    private object sICCodeField;

    private object jgzzFiscalCodeField;

    private string personFirstNameField;

    private object personPreNameAdjunctField;

    private string personLastNameField;

    private object personMiddleNameField;

    private object personTitleField;

    private object personNameSuffixField;

    private object personPreviousLastNameField;

    private object personAcademicTitleField;

    private object countryField;

    private object address2Field;

    private object address1Field;

    private object address4Field;

    private object address3Field;

    private object postalCodeField;

    private object cityField;

    private object provinceField;

    private object stateField;

    private object countyField;

    private string statusField;

    private object uRLField;

    private object sICCodeTypeField;

    private string emailAddressField;

    private object gSAIndicatorFlagField;

    private object languageNameField;

    private object missionStatementField;

    private object categoryCodeField;

    private object thirdPartyFlagField;

    private object salutationField;

    private string createdByModuleField;

    private object certReasonCodeField;

    private object certificationLevelField;

    private object primaryPhonePurposeField;

    private ulong primaryPhoneContactPTIdField;

    private object primaryPhoneCountryCodeField;

    private string primaryPhoneLineTypeField;

    private string primaryPhoneNumberField;

    private object primaryPhoneAreaCodeField;

    private object preferredContactMethodField;

    private object primaryPhoneExtensionField;

    private object idenAddrLocationIdField;

    private ulong primaryEmailContactPTIdField;

    private object idenAddrPartySiteIdField;

    private object personLastNamePrefixField;

    private object primaryURLContactPTIdField;

    private object preferredNameField;

    private object personSecondLastNameField;

    private object preferredNameIdField;

    private object preferredContactPersonIdField;

    private bool internalFlagField;

    private object preferredFunctionalCurrencyField;

    private object genderField;

    private object maritalStatusField;

    private object commentsField;

    private object dateOfBirthField;

    private object userGUIDField;

    private string partyUniqueNameField;

    private string sourceSystemField;

    private string sourceSystemReferenceValueField;

    private object sourceSystemUpdateDateField;

    private CreatePersonValuePersonProfile personProfileField;

    private CreatePersonValueRelationship relationshipField;

    /// <remarks/>
    public ulong PartyId
    {
        get
        {
            return this.partyIdField;
        }
        set
        {
            this.partyIdField = value;
        }
    }

    /// <remarks/>
    public uint PartyNumber
    {
        get
        {
            return this.partyNumberField;
        }
        set
        {
            this.partyNumberField = value;
        }
    }

    /// <remarks/>
    public string PartyName
    {
        get
        {
            return this.partyNameField;
        }
        set
        {
            this.partyNameField = value;
        }
    }

    /// <remarks/>
    public string PartyType
    {
        get
        {
            return this.partyTypeField;
        }
        set
        {
            this.partyTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object ValidatedFlag
    {
        get
        {
            return this.validatedFlagField;
        }
        set
        {
            this.validatedFlagField = value;
        }
    }

    /// <remarks/>
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    public string OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SICCode
    {
        get
        {
            return this.sICCodeField;
        }
        set
        {
            this.sICCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object JgzzFiscalCode
    {
        get
        {
            return this.jgzzFiscalCodeField;
        }
        set
        {
            this.jgzzFiscalCodeField = value;
        }
    }

    /// <remarks/>
    public string PersonFirstName
    {
        get
        {
            return this.personFirstNameField;
        }
        set
        {
            this.personFirstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonPreNameAdjunct
    {
        get
        {
            return this.personPreNameAdjunctField;
        }
        set
        {
            this.personPreNameAdjunctField = value;
        }
    }

    /// <remarks/>
    public string PersonLastName
    {
        get
        {
            return this.personLastNameField;
        }
        set
        {
            this.personLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonMiddleName
    {
        get
        {
            return this.personMiddleNameField;
        }
        set
        {
            this.personMiddleNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonTitle
    {
        get
        {
            return this.personTitleField;
        }
        set
        {
            this.personTitleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonNameSuffix
    {
        get
        {
            return this.personNameSuffixField;
        }
        set
        {
            this.personNameSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonPreviousLastName
    {
        get
        {
            return this.personPreviousLastNameField;
        }
        set
        {
            this.personPreviousLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonAcademicTitle
    {
        get
        {
            return this.personAcademicTitleField;
        }
        set
        {
            this.personAcademicTitleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Country
    {
        get
        {
            return this.countryField;
        }
        set
        {
            this.countryField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Address2
    {
        get
        {
            return this.address2Field;
        }
        set
        {
            this.address2Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Address1
    {
        get
        {
            return this.address1Field;
        }
        set
        {
            this.address1Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Address4
    {
        get
        {
            return this.address4Field;
        }
        set
        {
            this.address4Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Address3
    {
        get
        {
            return this.address3Field;
        }
        set
        {
            this.address3Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PostalCode
    {
        get
        {
            return this.postalCodeField;
        }
        set
        {
            this.postalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object City
    {
        get
        {
            return this.cityField;
        }
        set
        {
            this.cityField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Province
    {
        get
        {
            return this.provinceField;
        }
        set
        {
            this.provinceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object State
    {
        get
        {
            return this.stateField;
        }
        set
        {
            this.stateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object County
    {
        get
        {
            return this.countyField;
        }
        set
        {
            this.countyField = value;
        }
    }

    /// <remarks/>
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object URL
    {
        get
        {
            return this.uRLField;
        }
        set
        {
            this.uRLField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SICCodeType
    {
        get
        {
            return this.sICCodeTypeField;
        }
        set
        {
            this.sICCodeTypeField = value;
        }
    }

    /// <remarks/>
    public string EmailAddress
    {
        get
        {
            return this.emailAddressField;
        }
        set
        {
            this.emailAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object GSAIndicatorFlag
    {
        get
        {
            return this.gSAIndicatorFlagField;
        }
        set
        {
            this.gSAIndicatorFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object LanguageName
    {
        get
        {
            return this.languageNameField;
        }
        set
        {
            this.languageNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object MissionStatement
    {
        get
        {
            return this.missionStatementField;
        }
        set
        {
            this.missionStatementField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object CategoryCode
    {
        get
        {
            return this.categoryCodeField;
        }
        set
        {
            this.categoryCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object ThirdPartyFlag
    {
        get
        {
            return this.thirdPartyFlagField;
        }
        set
        {
            this.thirdPartyFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Salutation
    {
        get
        {
            return this.salutationField;
        }
        set
        {
            this.salutationField = value;
        }
    }

    /// <remarks/>
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object CertReasonCode
    {
        get
        {
            return this.certReasonCodeField;
        }
        set
        {
            this.certReasonCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object CertificationLevel
    {
        get
        {
            return this.certificationLevelField;
        }
        set
        {
            this.certificationLevelField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhonePurpose
    {
        get
        {
            return this.primaryPhonePurposeField;
        }
        set
        {
            this.primaryPhonePurposeField = value;
        }
    }

    /// <remarks/>
    public ulong PrimaryPhoneContactPTId
    {
        get
        {
            return this.primaryPhoneContactPTIdField;
        }
        set
        {
            this.primaryPhoneContactPTIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhoneCountryCode
    {
        get
        {
            return this.primaryPhoneCountryCodeField;
        }
        set
        {
            this.primaryPhoneCountryCodeField = value;
        }
    }

    /// <remarks/>
    public string PrimaryPhoneLineType
    {
        get
        {
            return this.primaryPhoneLineTypeField;
        }
        set
        {
            this.primaryPhoneLineTypeField = value;
        }
    }

    /// <remarks/>
    public string PrimaryPhoneNumber
    {
        get
        {
            return this.primaryPhoneNumberField;
        }
        set
        {
            this.primaryPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhoneAreaCode
    {
        get
        {
            return this.primaryPhoneAreaCodeField;
        }
        set
        {
            this.primaryPhoneAreaCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactMethod
    {
        get
        {
            return this.preferredContactMethodField;
        }
        set
        {
            this.preferredContactMethodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhoneExtension
    {
        get
        {
            return this.primaryPhoneExtensionField;
        }
        set
        {
            this.primaryPhoneExtensionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object IdenAddrLocationId
    {
        get
        {
            return this.idenAddrLocationIdField;
        }
        set
        {
            this.idenAddrLocationIdField = value;
        }
    }

    /// <remarks/>
    public ulong PrimaryEmailContactPTId
    {
        get
        {
            return this.primaryEmailContactPTIdField;
        }
        set
        {
            this.primaryEmailContactPTIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object IdenAddrPartySiteId
    {
        get
        {
            return this.idenAddrPartySiteIdField;
        }
        set
        {
            this.idenAddrPartySiteIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonLastNamePrefix
    {
        get
        {
            return this.personLastNamePrefixField;
        }
        set
        {
            this.personLastNamePrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryURLContactPTId
    {
        get
        {
            return this.primaryURLContactPTIdField;
        }
        set
        {
            this.primaryURLContactPTIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredName
    {
        get
        {
            return this.preferredNameField;
        }
        set
        {
            this.preferredNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonSecondLastName
    {
        get
        {
            return this.personSecondLastNameField;
        }
        set
        {
            this.personSecondLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredNameId
    {
        get
        {
            return this.preferredNameIdField;
        }
        set
        {
            this.preferredNameIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactPersonId
    {
        get
        {
            return this.preferredContactPersonIdField;
        }
        set
        {
            this.preferredContactPersonIdField = value;
        }
    }

    /// <remarks/>
    public bool InternalFlag
    {
        get
        {
            return this.internalFlagField;
        }
        set
        {
            this.internalFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredFunctionalCurrency
    {
        get
        {
            return this.preferredFunctionalCurrencyField;
        }
        set
        {
            this.preferredFunctionalCurrencyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Gender
    {
        get
        {
            return this.genderField;
        }
        set
        {
            this.genderField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object MaritalStatus
    {
        get
        {
            return this.maritalStatusField;
        }
        set
        {
            this.maritalStatusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Comments
    {
        get
        {
            return this.commentsField;
        }
        set
        {
            this.commentsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DateOfBirth
    {
        get
        {
            return this.dateOfBirthField;
        }
        set
        {
            this.dateOfBirthField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object UserGUID
    {
        get
        {
            return this.userGUIDField;
        }
        set
        {
            this.userGUIDField = value;
        }
    }

    /// <remarks/>
    public string PartyUniqueName
    {
        get
        {
            return this.partyUniqueNameField;
        }
        set
        {
            this.partyUniqueNameField = value;
        }
    }

    /// <remarks/>
    public string SourceSystem
    {
        get
        {
            return this.sourceSystemField;
        }
        set
        {
            this.sourceSystemField = value;
        }
    }

    /// <remarks/>
    public string SourceSystemReferenceValue
    {
        get
        {
            return this.sourceSystemReferenceValueField;
        }
        set
        {
            this.sourceSystemReferenceValueField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SourceSystemUpdateDate
    {
        get
        {
            return this.sourceSystemUpdateDateField;
        }
        set
        {
            this.sourceSystemUpdateDateField = value;
        }
    }

    /// <remarks/>
    public CreatePersonValuePersonProfile PersonProfile
    {
        get
        {
            return this.personProfileField;
        }
        set
        {
            this.personProfileField = value;
        }
    }

    /// <remarks/>
    public CreatePersonValueRelationship Relationship
    {
        get
        {
            return this.relationshipField;
        }
        set
        {
            this.relationshipField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class CreatePersonValuePersonProfile
{

    private ulong personProfileIdField;

    private ulong partyIdField;

    private string personNameField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private object personPreNameAdjunctField;

    private string personFirstNameField;

    private object personMiddleNameField;

    private string personLastNameField;

    private object personNameSuffixField;

    private object personTitleField;

    private object personAcademicTitleField;

    private object personPreviousLastNameField;

    private object personInitialsField;

    private object jgzzFiscalCodeField;

    private object dateOfBirthField;

    private object placeOfBirthField;

    private object dateOfDeathField;

    private object genderField;

    private object declaredEthnicityField;

    private object maritalStatusField;

    private object maritalStatusEffectiveDateField;

    private object personalIncomeAmountField;

    private object rentOwnIndField;

    private object lastKnownGPSField;

    private System.DateTime effectiveStartDateField;

    private System.DateTime effectiveEndDateField;

    private bool internalFlagField;

    private string statusField;

    private string createdByModuleField;

    private bool deceasedFlagField;

    private object commentsField;

    private object personLastNamePrefixField;

    private object personSecondLastNameField;

    private object preferredFunctionalCurrencyField;

    private object origSystemField;

    private string origSystemReferenceField;

    private byte effectiveSequenceField;

    private object headOfHouseholdFlagField;

    private object householdIncomeAmountField;

    private object householdSizeField;

    private string effectiveLatestChangeField;

    private bool suffixOverriddenFlagField;

    private object uniqueNameSuffixField;

    private string corpCurrencyCodeField;

    private string curcyConvRateTypeField;

    private string currencyCodeField;

    private uint partyNumberField;

    private object salutationField;

    private object certReasonCodeField;

    private object certificationLevelField;

    private object preferredContactMethodField;

    private object preferredContactPersonIdField;

    private object primaryAddressLine1Field;

    private object primaryAddressLine2Field;

    private object primaryAddressLine3Field;

    private object primaryAddressLine4Field;

    private object aliasField;

    private object primaryAddressCityField;

    private object primaryAddressCountryField;

    private object primaryAddressCountyField;

    private string primaryEmailAddressField;

    private object primaryFormattedAddressField;

    private uint primaryFormattedPhoneNumberField;

    private object primaryLanguageField;

    private string partyUniqueNameField;

    private object primaryAddressPostalCodeField;

    private object preferredContactEmailField;

    private object preferredContactNameField;

    private object preferredContactPhoneField;

    private object preferredContactURLField;

    private object preferredNameField;

    private object preferredNameIdField;

    private ulong primaryEmailIdField;

    private object primaryPhoneAreaCodeField;

    private ulong primaryPhoneIdField;

    private object primaryPhoneCountryCodeField;

    private object primaryPhoneExtensionField;

    private string primaryPhoneLineTypeField;

    private string primaryPhoneNumberField;

    private object primaryPhonePurposeField;

    private object primaryWebIdField;

    private object pronunciationField;

    private object primaryAddressProvinceField;

    private object primaryAddressStateField;

    private object primaryURLField;

    private object validatedFlagField;

    private object primaryAddressLatitudeField;

    private object primaryAddressLongitudeField;

    private object primaryAddressLocationIdField;

    private bool favoriteContactFlagField;

    private object distanceField;

    private object salesAffinityCodeField;

    private object salesBuyingRoleCodeField;

    private object departmentCodeField;

    private object departmentField;

    private object jobTitleCodeField;

    private object jobTitleField;

    private bool doNotCallFlagField;

    private bool doNotContactFlagField;

    private bool doNotEmailFlagField;

    private bool doNotMailFlagField;

    private object lastContactDateField;

    private ulong primaryCustomerIdField;

    private ulong primaryCustomerRelationshipIdField;

    private string primaryCustomerNameField;

    private System.DateTime lastSourceUpdateDateField;

    private string lastUpdateSourceSystemField;

    private object dataCloudStatusField;

    private object lastEnrichmentDateField;

    /// <remarks/>
    public ulong PersonProfileId
    {
        get
        {
            return this.personProfileIdField;
        }
        set
        {
            this.personProfileIdField = value;
        }
    }

    /// <remarks/>
    public ulong PartyId
    {
        get
        {
            return this.partyIdField;
        }
        set
        {
            this.partyIdField = value;
        }
    }

    /// <remarks/>
    public string PersonName
    {
        get
        {
            return this.personNameField;
        }
        set
        {
            this.personNameField = value;
        }
    }

    /// <remarks/>
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonPreNameAdjunct
    {
        get
        {
            return this.personPreNameAdjunctField;
        }
        set
        {
            this.personPreNameAdjunctField = value;
        }
    }

    /// <remarks/>
    public string PersonFirstName
    {
        get
        {
            return this.personFirstNameField;
        }
        set
        {
            this.personFirstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonMiddleName
    {
        get
        {
            return this.personMiddleNameField;
        }
        set
        {
            this.personMiddleNameField = value;
        }
    }

    /// <remarks/>
    public string PersonLastName
    {
        get
        {
            return this.personLastNameField;
        }
        set
        {
            this.personLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonNameSuffix
    {
        get
        {
            return this.personNameSuffixField;
        }
        set
        {
            this.personNameSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonTitle
    {
        get
        {
            return this.personTitleField;
        }
        set
        {
            this.personTitleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonAcademicTitle
    {
        get
        {
            return this.personAcademicTitleField;
        }
        set
        {
            this.personAcademicTitleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonPreviousLastName
    {
        get
        {
            return this.personPreviousLastNameField;
        }
        set
        {
            this.personPreviousLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonInitials
    {
        get
        {
            return this.personInitialsField;
        }
        set
        {
            this.personInitialsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object JgzzFiscalCode
    {
        get
        {
            return this.jgzzFiscalCodeField;
        }
        set
        {
            this.jgzzFiscalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DateOfBirth
    {
        get
        {
            return this.dateOfBirthField;
        }
        set
        {
            this.dateOfBirthField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PlaceOfBirth
    {
        get
        {
            return this.placeOfBirthField;
        }
        set
        {
            this.placeOfBirthField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DateOfDeath
    {
        get
        {
            return this.dateOfDeathField;
        }
        set
        {
            this.dateOfDeathField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Gender
    {
        get
        {
            return this.genderField;
        }
        set
        {
            this.genderField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DeclaredEthnicity
    {
        get
        {
            return this.declaredEthnicityField;
        }
        set
        {
            this.declaredEthnicityField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object MaritalStatus
    {
        get
        {
            return this.maritalStatusField;
        }
        set
        {
            this.maritalStatusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object MaritalStatusEffectiveDate
    {
        get
        {
            return this.maritalStatusEffectiveDateField;
        }
        set
        {
            this.maritalStatusEffectiveDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonalIncomeAmount
    {
        get
        {
            return this.personalIncomeAmountField;
        }
        set
        {
            this.personalIncomeAmountField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object RentOwnInd
    {
        get
        {
            return this.rentOwnIndField;
        }
        set
        {
            this.rentOwnIndField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object LastKnownGPS
    {
        get
        {
            return this.lastKnownGPSField;
        }
        set
        {
            this.lastKnownGPSField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime EffectiveStartDate
    {
        get
        {
            return this.effectiveStartDateField;
        }
        set
        {
            this.effectiveStartDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime EffectiveEndDate
    {
        get
        {
            return this.effectiveEndDateField;
        }
        set
        {
            this.effectiveEndDateField = value;
        }
    }

    /// <remarks/>
    public bool InternalFlag
    {
        get
        {
            return this.internalFlagField;
        }
        set
        {
            this.internalFlagField = value;
        }
    }

    /// <remarks/>
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }

    /// <remarks/>
    public bool DeceasedFlag
    {
        get
        {
            return this.deceasedFlagField;
        }
        set
        {
            this.deceasedFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Comments
    {
        get
        {
            return this.commentsField;
        }
        set
        {
            this.commentsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonLastNamePrefix
    {
        get
        {
            return this.personLastNamePrefixField;
        }
        set
        {
            this.personLastNamePrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonSecondLastName
    {
        get
        {
            return this.personSecondLastNameField;
        }
        set
        {
            this.personSecondLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredFunctionalCurrency
    {
        get
        {
            return this.preferredFunctionalCurrencyField;
        }
        set
        {
            this.preferredFunctionalCurrencyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object OrigSystem
    {
        get
        {
            return this.origSystemField;
        }
        set
        {
            this.origSystemField = value;
        }
    }

    /// <remarks/>
    public string OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    public byte EffectiveSequence
    {
        get
        {
            return this.effectiveSequenceField;
        }
        set
        {
            this.effectiveSequenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object HeadOfHouseholdFlag
    {
        get
        {
            return this.headOfHouseholdFlagField;
        }
        set
        {
            this.headOfHouseholdFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object HouseholdIncomeAmount
    {
        get
        {
            return this.householdIncomeAmountField;
        }
        set
        {
            this.householdIncomeAmountField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object HouseholdSize
    {
        get
        {
            return this.householdSizeField;
        }
        set
        {
            this.householdSizeField = value;
        }
    }

    /// <remarks/>
    public string EffectiveLatestChange
    {
        get
        {
            return this.effectiveLatestChangeField;
        }
        set
        {
            this.effectiveLatestChangeField = value;
        }
    }

    /// <remarks/>
    public bool SuffixOverriddenFlag
    {
        get
        {
            return this.suffixOverriddenFlagField;
        }
        set
        {
            this.suffixOverriddenFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object UniqueNameSuffix
    {
        get
        {
            return this.uniqueNameSuffixField;
        }
        set
        {
            this.uniqueNameSuffixField = value;
        }
    }

    /// <remarks/>
    public string CorpCurrencyCode
    {
        get
        {
            return this.corpCurrencyCodeField;
        }
        set
        {
            this.corpCurrencyCodeField = value;
        }
    }

    /// <remarks/>
    public string CurcyConvRateType
    {
        get
        {
            return this.curcyConvRateTypeField;
        }
        set
        {
            this.curcyConvRateTypeField = value;
        }
    }

    /// <remarks/>
    public string CurrencyCode
    {
        get
        {
            return this.currencyCodeField;
        }
        set
        {
            this.currencyCodeField = value;
        }
    }

    /// <remarks/>
    public uint PartyNumber
    {
        get
        {
            return this.partyNumberField;
        }
        set
        {
            this.partyNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Salutation
    {
        get
        {
            return this.salutationField;
        }
        set
        {
            this.salutationField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object CertReasonCode
    {
        get
        {
            return this.certReasonCodeField;
        }
        set
        {
            this.certReasonCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object CertificationLevel
    {
        get
        {
            return this.certificationLevelField;
        }
        set
        {
            this.certificationLevelField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactMethod
    {
        get
        {
            return this.preferredContactMethodField;
        }
        set
        {
            this.preferredContactMethodField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactPersonId
    {
        get
        {
            return this.preferredContactPersonIdField;
        }
        set
        {
            this.preferredContactPersonIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressLine1
    {
        get
        {
            return this.primaryAddressLine1Field;
        }
        set
        {
            this.primaryAddressLine1Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressLine2
    {
        get
        {
            return this.primaryAddressLine2Field;
        }
        set
        {
            this.primaryAddressLine2Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressLine3
    {
        get
        {
            return this.primaryAddressLine3Field;
        }
        set
        {
            this.primaryAddressLine3Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressLine4
    {
        get
        {
            return this.primaryAddressLine4Field;
        }
        set
        {
            this.primaryAddressLine4Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Alias
    {
        get
        {
            return this.aliasField;
        }
        set
        {
            this.aliasField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressCity
    {
        get
        {
            return this.primaryAddressCityField;
        }
        set
        {
            this.primaryAddressCityField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressCountry
    {
        get
        {
            return this.primaryAddressCountryField;
        }
        set
        {
            this.primaryAddressCountryField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressCounty
    {
        get
        {
            return this.primaryAddressCountyField;
        }
        set
        {
            this.primaryAddressCountyField = value;
        }
    }

    /// <remarks/>
    public string PrimaryEmailAddress
    {
        get
        {
            return this.primaryEmailAddressField;
        }
        set
        {
            this.primaryEmailAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryFormattedAddress
    {
        get
        {
            return this.primaryFormattedAddressField;
        }
        set
        {
            this.primaryFormattedAddressField = value;
        }
    }

    /// <remarks/>
    public uint PrimaryFormattedPhoneNumber
    {
        get
        {
            return this.primaryFormattedPhoneNumberField;
        }
        set
        {
            this.primaryFormattedPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryLanguage
    {
        get
        {
            return this.primaryLanguageField;
        }
        set
        {
            this.primaryLanguageField = value;
        }
    }

    /// <remarks/>
    public string PartyUniqueName
    {
        get
        {
            return this.partyUniqueNameField;
        }
        set
        {
            this.partyUniqueNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressPostalCode
    {
        get
        {
            return this.primaryAddressPostalCodeField;
        }
        set
        {
            this.primaryAddressPostalCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactEmail
    {
        get
        {
            return this.preferredContactEmailField;
        }
        set
        {
            this.preferredContactEmailField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactName
    {
        get
        {
            return this.preferredContactNameField;
        }
        set
        {
            this.preferredContactNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactPhone
    {
        get
        {
            return this.preferredContactPhoneField;
        }
        set
        {
            this.preferredContactPhoneField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactURL
    {
        get
        {
            return this.preferredContactURLField;
        }
        set
        {
            this.preferredContactURLField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredName
    {
        get
        {
            return this.preferredNameField;
        }
        set
        {
            this.preferredNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredNameId
    {
        get
        {
            return this.preferredNameIdField;
        }
        set
        {
            this.preferredNameIdField = value;
        }
    }

    /// <remarks/>
    public ulong PrimaryEmailId
    {
        get
        {
            return this.primaryEmailIdField;
        }
        set
        {
            this.primaryEmailIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhoneAreaCode
    {
        get
        {
            return this.primaryPhoneAreaCodeField;
        }
        set
        {
            this.primaryPhoneAreaCodeField = value;
        }
    }

    /// <remarks/>
    public ulong PrimaryPhoneId
    {
        get
        {
            return this.primaryPhoneIdField;
        }
        set
        {
            this.primaryPhoneIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhoneCountryCode
    {
        get
        {
            return this.primaryPhoneCountryCodeField;
        }
        set
        {
            this.primaryPhoneCountryCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhoneExtension
    {
        get
        {
            return this.primaryPhoneExtensionField;
        }
        set
        {
            this.primaryPhoneExtensionField = value;
        }
    }

    /// <remarks/>
    public string PrimaryPhoneLineType
    {
        get
        {
            return this.primaryPhoneLineTypeField;
        }
        set
        {
            this.primaryPhoneLineTypeField = value;
        }
    }

    /// <remarks/>
    public string PrimaryPhoneNumber
    {
        get
        {
            return this.primaryPhoneNumberField;
        }
        set
        {
            this.primaryPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryPhonePurpose
    {
        get
        {
            return this.primaryPhonePurposeField;
        }
        set
        {
            this.primaryPhonePurposeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryWebId
    {
        get
        {
            return this.primaryWebIdField;
        }
        set
        {
            this.primaryWebIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Pronunciation
    {
        get
        {
            return this.pronunciationField;
        }
        set
        {
            this.pronunciationField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressProvince
    {
        get
        {
            return this.primaryAddressProvinceField;
        }
        set
        {
            this.primaryAddressProvinceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressState
    {
        get
        {
            return this.primaryAddressStateField;
        }
        set
        {
            this.primaryAddressStateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryURL
    {
        get
        {
            return this.primaryURLField;
        }
        set
        {
            this.primaryURLField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object ValidatedFlag
    {
        get
        {
            return this.validatedFlagField;
        }
        set
        {
            this.validatedFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressLatitude
    {
        get
        {
            return this.primaryAddressLatitudeField;
        }
        set
        {
            this.primaryAddressLatitudeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PrimaryAddressLongitude
    {
        get
        {
            return this.primaryAddressLongitudeField;
        }
        set
        {
            this.primaryAddressLongitudeField = value;
        }
    }

    /// <remarks/>
    public object PrimaryAddressLocationId
    {
        get
        {
            return this.primaryAddressLocationIdField;
        }
        set
        {
            this.primaryAddressLocationIdField = value;
        }
    }

    /// <remarks/>
    public bool FavoriteContactFlag
    {
        get
        {
            return this.favoriteContactFlagField;
        }
        set
        {
            this.favoriteContactFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Distance
    {
        get
        {
            return this.distanceField;
        }
        set
        {
            this.distanceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SalesAffinityCode
    {
        get
        {
            return this.salesAffinityCodeField;
        }
        set
        {
            this.salesAffinityCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SalesBuyingRoleCode
    {
        get
        {
            return this.salesBuyingRoleCodeField;
        }
        set
        {
            this.salesBuyingRoleCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DepartmentCode
    {
        get
        {
            return this.departmentCodeField;
        }
        set
        {
            this.departmentCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Department
    {
        get
        {
            return this.departmentField;
        }
        set
        {
            this.departmentField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object JobTitleCode
    {
        get
        {
            return this.jobTitleCodeField;
        }
        set
        {
            this.jobTitleCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object JobTitle
    {
        get
        {
            return this.jobTitleField;
        }
        set
        {
            this.jobTitleField = value;
        }
    }

    /// <remarks/>
    public bool DoNotCallFlag
    {
        get
        {
            return this.doNotCallFlagField;
        }
        set
        {
            this.doNotCallFlagField = value;
        }
    }

    /// <remarks/>
    public bool DoNotContactFlag
    {
        get
        {
            return this.doNotContactFlagField;
        }
        set
        {
            this.doNotContactFlagField = value;
        }
    }

    /// <remarks/>
    public bool DoNotEmailFlag
    {
        get
        {
            return this.doNotEmailFlagField;
        }
        set
        {
            this.doNotEmailFlagField = value;
        }
    }

    /// <remarks/>
    public bool DoNotMailFlag
    {
        get
        {
            return this.doNotMailFlagField;
        }
        set
        {
            this.doNotMailFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object LastContactDate
    {
        get
        {
            return this.lastContactDateField;
        }
        set
        {
            this.lastContactDateField = value;
        }
    }

    /// <remarks/>
    public ulong PrimaryCustomerId
    {
        get
        {
            return this.primaryCustomerIdField;
        }
        set
        {
            this.primaryCustomerIdField = value;
        }
    }

    /// <remarks/>
    public ulong PrimaryCustomerRelationshipId
    {
        get
        {
            return this.primaryCustomerRelationshipIdField;
        }
        set
        {
            this.primaryCustomerRelationshipIdField = value;
        }
    }

    /// <remarks/>
    public string PrimaryCustomerName
    {
        get
        {
            return this.primaryCustomerNameField;
        }
        set
        {
            this.primaryCustomerNameField = value;
        }
    }

    /// <remarks/>
    public System.DateTime LastSourceUpdateDate
    {
        get
        {
            return this.lastSourceUpdateDateField;
        }
        set
        {
            this.lastSourceUpdateDateField = value;
        }
    }

    /// <remarks/>
    public string LastUpdateSourceSystem
    {
        get
        {
            return this.lastUpdateSourceSystemField;
        }
        set
        {
            this.lastUpdateSourceSystemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DataCloudStatus
    {
        get
        {
            return this.dataCloudStatusField;
        }
        set
        {
            this.dataCloudStatusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object LastEnrichmentDate
    {
        get
        {
            return this.lastEnrichmentDateField;
        }
        set
        {
            this.lastEnrichmentDateField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/personService/")]
public partial class CreatePersonValueRelationship
{

    private ulong relationshipRecIdField;

    private ulong relationshipIdField;

    private ulong subjectIdField;

    private string subjectTypeField;

    private string subjectTableNameField;

    private ulong objectIdField;

    private string objectTypeField;

    private string objectTableNameField;

    private string relationshipCodeField;

    private string relationshipTypeField;

    private string roleField;

    private object commentsField;

    private System.DateTime startDateField;

    private System.DateTime endDateField;

    private string statusField;

    private string createdByField;

    private System.DateTime creationDateField;

    private string lastUpdatedByField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    private object additionalInformation1Field;

    private object additionalInformation2Field;

    private object additionalInformation3Field;

    private object additionalInformation4Field;

    private object additionalInformation5Field;

    private object additionalInformation6Field;

    private object additionalInformation7Field;

    private object additionalInformation8Field;

    private object additionalInformation9Field;

    private object additionalInformation10Field;

    private object additionalInformation11Field;

    private object additionalInformation12Field;

    private object additionalInformation13Field;

    private object additionalInformation14Field;

    private object additionalInformation15Field;

    private object additionalInformation16Field;

    private object additionalInformation17Field;

    private object additionalInformation18Field;

    private object additionalInformation19Field;

    private object additionalInformation20Field;

    private object additionalInformation21Field;

    private object additionalInformation22Field;

    private object additionalInformation23Field;

    private object additionalInformation24Field;

    private object additionalInformation25Field;

    private object additionalInformation26Field;

    private object additionalInformation27Field;

    private object additionalInformation28Field;

    private object additionalInformation29Field;

    private object additionalInformation30Field;

    private string directionCodeField;

    private object percentageOwnershipField;

    private object objectUsageCodeField;

    private object subjectUsageCodeField;

    private bool preferredContactFlagField;

    private string objectPartyNameField;

    private string partyNameField;

    private string currencyCodeField;

    private string curcyConvRateTypeField;

    private string corpCurrencyCodeField;

    private bool primaryCustomerFlagField;

    private string subjectEmailAddressField;

    private object objectEmailAddressField;

    private CreatePersonOrganizationContact organizationContactField;

    private CreatePersonPhone phoneField;

    private CreatePersonEmail emailField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong RelationshipRecId
    {
        get
        {
            return this.relationshipRecIdField;
        }
        set
        {
            this.relationshipRecIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong RelationshipId
    {
        get
        {
            return this.relationshipIdField;
        }
        set
        {
            this.relationshipIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong SubjectId
    {
        get
        {
            return this.subjectIdField;
        }
        set
        {
            this.subjectIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string SubjectType
    {
        get
        {
            return this.subjectTypeField;
        }
        set
        {
            this.subjectTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string SubjectTableName
    {
        get
        {
            return this.subjectTableNameField;
        }
        set
        {
            this.subjectTableNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public ulong ObjectId
    {
        get
        {
            return this.objectIdField;
        }
        set
        {
            this.objectIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string ObjectType
    {
        get
        {
            return this.objectTypeField;
        }
        set
        {
            this.objectTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string ObjectTableName
    {
        get
        {
            return this.objectTableNameField;
        }
        set
        {
            this.objectTableNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string RelationshipCode
    {
        get
        {
            return this.relationshipCodeField;
        }
        set
        {
            this.relationshipCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string RelationshipType
    {
        get
        {
            return this.relationshipTypeField;
        }
        set
        {
            this.relationshipTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string Role
    {
        get
        {
            return this.roleField;
        }
        set
        {
            this.roleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object Comments
    {
        get
        {
            return this.commentsField;
        }
        set
        {
            this.commentsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", DataType = "date")]
    public System.DateTime StartDate
    {
        get
        {
            return this.startDateField;
        }
        set
        {
            this.startDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", DataType = "date")]
    public System.DateTime EndDate
    {
        get
        {
            return this.endDateField;
        }
        set
        {
            this.endDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public byte ObjectVersionNumber
    {
        get
        {
            return this.objectVersionNumberField;
        }
        set
        {
            this.objectVersionNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation1
    {
        get
        {
            return this.additionalInformation1Field;
        }
        set
        {
            this.additionalInformation1Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation2
    {
        get
        {
            return this.additionalInformation2Field;
        }
        set
        {
            this.additionalInformation2Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation3
    {
        get
        {
            return this.additionalInformation3Field;
        }
        set
        {
            this.additionalInformation3Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation4
    {
        get
        {
            return this.additionalInformation4Field;
        }
        set
        {
            this.additionalInformation4Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation5
    {
        get
        {
            return this.additionalInformation5Field;
        }
        set
        {
            this.additionalInformation5Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation6
    {
        get
        {
            return this.additionalInformation6Field;
        }
        set
        {
            this.additionalInformation6Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation7
    {
        get
        {
            return this.additionalInformation7Field;
        }
        set
        {
            this.additionalInformation7Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation8
    {
        get
        {
            return this.additionalInformation8Field;
        }
        set
        {
            this.additionalInformation8Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation9
    {
        get
        {
            return this.additionalInformation9Field;
        }
        set
        {
            this.additionalInformation9Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation10
    {
        get
        {
            return this.additionalInformation10Field;
        }
        set
        {
            this.additionalInformation10Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation11
    {
        get
        {
            return this.additionalInformation11Field;
        }
        set
        {
            this.additionalInformation11Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation12
    {
        get
        {
            return this.additionalInformation12Field;
        }
        set
        {
            this.additionalInformation12Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation13
    {
        get
        {
            return this.additionalInformation13Field;
        }
        set
        {
            this.additionalInformation13Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation14
    {
        get
        {
            return this.additionalInformation14Field;
        }
        set
        {
            this.additionalInformation14Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation15
    {
        get
        {
            return this.additionalInformation15Field;
        }
        set
        {
            this.additionalInformation15Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation16
    {
        get
        {
            return this.additionalInformation16Field;
        }
        set
        {
            this.additionalInformation16Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation17
    {
        get
        {
            return this.additionalInformation17Field;
        }
        set
        {
            this.additionalInformation17Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation18
    {
        get
        {
            return this.additionalInformation18Field;
        }
        set
        {
            this.additionalInformation18Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation19
    {
        get
        {
            return this.additionalInformation19Field;
        }
        set
        {
            this.additionalInformation19Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation20
    {
        get
        {
            return this.additionalInformation20Field;
        }
        set
        {
            this.additionalInformation20Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation21
    {
        get
        {
            return this.additionalInformation21Field;
        }
        set
        {
            this.additionalInformation21Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation22
    {
        get
        {
            return this.additionalInformation22Field;
        }
        set
        {
            this.additionalInformation22Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation23
    {
        get
        {
            return this.additionalInformation23Field;
        }
        set
        {
            this.additionalInformation23Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation24
    {
        get
        {
            return this.additionalInformation24Field;
        }
        set
        {
            this.additionalInformation24Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation25
    {
        get
        {
            return this.additionalInformation25Field;
        }
        set
        {
            this.additionalInformation25Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation26
    {
        get
        {
            return this.additionalInformation26Field;
        }
        set
        {
            this.additionalInformation26Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation27
    {
        get
        {
            return this.additionalInformation27Field;
        }
        set
        {
            this.additionalInformation27Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation28
    {
        get
        {
            return this.additionalInformation28Field;
        }
        set
        {
            this.additionalInformation28Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation29
    {
        get
        {
            return this.additionalInformation29Field;
        }
        set
        {
            this.additionalInformation29Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object AdditionalInformation30
    {
        get
        {
            return this.additionalInformation30Field;
        }
        set
        {
            this.additionalInformation30Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string DirectionCode
    {
        get
        {
            return this.directionCodeField;
        }
        set
        {
            this.directionCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object PercentageOwnership
    {
        get
        {
            return this.percentageOwnershipField;
        }
        set
        {
            this.percentageOwnershipField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object ObjectUsageCode
    {
        get
        {
            return this.objectUsageCodeField;
        }
        set
        {
            this.objectUsageCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object SubjectUsageCode
    {
        get
        {
            return this.subjectUsageCodeField;
        }
        set
        {
            this.subjectUsageCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public bool PreferredContactFlag
    {
        get
        {
            return this.preferredContactFlagField;
        }
        set
        {
            this.preferredContactFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string ObjectPartyName
    {
        get
        {
            return this.objectPartyNameField;
        }
        set
        {
            this.objectPartyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string PartyName
    {
        get
        {
            return this.partyNameField;
        }
        set
        {
            this.partyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CurrencyCode
    {
        get
        {
            return this.currencyCodeField;
        }
        set
        {
            this.currencyCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CurcyConvRateType
    {
        get
        {
            return this.curcyConvRateTypeField;
        }
        set
        {
            this.curcyConvRateTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string CorpCurrencyCode
    {
        get
        {
            return this.corpCurrencyCodeField;
        }
        set
        {
            this.corpCurrencyCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public bool PrimaryCustomerFlag
    {
        get
        {
            return this.primaryCustomerFlagField;
        }
        set
        {
            this.primaryCustomerFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public string SubjectEmailAddress
    {
        get
        {
            return this.subjectEmailAddressField;
        }
        set
        {
            this.subjectEmailAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = true)]
    public object ObjectEmailAddress
    {
        get
        {
            return this.objectEmailAddressField;
        }
        set
        {
            this.objectEmailAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public CreatePersonOrganizationContact OrganizationContact
    {
        get
        {
            return this.organizationContactField;
        }
        set
        {
            this.organizationContactField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public CreatePersonPhone Phone
    {
        get
        {
            return this.phoneField;
        }
        set
        {
            this.phoneField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
    public CreatePersonEmail Email
    {
        get
        {
            return this.emailField;
        }
        set
        {
            this.emailField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class CreatePersonOrganizationContact
{

    private ulong orgContactIdField;

    private ulong partyRelationshipIdField;

    private ulong contactPartyIdField;

    private string personFirstNameField;

    private string personLastNameField;

    private string contactNameField;

    private object personPreNameAdjunctField;

    private object personMiddleNameField;

    private object personNameSuffixField;

    private object personPreviousLastNameField;

    private object personAcademicTitleField;

    private object salutationField;

    private object personLastNamePrefixField;

    private object preferredNameField;

    private object personSecondLastNameField;

    private object personLanguageNameField;

    private object personTitleField;

    private object personCertificationLevelField;

    private object personCertReasonCodeField;

    private ulong customerPartyIdField;

    private string customerUniqueNameField;

    private string customerNameField;

    private uint customerPartyNumberField;

    private object formattedPhoneNumberField;

    private object emailAddressField;

    private object webUrlField;

    private object commentsField;

    private uint contactNumberField;

    private object departmentCodeField;

    private object departmentField;

    private object jobTitleField;

    private bool decisionMakerFlagField;

    private object jobTitleCodeField;

    private bool referenceUseFlagField;

    private object rankField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private object partySiteIdField;

    private ulong origSystemReferenceField;

    private string createdByModuleField;

    private byte objectVersionNumberField;

    private object partySiteNameField;

    private object salesAffinityCodeField;

    private object salesAffinityCommentsField;

    private object salesBuyingRoleCodeField;

    private object salesInfluenceLevelCodeField;

    private object formattedAddressField;

    private object preferredContactMethodField;

    private string currencyCodeField;

    private string curcyConvRateTypeField;

    private string corpCurrencyCodeField;

    private bool preferredContactFlagField;

    private object contactFormattedAddressField;

    private object contactFormattedMultilineAddressField;

    private object customerEmailAddressField;

    private bool primaryCustomerFlagField;

    private CreatePersonOrganizationContactOrganizationContactRole organizationContactRoleField;

    /// <remarks/>
    public ulong OrgContactId
    {
        get
        {
            return this.orgContactIdField;
        }
        set
        {
            this.orgContactIdField = value;
        }
    }

    /// <remarks/>
    public ulong PartyRelationshipId
    {
        get
        {
            return this.partyRelationshipIdField;
        }
        set
        {
            this.partyRelationshipIdField = value;
        }
    }

    /// <remarks/>
    public ulong ContactPartyId
    {
        get
        {
            return this.contactPartyIdField;
        }
        set
        {
            this.contactPartyIdField = value;
        }
    }

    /// <remarks/>
    public string PersonFirstName
    {
        get
        {
            return this.personFirstNameField;
        }
        set
        {
            this.personFirstNameField = value;
        }
    }

    /// <remarks/>
    public string PersonLastName
    {
        get
        {
            return this.personLastNameField;
        }
        set
        {
            this.personLastNameField = value;
        }
    }

    /// <remarks/>
    public string ContactName
    {
        get
        {
            return this.contactNameField;
        }
        set
        {
            this.contactNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonPreNameAdjunct
    {
        get
        {
            return this.personPreNameAdjunctField;
        }
        set
        {
            this.personPreNameAdjunctField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonMiddleName
    {
        get
        {
            return this.personMiddleNameField;
        }
        set
        {
            this.personMiddleNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonNameSuffix
    {
        get
        {
            return this.personNameSuffixField;
        }
        set
        {
            this.personNameSuffixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonPreviousLastName
    {
        get
        {
            return this.personPreviousLastNameField;
        }
        set
        {
            this.personPreviousLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonAcademicTitle
    {
        get
        {
            return this.personAcademicTitleField;
        }
        set
        {
            this.personAcademicTitleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Salutation
    {
        get
        {
            return this.salutationField;
        }
        set
        {
            this.salutationField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonLastNamePrefix
    {
        get
        {
            return this.personLastNamePrefixField;
        }
        set
        {
            this.personLastNamePrefixField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredName
    {
        get
        {
            return this.preferredNameField;
        }
        set
        {
            this.preferredNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonSecondLastName
    {
        get
        {
            return this.personSecondLastNameField;
        }
        set
        {
            this.personSecondLastNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonLanguageName
    {
        get
        {
            return this.personLanguageNameField;
        }
        set
        {
            this.personLanguageNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonTitle
    {
        get
        {
            return this.personTitleField;
        }
        set
        {
            this.personTitleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonCertificationLevel
    {
        get
        {
            return this.personCertificationLevelField;
        }
        set
        {
            this.personCertificationLevelField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PersonCertReasonCode
    {
        get
        {
            return this.personCertReasonCodeField;
        }
        set
        {
            this.personCertReasonCodeField = value;
        }
    }

    /// <remarks/>
    public ulong CustomerPartyId
    {
        get
        {
            return this.customerPartyIdField;
        }
        set
        {
            this.customerPartyIdField = value;
        }
    }

    /// <remarks/>
    public string CustomerUniqueName
    {
        get
        {
            return this.customerUniqueNameField;
        }
        set
        {
            this.customerUniqueNameField = value;
        }
    }

    /// <remarks/>
    public string CustomerName
    {
        get
        {
            return this.customerNameField;
        }
        set
        {
            this.customerNameField = value;
        }
    }

    /// <remarks/>
    public uint CustomerPartyNumber
    {
        get
        {
            return this.customerPartyNumberField;
        }
        set
        {
            this.customerPartyNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object FormattedPhoneNumber
    {
        get
        {
            return this.formattedPhoneNumberField;
        }
        set
        {
            this.formattedPhoneNumberField = value;
        }
    }

    /// <remarks/>
    public object EmailAddress
    {
        get
        {
            return this.emailAddressField;
        }
        set
        {
            this.emailAddressField = value;
        }
    }

    /// <remarks/>
    public object WebUrl
    {
        get
        {
            return this.webUrlField;
        }
        set
        {
            this.webUrlField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Comments
    {
        get
        {
            return this.commentsField;
        }
        set
        {
            this.commentsField = value;
        }
    }

    /// <remarks/>
    public uint ContactNumber
    {
        get
        {
            return this.contactNumberField;
        }
        set
        {
            this.contactNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object DepartmentCode
    {
        get
        {
            return this.departmentCodeField;
        }
        set
        {
            this.departmentCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Department
    {
        get
        {
            return this.departmentField;
        }
        set
        {
            this.departmentField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object JobTitle
    {
        get
        {
            return this.jobTitleField;
        }
        set
        {
            this.jobTitleField = value;
        }
    }

    /// <remarks/>
    public bool DecisionMakerFlag
    {
        get
        {
            return this.decisionMakerFlagField;
        }
        set
        {
            this.decisionMakerFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object JobTitleCode
    {
        get
        {
            return this.jobTitleCodeField;
        }
        set
        {
            this.jobTitleCodeField = value;
        }
    }

    /// <remarks/>
    public bool ReferenceUseFlag
    {
        get
        {
            return this.referenceUseFlagField;
        }
        set
        {
            this.referenceUseFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object Rank
    {
        get
        {
            return this.rankField;
        }
        set
        {
            this.rankField = value;
        }
    }

    /// <remarks/>
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PartySiteId
    {
        get
        {
            return this.partySiteIdField;
        }
        set
        {
            this.partySiteIdField = value;
        }
    }

    /// <remarks/>
    public ulong OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }

    /// <remarks/>
    public byte ObjectVersionNumber
    {
        get
        {
            return this.objectVersionNumberField;
        }
        set
        {
            this.objectVersionNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PartySiteName
    {
        get
        {
            return this.partySiteNameField;
        }
        set
        {
            this.partySiteNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SalesAffinityCode
    {
        get
        {
            return this.salesAffinityCodeField;
        }
        set
        {
            this.salesAffinityCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SalesAffinityComments
    {
        get
        {
            return this.salesAffinityCommentsField;
        }
        set
        {
            this.salesAffinityCommentsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SalesBuyingRoleCode
    {
        get
        {
            return this.salesBuyingRoleCodeField;
        }
        set
        {
            this.salesBuyingRoleCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object SalesInfluenceLevelCode
    {
        get
        {
            return this.salesInfluenceLevelCodeField;
        }
        set
        {
            this.salesInfluenceLevelCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object FormattedAddress
    {
        get
        {
            return this.formattedAddressField;
        }
        set
        {
            this.formattedAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object PreferredContactMethod
    {
        get
        {
            return this.preferredContactMethodField;
        }
        set
        {
            this.preferredContactMethodField = value;
        }
    }

    /// <remarks/>
    public string CurrencyCode
    {
        get
        {
            return this.currencyCodeField;
        }
        set
        {
            this.currencyCodeField = value;
        }
    }

    /// <remarks/>
    public string CurcyConvRateType
    {
        get
        {
            return this.curcyConvRateTypeField;
        }
        set
        {
            this.curcyConvRateTypeField = value;
        }
    }

    /// <remarks/>
    public string CorpCurrencyCode
    {
        get
        {
            return this.corpCurrencyCodeField;
        }
        set
        {
            this.corpCurrencyCodeField = value;
        }
    }

    /// <remarks/>
    public bool PreferredContactFlag
    {
        get
        {
            return this.preferredContactFlagField;
        }
        set
        {
            this.preferredContactFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object ContactFormattedAddress
    {
        get
        {
            return this.contactFormattedAddressField;
        }
        set
        {
            this.contactFormattedAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object ContactFormattedMultilineAddress
    {
        get
        {
            return this.contactFormattedMultilineAddressField;
        }
        set
        {
            this.contactFormattedMultilineAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object CustomerEmailAddress
    {
        get
        {
            return this.customerEmailAddressField;
        }
        set
        {
            this.customerEmailAddressField = value;
        }
    }

    /// <remarks/>
    public bool PrimaryCustomerFlag
    {
        get
        {
            return this.primaryCustomerFlagField;
        }
        set
        {
            this.primaryCustomerFlagField = value;
        }
    }

    /// <remarks/>
    public CreatePersonOrganizationContactOrganizationContactRole OrganizationContactRole
    {
        get
        {
            return this.organizationContactRoleField;
        }
        set
        {
            this.organizationContactRoleField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
public partial class CreatePersonOrganizationContactOrganizationContactRole
{

    private ulong orgContactRoleIdField;

    private ulong origSystemReferenceField;

    private string createdByField;

    private string roleTypeField;

    private ulong orgContactIdField;

    private System.DateTime creationDateField;

    private object roleLevelField;

    private bool primaryFlagField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private string lastUpdateLoginField;

    private string primaryContactPerRoleTypeField;

    private object requestIdField;

    private string statusField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    /// <remarks/>
    public ulong OrgContactRoleId
    {
        get
        {
            return this.orgContactRoleIdField;
        }
        set
        {
            this.orgContactRoleIdField = value;
        }
    }

    /// <remarks/>
    public ulong OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    public string RoleType
    {
        get
        {
            return this.roleTypeField;
        }
        set
        {
            this.roleTypeField = value;
        }
    }

    /// <remarks/>
    public ulong OrgContactId
    {
        get
        {
            return this.orgContactIdField;
        }
        set
        {
            this.orgContactIdField = value;
        }
    }

    /// <remarks/>
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object RoleLevel
    {
        get
        {
            return this.roleLevelField;
        }
        set
        {
            this.roleLevelField = value;
        }
    }

    /// <remarks/>
    public bool PrimaryFlag
    {
        get
        {
            return this.primaryFlagField;
        }
        set
        {
            this.primaryFlagField = value;
        }
    }

    /// <remarks/>
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    public string PrimaryContactPerRoleType
    {
        get
        {
            return this.primaryContactPerRoleTypeField;
        }
        set
        {
            this.primaryContactPerRoleTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    public byte ObjectVersionNumber
    {
        get
        {
            return this.objectVersionNumberField;
        }
        set
        {
            this.objectVersionNumberField = value;
        }
    }

    /// <remarks/>
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class CreatePersonPhone
{

    private ulong contactPointIdField;

    private string contactPointTypeField;

    private string statusField;

    private string ownerTableNameField;

    private ulong ownerTableIdField;

    private bool primaryFlagField;

    private ulong origSystemReferenceField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    private string contactPointPurposeField;

    private string primaryByPurposeField;

    private System.DateTime startDateField;

    private System.DateTime endDateField;

    private ulong relationshipIdField;

    private object partyUsageCodeField;

    private object origSystemField;

    private object phoneCallingCalendarField;

    private object lastContactDtTimeField;

    private object phoneAreaCodeField;

    private object phoneCountryCodeField;

    private string phoneNumberField;

    private object phoneExtensionField;

    private string phoneLineTypeField;

    private string rawPhoneNumberField;

    private object pagerTypeCodeField;

    private uint formattedPhoneNumberField;

    private uint transposedPhoneNumberField;

    private string partyNameField;

    private object timezoneCodeField;

    private bool overallPrimaryFlagField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId
    {
        get
        {
            return this.contactPointIdField;
        }
        set
        {
            this.contactPointIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string ContactPointType
    {
        get
        {
            return this.contactPointTypeField;
        }
        set
        {
            this.contactPointTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OwnerTableName
    {
        get
        {
            return this.ownerTableNameField;
        }
        set
        {
            this.ownerTableNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong OwnerTableId
    {
        get
        {
            return this.ownerTableIdField;
        }
        set
        {
            this.ownerTableIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool PrimaryFlag
    {
        get
        {
            return this.primaryFlagField;
        }
        set
        {
            this.primaryFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public byte ObjectVersionNumber
    {
        get
        {
            return this.objectVersionNumberField;
        }
        set
        {
            this.objectVersionNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public string ContactPointPurpose
    {
        get
        {
            return this.contactPointPurposeField;
        }
        set
        {
            this.contactPointPurposeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PrimaryByPurpose
    {
        get
        {
            return this.primaryByPurposeField;
        }
        set
        {
            this.primaryByPurposeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public System.DateTime StartDate
    {
        get
        {
            return this.startDateField;
        }
        set
        {
            this.startDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public System.DateTime EndDate
    {
        get
        {
            return this.endDateField;
        }
        set
        {
            this.endDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong RelationshipId
    {
        get
        {
            return this.relationshipIdField;
        }
        set
        {
            this.relationshipIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PartyUsageCode
    {
        get
        {
            return this.partyUsageCodeField;
        }
        set
        {
            this.partyUsageCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object OrigSystem
    {
        get
        {
            return this.origSystemField;
        }
        set
        {
            this.origSystemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PhoneCallingCalendar
    {
        get
        {
            return this.phoneCallingCalendarField;
        }
        set
        {
            this.phoneCallingCalendarField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object LastContactDtTime
    {
        get
        {
            return this.lastContactDtTimeField;
        }
        set
        {
            this.lastContactDtTimeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PhoneAreaCode
    {
        get
        {
            return this.phoneAreaCodeField;
        }
        set
        {
            this.phoneAreaCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PhoneCountryCode
    {
        get
        {
            return this.phoneCountryCodeField;
        }
        set
        {
            this.phoneCountryCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PhoneNumber
    {
        get
        {
            return this.phoneNumberField;
        }
        set
        {
            this.phoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PhoneExtension
    {
        get
        {
            return this.phoneExtensionField;
        }
        set
        {
            this.phoneExtensionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PhoneLineType
    {
        get
        {
            return this.phoneLineTypeField;
        }
        set
        {
            this.phoneLineTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string RawPhoneNumber
    {
        get
        {
            return this.rawPhoneNumberField;
        }
        set
        {
            this.rawPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PagerTypeCode
    {
        get
        {
            return this.pagerTypeCodeField;
        }
        set
        {
            this.pagerTypeCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public uint FormattedPhoneNumber
    {
        get
        {
            return this.formattedPhoneNumberField;
        }
        set
        {
            this.formattedPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public uint TransposedPhoneNumber
    {
        get
        {
            return this.transposedPhoneNumberField;
        }
        set
        {
            this.transposedPhoneNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PartyName
    {
        get
        {
            return this.partyNameField;
        }
        set
        {
            this.partyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object TimezoneCode
    {
        get
        {
            return this.timezoneCodeField;
        }
        set
        {
            this.timezoneCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool OverallPrimaryFlag
    {
        get
        {
            return this.overallPrimaryFlagField;
        }
        set
        {
            this.overallPrimaryFlagField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/relationshipService/", IsNullable = false)]
public partial class CreatePersonEmail
{

    private ulong contactPointIdField;

    private string contactPointTypeField;

    private string statusField;

    private string ownerTableNameField;

    private ulong ownerTableIdField;

    private bool primaryFlagField;

    private ulong origSystemReferenceField;

    private System.DateTime lastUpdateDateField;

    private string lastUpdatedByField;

    private System.DateTime creationDateField;

    private string createdByField;

    private string lastUpdateLoginField;

    private object requestIdField;

    private byte objectVersionNumberField;

    private string createdByModuleField;

    private string contactPointPurposeField;

    private string primaryByPurposeField;

    private System.DateTime startDateField;

    private System.DateTime endDateField;

    private ulong relationshipIdField;

    private object partyUsageCodeField;

    private object origSystemField;

    private object emailFormatField;

    private string emailAddressField;

    private string partyNameField;

    private bool overallPrimaryFlagField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong ContactPointId
    {
        get
        {
            return this.contactPointIdField;
        }
        set
        {
            this.contactPointIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string ContactPointType
    {
        get
        {
            return this.contactPointTypeField;
        }
        set
        {
            this.contactPointTypeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string OwnerTableName
    {
        get
        {
            return this.ownerTableNameField;
        }
        set
        {
            this.ownerTableNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong OwnerTableId
    {
        get
        {
            return this.ownerTableIdField;
        }
        set
        {
            this.ownerTableIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool PrimaryFlag
    {
        get
        {
            return this.primaryFlagField;
        }
        set
        {
            this.primaryFlagField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong OrigSystemReference
    {
        get
        {
            return this.origSystemReferenceField;
        }
        set
        {
            this.origSystemReferenceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public System.DateTime LastUpdateDate
    {
        get
        {
            return this.lastUpdateDateField;
        }
        set
        {
            this.lastUpdateDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdatedBy
    {
        get
        {
            return this.lastUpdatedByField;
        }
        set
        {
            this.lastUpdatedByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public System.DateTime CreationDate
    {
        get
        {
            return this.creationDateField;
        }
        set
        {
            this.creationDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedBy
    {
        get
        {
            return this.createdByField;
        }
        set
        {
            this.createdByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string LastUpdateLogin
    {
        get
        {
            return this.lastUpdateLoginField;
        }
        set
        {
            this.lastUpdateLoginField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object RequestId
    {
        get
        {
            return this.requestIdField;
        }
        set
        {
            this.requestIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public byte ObjectVersionNumber
    {
        get
        {
            return this.objectVersionNumberField;
        }
        set
        {
            this.objectVersionNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string CreatedByModule
    {
        get
        {
            return this.createdByModuleField;
        }
        set
        {
            this.createdByModuleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public string ContactPointPurpose
    {
        get
        {
            return this.contactPointPurposeField;
        }
        set
        {
            this.contactPointPurposeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PrimaryByPurpose
    {
        get
        {
            return this.primaryByPurposeField;
        }
        set
        {
            this.primaryByPurposeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public System.DateTime StartDate
    {
        get
        {
            return this.startDateField;
        }
        set
        {
            this.startDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", DataType = "date")]
    public System.DateTime EndDate
    {
        get
        {
            return this.endDateField;
        }
        set
        {
            this.endDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public ulong RelationshipId
    {
        get
        {
            return this.relationshipIdField;
        }
        set
        {
            this.relationshipIdField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object PartyUsageCode
    {
        get
        {
            return this.partyUsageCodeField;
        }
        set
        {
            this.partyUsageCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/", IsNullable = true)]
    public object OrigSystem
    {
        get
        {
            return this.origSystemField;
        }
        set
        {
            this.origSystemField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public object EmailFormat
    {
        get
        {
            return this.emailFormatField;
        }
        set
        {
            this.emailFormatField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string EmailAddress
    {
        get
        {
            return this.emailAddressField;
        }
        set
        {
            this.emailAddressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public string PartyName
    {
        get
        {
            return this.partyNameField;
        }
        set
        {
            this.partyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/apps/cdm/foundation/parties/contactPointService/")]
    public bool OverallPrimaryFlag
    {
        get
        {
            return this.overallPrimaryFlagField;
        }
        set
        {
            this.overallPrimaryFlagField = value;
        }
    }
}

