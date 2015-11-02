using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.DataKonverter.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Database.Models;
using DocumentTools.Services;
using GeneralTools.Contracts;
using System.Data;

namespace ServicesMvc.DataKonverter.Controllers
{
    public class AdminController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<KroschkeDataKonverterViewModel>(); } }

        public KroschkeDataKonverterViewModel ViewModel
        {
            get { return GetViewModel<KroschkeDataKonverterViewModel>(); }
            set { SetViewModel(value); }
        }

        public AdminController(IAppSettings appSettings, ILogonContextDataService logonContext, IDataKonverterDataService dataKonverterDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModelExpicit(ViewModel, appSettings, logonContext, dataKonverterDataService);
        }

        private void InitViewModelExpicit(KroschkeDataKonverterViewModel vm, IAppSettings appSettings, ILogonContextDataService logonContext, IDataKonverterDataService dataKonverterDataService)
        {
            InitViewModel(vm, appSettings, logonContext, dataKonverterDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            //CkgDomainLogic.Autohaus.Models.Zulassungsdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            //CkgDomainLogic.Autohaus.Models.Fahrzeugdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
        }

        [CkgApplication]
        public ActionResult Index()
        {
            // return View("Partial/Konfiguration");
            // return View("Test");
            // var engine = new FileHelperEngine(typeof(Customer));
//             var engine = new DocumentTools.Services. FileHelperEngine(typeof(Customer));
            // var test = DocumentTools.Services.ExcelDocumentFactory
            // var dataTable = ExcelDocumentFactory.ReadToDataTable(@"C:\tmp\Testfile.xlsx", true, "s", true);

            // var dataTable = DocumentTools.Services.ExcelDocumentFactory.ReadToDataTable((@"C:\tmp\Testfile.xlsx", true, "s", true);
            // var dataTable = DocumentTools.Services.ExcelDocumentFactory.ReadToDataTableWithFirstRowAsPropertyMapping<>(@"C:\tmp\Testfile.xlsx", "");

            //var list = new ExcelDocumentFactory().ReadToDataTable(@"C:\tmp\Testfile.xlsx", "", CreateFromDataRowWithHeaderAndContentInSeparateColumns<FehlteilEtikett>).ToList();
            //if (dataTable.Columns.Count == 0)
            //    return new List<T>();
            //var rowToStart = headerRowAvailable ? 0 : -1;
            //return dataTable.AsEnumerable()
            //            .Where(row => dataTable.Rows.IndexOf(row) > rowToStart)
            //    // skip the first row, we asume it's the header row
            //            .Select(row => createFromDataRow != null ? createFromDataRow(row) : AutoCreateFromDataRow<T>(row, commaSeparatedAutoPropertyNamesToIgnore));

            // var asdf = DocumentTools.Services.

            //var tmpFileName = Path.Combine(tempFolder, resourceName);
            //var convert = DocumentTools.Services.SpireXlsFactory.ConvertExcelToCsv(@"C:\tmp\Testfile.xlsx", @"C:\tmp\Testfile.txt");

            // var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DataKonverter\\";

            // var tempFolder = ViewModel.GetUploadPathTemp(); //  HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));

            // var convert = DocumentTools.Services.SpireXlsFactory.ConvertExcelToCsv(@"C:\tmp\Testfile.xlsx", @"C:\tmp\Testfile.txt");
            
            var tmpDestFile = ViewModel.ConvertExcelToCsv("Testfile.xlsx", "Testfile.csv");

            var test = ViewModel.DataKonverterDataService.GetSourceFile(tmpDestFile, true, ";");

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Prozessauswahl()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Konfiguration()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Testimport()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Abschluss()
        {
            return View();
        }


        #region Ajax

        [HttpPost]
        public JsonResult LiveTransform(string input, string func)
        {
            var output = "";
            output = string.Format("#{0}", input);

            return Json(new { Output = output });
        }

        #endregion

    }
}