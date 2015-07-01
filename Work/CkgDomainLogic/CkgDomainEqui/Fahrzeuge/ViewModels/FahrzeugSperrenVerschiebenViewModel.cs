using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeugSperrenVerschiebenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrzeugSperrenVerschiebenDataService DataService { get { return CacheGet<IFahrzeugSperrenVerschiebenDataService>(); } }

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
                if (FahrzeugSelektor.Auswahl == "UPLOAD")
                    return FahrzeugeUpload;

                if (!EditMode)
                    return FahrzeugeGesamt;

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
        public List<Fahrzeuguebersicht> SelektierteFahrzeuge
        {
            get { return Fahrzeuge.FindAll(e => e.IsSelected); }
        }

        public bool EditMode { get; set; }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> GridItems
        {
            get
            {
                if (EditMode)
                    return FahrzeugeFiltered;

                return SelektierteFahrzeuge;
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

        public void Init()
        {
            PropertyCacheClear(this, m => m.FahrzeugSelektor);
            PropertyCacheClear(this, m => m.FahrzeugeUpload);
        }

        public void LoadFahrzeuge()
        {
            EditMode = true;

            FahrzeugeGesamt = DataService.GetFahrzeuge();

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

        public FahrzeugSperrenVerschieben GetUiModelSperrenVerschieben(bool sperren = true)
        {
            var item = Fahrzeuge.FirstOrDefault(f => f.IsSelected);

            return new FahrzeugSperrenVerschieben
                {
                    Sperren = sperren,
                    Sperrtext = (item != null ? item.BemerkungSperre : ""),
                    BemerkungIntern = (item != null ? item.BemerkungIntern : ""),
                    BemerkungExtern = (item != null ? item.BemerkungExtern : "")
                };
        }

        public bool SperrenMoeglich(bool sperren)
        {
            return Fahrzeuge.None(f => f.IsSelected && f.Gesperrt == sperren);
        }

        public void FahrzeugeSperren(ref FahrzeugSperrenVerschieben model, ModelStateDictionary state)
        {
            EditMode = false;

            var fzge = SelektierteFahrzeuge;

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
            EditMode = false;

            var fzge = SelektierteFahrzeuge;

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
            EditMode = false;

            var fzge = SelektierteFahrzeuge;

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

        #region CSV Upload

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> FahrzeugeUpload
        {
            get { return PropertyCacheGet(() => new List<Fahrzeuguebersicht>()); }
            private set { PropertyCacheSet(value); }
        }

        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable(CsvUploadServerFileName, true, "", CreateInstanceFromDatarow, ',').ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            FahrzeugeUpload = list;

            return true;
        }

        static Fahrzeuguebersicht CreateInstanceFromDatarow(DataRow row)
        {
            var item = new Fahrzeuguebersicht
            {
                Fahrgestellnummer = row[0].ToString()
            };
            return item;
        }

        public void SaveUploadItems()
        {
            foreach (var item in FahrzeugeUpload)
            {
                var fzgData = FahrzeugeGesamt.FirstOrDefault(f => f.Fahrgestellnummer == item.Fahrgestellnummer);
                
                if (fzgData == null)
                {
                    item.UploadedFound = false;
                }
                else
                {
                    ModelMapping.Copy(fzgData, item);
                    item.UploadedFound = true;
                    item.IsSelected = true;
                }
            }

            DataMarkForRefresh();
        }

        #endregion
    }
}
