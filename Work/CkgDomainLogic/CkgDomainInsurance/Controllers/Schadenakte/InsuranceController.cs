using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Models;
using CkgDomainLogic.Insurance.ViewModels;
using DocumentTools.Services;
using GeneralTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Insurance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class InsuranceController 
    {
        public SchadenakteViewModel SchadenakteViewModel { get { return GetViewModel<SchadenakteViewModel>(); } }

        [HttpPost]
        public ActionResult ShowSchadenakte(int id)
        {
            EventsViewModel.LoadSchadenfall(id);
            SchadenakteViewModel.LoadSchadenakte(EventsViewModel.SchadenfallCurrent);

            return PartialView("Schadenakte/Schadenakte", SchadenakteViewModel);
        }

        #region Dokumente

        [GridAction]
        public ActionResult SchadenakteDocsAjaxBinding()
        {
            return View(new GridModel(SchadenakteViewModel.DocsViewModel.SchadenakteDocumentsFiltered));
        }

        [HttpPost]
        public ActionResult ShowSchadenakteDocEdit(string id)
        {
            var doc = SchadenakteViewModel.DocsViewModel.GetSchadenakteDoc(id);

            ViewBag.AuswahlKategorie = SchadenakteViewModel.DocsViewModel.Categories;

            return PartialView("Schadenakte/Partial/Dokumente/SchadenakteDocEdit", doc);
        }

        [HttpPost]
        public ActionResult UpdateSchadenakteDoc(SchadenakteDocument model)
        {
            if (ModelState.IsValid)
            {
                SchadenakteViewModel.DocsViewModel.UpdateDocument(model, ModelState);
            }

            ViewBag.AuswahlKategorie = SchadenakteViewModel.DocsViewModel.Categories;

            return PartialView("Schadenakte/Partial/Dokumente/SchadenakteDocEdit", model);
        }

        [HttpPost]
        public ActionResult DeleteSchadenakteDoc(string id)
        {
            var erg = SchadenakteViewModel.DocsViewModel.DeleteDocument(id);

            if (erg > 0)
            {
                // zugehörige Datei löschen
                var id10 = id.PadLeft(10, '0');
                var folderPath = string.Concat(FileSourcePath, id10.Substring(0, 7));
                var dateien = Directory.GetFiles(folderPath, string.Concat(id10, ".*"));
                if (dateien.Length > 0)
                {
                    System.IO.File.Delete(dateien[0]);
                }
            }

            return Json(new { Success = (erg > 0), Message = (erg > 0  ? Localize.DeleteSuccessful : Localize.DeleteFailed) });
        }

        [HttpPost]
        public ActionResult CreateSchadenakteDoc()
        {
            var doc = SchadenakteViewModel.DocsViewModel.GetNewSchadenakteDoc();

            ViewBag.AuswahlKategorie = SchadenakteViewModel.DocsViewModel.GetCategoriesWithoutDocuments();

            return PartialView("Schadenakte/Partial/Dokumente/SchadenakteDocCreate", doc);
        }

        [HttpPost]
        public ActionResult SetSchadenakteDocProperties(string categoryId, string dienstleister)
        {
            int tempInt;
            Int32.TryParse(categoryId, out tempInt);
            SchadenakteViewModel.DocsViewModel.NewDocCategoryID = tempInt;
            SchadenakteViewModel.DocsViewModel.NewDocDienstleister = dienstleister;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UploadSchadenakteDoc(IEnumerable<HttpPostedFileBase> uploadFiles)
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
            FileInfo fileInfo = new FileInfo(uploadFile.FileName);
            string originalFilename = Path.GetFileNameWithoutExtension(uploadFile.FileName);
            string extension = fileInfo.Extension.Replace(".", string.Empty).ToLower();

            var doc = SchadenakteViewModel.DocsViewModel.GetNewSchadenakteDoc();

            doc.SchadenfallID = SchadenakteViewModel.DocsViewModel.SchadenfallID;
            doc.FileType = extension;
            doc.FileName = originalFilename;
            doc.CategoryID = SchadenakteViewModel.DocsViewModel.NewDocCategoryID;
            doc.Uploaded = DateTime.Now;
            doc.Dienstleister = SchadenakteViewModel.DocsViewModel.NewDocDienstleister;

            var docId = SchadenakteViewModel.DocsViewModel.CreateDocument(doc, ModelState);

            if (ModelState.IsValid)
            {
                // Ordernamen bestehen aus den vorderen 7 Stellen der ID, um nicht mehr als 1000 Dateien pro Ordner zu haben
                var newFilename = docId.ToString().PadLeft(10, '0');
                var folderPath = string.Concat(FileSourcePath, newFilename.Substring(0, 7));
                if (!Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", string.Format(Localize.DocumentCannotCreateFolderDetail, folderPath));
                        return;
                    }
                }
                uploadFile.SaveAs(string.Concat(folderPath, @"\", newFilename, ".", extension));
            }       
        }

        private JsonResult VerifyDocument(HttpPostedFileBase uploadFile)
        {

            if (!CheckFolderAvailablilityAndCreate())
            {
                return Json(new { success = false, message = Localize.DocumentCannotCreateFolder }, "text/plain");
            }


            if (!CheckFileExtension(uploadFile.FileName))
            {
                return Json(new { success = false, message = Localize.CustomerDocumentUploadLegalFiletypeWarning }, "text/plain"); ;
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
                var fileSourcePath = string.Concat(folder, @"Kundendokumente\", LogonContext.KundenNr, @"\");
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
                LogService logService = new LogService(string.Empty, string.Empty);
                Exception wrappepException = new Exception(string.Format(Localize.DocumentCannotCreateFolderDetail, FileSourcePath));
                // Via Elmah eine detailierte Meldung erzeugung mit dem Namen des Verzeichnis was nicht erstellt werden könnte
                logService.LogElmahError(wrappepException, LogonContext);
                // Diese Fehlermeldung beinhaltet keine Informationen zum Verzeichnis
                throw new Exception(Localize.DocumentCannotCreateFolder);
            }

            return true;
        }

        private bool CheckFileExtension(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            string extension = fileInfo.Extension;

            switch (extension.ToLower())
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
                    break;
                default:
                    return false;
            }
            return true;
        }

        public FileResult Get(int id)
        {
            var doc = SchadenakteViewModel.DocsViewModel.Documents.Find(d => d.ID == id);
            var filename = id.ToString().PadLeft(10, '0');
            var folderPath = string.Concat(FileSourcePath, filename.Substring(0, 7));

            var virtualFilePath = string.Concat(folderPath, @"\", filename, ".", doc.FileType);
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualFilePath));
        }

        [HttpPost]
        public ActionResult FilterSchadenakteDocsGrid(string filterValue, string filterColumns)
        {
            SchadenakteViewModel.DocsViewModel.FilterSchadenakteDocuments(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportSchadenakteDocsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = SchadenakteViewModel.DocsViewModel.SchadenakteDocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Dokumente", dt);

            return new EmptyResult();
        }

        public ActionResult ExportSchadenakteDocsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = SchadenakteViewModel.DocsViewModel.SchadenakteDocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Dokumente", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Kategorien

        [GridAction]
        public ActionResult DocCategoriesAjaxBinding()
        {
            return View(new GridModel(SchadenakteViewModel.DocsViewModel.CategoriesFiltered));
        }

        [HttpPost]
        public ActionResult ShowDocCategories()
        {
            SchadenakteViewModel.DocsViewModel.LoadDocCategories();

            return PartialView("Schadenakte/Partial/Dokumente/DocCategoriesGrid", SchadenakteViewModel.DocsViewModel);
        }

        [HttpPost]
        public ActionResult ShowDocCategoryEdit(string id)
        {
            var cat = SchadenakteViewModel.DocsViewModel.GetDocCategory(id);

            return PartialView("Schadenakte/Partial/Dokumente/DocCategoryEdit", cat);
        }

        [HttpPost]
        public ActionResult SaveDocCategory(CustomerDocumentCategory model)
        {
            if (ModelState.IsValid)
            {
                SchadenakteViewModel.DocsViewModel.SaveCategory(model, ModelState);
            }

            return PartialView("Schadenakte/Partial/Dokumente/DocCategoryEdit", model);
        }

        [HttpPost]
        public ActionResult DeleteDocCategory(string id)
        {
            var erg = SchadenakteViewModel.DocsViewModel.DeleteCategory(id);

            return Json(new { Success = (erg > 0), Message = (erg > 0 ? Localize.DeleteSuccessful : Localize.DeleteFailed) });
        }

        [HttpPost]
        public ActionResult CreateDocCategory()
        {
            var cat = SchadenakteViewModel.DocsViewModel.GetNewDocCategory();

            return PartialView("Schadenakte/Partial/Dokumente/DocCategoryEdit", cat);
        }

        [HttpPost]
        public ActionResult FilterDocCategoriesGrid(string filterValue, string filterColumns)
        {
            SchadenakteViewModel.DocsViewModel.FilterCategories(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDocCategoriesFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = SchadenakteViewModel.DocsViewModel.CategoriesFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Kategorien", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDocCategoriesFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = SchadenakteViewModel.DocsViewModel.CategoriesFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Kategorien", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
