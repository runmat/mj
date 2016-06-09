using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Remarketing.ViewModels
{
    public class FehlendeDatenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFehlendeDatenDataService DataService { get { return CacheGet<IFehlendeDatenDataService>(); } }

        public FehlendeDatenSelektor Selektor
        {
            get { return PropertyCacheGet(() => new FehlendeDatenSelektor { Auswahl = "A" }); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Vermieter> Vermieter
        {
            get { return PropertyCacheGet(() => DataService.GetVermieter()); }
        }

        public List<SimpleUploadItem> UploadItems { get; set; }

        [XmlIgnore]
        public List<FehlendeDaten> FehlendeDaten
        {
            get { return PropertyCacheGet(() => new List<FehlendeDaten>()); }
            protected set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FehlendeDaten> FehlendeDatenFiltered
        {
            get { return PropertyCacheGet(() => FehlendeDaten); }
            protected set { PropertyCacheSet(value); }
        }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        private void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FehlendeDatenFiltered);
        }

        public bool ExcelUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            var randomfilename = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(fileName);

            var tempPath = (AppSettings == null ? "" : AppSettings.TempPath);

            var nameSaved = (fileSaveAction == null ? fileName : fileSaveAction(tempPath, randomfilename, extension));

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var fileSaved = Path.Combine(tempPath, nameSaved + extension);

            var list = new ExcelDocumentFactory().ReadToDataTable(fileSaved, true, "", CreateInstanceFromDatarow, '*', true, true).ToList();

            if (AppSettings != null)
                FileService.TryFileDelete(fileSaved);

            if (list.None())
                return false;

            UploadItems = list;

            return true;
        }

        static SimpleUploadItem CreateInstanceFromDatarow(DataRow row)
        {
            return new SimpleUploadItem { Wert = row[0].ToString() };
        }

        public void LoadFehlendeDaten(FehlendeDatenSelektor selektor, Action<string, string> addModelError)
        {
            var auswahlChanged = (Selektor.Auswahl != selektor.Auswahl);

            if (auswahlChanged)
            {
                Selektor.Auswahl = selektor.Auswahl;
                selektor.AuswahlChanged = true;
                return;
            }

            Selektor = selektor;

            if (UploadItems == null)
                UploadItems = new List<SimpleUploadItem>();

            if (Selektor.Auswahl == "F" && !string.IsNullOrEmpty(Selektor.FahrgestellNr))
                UploadItems.Add(new SimpleUploadItem { Wert = Selektor.FahrgestellNr });
            else if (Selektor.Auswahl == "K" && !string.IsNullOrEmpty(Selektor.Kennzeichen))
                UploadItems.Add(new SimpleUploadItem { Wert = Selektor.Kennzeichen });

            if (Selektor.Auswahl == "F" && UploadItems.None())
            {
                addModelError("", Localize.PleaseUploadAFileOrEnterAChassisNo);
                return;
            }

            if (Selektor.Auswahl == "K" && UploadItems.None())
            {
                addModelError("", Localize.PleaseUploadAFileOrEnterALicenseNo);
                return;
            }

            FehlendeDaten = DataService.GetFehlendeDaten(Selektor, UploadItems);

            DataMarkForRefresh();

            if (FehlendeDaten.None())
                addModelError("", Localize.NoDataFound);
        }

        public void FilterFehlendeDaten(string filterValue, string filterProperties)
        {
            FehlendeDatenFiltered = FehlendeDaten.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
