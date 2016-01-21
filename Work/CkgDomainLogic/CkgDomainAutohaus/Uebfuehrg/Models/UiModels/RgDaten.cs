using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class RgDaten : CommonUiModel, IValidatableObject
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

        [LocalizedDisplay(LocalizeConstants.InvoiceRecipientDeviant)]
        public string ReKundenNr { get; set; }

        [XmlIgnore]
        public Adresse ReKunde { get { return ReAdressen.FirstOrDefault(r => r.KundenNr == ReKundenNr) ?? new Adresse(); } }

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

        [LocalizedDisplay(LocalizeConstants.InvoicePayer)]
        public string RgKundenNr { get; set; }

        [XmlIgnore]
        public Adresse RgKunde { get { return RgAdressen.FirstOrDefault(r => r.KundenNr == RgKundenNr) ?? new Adresse(); } }

        #endregion


        #region AG

        private List<KundeAusHierarchie> _kundenAusHierarchie;

        [XmlIgnore]
        public List<KundeAusHierarchie> KundenAusHierarchie
        {
            get
            {
                if (GetKundenAusHierarchie == null)
                    return new List<KundeAusHierarchie>();

                if (_kundenAusHierarchie != null)
                    return _kundenAusHierarchie;

                _kundenAusHierarchie = GetKundenAusHierarchie().ToList();
                if (_kundenAusHierarchie.None())
                {
                    _kundenAusHierarchie = new List<KundeAusHierarchie> { new KundeAusHierarchie { KundenNr = KundenNrUser } };
                    AgKundenNr = KundenNrUser;
                }
                else if (_kundenAusHierarchie.Count() == 1)
                    AgKundenNr = _kundenAusHierarchie.First().KundenNr;

                return _kundenAusHierarchie;
            }
        }

        [LocalizedDisplay(LocalizeConstants.Principal)]
        public string AgKundenNr { get; set; }

        [XmlIgnore]
        public KundeAusHierarchie AgKunde { get { return KundenAusHierarchie.FirstOrDefault(k => k.KundenNr == AgKundenNr) ?? new KundeAusHierarchie(); } }

        #endregion

        public string KundenNrUser { get; set; }

        public string KundenNr { get; set; }

        [XmlIgnore]
        public Func<List<Adresse>> GetRechnungsAdressen { get; set; }

        [XmlIgnore]
        public Func<List<KundeAusHierarchie>> GetKundenAusHierarchie { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(AgKundenNr) && (String.IsNullOrEmpty(RgKundenNr) || String.IsNullOrEmpty(ReKundenNr)))
                yield return new ValidationResult(Localize.AllRequiredInputFieldsMustBeFilled);
        }

        public override string GetSummaryString()
        {
            var s = "";

            if (!String.IsNullOrEmpty(AgKundenNr) && !String.IsNullOrEmpty(AgKunde.Name1))
                s += String.Format("Auftraggeber:<br/>{0}<br/>", AgKunde.GetSummaryString());

            s += String.Format("Rechnungsempfänger:<br/>{0}<br/>" + "Rechnungs-Regulierer:<br/>{1}<br/>",
                (String.IsNullOrEmpty(ReKunde.Name1) && ReKundenNr == AgKundenNr && !String.IsNullOrEmpty(AgKunde.Name1) ? AgKunde.GetSummaryString() : ReKunde.GetSummaryString()),
                (String.IsNullOrEmpty(RgKunde.Name1) && RgKundenNr == AgKundenNr && !String.IsNullOrEmpty(AgKunde.Name1) ? AgKunde.GetSummaryString() : RgKunde.GetSummaryString()));

            return s;
        }

        public void MarkForRefreshRgReKundenNr()
        {
            RgKundenNr = ReKundenNr = null;
            _rgAdressen = _reAdressen = null;
        }
    }
}
