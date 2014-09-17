using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using Ionic.Zip;
using SapORM.Contracts;

namespace CkgDomainLogic.Ueberfuehrung.ViewModels
{
    public class UeberfuehrungHistoryViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IUeberfuehrungDataService DataService { get { return CacheGet<IUeberfuehrungDataService>(); } }

        public HistoryAuftragFilter HistoryAuftragFilter { get; set; }

        public List<HistoryAuftrag> HistoryAuftraege { get; set; }

        public HistoryAuftrag HistoryAuftragCurrent { get; set; }

        public bool ImageFilesAvailable { get { return ImageFileNames != null && ImageFileNames.Any(); } }
        public bool PdfFilesAvailable { get { return PdfFileNames != null && PdfFileNames.Any(); } }

        public List<string> ImageFileNames { get; set; }
        public List<string> ThumbImageFileNames { get; set; }
        public List<string> PdfFileNames { get; set; }

        public List<int> ImageFileTours { get { return GetToursForFileNames(ImageFileNames); } }
        public List<int> PdfFileTours { get { return GetToursForFileNames(PdfFileNames); } }

        string DestinationRelativePath { get; set; }


        public void DataMarkForRefresh()
        {
            HistoryAuftragFilter = new HistoryAuftragFilter
                {
                    UeberfuehrungsDatumVon = DateTime.Now.AddDays(-30),
                    UeberfuehrungsDatumBis = DateTime.Now.AddDays(0),
                    AuftragsArt = "A",
                    AuftragGeberAdressen = DataService.GetRechnungsAdressen().Where(a => a.SubTyp == "RG").ToListOrEmptyList(),
                };
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

        public void ValidateHistoryAuftragFilter(ref HistoryAuftragFilter filter, Action<Expression<Func<HistoryAuftragFilter, object>>> addModelError)
        {
            filter.AuftragGeberAdressen = DataService.GetRechnungsAdressen().Where(a => a.SubTyp == "RG").ToListOrEmptyList();
            filter.Validate(addModelError);
        }

        public List<HistoryAuftrag> GetHistoryAuftraege(HistoryAuftragFilter filter)
        {
            return HistoryAuftraege = DataService.GetHistoryAuftraege(filter);
        }

        public List<HistoryAuftrag> GetHistoryAuftraege()
        {
            return GetHistoryAuftraege(HistoryAuftragFilter);
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


            //var t1 = GetHistoryTourFromFilename(sourcePdfFiles[0]);
            //var t2 = GetHistoryTourFromFilename(ImageFileNames[0]);
            //var t3 = GetHistoryTourFromFilename(sourceJpgThumbFiles[0]);

            //var xxx = PdfFileTours;
            //var yyy = ImageFileTours;

            //var ggg = GetPdfFileNamesForTour(1);
            //var gggg = GetPdfFileNamesForTour(2);
            //var ggggg = GetImageFileNamesForTour(1);
            //var gggggg = GetImageFileNamesForTour(2);

            //var zzz = Path.Combine(AppSettings.WebViewRelativePath, destinationRelativePath, ImageFileNames[0]).Replace(@"\", "/");
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

            var auftragGeber = HistoryAuftragFilter.SelectedAuftragGeberAdresse.KundenNr.ToSapKunnr(); //LogonContext.KundenNr.ToSapKunnr()
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
            var auftragGeber = HistoryAuftragFilter.SelectedAuftragGeberAdresse.KundenNr.ToSapKunnr(); 

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