using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrer.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrer.Contracts;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using SapORM.Contracts;
using WebTools.Services;

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
                    "  ,{\"title\":\"Bemerkung\",\"member\":\"{3}\",\"type\":\"String\"}" +
                    "]").FormatPropertyParams<FahrerTagBelegung>(m => m.Datum, m => m.Verfuegbarkeit, m => m.FahrerAnzahl, m => m.Kommentar);
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


        public string AuftragsFahrtTypen { get { return string.Format("H,{0};R,{1}", Localize._FahrtHin, Localize._FahrtRueck); } }


        private IFahrerAuftragsFahrt _selectedFahrerAuftrag;
        public IFahrerAuftragsFahrt SelectedFahrerAuftrag
        {
            get { return _selectedFahrerAuftrag ?? FahrerAuftragsFahrten.FirstOrDefault(a => a.UniqueKey == SelectedFahrerAuftragsKey); }
            private set { _selectedFahrerAuftrag = value; }
        }


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
            var pdfBytesList = new List<byte[]>();

            var pdfBytesAuftrag = DataService.GetAuftragsPdfBytes(auftragsNr);

            pdfBytesList.Add(pdfBytesAuftrag);

            var auftrag = FahrerAuftraege.FirstOrDefault(a => a.AuftragsNr == auftragsNr);
            if (auftrag != null)
            {
                var verzeichnis = Path.Combine(AppSettings.UploadFilePath, auftrag.KundenNr.ToSapKunnr(), auftragsNr.PadLeft(10, '0'), "vertraege");

                if (Directory.Exists(verzeichnis))
                {
                    var dateien = Directory.GetFiles(verzeichnis);

                    foreach (var datei in dateien)
                    {
                        var pdfBytes = File.ReadAllBytes(datei);

                        pdfBytesList.Add(pdfBytes);
                    }
                }
            }

            return PdfDocumentFactory.MergePdfDocuments(pdfBytesList);
        }

        #endregion


        #region Foto / Protokoll Upload

        public bool ModeProtokoll { get; set; }

        [LocalizedDisplay(LocalizeConstants.ProtocolHasMultiplePages)]
        public bool ProtokollHasMultipleImages { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [Required]
        public string SonstigeAuftragsNummer
        {
            get
            {
                return SelectedFahrerAuftrag == null || !SelectedFahrerAuftrag.IstSonstigerAuftrag ? "" : SelectedFahrerAuftrag.AuftragsNr;
            }
            set
            {
                if (SelectedFahrerAuftrag == null || !SelectedFahrerAuftrag.IstSonstigerAuftrag)
                    return;

                SelectedFahrerAuftrag.AuftragsNr = value;
            }
        }

        [LocalizedDisplay(LocalizeConstants._Fahrt)]
        public string SonstigerAuftragsFahrtTyp  { get { return PropertyCacheGet(() => "H"); } set { PropertyCacheSet(value); } }

        public IFahrerAuftragsFahrt SonstigerAuftrag { get; set; }

        public bool IstSonstigerAuftrag { get { return SelectedFahrerAuftrag != null && SelectedFahrerAuftrag.IstSonstigerAuftrag; } }

        public List<IFahrerAuftragsFahrt> FahrerAuftragsFahrtenForLoading
        {
            get { return new List<IFahrerAuftragsFahrt> { new FahrerAuftragsFahrt { AuftragsNr = "-1" } }; }
        }

        public List<IFahrerAuftragsFahrt> FahrerAuftragsFahrten
        {
            get { return PropertyCacheGet(() => new List<IFahrerAuftragsFahrt>()); }
            set { PropertyCacheSet(value); }
        }

        public string FotoUploadPathVirtual { get { return GetFotoOrProtocolPath(ConfigurationManager.AppSettings["FahrerFotoUploadPathVirtual"]); } }
        public string FotoUploadPath { get { return GetFotoOrProtocolPath(ConfigurationManager.AppSettings["FahrerFotoUploadPath"]); } }
        public string FotoUploadPathBackup { get { return GetFotoOrProtocolPath(ConfigurationManager.AppSettings["FahrerFotoUploadPathBackup"]); } }

        public string FotoUploadPathThumbnails { get { return Path.Combine(FotoUploadPath, "_tb"); } }
        public string FotoUploadPathThumbnailsVirtual { get { return Path.Combine(FotoUploadPathVirtual, "_tb"); } }
        // ReSharper disable ConvertClosureToMethodGroup
        public List<string> UploadedImageFiles { get { return PropertyCacheGet(() => GetUploadedImageFiles()); } }
                                                                                                                                    // ReSharper restore ConvertClosureToMethodGroup

        public string LoadFahrerAuftragsFahrten()
        {
            if (ModeProtokoll)
            {
                FahrerAuftragsFahrten = DataService.LoadFahrerAuftragsProtokolle().ToList();
                FahrerAuftragsFahrten.Insert(0, new FahrerAuftragsProtokoll { IstSonstigerAuftrag = true, ProtokollArt = "SONSTIGES", ProtokollName = "SONSTIGES" });
                FahrerAuftragsFahrten.Insert(0, new FahrerAuftragsProtokoll());

                if (FahrerAuftragsFahrten.Any(f => ((FahrerAuftragsProtokoll) f).ProtokollName.NotNullOrEmpty().Contains("_")))
                    return Localize.ErrorNoUnderscoresAllowedInProtocolTypes;
            }
            else
            {
                FahrerAuftragsFahrten = DataService.LoadFahrerAuftragsFahrten().ToList();
                FahrerAuftragsFahrten.Insert(0, new FahrerAuftragsFahrt());
            }

            return "";
        }

        public void SetSelectedFahrerAuftragsKey(string auftragsNr)
        {
            SelectedFahrerAuftragsKey = auftragsNr;
            DataMarkForRefreshUploadedImageFiles();
        }

        public void SetProtokollHasMultipleImages(bool check)
        {
            ProtokollHasMultipleImages = check;
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

        public string GetUploadedPdfFileName(FahrerAuftragsProtokoll auftrag = null)
        {
            if (auftrag == null)
                if (!IstSonstigerAuftrag)
                    auftrag = SelectedFahrerAuftrag as FahrerAuftragsProtokoll;
                else
                    auftrag = SonstigerAuftrag as FahrerAuftragsProtokoll;

            if (auftrag == null)
                return "";

            if (auftrag.KundenNr.IsNullOrEmpty())
                auftrag.KundenNr = LogonContext.KundenNr;

            return auftrag.Filename;
        }

        private static string GetUploadedImageFileName(string auftragsNr, string imageIndex, string fahrerNr, string fahrtNr, string protokollName)
        {
            var imageMask = imageIndex.ToInt() == -1 ? "*" : imageIndex.ToInt().ToString("0000");

            return string.Format("{0}-{1}-{2}-{3}{4}", auftragsNr.TrimStart('0'), imageMask, fahrerNr.TrimStart('0'), fahrtNr, protokollName.PrependIfNotNull("-"));
        }

        private string GetFotoOrProtocolPath(string path)
        {
            if (!ModeProtokoll)
                return path;

            var slash = (path.Contains("/") ? "/" : "\\");

            return string.Format("{0}{1}{2}", path, slash, "Protokolle");
        }

        public List<string> GetUploadedImageFiles()
        {
            TryDirectoryCreateAndRaiseError(FotoUploadPath);
            TryDirectoryCreateAndRaiseError(FotoUploadPathThumbnails);

            var auftrag = SelectedFahrerAuftrag;
            if (auftrag == null)
                return new List<string>();

            var existingImageFiles = Directory.GetFiles(FotoUploadPath, string.Format("{0}.{1}",
                                               GetUploadedImageFileName(auftrag.AuftragsNrFriendly, "*", "*", auftrag.Fahrt, auftrag.ProtokollName),
                                               "*"));

            return existingImageFiles.ToListOrEmptyList().OrderBy(GetImageIndexFromFileName).Select(Path.GetFileName).ToList();
        }

        public bool SaveUploadedImageFile(string clientFileName, Action<string> saveAction)
        {
            var auftrag = SelectedFahrerAuftrag;
            var uploadImageIndex = (GetImageIndexFromFileName(UploadedImageFiles.LastOrDefault()) + 1).ToString();

            // validation
            var auftragsNrFriendly = auftrag.AuftragsNrFriendly;
            var extension = Path.GetExtension(clientFileName);

            if (auftragsNrFriendly.Trim().Length == 0 || extension.ToLower() == ".exe")
                return false;

            var serverFileName = string.Format("{0}{1}",
                                               GetUploadedImageFileName(auftragsNrFriendly, uploadImageIndex, DataService.FahrerID, auftrag.Fahrt, auftrag.ProtokollName),
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
            ImagingService.ScaleAndSaveImage(destinationFileName, thumbnailFileName, ModeProtokoll ? 600 : 200);
            
            DataMarkForRefreshUploadedImageFiles();

            return true;
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

        public string ProtokollGetFullPdfFilePath()
        {
            return Path.Combine(FotoUploadPath, GetUploadedPdfFileName());
        }

        public bool ProtokollCreateAndShowPdf()
        {
            var pdfFileName = ProtokollGetFullPdfFilePath();
            var imageServerFileNames = UploadedImageFiles.Select(serverFileName => Path.Combine(FotoUploadPath, serverFileName));
            PdfDocumentFactory.CreatePdfFromImages(imageServerFileNames, pdfFileName, true, true);

            return true;
        }
        
        public bool ProtokollDeleteUploadedImagesAndPdf()
        {
            var pdfFileName = Path.Combine(FotoUploadPath, GetUploadedPdfFileName());

            FileService.TryFileDelete(pdfFileName);
            UploadedImageFiles.ToList().ForEach(f => DeleteUploadedImage(f));

            return true;
        }

        public void SetParamProtokollMode(string modeProtokoll)
        {
            ModeProtokoll = modeProtokoll.IsNotNullOrEmpty();
        }

        public bool ProtokollTryLoadSonstigenAuftrag(string auftragsnr, string fahrtTyp)
        {
            if (auftragsnr.ToInt() <= 0)
                return false;

            auftragsnr = auftragsnr.ToSapKunnr();
            var storedAuftrag = DataService.LoadFahrerAuftragsEinzelProtokoll(auftragsnr, fahrtTyp);
            if (storedAuftrag == null)
                return false;

            SonstigerAuftrag = storedAuftrag;
            SonstigeAuftragsNummer = auftragsnr;
            SonstigerAuftragsFahrtTyp = fahrtTyp;

            if (SelectedFahrerAuftrag != null)
            {
                SelectedFahrerAuftrag.AuftragsNr = auftragsnr;
                SelectedFahrerAuftrag.Fahrt = fahrtTyp;
                SelectedFahrerAuftrag.ProtokollName = storedAuftrag.ProtokollName;
            }

            DataMarkForRefreshUploadedImageFiles();

            return true;
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


        #region Protokollarchivierung

        public List<SelectItem> QmCodes { get { return DataService.QmCodes; } }

        public List<FahrerAuftragsProtokoll> FahrerProtokolle
        {
            get { return PropertyCacheGet(() => new List<FahrerAuftragsProtokoll>()); }
            set { PropertyCacheSet(value); }
        }

        public string ProtokollEditFileName { get; private set; }

        public void LoadFahrerProtokolle()
        {
            var verzeichnis = new DirectoryInfo(FotoUploadPath);
            var dateien = verzeichnis.GetFiles(string.Format(FahrerAuftragsProtokoll.FahrerProtokollFilenamePattern, "*", "*", "*", "*"));

            FahrerProtokolle.Clear();

            foreach (var datei in dateien)
            {
                var teile = Path.GetFileNameWithoutExtension(datei.Name).NotNullOrEmpty().Split('_');

                var protArt = teile[3];

                if (teile.Length > 5)
                {
                    for (var i = 4; i < (teile.Length - 1); i++)
                    {
                        protArt += "_" + teile[i];
                    }
                }

                FahrerProtokolle.Add(new FahrerAuftragsProtokoll
                {
                    KundenNr = teile[0],
                    AuftragsNr = teile[1],
                    ProtokollArt = protArt,
                    ProtokollName = protArt,
                    Fahrt = teile[teile.Length - 1]
                });
            }

            PropertyCacheClear(this, m => m.FahrerProtokolleFiltered);
        }

        public ProtokollEditModel GetProtokollEditModel(string fileName)
        {
            ProtokollEditFileName = fileName;
            var prot = FahrerProtokolle.FirstOrDefault(p => p.Filename == ProtokollEditFileName);

            if (prot == null)
                return null;

            var mailAdr = DataService.GetProtokollArchivierungMailAdressenAndReferenz(prot);

            return new ProtokollEditModel { Protokoll = prot, MailAdressen = String.Join(";", mailAdr) };
        }

        public byte[] GetProtokollEditPdf()
        {
            if (FahrerProtokolle.None(p => p.Filename == ProtokollEditFileName))
                return new byte[0];

            var physPath = Path.Combine(FotoUploadPath, ProtokollEditFileName);
            return File.ReadAllBytes(physPath);
        }

        public void ProtokollArchivieren(ProtokollEditModel model, ModelStateDictionary state)
        {
            var erg = DataService.SaveProtokollAndQmDaten(model);
            if (!string.IsNullOrEmpty(erg))
            {
                state.AddModelError(string.Empty, erg);
                return;
            }
            
            erg = ArchiviereProtokoll(model);
            if (!string.IsNullOrEmpty(erg))
            {
                state.AddModelError(string.Empty, erg);
                return;
            }

            if (!SendeProtokollArchivierungsMail(model))
            {
                state.AddModelError(string.Empty, Localize.ErrorMailCouldNotBeSent);
                return;
            }

            FahrerProtokolle.RemoveAll(p => p.Filename == model.Protokoll.Filename);
            PropertyCacheClear(this, m => m.FahrerProtokolleFiltered);
        }

        private string ArchiviereProtokoll(ProtokollEditModel model)
        {
            try
            {
                var archivPfad = GeneralConfiguration.GetConfigValue("FahrerProtokollArchivierung", "ArchivPfad");

                if (!archivPfad.EndsWith("\\"))
                    archivPfad += "\\";

                var archivPfadKunde = archivPfad + model.Protokoll.KundenNr + "\\";
                if (!Directory.Exists(archivPfadKunde))
                    Directory.CreateDirectory(archivPfadKunde);

                var archivPfadKundeAuftrag = archivPfadKunde + model.Protokoll.AuftragsNr + "\\";
                if (!Directory.Exists(archivPfadKundeAuftrag))
                    Directory.CreateDirectory(archivPfadKundeAuftrag);

                var quellPfad = Path.Combine(FotoUploadPath, model.Protokoll.Filename);
                var zielPfad = Path.Combine(archivPfadKundeAuftrag, model.Protokoll.Filename);

                if (File.Exists(zielPfad))
                    File.Delete(zielPfad);

                File.Move(quellPfad, zielPfad);

                return "";
            }
            catch (Exception ex)
            {
                return string.Format("{0}: {1}", Localize.Error, ex.Message);
            }
        }

        private bool SendeProtokollArchivierungsMail(ProtokollEditModel model)
        {
            if (!string.IsNullOrEmpty(model.MailAdressen))
            {
                var mailBetreff = string.Format("Bestandsnummer: {0}", (String.IsNullOrEmpty(model.Protokoll.Referenz) ? model.Protokoll.VIN : model.Protokoll.Referenz));
                var mailText = GeneralConfiguration.GetConfigValue("FahrerProtokollArchivierung", "MailText").Replace("{br}", Environment.NewLine);

                var mailService = new SmtpMailService(AppSettings);

                return mailService.SendMail(model.MailAdressen, mailBetreff, mailText);
            }

            return true;
        }

        public string ProtokollLoeschen()
        {
            try
            {
                var prot = FahrerProtokolle.FirstOrDefault(p => p.Filename == ProtokollEditFileName);

                if (prot == null)
                    return Localize.Error;

                var dateiPfad = Path.Combine(FotoUploadPath, prot.Filename);

                File.Delete(dateiPfad);

                FahrerProtokolle.RemoveAll(p => p.Filename == ProtokollEditFileName);
                PropertyCacheClear(this, m => m.FahrerProtokolleFiltered);

                SelectedFahrerAuftrag = prot;
                PropertyCacheClear(this, e => e.UploadedImageFiles);
                ProtokollDeleteUploadedImagesAndPdf();
                SelectedFahrerAuftrag = null;

                return "";
            }
            catch (Exception ex)
            {
                return String.Format("{0}: {1}", Localize.Error, ex.Message);
            }
        }

        #region Filter

        [XmlIgnore]
        public List<FahrerAuftragsProtokoll> FahrerProtokolleFiltered
        {
            get { return PropertyCacheGet(() => FahrerProtokolle); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterFahrerProtokolle(string filterValue, string filterProperties)
        {
            FahrerProtokolleFiltered = FahrerProtokolle.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion

        #endregion
    }
}
