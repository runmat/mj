using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models.DataModels;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace CkgDomainLogic.Controllers
{
    public class DocumentController : CkgDomainController
    {
        public DocumentController(IAppSettings appSettings, ILogonContextDataService logonContextdataService, IInfoCenterDataService infoCenterDataService) : base(appSettings, logonContextdataService)
        {
            InitViewModel(DokumentViewModel, appSettings, logonContextdataService, infoCenterDataService);
        }

        public DokumentViewModel DokumentViewModel { get { return GetViewModel<DokumentViewModel>(); } private set { SetViewModel(value); } }

        public override string DataContextKey { get { return GetDataContextKey<DokumentViewModel>(); } }

        #region Dokument Grid

        [CkgApplication]
        public ActionResult DocumentsForCurrentGroup()
        {
            DokumentViewModel = null;
            return View("Index", DokumentViewModel);
        }

        [CkgApplication]
        public ActionResult DocumentsForCurrentCustomer()
        {
            DokumentViewModel = null;
                                                                                        // ReSharper disable PossibleNullReferenceException
            DokumentViewModel.IsAdministrator = true;
                                                                                        // ReSharper restore PossibleNullReferenceException
            return View("Index", DokumentViewModel);
        }

        [GridAction]
        public ActionResult DocumentsAjaxBinding()
        {
            return PartialView(new GridModel(DokumentViewModel.DokumentsFiltered));
        }

        [HttpPost]
        public ActionResult FilterDokumentGrid(string filterValue, string filterColumns)
        {
            DokumentViewModel.FilterDokuments(filterValue, filterColumns);

            return new EmptyResult();
        }

        public FileResult Get(int id)
        {
            var dokument = DokumentViewModel.Dokuments.Single(x => x.DocumentID == id);
            var virtualFilePath = string.Concat(FileSourcePath, dokument.FileName, ".", dokument.FileType);
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualFilePath));
        }

        #endregion

        #region Upload und Überschreiben

        [HttpPost]
        public ActionResult UploadDocuments(IEnumerable<HttpPostedFileBase> uploadFiles, string documentTypeId)
        {
            //// Selektion übernehmen
            //if (model.SelectedWebGroups == null)
            //{
            //    model.SelectedWebGroups = new List<string>();
            //}
            //DokumentViewModel.NewDocumentProperties = model;

            // Prüfen, ob Upload-Verzeichnis ok und Dateitypen erlaubt
            foreach (var uploadfile in uploadFiles)
            {
                var verifyResult = VerifyDocument(uploadfile);
                if (verifyResult != null)
                {
                    return verifyResult;
                }
            }

            // Datei(en) hochladen/anlegen
            foreach (var uploadfile in uploadFiles)
            {
                InsertNewDocument(uploadfile);
            }

            return new EmptyResult();
        }   

        private void InsertNewDocument(HttpPostedFileBase uploadFile)
        {
            var fileInfo = new FileInfo(uploadFile.FileName);
            var extension = fileInfo.Extension.Replace(".", string.Empty);

            var fileName = uploadFile.SavePostedFile(FileSourcePath, Path.GetFileNameWithoutExtension(uploadFile.FileName), fileInfo.Extension);

            var now = DateTime.Now;

            var dokument = new Dokument
                {
                    DocTypeID = DokumentViewModel.NewDocumentProperties.DocTypeID,
                    FileName = fileName,
                    FileSize = uploadFile.ContentLength,
                    LastEdited = now,
                    Uploaded = now,
                    CustomerID = LogonContext.User.CustomerID,
                    FileType = extension
                };

            dokument = DokumentViewModel.SaveItem(dokument, (s, s1) => { });

            DokumentViewModel.NewDocumentProperties.ID = dokument.DocumentID;
            DokumentViewModel.NewDocumentProperties.Name = dokument.FileName;

            DokumentViewModel.DataService.SaveDocument(DokumentViewModel.NewDocumentProperties);
        }

        //private void ReplaceExistingDocument(HttpPostedFileBase uploadFile)
        //{
        //    uploadFile.DeleteExistingAndSavePostedFile(FileSourcePath);

        //    var fileInfo = new FileInfo(uploadFile.FileName);
        //    var extension = fileInfo.Extension.Replace(".", string.Empty);
        //    var fileName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
        //    var now = DateTime.Now;

        //    var dokument = DokumentViewModel.Dokuments.Single(x => x.FileName == fileName && x.FileType == extension);

        //    dokument.DocTypeID = DokumentViewModel.NewDocumentProperties.DocTypeID;
        //    dokument.Uploaded = now;
        //    dokument.LastEdited = now;

        //    DokumentViewModel.SaveItem(dokument, (s, s1) => { });
        //}

        private JsonResult VerifyDocument(HttpPostedFileBase uploadFile)
        {

            if (!CheckFolderAvailablilityAndCreate())
            {
                return Json(new { success = false, message = Localize.DocumentCannotCreateFolder }, "text/plain");
            }


            if (!CheckFileExtention(uploadFile.FileName))
            {
                return Json(new { success = false, message = Localize.FileUploadLegalFileTypesWarning }, "text/plain"); 
            }

            return null;
        }

        /// <summary>
        /// Pfad für die Speicherung der Dokumente
        /// </summary>
        private string FileSourcePath
        {
            get
            {
                var folder = ConfigurationManager.AppSettings["DownloadPathSamba"];
                var fileSourcePath = string.Concat(folder, LogonContext.KundenNr, @"\");
                return fileSourcePath;
            }
        }

        private bool CheckFolderAvailablilityAndCreate()
        {
            if (Directory.Exists(FileSourcePath))
            {
                return true;                
            }

            try
            {
                Directory.CreateDirectory(FileSourcePath);
            }
            catch (Exception)
            {
                var logService = new LogService(string.Empty, string.Empty);
                var wrappepException = new Exception(string.Format(Localize.DocumentCannotCreateFolderDetail, FileSourcePath));
                // Via Elmah eine detailierte Meldung erzeugung mit dem Namen des Verzeichnis was nicht erstellt werden könnte
                logService.LogElmahError(wrappepException, LogonContext);
                // Diese Fehlermeldung beinhaltet keine Informationen zum Verzeichnis
                throw new Exception(Localize.DocumentCannotCreateFolder);
            }

            return true;
        }

        private bool CheckFileExtention(string filename)
        {
            var fileInfo = new FileInfo(filename);
            var extension = fileInfo.Extension;

            switch (extension)
            {
                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".txt":
                case ".csv":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".apk":
                    break;
                default:
                    return false;
            }
            return true;
        }

        #endregion

        #region Create

        [HttpGet]
        public ActionResult CreateDocument()
        {
            var model = new DocumentErstellenBearbeiten();

            SetSources(model);
            model.DocTypeID = 1; //default-Dokumententyp

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SetDocumentProperties(string docTypeId, string userGroups)
        {
            DokumentViewModel.NewDocumentProperties = new DocumentErstellenBearbeiten
                {
                    DocTypeID = Int32.Parse(docTypeId),
                    SelectedWebGroups = new List<string>()
                };
            if (!String.IsNullOrEmpty(userGroups))
            {
                var teile = userGroups.Split(',');
                foreach (var teil in teile)
                {
                    DokumentViewModel.NewDocumentProperties.SelectedWebGroups.Add(teil);
                }
            }

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult ShowUpload()
        {
            return PartialView("Upload");
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dokument = DokumentViewModel.Dokuments.Single(x => x.DocumentID == id);
            var dokumentGruppen = from dokumentRight in DokumentViewModel.DataService.DocumentRights
                                  where dokumentRight.DocumentID == id
                                  select dokumentRight.GroupID.ToString();

            var model = new DocumentErstellenBearbeiten {ID = id, DocTypeID = dokument.DocTypeID};
            SetSources(model);
            model.SelectedWebGroups = dokumentGruppen.ToList();
            
            model.IsValid = true;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(DocumentErstellenBearbeiten model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            if (model.SelectedWebGroups == null)
            {
                model.SelectedWebGroups = new List<string>();
            }

            var result = DokumentViewModel.DataService.SaveDocument(model);

            if (!result)
            {
                ModelState.AddModelError("", "Speicherung schlug fehl");
            }
            else
            {
                model.IsValid = true;
            }

            SetSources(model);
            return PartialView(model);
        }

        private void SetSources(DocumentErstellenBearbeiten model)
        {
            var gruppen = (from gruppe in DokumentViewModel.DataService.UserGroupsOfCurrentCustomer
                           select new SelectListItem
                           {
                               Text = gruppe.GroupName,
                               Value = gruppe.GroupID.ToString()
                           }).ToList();

            var dokumentTypes = (from documentType in DokumentViewModel.DataService.DocumentTypes
                                select new SelectListItem
                                {
                                    Text = documentType.DocTypeName,
                                    Value = documentType.DocumentTypeID.ToString()
                                }).ToList();

            model.AvailableWebGroups = gruppen;
            model.AvailableDocumentTypes = dokumentTypes.ToList();
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dokument = DokumentViewModel.Dokuments.Single(x => x.DocumentID == id);
            var result = DokumentViewModel.DataService.DeleteDocument(dokument);

            var fileInfo = new FileInfo(string.Concat(FileSourcePath, dokument.FileName, ".", dokument.FileType));
            fileInfo.Delete();

            return Json(result);
        }

        #endregion

        #region DocumentType
        
        [CkgApplication]
        public ActionResult DocumentType()
        {
            DokumentViewModel = null;
            return View("DocumentTypeIndex", DokumentViewModel);
        }

        public ActionResult DocumentTypeAjaxBinding()
        {
            return Json(new GridModel(DokumentViewModel.DocumentTypesFiltered));
        }

        [HttpPost]
        public ActionResult FilterDocumentTypeGrid(string filterValue, string filterColumns)
        {
            DokumentViewModel.FilterDocumentTypes(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Create

        [HttpGet]
        public ActionResult CreateDocumentType()
        {
            var documentType = new DocumentType();
            return PartialView(documentType);
        }

        [HttpPost]
        public ActionResult CreateDocumentType(DocumentType documentType)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(documentType);
            }

            var saveDokumentType = DokumentViewModel.DataService.CreateDocumentType(documentType, (s, s1) => { });

            return PartialView(saveDokumentType);
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult EditDocumentType(int id)
        {
            var documentType = DokumentViewModel.DataService.DocumentTypes.Single(x => x.DocumentTypeID == id);
            return PartialView(documentType);
        }

        [HttpPost]
        public ActionResult EditDocumentType(DocumentType documentType)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(documentType);
            }

            var saveDokumentType = DokumentViewModel.DataService.EditDocumentType(documentType, (s, s1) => { });

            return PartialView(saveDokumentType);
        }

        #endregion

        public ActionResult DeleteDocumentType(int id)
        {
            // Kann ich löschen?
            var numberOfDocumentsUsingdocType = DokumentViewModel.DataService.DokumentsForCurrentCustomer.Count(document => document.DocTypeID == id);

            if (numberOfDocumentsUsingdocType > 0)
            {
                return Json(numberOfDocumentsUsingdocType);
            }

            var documentType = DokumentViewModel.DataService.DocumentTypes.Single(x => x.DocumentTypeID == id);

            DokumentViewModel.DataService.DeleteDocumentType(documentType);
            // Ich Rückgabewert verwende ich im Moment nicht, wenn etwas schief geht bei der Speicherung dann wird eine Ausnahme geworfen

            return Json(0);    
           
        }

        #endregion

        #region Export

        public ActionResult ExportDokumentsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = DokumentViewModel.DokumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Dokumente", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDokumentsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = DokumentViewModel.DokumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Dokumente", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult ExportDocumentTypesFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = DokumentViewModel.DocumentTypesFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("DocumentTypes", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDocumentTypesFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = DokumentViewModel.DocumentTypesFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("DocumentTypes", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

    }
}