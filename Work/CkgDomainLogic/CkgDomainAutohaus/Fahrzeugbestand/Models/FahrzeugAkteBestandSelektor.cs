using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class FahrzeugAkteBestandSelektor
    {
        private string _fin;
        private string _kennzeichen;

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN
        {
            get { return _fin.NotNullOrEmpty().ToUpper(); }
            set { _fin = value.NotNullOrEmpty().ToUpper(); }
        }


        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.Buyer)]
        public string Kaeufer { get; set; }


        [LocalizedDisplay(LocalizeConstants.ZBIInventoryInfo)]
        public string BriefbestandsInfo { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBIIStorageLocation)]
        public string BriefLagerort { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleLocation)]
        public string FahrzeugStandort { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? ErstZulassungsgDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDateCurrent)]
        public DateTime? ZulassungsgDatumAktuell { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public DateTime? AbmeldeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen.NotNullOrEmpty().ToUpper(); }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        public bool CocVorhanden { get; set; }

        
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrzeugbestandViewModel> GetViewModel { get; set; }

        public List<Adresse> HalterForSelection { get { return GetViewModel().HalterForSelection; } }

        public List<Adresse> KaeuferForSelection { get { return GetViewModel().KaeuferForSelection; } }

        public bool TmpEnforcePartnerDropdownRefresh { get; set; }
    }
}
