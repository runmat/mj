using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using GeneralTools.Services;
using MvcTools.Web;
using ServicesMvc.Models;
using ServicesMvc.ViewModels;

namespace ServicesMvc.Controllers
{
    public class TestController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<TestViewModel>(); } }

        public TestViewModel ViewModel { get { return GetViewModel<TestViewModel>(); } }


        public TestController(IAppSettings appSettings, ILogonContextDataService logonContext)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext);
        }

        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    return View(ViewModel);
        //}

        //[HttpPost]
        //public ActionResult TestFormSave(TestFilterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ViewModel.FilterModel = model;
        //    }

        //    return PartialView("Partial/TestForm", model);
        //}

        [AllowAnonymous]
        [HttpPost]
        public ActionResult TestForm2Save(TestFilterModel2 model)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                //ViewModel.FilterModel2 = model;
                //ViewModel.FilterModel2.FahrerTagBelegungen.Add(new FahrerTagBelegung { Tag = DateTime.Parse("04.02.2014"), Belegung = FahrerTagBelegungsTyp.Verfuegbar });
            }

            return PartialView("Partial/TestFormDatePicker", ViewModel.FilterModel2);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SaveSelectedBelegungen(string model, int fahrerAnzahl, string belegungsType)
        {
            var modelObject = JSon.Deserialize<TestFilterModel2>(model);

            ViewModel.FilterModel2.FahrerTagBelegungen = modelObject.FahrerTagBelegungen;
            ViewModel.FilterModel2.FahrerAnzahl = fahrerAnzahl;
            ViewModel.FilterModel2.BelegungsTyp = (FahrerTagBelegungsTyp)Enum.Parse(typeof(FahrerTagBelegungsTyp), belegungsType);

            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult ImageUpload()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DatePicker()
        {
            return View(ViewModel);
        }

        private const string RootFolder = "~/TmpUpload/Images/";

        [HttpPost]
        public ActionResult UploadImage(IEnumerable<HttpPostedFileBase> files)
        {
            var fileArray = files.ToArray();
            var file = fileArray[0];

            var fileName = Path.Combine(Server.MapPath(RootFolder), file.FileName);
            FileService.TryFileDelete(fileName);
            file.SaveAs(fileName);

            var url = VirtualPathUtility.ToAbsolute(RootFolder) + Path.GetFileName(fileName);

            return Json(new
            {
                files = new object[] { new { url }  }
            }, "text/plain");
        }
    }
}
