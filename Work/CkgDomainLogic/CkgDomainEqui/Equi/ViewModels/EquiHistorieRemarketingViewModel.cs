using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EquiHistorieRemarketingViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IEquiHistorieDataService DataService { get { return CacheGet<IEquiHistorieDataService>(); } }

        [XmlIgnore]
        public IEasyAccessDataService EasyAccessDataService { get { return CacheGet<IEasyAccessDataService>(); } }

        public EquiHistorieSuchparameter Suchparameter
        {
            get { return PropertyCacheGet(() => new EquiHistorieSuchparameter()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<EquiHistorieRemarketingInfo> HistorieInfos
        {
            get { return PropertyCacheGet(() => new List<EquiHistorieRemarketingInfo>()); }
            private set { PropertyCacheSet(value); }
        }

        public EquiHistorieRemarketing EquipmentHistorie { get; set; }

        public int LoadHistorieInfos(ModelStateDictionary state)
        {
            HistorieInfos = DataService.GetHistorieRemarketingInfos(Suchparameter);

            PropertyCacheClear(this, m => m.HistorieInfosFiltered);

            Suchparameter.AnzahlTreffer = HistorieInfos.Count;

            if (HistorieInfos.None())
                state.AddModelError("", Localize.NoDataFound);

            return Suchparameter.AnzahlTreffer;
        }

        public void LoadHistorie(string fin)
        {
            if (!string.IsNullOrEmpty(fin))
                EquipmentHistorie = DataService.GetHistorieRemarketingDetail(fin);
            else if (HistorieInfos.Count == 1)
                EquipmentHistorie = DataService.GetHistorieRemarketingDetail(HistorieInfos[0].FahrgestellNr);
        }

        public byte[] GetBelastungsanzeigePdf()
        {
            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenLagerort", "0", LogonContext.CustomerID);
            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenName", "0", LogonContext.CustomerID);

            if (string.IsNullOrEmpty(lagerort) || string.IsNullOrEmpty(archiv))
                return null;

            var mitJahr = ApplicationConfiguration.GetApplicationConfigValue("ArchivBelastungsanzeigenMitJahr", "0", LogonContext.CustomerID);
            if (string.Compare(mitJahr, "true", true) == 0 && EquipmentHistorie.Belastungsanzeige.Erstellungsdatum.HasValue)
                archiv += EquipmentHistorie.Belastungsanzeige.Erstellungsdatum.ToString("yy");

            var relDocPaths = EasyAccessDataService.GetDocuments(lagerort, archiv, "SGW", string.Format(".1001={0}", EquipmentHistorie.HistorieInfo.FahrgestellNr));

            var fileList = new List<byte[]>();

            relDocPaths.ForEach(d => fileList.Add(File.ReadAllBytes(HttpContext.Current.Server.MapPath(d.Replace("\\", "/")))));

            return (fileList.None() ? null : PdfDocumentFactory.MergePdfDocuments(fileList));
        }

        public byte[] GetSchadensgutachtenPdf()
        {
            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivSchadensgutachtenLagerort", "0", LogonContext.CustomerID);
            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivSchadensgutachtenName", "0", LogonContext.CustomerID);

            if (string.IsNullOrEmpty(lagerort) || string.IsNullOrEmpty(archiv))
                return null;

            var mitJahr = ApplicationConfiguration.GetApplicationConfigValue("ArchivSchadensgutachtenMitJahr", "0", LogonContext.CustomerID);
            if (string.Compare(mitJahr, "true", true) == 0 && EquipmentHistorie.UploaddatumSchadensgutachten.Length == 10)
                archiv += EquipmentHistorie.UploaddatumSchadensgutachten.Substring(8, 2);

            var relDocPaths = EasyAccessDataService.GetDocuments(lagerort, archiv, "SGW", string.Format(".1001={0}", EquipmentHistorie.HistorieInfo.FahrgestellNr));

            var fileList = new List<byte[]>();

            relDocPaths.ForEach(d => fileList.Add(File.ReadAllBytes(HttpContext.Current.Server.MapPath(d.Replace("\\", "/")))));

            return (fileList.None() ? null : PdfDocumentFactory.MergePdfDocuments(fileList));
        }

        public byte[] GetReparaturKalkulationPdf()
        {
            var repKalkUrl = ApplicationConfiguration.GetApplicationConfigValue("RepKalkUrl", "0", LogonContext.CustomerID, LogonContext.Group.GroupID);
            var fileList = new List<byte[]>();

            using (var clnt = new WebClient())
            {
                for (var i = 0; i < EquipmentHistorie.AnzahlReparaturKalkulationen; i++)
                {
                    var downloadUrl = string.Format("{0}?fin={1}&nummer={2}", repKalkUrl, EquipmentHistorie.HistorieInfo.FahrgestellNr, i + 1);
                    var downloadFile = clnt.DownloadData(downloadUrl);
                    if (downloadFile != null)
                        fileList.Add(downloadFile);
                }
            }

            return (fileList.None() ? null : PdfDocumentFactory.MergePdfDocuments(fileList));
        }

        public byte[] GetRechnungPdf()
        {
            var lagerort = ApplicationConfiguration.GetApplicationConfigValue("ArchivRechnungenLagerort", "0", LogonContext.CustomerID);
            var archiv = ApplicationConfiguration.GetApplicationConfigValue("ArchivRechnungenName", "0", LogonContext.CustomerID);

            if (string.IsNullOrEmpty(lagerort) || string.IsNullOrEmpty(archiv))
                return null;

            var relDocPaths = EasyAccessDataService.GetDocuments(lagerort, archiv, "SGW", string.Format(".1001={0}&.1003={1}", "S", EquipmentHistorie.Schadenrechnung.RechnungsNr));

            var fileList = new List<byte[]>();

            relDocPaths.ForEach(d => fileList.Add(File.ReadAllBytes(HttpContext.Current.Server.MapPath(d.Replace("\\", "/")))));

            return (fileList.None() ? null : PdfDocumentFactory.MergePdfDocuments(fileList));
        }

        #region Filter

        [XmlIgnore]
        public List<EquiHistorieRemarketingInfo> HistorieInfosFiltered
        {
            get { return PropertyCacheGet(() => HistorieInfos); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHistorieInfos(string filterValue, string filterProperties)
        {
            HistorieInfosFiltered = HistorieInfos.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
