using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
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
                if (!EditMode)
                    return FahrzeugeGesamt;

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

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<FahrzeugVersand> GridItems
        {
            get
            {
                if (EditMode)
                    return FahrzeugeFiltered;

                return SelektierteFahrzeuge;
            }
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
            EditMode = true;

            FahrzeugeGesamt = DataService.GetFahrzeugVersendungen("DE", null);

            DataMarkForRefresh();
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void SelectFahrzeug(string vin, bool select)
        {
            if (EditMode)
            {
                var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer == vin);
                if (fzg == null)
                    return;

                fzg.IsSelected = select;
            }
        }

        public void SelectFahrzeuge(bool select)
        {
            if (EditMode)
                FahrzeugeFiltered.ForEach(f => f.IsSelected = select);
        }

        //public FahrzeugSperrenEntsperren GetUiModelSperrenEntsperren(bool sperren = true)
        //{
        //    //var item = Fahrzeuge.FirstOrDefault(f => f.IsSelected);

        //    return new FahrzeugSperrenEntsperren
        //        {
        //            Sperren = sperren
        //        };
        //}

        public bool SperrenMoeglich(bool sperren)
        {
            return Fahrzeuge.None(f => f.IsSelected && f.Gesperrt == sperren);
        }

        //public void FahrzeugeSperren(ref FahrzeugSperrenEntsperren model, ModelStateDictionary state)
        //{
        //    EditMode = false;

        //    var fzge = SelektierteFahrzeuge;

        //    var anzOk = DataService.FahrzeugeSperren(model.Sperren, model.Sperrtext, ref fzge);

        //    var neuGesperrt = model.Sperren;
        //    fzge.ToList().ForEach(f =>
        //    {
        //        f.Gesperrt = neuGesperrt;
        //    });

        //    model.Message = string.Format((model.Sperren ? Localize.NVehiclesLockedSuccessfully : Localize.NVehiclesUnlockedSuccessfully), anzOk);
        //}
    }
}
