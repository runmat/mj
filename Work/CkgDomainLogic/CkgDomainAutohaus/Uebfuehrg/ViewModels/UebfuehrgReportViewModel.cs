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

        [XmlIgnore]
        private bool IstKroschke { get { return (LogonContext.Customer.AccountingArea == 1010); } }

        public void LoadHistoryAuftraege()
        {
            HistoryAuftragSelector.KundenNr = LogonContext.KundenNr.ToSapKunnr();
            HistoryAuftragSelector.KundenNrUser = LogonContext.KundenNr.ToSapKunnr();

            if (IstKroschke && !String.IsNullOrEmpty(HistoryAuftragSelector.AgKundenNr))
                HistoryAuftragSelector.KundenNr = HistoryAuftragSelector.AgKundenNr;

            HistoryAuftraege = DataService.GetHistoryAuftraege(HistoryAuftragSelector)
                .OrderByDescending(a => a.AuftragsNr)
                .ThenBy(a => a.Fahrt)
                .GroupBy(a => a.AuftragsNr + "-" + a.Fahrt)
                .Select(grp => grp.First())
                .ToList();

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
            HistoryAuftragSelector = new HistoryAuftragSelector
            {
                AuftragsArt = "A",
                KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                KundenNrUser = LogonContext.KundenNr.ToSapKunnr(),
                GetKundenAusHierarchie = () => DataService.KundenAusHierarchie
            };

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.HistoryAuftraegeFiltered);
        }

        public void Validate(Action<string, string> addModelError, out bool dateRangeResetOccurred)
        {
            var selector = HistoryAuftragSelector;

            dateRangeResetOccurred = false;
            if (selector.AuftragsNr.IsNotNullOrEmpty() || selector.Kennzeichen.IsNotNullOrEmpty() || selector.Referenz.IsNotNullOrEmpty())
            {
                dateRangeResetOccurred = true;
                HistoryAuftragSelector.UeberfuehrungsDatumRange.IsSelected = false;
                HistoryAuftragSelector.AuftragsDatumRange.IsSelected = false;
            }

            if (selector.UeberfuehrungsDatumRange.MoreDaysThan(92))
                addModelError("", string.Format(Localize.DateRangeMax3Months, Localize.DateOfReceipt));
            
            if (selector.AuftragsDatumRange.MoreDaysThan(92))
                addModelError("", string.Format(Localize.DateRangeMax3Months, Localize.OrderDate));

            if (!ModelBase.AtLeastOneRequiredAsGroupPropertyIsValid(selector))
                addModelError("", Localize.PleaseChooseAtLeastOneOption);
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

        public void LoadHistoryAuftragDetails(string auftragsNr, string fahrt, Func<string, string> mapToUrlFunction)
        {
            HistoryAuftragCurrent = HistoryAuftraege.FirstOrDefault(a => a.AuftragsNr.ToSapKunnr() == auftragsNr.ToSapKunnr() && a.Fahrt == fahrt);

            var auftrag = HistoryAuftragCurrent;
            if (auftrag == null)
                return;

            DestinationRelativePath = string.Format("{0}_{1}", "Temp_", Guid.NewGuid());
            FileService.TryDirectoryCreate(Path.Combine(AppSettings.WebViewAbsolutePath, DestinationRelativePath));

            // Get web folder urls for big images
            ImageFileNames = GetTempFolderPathForFiles("*{0}*.jpg", fahrt, mapToUrlFunction).ToList();
            
            // Get web folder urls for pdf files
            var availablePdfs = GetTempFolderPathForFiles("*{0}*.pdf", fahrt, mapToUrlFunction).ToList();

            PdfFileNames = FilterPdfsForUeberfuhrungsauftrag(availablePdfs, auftragsNr, fahrt).ToList();

            // Get web folder urls for thumb images
            var mask = "THUMB_*{0}*.jpg";
            ThumbImageFileNames = GetTempFolderPathForFiles(mask, fahrt, mapToUrlFunction).ToList();
            // Also copy thumb images at this point
            // Note: Big images and pdf files will be copied only if user clicks on the apropiate thumbnail
            CopyFilesToTempFolder(mask);
        }

        public IEnumerable<string> FilterPdfsForUeberfuhrungsauftrag(IEnumerable<string> liste, string auftragsnummer, string fahrt)
        {
            string fahrtAlt = "XXX"; // Wert für den Fall dass kein unbekannte Daten übergeben wurden
            string fahrtAltProtokoll = "XXX";
            string fahrtNeu = "XXX";
            if (fahrt == "1")
            {
                fahrtAlt = "-1";
                fahrtAltProtokoll = "-1P";
                fahrtNeu = "_H";
            }

            if (fahrt == "2")
            {
                fahrtAlt = "-2";
                fahrtAltProtokoll = "-2P";
                fahrtNeu = "_R";
            }

            return liste.Select(x => Path.GetFileNameWithoutExtension(x.ToUpper()))
                        .Where(n =>(n.Contains(auftragsnummer.TrimStart(new[] { '0' })) && (n.Contains(fahrtAlt) || n.Contains(fahrtAltProtokoll) )) ||
                        (n.Contains(auftragsnummer) && n.EndsWith(fahrtNeu)));
        } 

        public void CopySingleBigImage(int tour, int singleFileNr)
        {
            CopyFilesToTempFolder("*{0}*.jpg", singleFileNr, tour);
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

        IEnumerable<string> GetTempFolderPathForFiles(string fileMask, string fahrt, Func<string, string> mapToUrlFunction)
        {
            var sourcePath = GetSourcePath();
            var filesInDirectory = FileService.TryDirectoryGetFiles(sourcePath, string.Format(fileMask, HistoryAuftragCurrent.AuftragsNrWebViewTrimmed));

            return filesInDirectory
                .Where(f => fahrt == GetTourFromFilename(f).ToString())
                .Select(f => mapToUrlFunction(Path.Combine(AppSettings.WebViewAbsolutePath, DestinationRelativePath, FileService.PathGetFileName(f)))).OrderBy(f => f);
        }

        public int GetTourFromFilename(string filename)
        {

            // Diese Logik funktioniert nur bei PDFs mit dem Format 0027904333-0001-1P.pdf
            // PDFs mit dem Format 0000325503_0027904332_D_e-alp-ru_H.pdf werden hier nicht erkannt.
            //if (filename.LastIndexOf('-') == -1)
            //    return 0;

            //filename = filename.Substring(filename.LastIndexOf('-'));
            
            //int tour;
            //if (!Int32.TryParse(filename.Substring(1, 1), out tour))
            //    return -1;

            //return tour;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename.ToUpper());

            if (fileNameWithoutExtension.EndsWith("_H") || fileNameWithoutExtension.EndsWith("-1") || fileNameWithoutExtension.EndsWith("-1P"))
            {
                return 1;
            }

            if (fileNameWithoutExtension.EndsWith("_R") || fileNameWithoutExtension.EndsWith("-2") || fileNameWithoutExtension.EndsWith("-2P"))
            {
                return 2;
            }

            return -1;

        }

        public string GetPdfFilesAsZip()
        {
            var sourcePath = GetSourcePath();

            if (sourcePath == null || DestinationRelativePath == null)
                return "";

            var zipFileNameWithoutExtensions = string.Format("Ueberfuehrungsprotokolle_{0}_Fahrt_{1}", HistoryAuftragCurrent.AuftragsNrWebViewTrimmed, HistoryAuftragCurrent.Fahrt);

            var zip = new ZipFile();

            // beim InitData wurden die PDF Dateien bereits gefiltert, Ergebnis wurde in der Property PdfFileNames gespeichert
            // Stattdessen wird auf die Verzeichnise erneut zugegriffen ohne dass eine Filterung vorgenommen wird

            // Folgende Ermittlungen der PDF also Ausschalten, nur noch die vor-gefilterten Daten anzeigen

            var allPdfFiles = FileService.TryDirectoryGetFiles(sourcePath, "*.pdf");

            // Liste mit den bereits ermittleten PdfFileNames abgleichen
            // PdfFileNames beinhaltet den Dateinamne ohne Extension und ohne verzeichnis da alles in der VM gepsiechert ist
            var pdfFilesToZip = allPdfFiles.Where(x => PdfFileNames.Contains(Path.GetFileNameWithoutExtension(x.ToUpper())));

            foreach (var pdfFileName in pdfFilesToZip)
            {
                zip.AddFile(pdfFileName);
            }

            var zipFileName = GetDestinationFileName(zipFileNameWithoutExtensions + ".zip", DestinationRelativePath);
            if (!FileService.TryFileDelete(zipFileName))
                return "";

            zip.Save(zipFileName);

            return zipFileName;
        }
    }
}