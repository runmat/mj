using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models.HolBringService;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;
using WebTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class HolBringServiceViewModel : CkgBaseViewModel
    {
        public IHolBringServiceDataService DataService { get { return CacheGet<IHolBringServiceDataService>(); } }

        #region Models der einzelnen Partials
        public Auftraggeber Auftraggeber            // public Auftraggeber Auftraggeber { get; set; }
        {
            get { return PropertyCacheGet(() => Auftraggeber); }
            set { PropertyCacheSet(value); }
        }

        public Abholung Abholung                    // public Abholung Abholung { get; set; }
        {
            get { return PropertyCacheGet(() => Abholung); }
            set { PropertyCacheSet(value); }
        }

        public Anlieferung Anlieferung              // public Anlieferung Anlieferung { get; set; }
        {
            get { return PropertyCacheGet(() => Anlieferung); }
            set { PropertyCacheSet(value); }
        }

        public Upload Upload                        // public Upload Upload { get; set; }
        {
            get { return PropertyCacheGet(() => Upload); }
            set { PropertyCacheSet(value); }
        }

        public Overview Overview { get; set; }

        public Mail SendMail { get; set; }

        #endregion

        public GlobalViewData GlobalViewData;   // Model für Nutzung in allen Partials

        #region Wizard
        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() => new Dictionary<string, string>
                {
                    { "Auftraggeber", Localize.Client },
                    { "Abholung", Localize.Pickup },
                    { "Anlieferung", Localize.DeliveryHolBringService },
                    { "Upload", Localize.Upload },
                    { "Overview", Localize.Overview },
                    { "SendMail", Localize.Ready + " !" },
                });
            }
        }

        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        public string FirstStepPartialViewName
        {
            get { return string.Format("{0}", StepKeys[0]); }
        }

        public BapiParameterSet GetBapiParameterSets
        {
            get
            {
                var ansprechpartner = Auftraggeber.Ansprechpartner.Replace("|" + Auftraggeber.AnsprechpartnerTel, "");
                var abholungMobilitaetsfahrzeug = Abholung.AbholungMobilitaetsfahrzeug == true ? "X" : "";
                var anlieferungMobilitaetsfahrzeug = Anlieferung.AnlieferungMobilitaetsfahrzeug == true ? "X" : "";

                DateTime? abholungDt = null;
                if (Abholung.AbholungDatum.HasValue)
                {
                    abholungDt = Abholung.AbholungDatum;
                    abholungDt = abholungDt.Value.AddHours(Convert.ToDouble(Abholung.AbholungUhrzeitStunden));
                    abholungDt = abholungDt.Value.AddMinutes(Convert.ToDouble(Abholung.AbholungUhrzeitMinuten));
                }

                DateTime? anlieferungAbholungAbDt = null;
                DateTime? anlieferungAbholungBisDt = null;

                if (Anlieferung.AnlieferungDatum.HasValue)
                {
                    anlieferungAbholungAbDt = Anlieferung.AnlieferungDatum;
                    anlieferungAbholungAbDt = anlieferungAbholungAbDt.Value.AddHours(Convert.ToDouble(Anlieferung.AbholungAbUhrzeitStunden));
                    anlieferungAbholungAbDt = anlieferungAbholungAbDt.Value.AddMinutes(Convert.ToDouble(Anlieferung.AbholungAbUhrzeitMinuten));
                
                    anlieferungAbholungBisDt = Anlieferung.AnlieferungDatum;
                    anlieferungAbholungBisDt = anlieferungAbholungBisDt.Value.AddHours(Convert.ToDouble(Anlieferung.AnlieferungBisUhrzeitStunden));
                    anlieferungAbholungBisDt = anlieferungAbholungBisDt.Value.AddMinutes(Convert.ToDouble(Anlieferung.AnlieferungBisUhrzeitMinuten));
                }

                var bapiParameterSet = new BapiParameterSet
                {
                    AbholungAnsprechpartner = Abholung.AbholungAnsprechpartner, //  "AbholungAnsprechpartner",
                    AbholungDateTime = abholungDt, // Abholung.AbholungDatum, //  new DateTime(2015, 7, 1),
                    AbholungHinweis = Abholung.AbholungHinweis, //  "AbholungHinweis",
                    AbholungKunde = Abholung.AbholungKunde, //  "AbholungKunde",
                    AbholungMobilitaetsfahrzeug = abholungMobilitaetsfahrzeug, 
                    AbholungOrt = Abholung.AbholungOrt, //  "AbholungOrt",
                    AbholungPlz = Abholung.AbholungPlz, 
                    AbholungStrasseHausNr = Abholung.AbholungStrasseHausNr, //  "AbholungStrasseHausNr",
                    AbholungTel = Abholung.AbholungTel, //  "AbholungTel",
                    AnlieferungAbholungAbDt = anlieferungAbholungAbDt, //  new DateTime(2015, 7, 2, 10, 15, 20), // Anlieferung.AnlieferungDatum, //  new DateTime(2015, 7, 2, 10, 0, 0),
                    AnlieferungAnlieferungBisDt = anlieferungAbholungBisDt, // Anlieferung.AnlieferungDatum, //  new DateTime(2015, 7, 2, 15, 15, 0),
                    AnlieferungAnsprechpartner = Anlieferung.AnlieferungAnsprechpartner, //  "AnlieferungAnsprechpartner",
                    AnlieferungHinweis = Anlieferung.AnlieferungHinweis, //  "AnlieferungHinweis",
                    AnlieferungKunde = Anlieferung.AnlieferungKunde, //  "AnlieferungKunde",
                    AnlieferungMobilitaetsfahrzeug = anlieferungMobilitaetsfahrzeug , //  "X" = true
                    AnlieferungOrt = Anlieferung.AnlieferungOrt, //  "AnlieferungOrt",
                    AnlieferungPlz = Anlieferung.AnlieferungPlz, //  "22222",
                    AnlieferungStrasseHausNr = Anlieferung.AnlieferungStrasseHausNr, //  "AnlieferungStrasseHausNr",
                    AnlieferungTel = Anlieferung.AnlieferungTel, // "AnlieferungTel",
                    Ansprechpartner = ansprechpartner, // Auftraggeber.Ansprechpartner, //  "Ansprechpartner",
                    AnsprechpartnerTel = Auftraggeber.AnsprechpartnerTel, 
                    AuftragerstellerTel = Auftraggeber.AuftragerstellerTel, 
                    Auftragsersteller = Auftraggeber.Auftragsersteller,
                    BetriebName = Auftraggeber.BetriebName, 
                    BetriebStrasse = Auftraggeber.BetriebStrasse, //  "BetriebStraße",
                    BetriebHausNr = Auftraggeber.BetriebHausNr, // "BetriebHausNr",
                    BetriebPLZ = Auftraggeber.BetriebPLZ, // "BetriebPLZ",
                    BetriebOrt = Auftraggeber.BetriebOrt, // "BetriebOrt",
                    Fahrzeugart = Auftraggeber.Fahrzeugart, // Auftraggeber.FahrzeugartId.ToString(), //   "Fahrzeugart",
                    Kennnzeichen = Auftraggeber.Kennnzeichen, //   "Kennzeichen",
                    KundeTel = Auftraggeber.KundeTel, //   "KundeTel",
                    Repco = Auftraggeber.Repco, //   "Repco"
                };

                return bapiParameterSet;
            }
        }

        #endregion

        public byte[] GenerateSapPdf(List<BapiParameterSet> bapiParameterSets)
        {
            byte[] pdfGenerated;
            int retCode;
            string retMessage;

            DataService.GenerateSapPdf(bapiParameterSets, out pdfGenerated, out retCode, out retMessage);

            return pdfGenerated;
        }

        public void DataInit()
        {
            #region Globale Properties, nutzbar in allen Partials
            GlobalViewData = new GlobalViewData
                {
                    BetriebeSap = DataService.LoadKundenFromSap(),

                    Fahrzeugarten = DataService.GetFahrzeugarten,
                    FeiertageAsString =  DateService.FeiertageAsString,
                    AnsprechpartnerList = DataService.GetAnsprechpartner
                };

            #endregion

            Auftraggeber = new Auftraggeber
                {
                    Auftragsersteller = DataService.GetUsername,
                    AuftragerstellerTel = DataService.GetUserTel,
                };

            Abholung = new Abholung();
            Anlieferung = new Anlieferung();
            Upload = new Upload();
            Overview = new Overview();
            SendMail = new Mail();

            DataMarkForRefresh();
        }

        public void CopyDefaultValuesToAnlieferung(Abholung model)
        {
            if (!string.IsNullOrEmpty(model.AbholungStrasseHausNr) && string.IsNullOrEmpty(Anlieferung.AnlieferungStrasseHausNr))
                Anlieferung.AnlieferungStrasseHausNr = model.AbholungStrasseHausNr;

            if (!string.IsNullOrEmpty(model.AbholungPlz) && string.IsNullOrEmpty(Anlieferung.AnlieferungPlz))
                Anlieferung.AnlieferungPlz = model.AbholungPlz;

            if (!string.IsNullOrEmpty(model.AbholungOrt) && string.IsNullOrEmpty(Anlieferung.AnlieferungOrt))
                Anlieferung.AnlieferungOrt = model.AbholungOrt;

            if (!string.IsNullOrEmpty(model.AbholungAnsprechpartner) && string.IsNullOrEmpty(Anlieferung.AnlieferungAnsprechpartner))
                Anlieferung.AnlieferungAnsprechpartner = model.AbholungAnsprechpartner;

            if (!string.IsNullOrEmpty(model.AbholungStrasseHausNr) && string.IsNullOrEmpty(Anlieferung.AnlieferungStrasseHausNr))
                Anlieferung.AnlieferungStrasseHausNr = model.AbholungStrasseHausNr;

            if (!string.IsNullOrEmpty(model.AbholungTel) && string.IsNullOrEmpty(Anlieferung.AnlieferungTel))
                Anlieferung.AnlieferungTel = model.AbholungTel;
        }

        public void DataMarkForRefresh()
        {
        }

        public bool PdfUploadFileSave(string fileName, Func<string, string, string, string> fileSaveAction)
        {
            const string extension = ".pdf"; 

            Upload.UploadFileName = fileName;
            var randomfilename = Guid.NewGuid().ToString();
            
            Upload.UploadServerFileName = Path.Combine(AppSettings.TempPath, randomfilename + extension);

            var nameSaved = fileSaveAction(AppSettings.TempPath, randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var bytes = File.ReadAllBytes(AppSettings.TempPath + @"\" + nameSaved + extension);
            Upload.PdfBytes = bytes;
            Overview.PdfUploaded = bytes;

            return true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;    
        }

        public bool SendMailTo()
        {
            var mailService = new SmtpMailService(AppSettings);

            var result = mailService.SendMail(SendMail.MailReceiver, Auftraggeber.Repco, "Hol- und BringService", new[] { Overview.PdfMergedFilename });

            return result;
        }

        public void MergePdf()
        {
            const string extension = ".pdf"; 

            var pdfOutput = Overview.PdfGenerated;

            if (Overview.PdfUploaded != null)
            {
                var docList = new List<byte[]>
                {
                    Overview.PdfGenerated, Overview.PdfUploaded
                };

                pdfOutput = PdfDocumentFactory.MergePdfDocuments(docList);
                Overview.PdfMerged = pdfOutput;
            }
            
            Overview.PdfMergedFilename = Path.Combine(AppSettings.TempPath, Auftraggeber.Repco + "_" + Guid.NewGuid().ToString() + extension);

            File.WriteAllBytes(Overview.PdfMergedFilename, pdfOutput);
            
        }

        public void SetBetriebAddress()
        {
            var betriebAddress = GlobalViewData.BetriebeSap.FirstOrDefault(x => x.KUNNR == Auftraggeber.Betrieb);
            if (betriebAddress == null) return;
            Auftraggeber.BetriebName = betriebAddress.NAME1;
            Auftraggeber.BetriebStrasse = betriebAddress.STREET;
            Auftraggeber.BetriebHausNr = betriebAddress.HOUSE_NUM1;
            Auftraggeber.BetriebPLZ = betriebAddress.POST_CODE1;
            Auftraggeber.BetriebOrt = betriebAddress.CITY1;
        }
    }
}
