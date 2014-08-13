using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Ueberfuehrung.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    [XmlInclude(typeof(Fahrzeug))]
    [XmlInclude(typeof(Adresse))]
    [XmlInclude(typeof(DienstleistungsAuswahl))]
    [XmlInclude(typeof(Bemerkungen))]
    [XmlInclude(typeof(UploadFiles))]
    [XmlInclude(typeof(Summary))]

    public class UiModel : IUiView
    {
        public int UiIndex { get; set; }

        public string GroupName { get; set; }

        public string SubGroupName { get; set; }

        public string Header { get; set; }

        private string _headerShort;

        public string HeaderShort
        {
            get { return !string.IsNullOrEmpty(_headerShort) ? _headerShort : Header; }
            set { _headerShort = value; }
        }

        public string HeaderCssClass { get; set; }

        public string FormLayerAdditionalCssClass { get; set; }

        [XmlIgnore]
        public virtual string ViewName { get { return ""; } }

        public bool IsMandatory { get; set; }

        public bool RequestClearModel { get; set; }

        public string ValidationErrorMessage { get; set; }

        public string ValidationDependencyErrorFirstProperty { get; set; }

        public string CustomErrorMessage { get; set; }

        [XmlIgnore]
        public virtual bool IsValid
        {
            get
            {
                return IsValidCustom || ( ValidationAdditionalErrorProperties.None() 
                                          && (IsEmpty || ModelBase.AllNotNull(this, RequiredButModelOptionalPropertyNameListToCheck)));
            }
        }

        public bool IsValidCustom { get; set; }

        [XmlIgnore]
        public virtual bool IsEmptyAsGroup
        {
            get
            {
                return ModelBase.AllNull(this, RequiredAsGroupPropertyNameListToCheck);
            }
        }

        [XmlIgnore]
        public virtual List<string> RequiredAsGroupPropertyNameListToCheck
        {
            get { return ModelBase.GetRequiredAsGroupPropertyNameListToCheck(this); }
        }

        [XmlIgnore]
        public virtual bool IsEmpty
        {
            get
            {
                return ModelBase.AllNull(this, RequiredButModelOptionalPropertyNameListToCheck);
            }
        }

        [XmlIgnore]
        public virtual List<string> RequiredButModelOptionalPropertyNameListToCheck
        {
            get { return ModelBase.GetRequiredButModelOptionalPropertyNameListToCheck(this); }
        }

        [XmlIgnore]
        public List<string> RequiredButModelOptionalPropertiesWithNullValues
        {
            get { return ModelBase.GetRequiredButModelOptionalPropertyNamesWithNullValues(this, RequiredButModelOptionalPropertyNameListToCheck); }
        }

        [XmlIgnore]
        public virtual GeneralEntity SummaryItem
        {
            get { return new GeneralEntity(); }
        }

        public bool IgnoreSummaryItem { get; set; }

        public List<UiModel> ParallelSummaryUiModels { get; set; }

        /// <summary>
        /// Deaktiviert den das "Abhaken" einer Form wenn sie erfolgreich validiert ist (türkisfarbener Haken im Form-Header rechts außen)
        /// </summary>
        public bool ValidationFormHeaderCheckStyleDisabled { get; set; }

        [XmlIgnore]
        public virtual XmlDictionary<string, string> ValidationAdditionalErrorProperties
        {
            get { return new XmlDictionary<string, string>();}
        }
    }
}