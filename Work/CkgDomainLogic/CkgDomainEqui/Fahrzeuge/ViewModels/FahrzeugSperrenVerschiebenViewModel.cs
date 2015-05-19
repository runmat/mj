using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
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
        public List<Domaenenfestwert> Farben { get { return PropertyCacheGet(() => DataService.GetFarben()); } }

        [XmlIgnore]
        public List<FahrzeuguebersichtPDI> Pdis { get { return PropertyCacheGet(() => DataService.GetPDIStandorte()); } }

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

        public void SelectFahrzeug(string vin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer == vin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
        }

        public void SelectFahrzeuge(bool select, out int allSelectionCount)
        {
            FahrzeugeFiltered.ForEach(f => f.IsSelected = select);

            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
        }

        public FahrzeugSperrenVerschieben GetUiModelSperrenVerschieben(bool sperren = true)
        {
            var item = Fahrzeuge.FirstOrDefault(f => f.IsSelected);

            return new FahrzeugSperrenVerschieben { Sperren = sperren, Sperrtext = (item != null ? item.BemerkungSperre : "") };
        }

        public bool SperrenMoeglich(bool sperren)
        {
            return Fahrzeuge.None(f => f.IsSelected && f.Gesperrt == sperren);
        }

        public void FahrzeugeSperren(ref FahrzeugSperrenVerschieben model, ModelStateDictionary state)
        {
            var fzge = Fahrzeuge.Where(f => f.IsSelected).ToList();

            var anzOk = DataService.FahrzeugeSperren(model.Sperren, model.Sperrtext, ref fzge);

            var neuGesperrt = model.Sperren;
            var neuText = model.Sperrtext;
            fzge.Where(f => f.Bearbeitungsstatus == Localize.OK).ToList().ForEach(f =>
            {
                f.Gesperrt = neuGesperrt;
                f.BemerkungSperre = neuText;
            });

            model.Message = String.Format((model.Sperren ? Localize.NVehiclesLockedSuccessfully : Localize.NVehiclesUnlockedSuccessfully), anzOk);
        }

        public void FahrzeugeVerschieben(ref FahrzeugSperrenVerschieben model)
        {
            var fzge = Fahrzeuge.Where(f => f.IsSelected).ToList();

            var anzOk = DataService.FahrzeugeVerschieben(model.ZielPdi, ref fzge);

            var neuPdi = model.ZielPdi;
            fzge.Where(f => f.Bearbeitungsstatus == Localize.OK).ToList().ForEach(f =>
            {
                f.Carport = neuPdi;
                var pdi = Pdis.FirstOrDefault(p => p.PDIKey == f.Carport);
                f.DadPdi = (pdi != null ? pdi.DadPdi : "");
                f.Carportname = (pdi != null ? pdi.PDIText : "");
            });

            model.Message = String.Format(Localize.NVehiclesRelocatedSuccessfully, anzOk);
        }

        public void FahrzeugeTexteErfassen(ref FahrzeugSperrenVerschieben model)
        {
            var fzge = Fahrzeuge.Where(f => f.IsSelected).ToList();

            var anzOk = DataService.FahrzeugeTexteErfassen(model.BemerkungIntern, model.BemerkungExtern, ref fzge);

            var neuBemIntern = model.BemerkungIntern;
            var neuBemExtern = model.BemerkungExtern;
            fzge.Where(f => f.Bearbeitungsstatus == Localize.OK).ToList().ForEach(f =>
            {
                f.BemerkungIntern = neuBemIntern;
                f.BemerkungExtern = neuBemExtern;
            });

            model.Message = String.Format(Localize.TextsForNVehiclesChangedSuccessfully, anzOk);
        }
    }
}
