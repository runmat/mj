using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace CkgDomainLogic.Controllers
{
    public class DocumentController : CkgDomainController
    {
        public DocumentController(IAppSettings appSettings, ILogonContextDataService logonContext, IInfoCenterDataService infoCenterDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, infoCenterDataService);
            InitModelStatics();
        }

        private void InitModelStatics()
        {
            DokumentErstellenBearbeiten.GetViewModel = GetViewModel<DocumentViewModel>;
        }

        public DocumentViewModel ViewModel { get { return GetViewModel<DocumentViewModel>(); } }

        public override string DataContextKey { get { return GetDataContextKey<DocumentViewModel>(); } }

        [CkgApplication]
        public ActionResult DocumentsForCurrentGroup()
        {
            ViewModel.DataInit(false, false);

            return View("Index", ViewModel);
        }

        [CkgApplication]
        public ActionResult DocumentsForCurrentCustomer()
        {
            ViewModel.DataInit(false, true);

            return View("Index", ViewModel);
        }

        [CkgApplication]
        public ActionResult DocumentsGeneral()
        {
            ViewModel.DataInit(true, false);

            return View("Index", ViewModel);
        }

        [CkgApplication]
        public ActionResult DocumentsGeneralAdmin()
        {
            ViewModel.DataInit(true, true);

            return View("Index", ViewModel);
        }

        #region Dokument Grid

        [GridAction]
        public ActionResult DocumentsAjaxBinding()
        {
            return PartialView(new GridModel(ViewModel.DocumentsFiltered));
        }

        [HttpPost]
        public ActionResult FilterDokumentGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterDocuments(filterValue, filterColumns);

            return new EmptyResult();
        }

        public FileResult Get(int id)
        {
            var dokument = ViewModel.Documents.Single(x => x.DocumentID == id);
            var virtualFilePath = string.Concat(FileSourcePath, dokument.FileName, ".", dokument.FileType);
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualFilePath));
        }

        #endregion

        #region Upload und Überschreiben

        [HttpPost]
        public ActionResult UploadDocuments(IEnumerable<HttpPostedFileBase> uploadFiles, string documentTypeId)
        {
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

            ViewModel.SaveDocument(fileName, extension, uploadFile.ContentLength);
        }

        private JsonResult VerifyDocument(HttpPostedFileBase uploadFile)
        {
            if (!CheckFolderAvailablilityAndCreate())
                return Json(new { success = false, message = Localize.DocumentCannotCreateFolder });

            if (!CheckFileExtention(uploadFile.FileName))
                return Json(new { success = false, message = Localize.FileUploadLegalFileTypesWarning }); 

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
            return PartialView(ViewModel.NewDocumentProperties);
        }

        [HttpPost]
        public ActionResult SetDocumentProperties(string docTypeId, string userGroups, string tags)
        {
            ViewModel.NewDocumentProperties.DocTypeID = docTypeId.ToInt(0);
            ViewModel.NewDocumentProperties.SelectedWebGroups = new List<string>();
            ViewModel.NewDocumentProperties.Tags = tags;

            if (!String.IsNullOrEmpty(userGroups))
            {
                var teile = userGroups.Split(',');
                foreach (var teil in teile)
                {
                    ViewModel.NewDocumentProperties.SelectedWebGroups.Add(teil);
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
            return PartialView(ViewModel.GetDocumentModel(id));
        }

        [HttpPost]
        public ActionResult Edit(DokumentErstellenBearbeiten model)
        {
            if (ModelState.IsValid)
            {
                var resultOk = ViewModel.SaveDocument(model);

                if (!resultOk)
                    ModelState.AddModelError("", "Speicherung schlug fehl");
            }

            return PartialView(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var dokument = ViewModel.Documents.Single(x => x.DocumentID == id);
            var filePath = string.Concat(FileSourcePath, dokument.FileName, ".", dokument.FileType);

            var resultOk = ViewModel.DeleteDocument(id);

            if (resultOk)
            {
                var fileInfo = new FileInfo(filePath);
                fileInfo.Delete();
            }

            return new EmptyResult();
        }

        #endregion

        #region DocumentType
        
        public ActionResult DocumentTypeAjaxBinding()
        {
            return Json(new GridModel(ViewModel.DocumentTypesFiltered));
        }

        [HttpPost]
        public ActionResult FilterDocumentTypeGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterDocumentTypes(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult CreateOrEditDocumentType(int id)
        {
            return PartialView(ViewModel.GetDocumentType(id));
        }

        [HttpPost]
        public ActionResult CreateOrEditDocumentType(DocumentType documentType)
        {
            if (ModelState.IsValid)
                documentType = ViewModel.SaveDocumentType(documentType);

            return PartialView(documentType);
        }

        public ActionResult DeleteDocumentType(int id)
        {
            // Kann ich löschen?
            var numberOfDocumentsUsingdocType = ViewModel.NumberOfDocumentsUsingDocType(id);

            if (numberOfDocumentsUsingdocType > 0)
            {
                return Json(numberOfDocumentsUsingdocType);
            }

            ViewModel.DeleteDocumentType(id);

            return new EmptyResult();    
        }

        #endregion

        #region Export

        public ActionResult ExportDokumentsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Dokumente", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDokumentsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Dokumente", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult ExportDocumentTypesFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DocumentTypesFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("DocumentTypes", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDocumentTypesFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DocumentTypesFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("DocumentTypes", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

    }
}