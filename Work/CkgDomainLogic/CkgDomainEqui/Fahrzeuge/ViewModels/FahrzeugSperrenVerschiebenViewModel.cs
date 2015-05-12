using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeugSperrenVerschiebenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugSperrenVerschiebenDataService DataService { get { return CacheGet<IFahrzeugSperrenVerschiebenDataService>(); } }

        [XmlIgnore]
        public List<Domaenenfestwert> Farben
        {
            get { return PropertyCacheGet(() => new List<Domaenenfestwert>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> FahrzeugeGesamt
        {
            get { return PropertyCacheGet(() => new List<Fahrzeuguebersicht>()); }
            private set { PropertyCacheSet(value); }
        }

        public List<Fahrzeuguebersicht> Fahrzeuge
        {
            get
            {
                var liste = (FahrzeugSelektor.NurMitBemerkung
                                 ? FahrzeugeGesamt.Where(f =>
                                     f.BemerkungSperre.IsNotNullOrEmpty() || f.BemerkungIntern.IsNotNullOrEmpty() ||
                                     f.BemerkungExtern.IsNotNullOrEmpty()).ToList()
                                 : FahrzeugeGesamt);

                switch (FahrzeugSelektor.Auswahl)
                {
                    case "ALLE":
                        return liste;
                    case "GESP":
                        return liste.Where(f => f.Gesperrt).ToList();
                    case "NGESP":
                        return liste.Where(f => !f.Gesperrt).ToList();
                    default:
                        return new List<Fahrzeuguebersicht>();
                }
            }
        }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        public FahrzeugSperrenVerschiebenSelektor FahrzeugSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeugSperrenVerschiebenSelektor { Auswahl = "ALLE"}); }
            set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void ApplyDatenfilter(string auswahl, bool nurMitBemerkung)
        {
            FahrzeugSelektor.Auswahl = auswahl;
            FahrzeugSelektor.NurMitBemerkung = nurMitBemerkung;
        }

        public void LoadFahrzeuge()
        {
            Farben = DataService.GetFarben();
            FahrzeugeGesamt = DataService.GetFahrzeuge();

            FahrzeugeGesamt.ForEach(f => f.Farbname = GetFarbName(f.Farbcode));

            DataMarkForRefresh();
        }

        private string GetFarbName(string farbCode)
        {
            var farbe = Farben.FirstOrDefault(f => f.Wert == farbCode);

            if (farbe != null)
                return farbe.Beschreibung;

            return "";
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
