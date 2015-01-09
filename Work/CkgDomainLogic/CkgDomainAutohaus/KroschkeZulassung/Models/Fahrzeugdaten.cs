using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Fahrzeugdaten
    {
        private string _kostenstelle;
        private string _bestellNr;
        private string _auftragsNr;

        [Required]
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr
        {
            get { return _auftragsNr.NotNullOrEmpty().ToUpper(); }
            set { _auftragsNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string Zb2Nr { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugartId { get; set; }

        public Domaenenfestwert Fahrzeugart
        {
            get
            {
                if (FahrzeugartList == null)
                    return new Domaenenfestwert();

                var option = FahrzeugartList.FirstOrDefault(fa => fa.Wert == FahrzeugartId);
                if (option == null)
                    return new Domaenenfestwert();

                return option;
            }
        }

        [XmlIgnore]
        static public List<Domaenenfestwert> FahrzeugartList { get; set; }

        public bool IstAnhaenger { get { return (FahrzeugartId.NotNullOrEmpty().Trim() == "3"); } }

        public bool IstMotorrad { get { return (FahrzeugartId.NotNullOrEmpty().Trim() == "5"); } }

        [LocalizedDisplay(LocalizeConstants.SellerAbbreviation)]
        public string VerkaeuferKuerzel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.CostCenter)]
        public string Kostenstelle
        {
            get { return _kostenstelle.NotNullOrEmpty().ToUpper(); }
            set { _kostenstelle = value.NotNullOrEmpty().ToUpper(); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.OrderCode)]
        public string BestellNr
        {
            get { return _bestellNr.NotNullOrEmpty().ToUpper(); }
            set { _bestellNr = value.NotNullOrEmpty().ToUpper(); }
        }

        public string GetSummaryString()
        {
            var s = String.Format("{0}: {1}", Localize.OrderNumber, AuftragsNr);
            s += String.Format("<br/>{0}: {1}", Localize.VehicleType, Fahrzeugart.Beschreibung);
            s += String.Format("<br/>{0}: {1}", Localize.VIN, FahrgestellNr);
            s += String.Format("<br/>{0}: {1}", Localize.SellerAbbreviation, VerkaeuferKuerzel);
            s += String.Format("<br/>{0}: {1}", Localize.ZB2, Zb2Nr);
            s += String.Format("<br/>{0}: {1}", Localize.CostCenter, Kostenstelle);
            s += String.Format("<br/>{0}: {1}", Localize.OrderCode, BestellNr);

            return s;
        }
    }
}
