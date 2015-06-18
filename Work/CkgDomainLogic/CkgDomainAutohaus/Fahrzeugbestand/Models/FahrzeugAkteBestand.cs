using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.Linq;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class FahrzeugAkteBestand
    {
        private string _fin;
        private string _kennzeichen;
        private string _typSchluessel;

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        public string FIN
        {
            get { return _fin.NotNullOrEmpty().ToUpper(); }
            set { _fin = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.VinID)]
        public string FinID { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrzeugbestandViewModel> GetViewModel { get; set; }

        public bool IsSelected { get; set; }    // ##MMA##

        #region Fahrzeug Akte

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        [Required]
        [Length(5)]
        public string HerstellerSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        [Required]
        [Length(3)]
        public string TypSchluessel
        {
            get { return _typSchluessel.NotNullOrEmpty().ToUpper(); }
            set { _typSchluessel = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.VvsKey)]
        [Required]
        [Length(5)]
        public string VvsSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.VvsCheckDigit)]
        [Required]
        [Length(1)]
        public string VvsPruefZiffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        [MaxLength(25)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.TradeName)]
        [Length(25)]
        public string HandelsName { get; set; }


        [GridHidden, NotMapped]
        public bool AkteIsValid
        {
            get
            {
                return HerstellerSchluessel.IsNotNullOrEmpty() && 
                       TypSchluessel.IsNotNullOrEmpty() &&
                       VvsSchluessel.IsNotNullOrEmpty() && 
                       VvsPruefZiffer.IsNotNullOrEmpty();
            }
        }

        [GridHidden, NotMapped]
        public bool AkteJustCreated { get; set; }

        #endregion



        #region Fahrzeug Bestand

        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.Buyer)]
        public string Kaeufer { get; set; }


        [LocalizedDisplay(LocalizeConstants.ZBIInventoryInfo)]
        [Length(1)]
        [GridHidden]
        public string BriefbestandsInfo { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBIInventoryInfo)]
        public string BriefbestandsInfoAsText
        {
            get
            {
                var entry = BriefbestandsInfoOptionen.FirstOrDefault(i => i.Key == BriefbestandsInfo);
                return entry == null ? "" : entry.Text;
            }
        }

        public static List<SelectItem> BriefbestandsInfoOptionen
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("0", Localize.InStock),
                    new SelectItem("1", Localize.TempDispatched),
                    new SelectItem("2", Localize.FinalDispatched),
                };
            }
        }

        [LocalizedDisplay(LocalizeConstants.ZBIIStorageLocation)]
        [Length(30)]
        public string BriefLagerort { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleLocation)]
        [Length(30)]
        public string FahrzeugStandort { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? ErstZulassungsgDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDateCurrent)]
        public DateTime? ZulassungsgDatumAktuell { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationDate)]
        public DateTime? AbmeldeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Length(15)]
        public string Kennzeichen
        {
            get { return _kennzeichen.NotNullOrEmpty().ToUpper(); }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        [Length(25)]
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        public bool CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        [Length(35)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleSpecies)]
        public string FahrzeugArt { get; set; }

        public static List<SelectItem> FahrzeugArtOptionen
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("PKW", "PKW"),
                    new SelectItem("LKW", "LKW"),
                    new SelectItem("Andere", Localize.Others),
                };
            }
        }

        [LocalizedDisplay(LocalizeConstants.SalesDivision)]
        public string VerkaufsSparte { get; set; }

        public static List<SelectItem> VerkaufsSpartenOptionen
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("Neuwagen", Localize.NewCars),
                    new SelectItem("Gebr.wagen", Localize.UsedCars),
                    new SelectItem("Andere", Localize.Others),
                };
            }
        }

        [LocalizedDisplay(LocalizeConstants.VehicleNo)]
        [Length(15)]
        public string FahrzeugNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [Length(15)]
        public string AuftragsNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CompanyRef1)]
        [Length(10)]
        public string FirmenReferenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CompanyRef2)]
        [Length(10)]
        public string FirmenReferenz2 { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public Adresse SelectedHalter
        {
            get
            {
                return (GetViewModel != null ? GetViewModel().GetPartnerAdresse("HALTER", Halter) : null);
            }
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public Adresse SelectedKaeufer
        {
            get
            {
                return (GetViewModel != null ? GetViewModel().GetPartnerAdresse("KAEUFER", Kaeufer) : null);
            }
        }

        #endregion

    }
}
