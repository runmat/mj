using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using Ionic.Zip;
using SapORM.Contracts;

namespace CkgDomainLogic.Uebfuehrg.ViewModels
{
    public class UebfuehrgReportViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IUebfuehrgDataService DataService { get { return CacheGet<IUebfuehrgDataService>(); } }


        public HistoryAuftragSelector HistoryAuftragSelector
        {
            get { return PropertyCacheGet(() => new HistoryAuftragSelector()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<HistoryAuftrag> HistoryAuftraege
        {
            get { return PropertyCacheGet(() => new List<HistoryAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<HistoryAuftrag> HistoryAuftraegeFiltered
        {
            get { return PropertyCacheGet(() => HistoryAuftraege); }
            private set { PropertyCacheSet(value); }
        }

        public void LoadHistoryAuftraege()
        {
            HistoryAuftraege = DataService.GetHistoryAuftraege(HistoryAuftragSelector)
                .OrderByDescending(a => a.AuftragsNr)
                .ThenBy(a => a.Fahrt).ToList();

            DataMarkForRefresh();
        }

        public void FilterHistoryAuftraege(string filterValue, string filterProperties)
        {
            HistoryAuftraegeFiltered = HistoryAuftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
        
        public HistoryAuftrag HistoryAuftragCurrent { get; set; }


        public bool ImageFilesAvailable { get { return ImageFileNames != null && ImageFileNames.Any(); } }
        public bool PdfFilesAvailable { get { return PdfFileNames != null && PdfFileNames.Any(); } }

        public List<string> ImageFileNames { get; set; }
        public List<string> ThumbImageFileNames { get; set; }
        public List<string> PdfFileNames { get; set; }

        public List<int> ImageFileTours { get { return GetToursForFileNames(ImageFileNames); } }
        public List<int> PdfFileTours { get { return GetToursForFileNames(PdfFileNames); } }

        string DestinationRelativePath { get; set; }


        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            HistoryAuftragSelector = new HistoryAuftragSelector
                {
                    AuftragsArt = "A",
                    KundenNr = LogonContext.KundenNr.ToSapKunnr()
                };
            
            PropertyCacheClear(this, m => m.HistoryAuftraegeFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
            var selector = HistoryAuftragSelector;

            if (selector.ErfassungsDatumRange.IsSelected &&
                (selector.ErfassungsDatumRange.EndDate.GetValueOrDefault() - selector.ErfassungsDatumRange.StartDate.GetValueOrDefault()).TotalDays > 95)
                addModelError("", string.Format(Localize.DateRangeMax3Months, Localize.DateOfReceipt));
            
            if (selector.AuftragsDatumRange.IsSelected &&
                (selector.AuftragsDatumRange.EndDate.GetValueOrDefault() - selector.AuftragsDatumRange.StartDate.GetValueOrDefault()).TotalDays > 95)
                addModelError("", string.Format(Localize.DateRangeMax3Months, Localize.OrderDate));
        }

        public List<string> GetImageFileNamesForTour(int tour)
        {
            return GetFileNamesForTour(ImageFileNames, tour);
        }

        public List<string> GetThumbImageFileNamesForTour(int tour)
        {
            return GetFileNamesForTour(ThumbImageFileNames, tour);
        }

        public List<string> GetPdfFileNamesForTour(int tour)
        {
            return GetFileNamesForTour(PdfFileNames, tour);
        }

        public string GetImageFileNameForIndex(int tour, int index)
        {
            return GetFileNameForIndex(ImageFileNames, tour, index);
        }

        List<string> GetFileNamesForTour(IEnumerable<string> fileNames, int tour)
        {
            return fileNames == null ? new List<string>() : fileNames.Where(f => tour == GetTourFromFilename(f)).ToList();
        }

        List<int> GetToursForFileNames(IEnumerable<string> fileNames)
        {
            return fileNames == null ? new List<int>() : fileNames.GroupBy(GetTourFromFilename).Select(f => f.Key).ToList(); 
        }

        string GetFileNameForIndex(IEnumerable<string> fileNames, int tour, int index)
        {
            return fileNames.FirstOrDefault(f => index == GetIndexFromFilename(FileService.PathGetFileName(f)) && tour == GetTourFromFilename(FileService.PathGetFileName(f)));
        }

        public int GetIndexFromFilename(string fileName)
        {
            fileName = FileService.PathGetFileName(fileName);

            var firstSeparator = fileName.IndexOf('-');
            if (firstSeparator == -1) return -1;
            var secondSeparator = fileName.IndexOf('-', firstSeparator + 1);
            if (secondSeparator == -1) return -1;

            var indexAsString = fileName.Substring(firstSeparator + 1, secondSeparator - firstSeparator - 1);
            int index;
            if (!Int32.TryParse(indexAsString, out index))
                return -1;

            return index;
        }

        public void PrepareHistoryAuftragsDokumente(string auftragsNr, string fahrt)
        {
            HistoryAuftragCurrent = HistoryAuftraege.FirstOrDefault(a => a.AuftragsNr.ToSapKunnr() == auftragsNr.ToSapKunnr() && a.Fahrt == fahrt);

            var auftrag = HistoryAuftragCurrent;
            if (auftrag == null)
                return;


            DestinationRelativePath = string.Format("{0}_{1}", "Temp_", Guid.NewGuid());
            FileService.TryDirectoryCreate(Path.Combine(AppSettings.WebViewAbsolutePath, DestinationRelativePath));

            // Get web folder urls for big images
            ImageFileNames = GetTempFolderPathForFiles("{0}*.jpg", fahrt).ToList();
            
            // Get web folder urls for pdf files
            PdfFileNames = GetTempFolderPathForFiles("{0}*.pdf", fahrt).ToList();

            // Get web folder urls for thumb images
            var mask = "THUMB_{0}*.jpg";
            ThumbImageFileNames = GetTempFolderPathForFiles(mask, fahrt).ToList();
            // Also copy thumb images at this point
            // Note: Big images and pdf files will be copied only if user clicks on the apropiate thumbnail
            CopyFilesToTempFolder(mask);
        }

        public void CopySingleBigImage(int tour, int singleFileNr)
        {
            CopyFilesToTempFolder("{0}*.jpg", singleFileNr, tour);
        }

        public void CopySinglePdf(int singleFileNr)
        {
            CopyFilesToTempFolder("{0}*.pdf", singleFileNr);
        }

        public void CopyFilesToTempFolder(string fileMask, int? singleFileNr = null, int? tour = null)
        {
            var sourcePath = GetSourcePath();

            if (sourcePath == null || DestinationRelativePath == null)
                return;

            var fileNames = FileService.TryDirectoryGetFiles(sourcePath, string.Format(fileMask, HistoryAuftragCurrent.AuftragsNrWebViewTrimmed));

            foreach (var fileName in fileNames)
                if (singleFileNr == null || (singleFileNr == GetIndexFromFilename(fileName) && tour == GetTourFromFilename(fileName)))
                    FileService.TryFileCopy(fileName, GetDestinationFileName(fileName, DestinationRelativePath));
        }

        string GetSourcePath()
        {
            if (HistoryAuftragCurrent == null)
                return null;

            var auftragGeber = LogonContext.KundenNr.ToSapKunnr(); 
            var path = Path.Combine(AppSettings.UploadFilePath, auftragGeber, HistoryAuftragCurrent.AuftragsNrWebView);
            return path;
        }

        string GetDestinationFileName(string fileName, string destinationRelativePath)
        {
            return Path.Combine(AppSettings.WebViewAbsolutePath, destinationRelativePath, FileService.PathGetFileName(fileName));
        }

        IEnumerable<string> GetTempFolderPathForFiles(string fileMask, string fahrt)
        {
            var sourcePath = GetSourcePath();
            var fileNames = FileService.TryDirectoryGetFiles(sourcePath, string.Format(fileMask, HistoryAuftragCurrent.AuftragsNrWebViewTrimmed));

            return fileNames
                .Where(f => fahrt == GetTourFromFilename(f).ToString())
                .Select(f => Path.Combine(AppSettings.WebViewRelativePath, DestinationRelativePath, FileService.PathGetFileName(f))
                .Replace(@"\", "/")).OrderBy(f => f);
        }

        public int GetTourFromFilename(string filename)
        {
            if (filename.LastIndexOf('-') == -1)
                return 0;

            filename = filename.Substring(filename.LastIndexOf('-'));
            
            int tour;
            if (!Int32.TryParse(filename.Substring(1, 1), out tour))
                return -1;

            return tour;
        }

        public string GetPdfFilesAsZip()
        {
            var sourcePath = GetSourcePath();

            if (sourcePath == null || DestinationRelativePath == null)
                return "";

            var zipFileNameWithoutExtensions = string.Format("Ueberfuehrungsprotokolle_{0}_Fahrt_{1}", HistoryAuftragCurrent.AuftragsNrWebViewTrimmed, HistoryAuftragCurrent.Fahrt);
            var auftragGeber = LogonContext.KundenNr.ToSapKunnr();

            var zip = new ZipFile();

            var fileNamesForFahrt = FileService.TryDirectoryGetFiles(sourcePath, string.Format("{0}*{1}P.pdf", HistoryAuftragCurrent.AuftragsNrWebViewTrimmed, HistoryAuftragCurrent.Fahrt));
            foreach (var fileName in fileNamesForFahrt)
                zip.AddFile(fileName, zipFileNameWithoutExtensions);

            var fileNamesGeneral = FileService.TryDirectoryGetFiles(sourcePath, string.Format("{0}_{1}*.pdf", auftragGeber, HistoryAuftragCurrent.AuftragsNrWebView));
            foreach (var fileName in fileNamesGeneral)
                zip.AddFile(fileName, zipFileNameWithoutExtensions);

            var zipFileName = GetDestinationFileName(zipFileNameWithoutExtensions + ".zip", DestinationRelativePath);
            if (!FileService.TryFileDelete(zipFileName))
                return "";

            zip.Save(zipFileName);

            return zipFileName;
        }
    }
}