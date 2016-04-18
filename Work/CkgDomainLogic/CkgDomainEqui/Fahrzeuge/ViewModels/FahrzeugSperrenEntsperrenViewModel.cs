using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeugSperrenEntsperrenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugSperrenVerschiebenDataService DataService { get { return CacheGet<IFahrzeugSperrenVerschiebenDataService>(); } }

        [XmlIgnore]
        public List<FahrzeugVersand> FahrzeugeGesamt
        {
            get { return PropertyCacheGet(() => new List<FahrzeugVersand>()); }
            private set { PropertyCacheSet(value); }
        }

        public List<FahrzeugVersand> Fahrzeuge
        {
            get
            {
                switch (FahrzeugSelektor.Auswahl)
                {
                    case "ALLE":
                        return FahrzeugeGesamt;
                    case "GESP":
                        return FahrzeugeGesamt.Where(f => f.Gesperrt).ToList();
                    case "NGESP":
                        return FahrzeugeGesamt.Where(f => !f.Gesperrt).ToList();
                    default:
                        return new List<FahrzeugVersand>();
                }
            }
        }

        [XmlIgnore]
        public List<FahrzeugVersand> SelektierteFahrzeuge
        {
            get { return Fahrzeuge.FindAll(e => e.IsSelected); }
        }

        [XmlIgnore]
        public List<FahrzeugVersand> GridItems
        {
            get { return FahrzeugeFiltered; }
        }

        [XmlIgnore]
        public List<FahrzeugVersand> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        public FahrzeugSperrenEntsperrenSelektor FahrzeugSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeugSperrenEntsperrenSelektor { Auswahl = "ALLE"}); }
            set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void ApplyFilter(string auswahl)
        {
            FahrzeugSelektor.Auswahl = auswahl;
        }

        public void Init()
        {
            PropertyCacheClear(this, m => m.FahrzeugSelektor);
        }

        public void LoadFahrzeuge()
        {
            FahrzeugeGesamt = DataService.GetFahrzeugVersendungen("DE", null);

            DataMarkForRefresh();
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void SelectFahrzeug(string vin, bool select)
        {
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer == vin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
        }

        public void SelectFahrzeuge(bool select)
        {
            FahrzeugeFiltered.ForEach(f => f.IsSelected = select);
        }

        public bool SperrenMoeglich(bool sperren)
        {
            return SelektierteFahrzeuge.None(f => f.Gesperrt == sperren);
        }

        public string FahrzeugeSperren(bool sperren, out string message, out bool success)
        {
            message = !SperrenMoeglich(sperren) 
                        ? Localize.ActionNotPossibleForFewOfSelectedItems 
                        : DataService.FahrzeugeVersendungenSperren(sperren, SelektierteFahrzeuge);

            var oldSelected = SelektierteFahrzeuge.Select(f => f.Fahrgestellnummer);

            LoadFahrzeuge();

            success = message.NotNullOrEmpty().ToLower().StartsWith("versandsperre geändert");
            var newFzgMitStatus = Fahrzeuge.Where(f => oldSelected.Contains(f.Fahrgestellnummer) && f.Gesperrt == sperren);

            if (success && FahrzeugSelektor.Auswahl.NotNullOrEmpty() == "ALLE" && newFzgMitStatus.Count() != oldSelected.Count())
            {
                message = "Achtung: Es wurde nicht genau die Anzahl an Fahrzeugen " + (sperren ? "gesperrt" : "entsperrt") + " die Sie ausgewählt hatten!";
                success = false;
            }

            return message;
        }
    }
}
