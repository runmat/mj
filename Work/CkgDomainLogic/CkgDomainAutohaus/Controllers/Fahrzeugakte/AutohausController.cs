using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Autohaus.ViewModels;
using DocumentTools.Services;
using GeneralTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Autohaus-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class AutohausController 
    {
        public FahrzeugakteViewModel FahrzeugakteViewModel { get { return GetViewModel<FahrzeugakteViewModel>(); } }

        [HttpPost]
        public ActionResult ShowFahrzeugakte(int id)
        {
            FahrzeugverwaltungViewModel.LoadFahrzeug(id);
            FahrzeugakteViewModel.LoadFahrzeugakte(FahrzeugverwaltungViewModel.FahrzeugCurrent);

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte", FahrzeugakteViewModel);
        }

        #region Zulassung

        [GridAction]
        public ActionResult BeauftragteZulassungenAjaxBinding()
        {
            return View(new GridModel(FahrzeugakteViewModel.BeauftragteZulassungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterBeauftragteZulassungenGrid(string filterValue, string filterColumns)
        {
            FahrzeugakteViewModel.DocsViewModel.FilterFahrzeugakteDocuments(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportBeauftragteZulassungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugakteViewModel.DocsViewModel.FahrzeugakteDocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Dokumente", dt);

            return new EmptyResult();
        }

        public ActionResult ExportBeauftragteZulassungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugakteViewModel.DocsViewModel.FahrzeugakteDocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Dokumente", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Dokumente

        [GridAction]
        public ActionResult FahrzeugakteDocsAjaxBinding()
        {
            return View(new GridModel(FahrzeugakteViewModel.DocsViewModel.FahrzeugakteDocumentsFiltered));
        }

        [HttpPost]
        public ActionResult ShowFahrzeugakteDocEdit(string id)
        {
            var doc = FahrzeugakteViewModel.DocsViewModel.GetFahrzeugakteDoc(id);

            ViewBag.AuswahlKategorie = FahrzeugakteViewModel.DocsViewModel.Categories;

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/FahrzeugakteDocEdit", doc);
        }

        [HttpPost]
        public ActionResult UpdateFahrzeugakteDoc(FahrzeugakteDocument model)
        {
            if (ModelState.IsValid)
            {
                FahrzeugakteViewModel.DocsViewModel.UpdateDocument(model, ModelState);
            }

            ViewBag.AuswahlKategorie = FahrzeugakteViewModel.DocsViewModel.Categories;

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/FahrzeugakteDocEdit", model);
        }

        [HttpPost]
        public ActionResult DeleteFahrzeugakteDoc(string id)
        {
            var erg = FahrzeugakteViewModel.DocsViewModel.DeleteDocument(id);

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
        public ActionResult CreateFahrzeugakteDoc()
        {
            var doc = FahrzeugakteViewModel.DocsViewModel.GetNewFahrzeugakteDoc();

            ViewBag.AuswahlKategorie = FahrzeugakteViewModel.DocsViewModel.Categories;

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/FahrzeugakteDocCreate", doc);
        }

        [HttpPost]
        public ActionResult SetFahrzeugakteDocProperties(string categoryId, string bemerkung)
        {
            int tempInt;
            Int32.TryParse(categoryId, out tempInt);
            FahrzeugakteViewModel.DocsViewModel.NewDocCategoryID = tempInt;
            FahrzeugakteViewModel.DocsViewModel.NewDocBemerkung = bemerkung;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UploadFahrzeugakteDoc(IEnumerable<HttpPostedFileBase> uploadFiles)
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

            var doc = FahrzeugakteViewModel.DocsViewModel.GetNewFahrzeugakteDoc();

            doc.FahrzeugID = FahrzeugakteViewModel.DocsViewModel.FahrzeugID;
            doc.FileType = extension;
            doc.FileName = originalFilename;
            doc.CategoryID = FahrzeugakteViewModel.DocsViewModel.NewDocCategoryID;
            doc.Uploaded = DateTime.Now;
            doc.Bemerkung = FahrzeugakteViewModel.DocsViewModel.NewDocBemerkung;

            var docId = FahrzeugakteViewModel.DocsViewModel.CreateDocument(doc, ModelState);

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
                return Json(new { success = false, message = Localize.CustomerDocumentUploadLegalFiletypeWarning }, "text/plain");
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
            var doc = FahrzeugakteViewModel.DocsViewModel.Documents.Find(d => d.ID == id);
            var filename = id.ToString().PadLeft(10, '0');
            var folderPath = string.Concat(FileSourcePath, filename.Substring(0, 7));

            var virtualFilePath = string.Concat(folderPath, @"\", filename, ".", doc.FileType);
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualFilePath));
        }

        [HttpPost]
        public ActionResult FilterFahrzeugakteDocsGrid(string filterValue, string filterColumns)
        {
            FahrzeugakteViewModel.DocsViewModel.FilterFahrzeugakteDocuments(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugakteDocsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugakteViewModel.DocsViewModel.FahrzeugakteDocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Dokumente", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugakteDocsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugakteViewModel.DocsViewModel.FahrzeugakteDocumentsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Dokumente", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Kategorien

        [GridAction]
        public ActionResult DocCategoriesAjaxBinding()
        {
            return View(new GridModel(FahrzeugakteViewModel.DocsViewModel.CategoriesFiltered));
        }

        [HttpPost]
        public ActionResult ShowDocCategories()
        {
            FahrzeugakteViewModel.DocsViewModel.LoadDocCategories();

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/DocCategoriesGrid", FahrzeugakteViewModel.DocsViewModel);
        }

        [HttpPost]
        public ActionResult ShowDocCategoryEdit(string id)
        {
            var cat = FahrzeugakteViewModel.DocsViewModel.GetDocCategory(id);

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/DocCategoryEdit", cat);
        }

        [HttpPost]
        public ActionResult SaveDocCategory(CustomerDocumentCategory model)
        {
            if (ModelState.IsValid)
            {
                FahrzeugakteViewModel.DocsViewModel.SaveCategory(model, ModelState);
            }

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/DocCategoryEdit", model);
        }

        [HttpPost]
        public ActionResult DeleteDocCategory(string id)
        {
            var erg = FahrzeugakteViewModel.DocsViewModel.DeleteCategory(id);

            return Json(new { Success = (erg > 0), Message = (erg > 0 ? Localize.DeleteSuccessful : Localize.DeleteFailed) });
        }

        [HttpPost]
        public ActionResult CreateDocCategory()
        {
            var cat = FahrzeugakteViewModel.DocsViewModel.GetNewDocCategory();

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Dokumente/DocCategoryEdit", cat);
        }

        [HttpPost]
        public ActionResult FilterDocCategoriesGrid(string filterValue, string filterColumns)
        {
            FahrzeugakteViewModel.DocsViewModel.FilterCategories(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDocCategoriesFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugakteViewModel.DocsViewModel.CategoriesFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Kategorien", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDocCategoriesFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugakteViewModel.DocsViewModel.CategoriesFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Kategorien", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
