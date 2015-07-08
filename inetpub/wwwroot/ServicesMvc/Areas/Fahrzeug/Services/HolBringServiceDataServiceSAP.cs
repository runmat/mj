// ReSharper disable RedundantUsingDirective
// ReSharper disable AccessToForEachVariableInClosure

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class HolBringServiceDataServiceSAP : CkgGeneralDataServiceSAP, IHolBringServiceDataService
    {
        public List<Domaenenfestwert> GetFahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }
        public List<Domaenenfestwert> GetAnsprechpartner { get { return PropertyCacheGet(() => LoadAnsprechpartnerList().ToList()); } }

        public List<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB> Kunden { get { return PropertyCacheGet(() => LoadKundenFromSap().ToList()); } }

        public string GetUsername { get { return (LogonContext).User.Username; } }
        public string GetUserTel { get { return (LogonContext).UserInfo.Telephone2; } }

        public IOrderedEnumerable<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB> LoadKundenFromSap()
        {
            Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.Init(SAP);

            var orgRef = (LogonContext).Organization.OrganizationReference;

            SAP.SetImportParameter("I_KUNNR", (string.IsNullOrEmpty(orgRef) ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
            SAP.SetImportParameter("I_VKORG", (LogonContext).Customer.AccountingArea.ToString());
            SAP.SetImportParameter("I_VKBUR", (LogonContext).Organization.OrganizationReference2);
            SAP.SetImportParameter("I_SPART", "01");

            var sapList = Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB.GetExportListWithExecute(SAP).OrderBy(x => x.NAME1);

            return sapList;
        }

        // public IOrderedEnumerable<Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN> GenerateSapPdf(BapiParameterSet bapiParameterSet, out byte[] pdfGenerated)
        public List<Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN> GenerateSapPdf(List<BapiParameterSet> bapiParameterSets, out byte[] pdfGenerated)
        {

            var errorMessage = SAP.ExecuteAndCatchErrors(

                    // exception safe SAP action:
                    () =>
                    {
                        var list = Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN.GetImportList(SAP);

                        foreach (var f in bapiParameterSets)
                            list.Add(new Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN 
                            {
                                ABHOLUNGANSPRECHPARTNER = f.AbholungAnsprechpartner,
                                ABHOLUNGDATETIME = f.AbholungDateTime.ToString("dd.MM.yyyy"),
                                ABHOLUNGHINWEIS = f.AbholungHinweis,
                                ABHOLUNGKUNDE = f.AbholungKunde,
                                ABHOLUNGMOBILITAETSFAHRZEUG = f.AbholungMobilitaetsfahrzeug.ToString(),
                                ABHOLUNGORT = f.AbholungOrt,
                                ABHOLUNGPLZ = f.AbholungPlz,
                                ABHOLUNGSTRASSEHAUSNR = f.AbholungStrasseHausNr,
                                ABHOLUNGTEL = f.AbholungTel,
                                ANLIEFERUNGABHOLUNGABDT = f.AnlieferungAbholungAbDt.ToString("dd.MM.yyyy"),
                                ANLIEFERUNGANLIEFERUNGBISDT = f.AnlieferungAnlieferungBisDt.ToString("dd.MM.yyyy"),
                                ANLIEFERUNGANSPRECHPARTNER = f.AnlieferungAnsprechpartner,
                                ANLIEFERUNGHINWEIS = f.AnlieferungHinweis,
                                ANLIEFERUNGKUNDE = f.AnlieferungKunde,
                                ANLIEFERUNGMOBILITAETSFAHRZEUG = f.AnlieferungMobilitaetsfahrzeug.ToString(),
                                ANLIEFERUNGORT = f.AnlieferungOrt,
                                ANLIEFERUNGPLZ = f.AnlieferungPlz,
                                ANLIEFERUNGSTRASSEHAUSNR = f.AnlieferungStrasseHausNr,
                                ANLIEFERUNGTEL = f.AnlieferungTel,
                                ANSPRECHPARTNER = f.Ansprechpartner,
                                ANSPRECHPARTNERTEL = f.AnsprechpartnerTel,
                                AUFTRAGERSTELLERTEL = f.AuftragerstellerTel,
                                AUFTRAGSERSTELLER = f.Auftragsersteller,
                                BETRIEBHAUSNR = f.BetriebHausNr,
                                BETRIEBNAME = f.BetriebName,
                                BETRIEBORT = f.BetriebOrt,
                                BETRIEBPLZ = f.BetriebPLZ,
                                BETRIEBSTRASSE = f.BetriebStrasse,
                                FAHRZEUGART = f.Fahrzeugart,
                                KENNNZEICHEN = f.Kennnzeichen,
                                KUNDETEL = f.KundeTel,
                                REPCO = f.Repco
                            });

                        Z_ZLD_AH_2015_HOLUNDBRING_PDF.Init(SAP);
                        SAP.ApplyImport(list);
                        SAP.Execute();

                        var retCode = SAP.GetExportParameter("E_SUBRC");
                        var retMessage = SAP.GetExportParameter("E_MESSAGE");
                    },

                    // SAP custom error handling:
                    () =>
                    {
                        var sapResult = SAP.ResultMessage;
                        if (SAP.ResultMessage.IsNotNullOrEmpty())
                            return sapResult;

                        return "";
                    });

            try { pdfGenerated = SAP.GetExportParameterByte("E_PDF"); }
            catch { pdfGenerated = null; }
            
            var sapList = Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN.GetExportListWithExecute(SAP);

            var pdf2 = SAP.GetExportParameterByte("E_PDF");

            return sapList;
        }

        public HolBringServiceDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        private IEnumerable<Domaenenfestwert> LoadFahrzeugartenFromSap()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE, SORTIEREN", "ZZLD_FAHRZ_ART", "DE", "X");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList);
        }

        public IEnumerable<Domaenenfestwert> LoadAnsprechpartnerList()
        {
            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                var sql = string.Format("SELECT WebUser.LastName + ', ' + WebUser.FirstName AS Beschreibung, WebUser.LastName + ', ' + WebUser.FirstName + '|' + WebUserInfo.telephone2 AS Wert FROM WebUser INNER JOIN WebMember ON WebUser.UserID = WebMember.UserID INNER JOIN WebGroup " +
                          "ON WebMember.GroupID = WebGroup.GroupID INNER JOIN WebUserInfo ON WebUser.UserID = WebUserInfo.id_user " +
                          "WHERE (dbo.WebUser.CustomerID = {0}) AND (dbo.WebGroup.GroupName = '{1}')", (LogonContext).User.CustomerID, (LogonContext).GroupName);

                var result = dbContext.Database.SqlQuery<Domaenenfestwert>(sql).OrderBy(x => x.Beschreibung).ToList();
                return result;
            }
        }

        
    }
}
