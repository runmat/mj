using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.Linq;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class CleverViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IAdressenDataService AdressenDataService
        {
            get { return CacheGet<IAdressenDataService>(); }
        }

        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService ZulassungDataService
        {
            get { return CacheGet<IZulassungDataService>(); }
        }

        [LocalizedDisplay(LocalizeConstants.KroschkeCleverSearch)]
        public string AnfrageText { get; set; }

        public bool AnfrageTextFormatError { get; set; }

        public string AnfrageOption { get; set; }

        [XmlIgnore]
        public List<SelectItem> AnfrageOptionen
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("EVB", Localize.EvbNumber),
                    new SelectItem("FIN", Localize.ChassisNo),
                    new SelectItem("KENNZ", Localize.LicenseNo),
                    new SelectItem("IBAN", Localize.IbanBankDetails)
                };
            }
        }

        [ScriptIgnore]
        public EvbInfo EvbDaten
        {
            get { return PropertyCacheGet(() => new EvbInfo()); }
            private set { PropertyCacheSet(value); }
        }

        [ScriptIgnore]
        public Bankdaten IbanDaten
        {
            get { return PropertyCacheGet(() => new Bankdaten()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<ZulassungsReportModel> ZulassungsReportItems
        {
            get { return PropertyCacheGet(() => new List<ZulassungsReportModel>()); }
            private set { PropertyCacheSet(value); }
        }


        public void DataInit()
        {
            AnfrageText = null;
            AnfrageOption = null;
        }

        public void PerformSearch(string anfrageText, string anfrageTyp, Action<string, string> addModelError)
        {
            AnfrageOption = anfrageTyp;
            AnfrageText = anfrageText.NotNullOrEmpty().Trim();
            AnfrageTextFormatError = false;

            switch (AnfrageOption)
            {
                case "EVB":
                    CheckEvb(AnfrageText.ToUpper().Replace(" ", ""), addModelError);
                    break;

                case "FIN":
                    LoadZulassungsReportItems(AnfrageText.ToUpper().Replace(" ", ""), "", addModelError);
                    break;

                case "KENNZ":
                    LoadZulassungsReportItems("", AnfrageText.ToUpper().Replace(" ", ""), addModelError);
                    break;

                case "IBAN":
                    CheckIban(AnfrageText.ToUpper().Replace(" ", ""), addModelError);
                    break;
            }
        }

        private void LoadZulassungsReportItems(string fahrgestellNr, string kennzeichen, Action<string, string> addModelError)
        {
            ZulassungsReportItems = ZulassungDataService.GetZulassungsReportItems(
                    new ZulassungsReportSelektor { Referenz2 = fahrgestellNr, Kennzeichen = kennzeichen, NurHauptDienstleistungen = true }, ZulassungDataService.Kunden, addModelError);

            if (ZulassungsReportItems.None())
                addModelError("", Localize.NoDataFound);
        }

        public ZulassungsReportModel GetZulassungsReportItem(string belegNr)
        {
            return ZulassungsReportItems.FirstOrDefault(z => z.BelegNummer == belegNr);
        }

        private void CheckEvb(string evb, Action<string, string> addModelError)
        {
            if (evb.Length != 7)
            {
                addModelError("", Localize.EvbNumberLengthMustBe7);
                AnfrageTextFormatError = true;
                EvbDaten = new EvbInfo { EvbNr = evb };
                return;
            }   

            bool searchOk;
            string message;

            EvbDaten = (AdressenDataService.GetEvbVersInfo(evb, out message, out searchOk) ?? new EvbInfo { EvbNr = evb });

            if (!searchOk)
                addModelError("", message);
        }

        private void CheckIban(string iban, Action<string, string> addModelError)
        {
            IbanDaten = ZulassungDataService.GetBankdaten(iban, addModelError);
            IbanDaten.Iban = iban;
        }
    }
}
