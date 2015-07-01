using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrer.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrer.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrer.ViewModels
{
    public class FahrerViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IFahrerDataService DataService { get { return CacheGet<IFahrerDataService>(); } }


        #region Verfügbarkeitsmeldung

        public IEnumerable ExcelDownloadFahrerMeldungenData
        {
            get { return FahrerBelegung.FahrerTagBelegungen; }
        }

        public string ExcelDownloadFahrerMeldungenJsonColumns
        {
            get
            {
                return
                    ("[" +
                    "   {\"title\":\"Datum\",\"member\":\"{0}\",\"type\":\"Date\",\"format\":\"{0:d}\"}" +
                    "  ,{\"title\":\"Verfügbarkeit\",\"member\":\"{1}\",\"type\":\"String\"}" +
                    "  ,{\"title\":\"Anzahl Fahrer\",\"member\":\"{2}\",\"type\":\"Int\"}" +
                    "]").FormatPropertyParams<FahrerTagBelegung>(m => m.Datum, m => m.Verfuegbarkeit, m => m.FahrerAnzahl);
            }
        }

        public FahrerBelegungViewModel FahrerBelegung
        {
            get { return PropertyCacheGet(() => new FahrerBelegungViewModel { FahrerTagBelegungen = new List<FahrerTagBelegung>() }); }
            set { PropertyCacheSet(value); }
        }


        public void LoadFahrerTagBelegungen()
        {
            DataService.MarkDataForRefresh();
            FahrerBelegung.FahrerTagBelegungen = DataService.FahrerTagBelegungen;
        }

        public void SaveFahrerTagBelegungen()
        {
            DataService.SaveFahrerTagBelegungen(FahrerBelegung.FahrerTagBelegungen);
        }

        #endregion


        #region Fahrer Aufträge
        
        [LocalizedDisplay(LocalizeConstants._blank)]
        public string FahrerAuftragsStatusFilter { get { return PropertyCacheGet(() => "NEW"); } set { PropertyCacheSet(value); } }
        
        [XmlIgnore]
        public string FahrerAuftragsStatusTypen
        {
            get
            {
                return string.Format("{0},{1};{2},{3};{4},{5}",
                                        "NEW", Localize.NewOrders, 
                                        "OK", Localize.AcceptedOrders, 
                                        "NO", Localize.RefusedOrders);
            }
        }

        public List<FahrerAuftrag> FahrerAuftraege
        {
            get { return PropertyCacheGet(() => new List<FahrerAuftrag>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrerAuftrag> FahrerAuftraegeFiltered
        {
            get { return PropertyCacheGet(() => FahrerAuftraege); }
            private set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.Order)]
        public string SelectedFahrerAuftragsKey { get; set; }

        public IFahrerAuftragsFahrt SelectedFahrerAuftrag { get { return FahrerAuftragsFahrten.FirstOrDefault(a => a.UniqueKey == SelectedFahrerAuftragsKey); } }


        public void LoadFahrerAuftraege(string status = null)
        {
            if (status != null)
                FahrerAuftragsStatusFilter = status;

            FahrerAuftraege = DataService.LoadFahrerAuftraege(FahrerAuftragsStatusFilter).ToListOrEmptyList();
            PropertyCacheClear(this, m => m.FahrerAuftraegeFiltered);
        }

        public string SetFahrerAuftragsStatus(string auftragsNr, string status)
        {
            var message = DataService.SetFahrerAuftragsStatus(auftragsNr, status);

            LoadFahrerAuftraege();

            return message;
        }

        public void FilterFahrerAuftraege(string filterValue, string filterProperties)
        {
            FahrerAuftraegeFiltered = FahrerAuftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public byte[] GetAuftragsPdfBytes(string auftragsNr)
        {
            return DataService.GetAuftragsPdfBytes(auftragsNr);
        }

        #endregion


        #region Foto Upload

        public bool ModeProtokoll { get; set; }

        public List<IFahrerAuftragsFahrt> FahrerAuftragsFahrten
        {
            get { return PropertyCacheGet(() => new List<IFahrerAuftragsFahrt>()); }
            set { PropertyCacheSet(value); }
        }

        public string FotoUploadPathVirtual { get { return ConfigurationManager.AppSettings["FahrerFotoUploadPathVirtual"]; } }
        public string FotoUploadPath { get { return ConfigurationManager.AppSettings["FahrerFotoUploadPath"]; } }

        public string FotoUploadPathThumbnails { get { return Path.Combine(FotoUploadPath, "_tb"); } }
        public string FotoUploadPathThumbnailsVirtual { get { return Path.Combine(FotoUploadPathVirtual, "_tb"); } }

        public string FotoUploadPathBackup { get { return ConfigurationManager.AppSettings["FahrerFotoUploadPathBackup"]; } }
                                                                                                                                    // ReSharper disable ConvertClosureToMethodGroup
        public List<string> UploadedImageFiles { get { return PropertyCacheGet(() => GetUploadedImageFiles()); } }
                                                                                                                                    // ReSharper restore ConvertClosureToMethodGroup

        public void LoadFahrerAuftragsFahrten()
        {
            if (ModeProtokoll)
            {
                FahrerAuftragsFahrten = DataService.LoadFahrerAuftragsProtokolle().ToList();
                FahrerAuftragsFahrten.Insert(0, new FahrerAuftragsProtokoll());
            }
            else
            {
                FahrerAuftragsFahrten = DataService.LoadFahrerAuftragsFahrten().ToList();
                FahrerAuftragsFahrten.Insert(0, new FahrerAuftragsFahrt());
            }
        }

        public void SetSelectedFahrerAuftragsKey(string auftragsNr)
        {
            SelectedFahrerAuftragsKey = auftragsNr;
            DataMarkForRefreshUploadedImageFiles();
        }

        static void TryDirectoryCreateAndRaiseError(string directoryName)
        {
            if (!FileService.TryDirectoryCreate(directoryName))
                throw new IOException(directoryName);
        }

        public void DataMarkForRefreshUploadedImageFiles()
        {
            PropertyCacheClear(this, m => m.UploadedImageFiles);
        }

        static string UrlFromServerFileName(string serverFileName, string virtualPath)
        {
            var url = VirtualPathUtility.ToAbsolute(Path.Combine(virtualPath, Path.GetFileName(serverFileName).NotNullOrEmpty()));

            return url;
        }

        public string GetOriginImageUrl(string fileName)
        {
            return UrlFromServerFileName(Path.Combine(FotoUploadPath, fileName), FotoUploadPathVirtual);
        }

        public string GetThumbnailImageUrl(string fileName)
        {
            return UrlFromServerFileName(Path.Combine(FotoUploadPath, fileName), FotoUploadPathThumbnailsVirtual);
        }

        private static int GetImageIndexFromFileName(string fileName)
        {
            if (fileName == null)
                return -1;

            var fileParts = fileName.Split('-');
            if (fileParts.Length < 2)
                return -1;

            return fileParts[1].ToInt();
        }

        public string GetUploadedImageFileName(string auftragsNr, string imageIndex, string fahrerNr, string fahrtNr)
        {
            var imageMask = imageIndex.ToInt() == -1 ? "*" : imageIndex.ToInt().ToString("0000");

            return string.Format("{0}-{1}-{2}-{3}", auftragsNr.TrimStart('0'), imageMask, fahrerNr.TrimStart('0'), fahrtNr);
        }

        public List<string> GetUploadedImageFiles()
        {
            TryDirectoryCreateAndRaiseError(FotoUploadPath);
            TryDirectoryCreateAndRaiseError(FotoUploadPathThumbnails);

            var auftrag = SelectedFahrerAuftrag;
            if (auftrag == null)
                return new List<string>();

            var existingImageFiles = Directory.GetFiles(FotoUploadPath, string.Format("{0}.{1}",
                                               GetUploadedImageFileName(auftrag.AuftragsNrFriendly, "*", DataService.FahrerID, auftrag.Fahrt), 
                                               "*"));

            return existingImageFiles.ToListOrEmptyList().OrderBy(GetImageIndexFromFileName).Select(Path.GetFileName).ToList();
        }

        public void SaveUploadedImageFile(string clientFileName, Action<string> saveAction)
        {
            var auftrag = SelectedFahrerAuftrag;
            var uploadImageIndex = (GetImageIndexFromFileName(UploadedImageFiles.LastOrDefault()) + 1).ToString();
            var serverFileName = string.Format("{0}{1}",
                                               GetUploadedImageFileName(auftrag.AuftragsNrFriendly, uploadImageIndex, DataService.FahrerID, auftrag.Fahrt), 
                                               Path.GetExtension(clientFileName));

            TryDirectoryCreateAndRaiseError(FotoUploadPath);
            var destinationFileName = Path.Combine(FotoUploadPath, serverFileName);
            FileService.TryFileDelete(destinationFileName);

            TryDirectoryCreateAndRaiseError(FotoUploadPathBackup);
            var backupFileName = Path.Combine(FotoUploadPathBackup, serverFileName);
            FileService.TryFileDelete(backupFileName);

            TryDirectoryCreateAndRaiseError(FotoUploadPathThumbnails);
            var thumbnailFileName = Path.Combine(FotoUploadPathThumbnails, serverFileName);
            FileService.TryFileDelete(thumbnailFileName);

            // save origin / backup file / create thumbnail
            saveAction(destinationFileName);
            FileService.TryFileCopy(destinationFileName, backupFileName);
            ImagingService.ScaleAndSaveImage(destinationFileName, thumbnailFileName, 200);
            
            DataMarkForRefreshUploadedImageFiles();
        }

        public bool DeleteUploadedImage(string imageFileName)
        {
            var serverFileName = UploadedImageFiles.FirstOrDefault(f => Path.GetFileName(f) == imageFileName);
            if (serverFileName == null)
                return false;

            var destinationFileName = Path.Combine(FotoUploadPath, serverFileName);
            FileService.TryFileDelete(destinationFileName);

            var backupFileName = Path.Combine(FotoUploadPathBackup, serverFileName);
            FileService.TryFileDelete(backupFileName);

            var thumbnailFileName = Path.Combine(FotoUploadPathThumbnails, serverFileName);
            FileService.TryFileDelete(thumbnailFileName);

            DataMarkForRefreshUploadedImageFiles();

            return true;
        }

        public void SetParamProtokollMode(string modeProtokoll)
        {
            ModeProtokoll = modeProtokoll.IsNotNullOrEmpty();
        }

        #endregion    


        #region Monitor / QM Auswertung

        public QmSelektor QmSelektor
        {
            get { return PropertyCacheGet(() => new QmSelektor { DatumsBereich = new DateRange(DateRangeType.LastYear) }); }
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.CountVotedRides)]
        public int QmFahrerRankingCount { get { return DataService.QmFahrerRankingCount; } }

        public List<QmFahrer> QmFahrerList { get { return DataService.QmFahrerList; } }

        public List<QmFleetMonitor> QmFleetMonitorList { get { return DataService.QmFleetMonitorList; } }


        public void Validate(Action<string, string> addModelError)
        {
        }

        public bool LoadQmData()
        {
            return DataService.LoadQmReportFleetData(QmSelektor.DatumsBereich);
        }

        #endregion
    }
}
