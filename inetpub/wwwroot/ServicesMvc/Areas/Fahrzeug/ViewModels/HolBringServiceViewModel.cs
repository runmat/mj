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

                double stunden = 0;
                double minuten = 0;

                DateTime? abholungDt = null;
                if (Abholung.AbholungDatum.HasValue)
                {
                    double.TryParse(Abholung.AbholungUhrzeitStunden, out stunden);  
                    double.TryParse(Abholung.AbholungUhrzeitMinuten, out minuten); 
                    abholungDt = Abholung.AbholungDatum;
                    abholungDt = abholungDt.Value.AddHours(stunden);
                    abholungDt = abholungDt.Value.AddMinutes(minuten);
                }

                DateTime? anlieferungAbholungAbDt = null;
                DateTime? anlieferungAbholungBisDt = null;

                if (Anlieferung.AnlieferungDatum.HasValue)
                {
                    double.TryParse(Anlieferung.AbholungAbUhrzeitStunden, out stunden); 
                    double.TryParse(Anlieferung.AbholungAbUhrzeitMinuten, out minuten); 
                    anlieferungAbholungAbDt = Anlieferung.AnlieferungDatum;
                    anlieferungAbholungAbDt = anlieferungAbholungAbDt.Value.AddHours(stunden);
                    anlieferungAbholungAbDt = anlieferungAbholungAbDt.Value.AddMinutes(minuten);

                    double.TryParse(Anlieferung.AnlieferungBisUhrzeitStunden, out stunden); 
                    double.TryParse(Anlieferung.AnlieferungBisUhrzeitMinuten, out minuten); 
                    anlieferungAbholungBisDt = Anlieferung.AnlieferungDatum;
                    anlieferungAbholungBisDt = anlieferungAbholungBisDt.Value.AddHours(stunden);
                    anlieferungAbholungBisDt = anlieferungAbholungBisDt.Value.AddMinutes(minuten);
                }

                var bapiParameterSet = new BapiParameterSet
                {
                    AbholungAnsprechpartner = Abholung.AbholungAnsprechpartner, 
                    AbholungDateTime = abholungDt, 
                    AbholungHinweis = Abholung.AbholungHinweis, 
                    AbholungKunde = Abholung.AbholungKunde, 
                    AbholungMobilitaetsfahrzeug = abholungMobilitaetsfahrzeug, 
                    AbholungOrt = Abholung.AbholungOrt, 
                    AbholungPlz = Abholung.AbholungPlz, 
                    AbholungStrasseHausNr = Abholung.AbholungStrasseHausNr, 
                    AbholungTel = Abholung.AbholungTel,
                    AnlieferungAbholungAbDt = anlieferungAbholungAbDt,
                    AnlieferungAnlieferungBisDt = anlieferungAbholungBisDt, 
                    AnlieferungAnsprechpartner = Anlieferung.AnlieferungAnsprechpartner,
                    AnlieferungHinweis = Anlieferung.AnlieferungHinweis, 
                    AnlieferungKunde = Anlieferung.AnlieferungKunde, 
                    AnlieferungMobilitaetsfahrzeug = anlieferungMobilitaetsfahrzeug , 
                    AnlieferungOrt = Anlieferung.AnlieferungOrt, 
                    AnlieferungPlz = Anlieferung.AnlieferungPlz,
                    AnlieferungStrasseHausNr = Anlieferung.AnlieferungStrasseHausNr, 
                    AnlieferungTel = Anlieferung.AnlieferungTel, 
                    Ansprechpartner = ansprechpartner, 
                    AnsprechpartnerTel = Auftraggeber.AnsprechpartnerTel, 
                    AuftragerstellerTel = Auftraggeber.AuftragerstellerTel, 
                    Auftragsersteller = Auftraggeber.Auftragsersteller,
                    BetriebName = Auftraggeber.BetriebName, 
                    BetriebStrasse = Auftraggeber.BetriebStrasse, 
                    BetriebHausNr = Auftraggeber.BetriebHausNr,
                    BetriebPLZ = Auftraggeber.BetriebPLZ, 
                    BetriebOrt = Auftraggeber.BetriebOrt, 
                    Fahrzeugart = Auftraggeber.Fahrzeugart, 
                    Kennnzeichen = Auftraggeber.Kennnzeichen,
                    KundeTel = Auftraggeber.KundeTel,
                    Repco = Auftraggeber.Repco, 
                };

                return bapiParameterSet;
            }
        }

        #endregion

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}", AppSettings.UploadFilePathTemp));
        }

        public byte[] GenerateSapPdf(List<BapiParameterSet> bapiParameterSets)
        {
            byte[] pdfGenerated;
            int retCode;
            string retMessage;

            DataService.GenerateSapPdf(bapiParameterSets, out pdfGenerated, out retCode, out retMessage);
            Overview.PdfCreateDt = DateTime.Now;

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

            var nameSaved = fileSaveAction(GetUploadPathTemp(), randomfilename, extension);

            if (string.IsNullOrEmpty(nameSaved))
                return false;

            var tmpFilename = GetUploadPathTemp() + @"\" + nameSaved + extension;

            var bytes = File.ReadAllBytes(tmpFilename);
            Overview.PdfUploaded = bytes;

            return true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;    
        }

        public string SendMailTo()
        {
            var mailReceiver = GetApplicationConfigValueForCustomer("PdfVersandEmailAdresse");
            if (mailReceiver.IsNullOrEmpty())
                return "Empfänger-eMail-Adresse nicht definiert (PdfVersandEmailAdresse). Es wurde keine eMail generiert.";

            var resultMessage = "Auftrag wurde erfolgreich versendet.";

            var mailService = new SmtpMailService(AppSettings);

            var subject = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Auftraggeber.Repco, Auftraggeber.BetriebOrt);
            const string body = "Hol- und BringService";

            var result = mailService.SendMailMain(mailReceiver, subject, body, new[] { Overview.PdfMergedFilename });

            if (result != null)
            {
                resultMessage = "Fehler beim Versenden des Auftrages: " + result;
            }

            return resultMessage;
        }

        public string GetPdfFilename()
        {
            return string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Auftraggeber.Repco, Auftraggeber.BetriebOrt);
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
            
            Overview.PdfMergedFilename = Path.Combine(AppSettings.TempPath, DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Auftraggeber.Repco + extension);

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
