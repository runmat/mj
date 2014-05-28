using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using SapORM.Contracts;
using SapORM.Models;
using WebTools.Services;
using AppModelMappings = CkgDomainLogic.Finance.Models.AppModelMappings;

namespace CkgDomainLogic.Finance.Services
{
    public class FinanceAktivcheckDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceAktivcheckDataService
    {
        public List<AktivcheckTreffer> Treffer { get { return PropertyCacheGet(() => LoadTrefferFromSap().ToList()); } }

        public List<Domaenenfestwert> Klassifizierungen { get { return PropertyCacheGet(() => LoadKlassifizierungenFromSap().ToList()); } }

        public FinanceAktivcheckDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshTreffer()
        {
            PropertyCacheClear(this, m => m.Treffer);
        }

        private IEnumerable<AktivcheckTreffer> LoadTrefferFromSap()
        {
            var sapList = Z_DPM_AKTIVCHECK_READ.ET_TREFF.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            foreach (var item in sapList)
            {
                var kl = Klassifizierungen.Find(k => k.Wert == item.KLASSIFIZIER);
                if (kl != null)
                {
                    item.Klassifizierungstext = kl.Beschreibung;
                }
            }

            return AppModelMappings.Z_DPM_AKTIVCHECK_READ_ET_TREFF_To_AktivcheckTreffer.Copy(sapList);
        }

        private IEnumerable<Domaenenfestwert> LoadKlassifizierungenFromSap()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE", "ZKLASSIFIZIERUNG", "DE");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList);
        }

        public void SaveVorgang(AktivcheckTreffer vorgang)
        {
            var vorgaenge = new List<AktivcheckTreffer>{ vorgang };

            Z_DPM_AKTIVCHECK_CHANGE.Init(SAP);

            var vgList = AppModelMappings.Z_DPM_AKTIVCHECK_CHANGE_IT_TREFF_From_AktivcheckTreffer.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(vgList);

            SAP.Execute();
        }

        public bool SendRequestMail(AktivcheckTreffer vorgang)
        {
            bool erg = false;

            try
            {
                var adressenDataService = new AdressenDataServiceSAP(SAP);
                adressenDataService.Init(AppSettings, LogonContext);
                var mailAdressen = adressenDataService.LoadFromSap(null, "MAIL_AKTIVCHECK", false);

                if ((mailAdressen != null) && (mailAdressen.Count > 0))
                {
                    var mailAdresse = mailAdressen.First();
                    var kundenName = ((ILogonContextDataService) LogonContext).CustomerName;
                    var kundenNummer = ((ILogonContextDataService) LogonContext).Customer.KUNNR;
                    var mailBetreff = "Aktivcheck - " + kundenName + " - Kontaktanfrage";

                    var mailService = new SmtpMailService(AppSettings);

                    string mailText = "Aktivcheck-Treffer<br/>";
                    mailText += "<br/>";
                    mailText += "Kunde: " + kundenName + " " + kundenNummer + "<br/>";
                    mailText += "<br/>";
                    mailText += "Anfrage zum Herstellen des Kontaktes zwischen den beteiligten Kunden für Klärfall Nr. " + vorgang.VorgangsID + ":<br/>";
                    mailText += "<br/>";
                    mailText += "FIN: " + vorgang.Fahrgestellnummer + "<br/>";
                    mailText += "ZBII: " + vorgang.ZB2 + "<br/>";
                    mailText += "Vertragsnr.: " + vorgang.Vertragsnummer + "<br/>";
                    mailText += "Erstellt: " + vorgang.Erstelldatum.ToShortDateString() + "<br/>";
                    mailText += "Zuletzt geprüft: " + vorgang.Pruefdatum.ToShortDateString() + "<br/>";
                    mailText += "Klassifizierung: " + vorgang.Klassifizierung + "<br/>";
                    mailText += "Bemerkung: " + vorgang.Bemerkung + "<br/>";

                    erg = mailService.SendMail(mailAdresse.Email, mailBetreff, mailText);
                }
            }
            catch (Exception)
            {
                erg = false;
            }

            return erg;
        }
    }
}
