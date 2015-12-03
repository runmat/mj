using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Equi.Services
{
    public class EquiHistorieDataServiceSAP : CkgGeneralDataServiceSAP, IEquiHistorieDataService
    {
        public EquiHistorieSuchparameter Suchparameter { get; set; }

        public List<EquiHistorieInfo> HistorieInfos { get { return PropertyCacheGet(() => LoadHistorieInfosFromSap().ToList()); } }

        public EquiHistorieDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new EquiHistorieSuchparameter();
        }

        public void MarkForRefreshHistorieInfos()
        {
            PropertyCacheClear(this, m => m.HistorieInfos);
        }

        private IEnumerable<EquiHistorieInfo> LoadHistorieInfosFromSap()
        {
            SAP.Init("Z_M_BRIEFLEBENSLAUF_001", "I_KUNNR", LogonContext.KundenNr.PadLeft(10, '0'));

            if (!string.IsNullOrEmpty(Suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_ZZKENN", Suchparameter.Kennzeichen.ToUpper());

            if (!string.IsNullOrEmpty(Suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_ZZFAHRG", Suchparameter.FahrgestellNr.ToUpper());

            if (!string.IsNullOrEmpty(Suchparameter.BriefNr))
                SAP.SetImportParameter("I_ZZBRIEF", Suchparameter.BriefNr.ToUpper());

            if (!string.IsNullOrEmpty(Suchparameter.VertragsNr))
                SAP.SetImportParameter("I_ZZREF1", Suchparameter.VertragsNr.ToUpper());

            var sapList = Z_M_BRIEFLEBENSLAUF_001.GT_EQUI.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_EQUI_To_EquiHistorieInfo.Copy(sapList);
        }

        public EquiHistorie GetEquiHistorie(string fahrgestellnummer, int appId)
        {
            EquiHistorie hist = null;

            SAP.Init("Z_M_BRIEFLEBENSLAUF_001", "I_KUNNR", LogonContext.KundenNr.PadLeft(10, '0'));

            SAP.SetImportParameter("I_ZZFAHRG", fahrgestellnummer);

            SAP.Execute();

            // GT_WEB
            var sapItemsHist = Z_M_BRIEFLEBENSLAUF_001.GT_WEB.GetExportList(SAP);
            var webItemsHist = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_WEB_To_EquiHistorie.Copy(sapItemsHist).ToList();

            // GT_QMEL
            var sapItemsMeld = Z_M_BRIEFLEBENSLAUF_001.GT_QMEL.GetExportList(SAP);
            var webItemsMeld = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_QMEL_To_EquiMeldungsdaten.Copy(sapItemsMeld).ToList();

            // GT_QMMA
            var sapItemsAktionen = Z_M_BRIEFLEBENSLAUF_001.GT_QMMA.GetExportList(SAP);
            var webItemsAktionen = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_QMMA_To_EquiAktionsdaten.Copy(sapItemsAktionen).ToList();
            
            // GT_ADDR
            var sapItemsAddr = Z_M_BRIEFLEBENSLAUF_001.GT_ADDR.GetExportList(SAP);
            var webItemsAddr = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_ADDR_To_EquiHaendlerdaten.Copy(sapItemsAddr).ToList();

            // GT_TEXT
            var sapItemsText = Z_M_BRIEFLEBENSLAUF_001.GT_TEXT.GetExportList(SAP);

            if (webItemsHist.Count > 0)
            {
                hist = webItemsHist[0];
                hist.Meldungen = webItemsMeld;
                hist.Aktionen = webItemsAktionen;

                if (webItemsAddr.Count > 0)
                {
                    hist.Haendlerdaten = webItemsAddr[0];
                }
                else
                {
                    hist.Haendlerdaten = new EquiHaendlerdaten();
                }
                hist.Haendlerdaten.HaendlerNr = hist.HaendlerNr;
                hist.Haendlerdaten.Finanzierungsart = hist.Finanzierungsart;
                    
                if (sapItemsText.Count > 0)
                {
                    var texte = from t in sapItemsText
                                select t.TDLINE;

                    hist.Bemerkungen = String.Join(Environment.NewLine, texte);
                }

                using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
                {
                    hist.Versandgrund = dbContext.GetAbrufgrundBezeichnung(hist.VersandgrundId);
                }

                hist.Typdaten = GetTypdaten(hist.Equipmentnummer);

                var custId = (LogonContext as ILogonContextDataService).Customer.CustomerID;
                var grpId = (LogonContext as ILogonContextDataService).Group.GroupID;

                hist.ShowTypdaten = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieTypdatenAnzeigen", appId.ToString(), custId, grpId).ToBool();
                hist.ShowMeldungen = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieLebenslaufAnzeigen", appId.ToString(), custId, grpId).ToBool();
                hist.ShowAktionen = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieUebermittlungAnzeigen", appId.ToString(), custId, grpId).ToBool();
                hist.ShowHaendlerdaten = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieHaendlerdatenAnzeigen", appId.ToString(), custId, grpId).ToBool();
            }

            return hist ?? new EquiHistorie();
        }

        private EquiTypdaten GetTypdaten(string equiNr)
        {
            var erg = new EquiTypdaten();

            SAP.InitExecute("Z_M_ABEZUFZG", "ZZKUNNR, ZZEQUNR", LogonContext.KundenNr.PadLeft(10, '0'), equiNr.PadLeft(18, '0'));

            erg.Abgasrichtlinie = SAP.GetExportParameter("ZZABGASRICHTL_TG");
            erg.AnzahlAchsen = SAP.GetExportParameter("ZZANZACHS").TrimStart('0');
            erg.AnzahlAntriebsachsen = SAP.GetExportParameter("ZZANTRIEBSACHS").TrimStart('0');
            erg.AnzahlSitze = SAP.GetExportParameter("ZZANZSITZE").TrimStart('0');
            erg.Aufbauart = SAP.GetExportParameter("ZZTEXT_AUFBAU");
            erg.Bemerkungen = String.Join(Environment.NewLine,
                SAP.GetExportParameter("ZZBEMER1"),
                SAP.GetExportParameter("ZZBEMER2"),
                SAP.GetExportParameter("ZZBEMER3"),
                SAP.GetExportParameter("ZZBEMER4"),
                SAP.GetExportParameter("ZZBEMER5"),
                SAP.GetExportParameter("ZZBEMER6"),
                SAP.GetExportParameter("ZZBEMER7"),
                SAP.GetExportParameter("ZZBEMER8"),
                SAP.GetExportParameter("ZZBEMER9"),
                SAP.GetExportParameter("ZZBEMER10"),
                SAP.GetExportParameter("ZZBEMER11"),
                SAP.GetExportParameter("ZZBEMER12"),
                SAP.GetExportParameter("ZZBEMER13"),
                SAP.GetExportParameter("ZZBEMER14")).TrimEnd('\r', '\n');
            erg.BereifungAchse1 = SAP.GetExportParameter("ZZBEREIFACHSE1");
            erg.BereifungAchse2 = SAP.GetExportParameter("ZZBEREIFACHSE2");
            erg.BereifungAchse3 = SAP.GetExportParameter("ZZBEREIFACHSE3");
            erg.Breite = SAP.GetExportParameter("ZZBREITEMIN").TrimStart('0');
            erg.Co2Emission = SAP.GetExportParameter("ZZCO2KOMBI");
            erg.Fabrikname = SAP.GetExportParameter("ZZFABRIKNAME");
            erg.Fahrgeraeusch = SAP.GetExportParameter("ZZFAHRGERAEUSCH").TrimStart('0');
            erg.Fahrzeugklasse = SAP.GetExportParameter("ZZFHRZKLASSE_TXT");
            erg.Farbcode = SAP.GetExportParameter("ZZFARBE");
            erg.Farbe = SAP.GetExportParameter("ZFARBE_KLAR");
            erg.FassungsvermoegenTank = SAP.GetExportParameter("ZZFASSVERMOEGEN");
            erg.GenehmigungsNr = SAP.GetExportParameter("ZZGENEHMIGNR");
            erg.Genehmigungsdatum = (DateTime?)(SAP.GetExportParameter("ZZGENEHMIGDAT").ToDateTimeOrNull());
            erg.Handelsname = SAP.GetExportParameter("ZZHANDELSNAME");
            erg.HerstSchluessel = SAP.GetExportParameter("ZZHERSTELLER_SCH");
            erg.Hersteller = SAP.GetExportParameter("ZZHERST_TEXT");
            erg.Hoechstgeschwindigkeit = SAP.GetExportParameter("ZZHOECHSTGESCHW");
            erg.Hoehe = SAP.GetExportParameter("ZZHOEHEMIN").TrimStart('0');
            erg.Hubraum = SAP.GetExportParameter("ZZHUBRAUM").TrimStart('0');
            erg.Kraftstoffart = SAP.GetExportParameter("ZZKRAFTSTOFF_TXT");
            erg.Kraftstoffcode = SAP.GetExportParameter("ZZCODE_KRAFTSTOF");
            erg.Laenge = SAP.GetExportParameter("ZZLAENGEMIN").TrimStart('0');
            erg.Leistung = SAP.GetExportParameter("ZZNENNLEISTUNG").TrimStart('0');
            erg.MaxAchslastAchse1 = SAP.GetExportParameter("ZZACHSL_A1_STA").TrimStart('0');
            erg.MaxAchslastAchse2 = SAP.GetExportParameter("ZZACHSL_A2_STA").TrimStart('0');
            erg.MaxAchslastAchse3 = SAP.GetExportParameter("ZZACHSL_A3_STA").TrimStart('0');
            erg.NationaleEmissionsklasseCode = SAP.GetExportParameter("ZZSLD");
            erg.NationaleEmissionsklasse = SAP.GetExportParameter("ZZNATIONALE_EMIK");
            erg.Standgeraeusch = SAP.GetExportParameter("ZZSTANDGERAEUSCH").TrimStart('0');
            erg.Typ = SAP.GetExportParameter("ZZKLARTEXT_TYP");
            erg.TypSchluessel = SAP.GetExportParameter("ZZTYP_SCHL");
            erg.UmdrehungenProMin = SAP.GetExportParameter("ZZBEIUMDREH").TrimStart('0');
            erg.Variante = SAP.GetExportParameter("ZZVARIANTE");
            erg.Version = SAP.GetExportParameter("ZZVERSION");
            erg.ZulGesamtgewicht = SAP.GetExportParameter("ZZZULGESGEW").TrimStart('0');

            return erg;
        }

        public List<EasyAccessArchiveDefinition> GetArchiveDefinitions()
        {
            var defList = new List<EasyAccessArchiveDefinition>();

            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName))
            {
                var archives = dbContext.GetArchives(LogonContext.User.CustomerID, LogonContext.Group.GroupID).ToList();

                archives.ForEach(a => defList.Add(new EasyAccessArchiveDefinition { Location = a.EasyLagerortName, Name = a.EasyArchivName, IndexName = a.EasyQueryIndexName }));
            }

            return defList;
        } 
    }
}
