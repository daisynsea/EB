namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.ResponseModels
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2003/05/soap-envelope", IsNullable = false)]
    public class SalesOrderReportResponseModel
    {
        private object headerField;

        private EnvelopeBody bodyField;

        /// <remarks/>
        public object Header
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
        public EnvelopeBody Body
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class EnvelopeBody
    {

        private runReportResponse runReportResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
        public runReportResponse runReportResponse
        {
            get
            {
                return this.runReportResponseField;
            }
            set
            {
                this.runReportResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService", IsNullable = false)]
    public class runReportResponse
    {

        private runReportResponseRunReportReturn runReportReturnField;

        /// <remarks/>
        public runReportResponseRunReportReturn runReportReturn
        {
            get
            {
                return this.runReportReturnField;
            }
            set
            {
                this.runReportReturnField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xmlns.oracle.com/oxp/service/PublicReportService")]
    public class runReportResponseRunReportReturn
    {

        private string reportBytesField;

        private string reportContentTypeField;

        private object reportFileIDField;

        private object reportLocaleField;

        private object metaDataListField;

        /// <remarks/>
        public string reportBytes
        {
            get
            {
                return this.reportBytesField;
            }
            set
            {
                this.reportBytesField = value;
            }
        }

        /// <remarks/>
        public string reportContentType
        {
            get
            {
                return this.reportContentTypeField;
            }
            set
            {
                this.reportContentTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object reportFileID
        {
            get
            {
                return this.reportFileIDField;
            }
            set
            {
                this.reportFileIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object reportLocale
        {
            get
            {
                return this.reportLocaleField;
            }
            set
            {
                this.reportLocaleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object metaDataList
        {
            get
            {
                return this.metaDataListField;
            }
            set
            {
                this.metaDataListField = value;
            }
        }
    }
}
