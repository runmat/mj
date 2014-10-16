using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class RgDaten : CommonUiModel
    {
        #region RE

        private List<Adresse> _reAdressen;
        [XmlIgnore]
        public List<Adresse> ReAdressen
        {
            get
            {
                if (GetRechnungsAdressen == null)
                    return new List<Adresse>();

                if (_reAdressen != null)
                    return _reAdressen;

                _reAdressen = GetRechnungsAdressen().Where(a => a.SubTyp == "RE").ToList();
                if (_reAdressen.None())
                {
                    _reAdressen = new List<Adresse> { new Adresse {KundenNr = KundenNr, AdressTyp = AdressenTyp.RechnungsAdresse, SubTyp = "RE"} };
                    ReKundenNr = KundenNr;
                }
                else if (_reAdressen.Count() == 1)
                    ReKundenNr = _reAdressen.First().KundenNr;

                return _reAdressen;
            }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.InvoiceRecipientDeviant)]
        public string ReKundenNr { get; set; }

        [XmlIgnore]
        public Adresse ReKunde { get { return RgAdressen.FirstOrDefault(r => r.KundenNr == ReKundenNr) ?? new Adresse(); } }

        #endregion


        #region RG

        private List<Adresse> _rgAdressen;

        [XmlIgnore]
        public List<Adresse> RgAdressen
        {
            get
            {
                if (GetRechnungsAdressen == null)
                    return new List<Adresse>();

                if (_rgAdressen != null)
                    return _rgAdressen;

                _rgAdressen = GetRechnungsAdressen().Where(a => a.SubTyp == "RG").ToList();
                if (_rgAdressen.None())
                {
                    _rgAdressen = new List<Adresse> { new Adresse {KundenNr = KundenNr, AdressTyp = AdressenTyp.RechnungsAdresse, SubTyp = "RG"} };
                    RgKundenNr = KundenNr;
                }
                else if (_rgAdressen.Count() == 1)
                    RgKundenNr = _rgAdressen.First().KundenNr;

                return _rgAdressen;
            }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.InvoicePayer)]
        public string RgKundenNr { get; set; }

        [XmlIgnore]
        public Adresse RgKunde { get { return RgAdressen.FirstOrDefault(r => r.KundenNr == RgKundenNr) ?? new Adresse(); } }

        #endregion


        public string KundenNr { get; set; }

        [XmlIgnore]
        public Func<List<Adresse>> GetRechnungsAdressen { get; set; }

        public override string GetSummaryString()
        {
            return string.Format(   "Rechnungsempfänger:<br/>{0}<br/>" +
                                    "Rechnungs-Regulierer:<br/>{1}<br/>", 
                                    ReKunde.GetSummaryString(),
                                    RgKunde.GetSummaryString());
        }
    }
}
