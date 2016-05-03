using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
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
        public EquiHistorieDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Standard

        public List<EquiHistorieInfo> GetHistorieInfos(EquiHistorieSuchparameter suchparameter)
        {
            Z_M_BRIEFLEBENSLAUF_001.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_ZZKENN", suchparameter.Kennzeichen.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_ZZFAHRG", suchparameter.FahrgestellNr.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.BriefNr))
                SAP.SetImportParameter("I_ZZBRIEF", suchparameter.BriefNr.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.VertragsNr))
                SAP.SetImportParameter("I_ZZREF1", suchparameter.VertragsNr.ToUpper());

            return AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_EQUI_To_EquiHistorieInfo.Copy(Z_M_BRIEFLEBENSLAUF_001.GT_EQUI.GetExportListWithExecute(SAP)).ToList();
        }

        public EquiHistorie GetHistorieDetail(string fin, int appId)
        {
            Z_M_BRIEFLEBENSLAUF_001.Init(SAP, "I_KUNNR, I_ZZFAHRG", LogonContext.KundenNr.ToSapKunnr(), fin);

            var sapItems = Z_M_BRIEFLEBENSLAUF_001.GT_WEB.GetExportListWithInitExecute(SAP);
            if (sapItems.None())
                return new EquiHistorie();

            var hist = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_WEB_To_EquiHistorie.Copy(sapItems).ToList().First();

            hist.Meldungen = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_QMEL_To_EquiMeldungsdaten.Copy(Z_M_BRIEFLEBENSLAUF_001.GT_QMEL.GetExportList(SAP)).ToList();

            hist.Aktionen = AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_QMMA_To_EquiAktionsdaten.Copy(Z_M_BRIEFLEBENSLAUF_001.GT_QMMA.GetExportList(SAP)).ToList();

            hist.Haendlerdaten = (AppModelMappings.Z_M_BRIEFLEBENSLAUF_001_GT_ADDR_To_EquiHaendlerdaten.Copy(Z_M_BRIEFLEBENSLAUF_001.GT_ADDR.GetExportList(SAP)).FirstOrDefault() ?? new EquiHaendlerdaten());

            hist.Haendlerdaten.HaendlerNr = hist.HaendlerNr;
            hist.Haendlerdaten.Finanzierungsart = hist.Finanzierungsart;

            var sapItemsText = Z_M_BRIEFLEBENSLAUF_001.GT_TEXT.GetExportList(SAP);

            if (sapItemsText.Any())
            {
                var texte = from t in sapItemsText
                            select t.TDLINE;

                hist.Bemerkungen = string.Join(Environment.NewLine, texte);
            }

            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                hist.Versandgrund = dbContext.GetAbrufgrundBezeichnung(hist.VersandgrundId);
            }

            hist.Typdaten = GetTypdaten(hist.Equipmentnummer);

            var custId = LogonContext.Customer.CustomerID;
            var grpId = LogonContext.Group.GroupID;

            hist.ShowTypdaten = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieTypdatenAnzeigen", appId.ToString(), custId, grpId).ToBool();
            hist.ShowMeldungen = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieLebenslaufAnzeigen", appId.ToString(), custId, grpId).ToBool();
            hist.ShowAktionen = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieUebermittlungAnzeigen", appId.ToString(), custId, grpId).ToBool();
            hist.ShowHaendlerdaten = ApplicationConfiguration.GetApplicationConfigValue("FzgHistorieHaendlerdatenAnzeigen", appId.ToString(), custId, grpId).ToBool();

            return hist;
        }

        #endregion
        
        #region Vermieter

        public List<EquiHistorieVermieterInfo> GetHistorieVermieterInfos(EquiHistorieSuchparameter suchparameter)
        {
            Z_DPM_FAHRZEUGHISTORIE_AVM.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_LICENSE_NUM", suchparameter.Kennzeichen.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_CHASSIS_NUM", suchparameter.FahrgestellNr.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.BriefNr))
                SAP.SetImportParameter("I_TIDNR", suchparameter.BriefNr.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.VertragsNr))
                SAP.SetImportParameter("I_LIZNR", suchparameter.VertragsNr.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.Referenz1))
                SAP.SetImportParameter("I_REFERENZ1", suchparameter.Referenz1);

            return AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EQUIS_To_EquiHistorieVermieterInfo.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS.GetExportListWithExecute(SAP)).ToList();
        }

        public EquiHistorieVermieter GetHistorieVermieterDetail(string fin)
        {
            Z_M_BRIEFLEBENSLAUF_001.Init(SAP, "I_KUNNR_AG, I_CHASSIS_NUM", LogonContext.KundenNr.ToSapKunnr(), fin);

            var sapItems = Z_DPM_FAHRZEUGHISTORIE_AVM.GT_UEBER.GetExportListWithInitExecute(SAP);
            if (sapItems.None())
                return new EquiHistorieVermieter();

            var hist = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_UEBER_To_EquiHistorieVermieter.Copy(sapItems).ToList().First();

            hist.HistorieInfo = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EQUIS_To_EquiHistorieVermieterInfo.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiHistorieVermieterInfo());

            hist.Einsteuerungsdaten = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EINST_To_EquiEinsteuerung.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EINST.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiEinsteuerung());

            hist.Aussteuerungsdaten = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_AUSST_To_EquiAussteuerung.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_AUSST.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiAussteuerung());

            hist.Typdaten = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_TYPEN_To_EquiTypdaten.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TYPEN.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiTypdaten());

            hist.Typdaten.Farbe = hist.Farbe;
            hist.Typdaten.Farbcode = hist.Farbcode;

            hist.LebenslaufZb2 = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_LLZB2_To_EquiMeldungsdaten.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLZB2.GetExportList(SAP)).ToList();

            hist.LebenslaufFsm = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_LLSCH_To_EquiMeldungsdaten.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLSCH.GetExportList(SAP)).ToList();

            hist.InhalteFsm = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_TUETE_To_EquiTueteninhalt.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TUETE.GetExportList(SAP)).ToList();

            return hist;
        }

        public byte[] GetHistorieVermieterAsPdf(string fin)
        {
            Z_DPM_DRUCK_FZG_HISTORIE_AVM.Init(SAP, "I_KUNNR_AG, I_CHASSIS_NUM", LogonContext.KundenNr.ToSapKunnr(), fin);

            SAP.Execute();

            return SAP.GetExportParameterByte("E_PDF");
        }

        #region Fahrzeug Anforderungen

        public List<FahrzeugAnforderung> GetFahrzeugAnforderungen(string fin)
        {
            Z_DPM_AVM_DOKUMENT_KOPIE.Init(SAP, "I_KUNNR_AG, I_CHASSIS_NUM", LogonContext.KundenNr.ToSapKunnr(), fin);

            return AppModelMappings.Z_DPM_AVM_DOKUMENT_KOPIE_GT_WEB_To_FahrzeugAnforderung.Copy(Z_DPM_AVM_DOKUMENT_KOPIE.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public void SaveFahrzeugAnforderung(FahrzeugAnforderung item)
        {
            Z_DPM_AVM_DOKUMENT_KOPIE.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            var sapList = AppModelMappings.Z_DPM_AVM_DOKUMENT_KOPIE_GT_WEB_To_FahrzeugAnforderung.CopyBack(new List<FahrzeugAnforderung> { item }).ToList();
            SAP.ApplyImport(sapList);

            SAP.Execute();
        }

        public List<SelectItem> GetFahrzeugAnforderungenDocTypes()
        {
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), "KOPIE");

            return AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_To_SelectItem.Copy(Z_DPM_READ_AUFTR_006.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }

        #endregion

        #endregion

        #region Common

        private EquiTypdaten GetTypdaten(string equiNr)
        {
            Z_M_ABEZUFZG.Init(SAP, "ZZKUNNR, ZZEQUNR", LogonContext.KundenNr.ToSapKunnr(), equiNr.PadLeft0(18));

            SAP.Execute();

            return new EquiTypdaten
            {
                Abgasrichtlinie = SAP.GetExportParameter("ZZABGASRICHTL_TG"),
                AnzahlAchsen = SAP.GetExportParameter("ZZANZACHS").TrimStart('0'),
                AnzahlAntriebsachsen = SAP.GetExportParameter("ZZANTRIEBSACHS").TrimStart('0'),
                AnzahlSitze = SAP.GetExportParameter("ZZANZSITZE").TrimStart('0'),
                Aufbauart = SAP.GetExportParameter("ZZTEXT_AUFBAU"),
                Bemerkungen = string.Join(Environment.NewLine,
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
                    SAP.GetExportParameter("ZZBEMER14")).TrimEnd('\r', '\n'),
                BereifungAchse1 = SAP.GetExportParameter("ZZBEREIFACHSE1"),
                BereifungAchse2 = SAP.GetExportParameter("ZZBEREIFACHSE2"),
                BereifungAchse3 = SAP.GetExportParameter("ZZBEREIFACHSE3"),
                Breite = SAP.GetExportParameter("ZZBREITEMIN").TrimStart('0'),
                Co2Emission = SAP.GetExportParameter("ZZCO2KOMBI"),
                Fabrikname = SAP.GetExportParameter("ZZFABRIKNAME"),
                Fahrgeraeusch = SAP.GetExportParameter("ZZFAHRGERAEUSCH").TrimStart('0'),
                Fahrzeugklasse = SAP.GetExportParameter("ZZFHRZKLASSE_TXT"),
                Farbcode = SAP.GetExportParameter("ZZFARBE"),
                Farbe = SAP.GetExportParameter("ZFARBE_KLAR"),
                FassungsvermoegenTank = SAP.GetExportParameter("ZZFASSVERMOEGEN"),
                GenehmigungsNr = SAP.GetExportParameter("ZZGENEHMIGNR"),
                Genehmigungsdatum = SAP.GetExportParameter("ZZGENEHMIGDAT").ToNullableDateTime(),
                Handelsname = SAP.GetExportParameter("ZZHANDELSNAME"),
                HerstSchluessel = SAP.GetExportParameter("ZZHERSTELLER_SCH"),
                Hersteller = SAP.GetExportParameter("ZZHERST_TEXT"),
                Hoechstgeschwindigkeit = SAP.GetExportParameter("ZZHOECHSTGESCHW"),
                Hoehe = SAP.GetExportParameter("ZZHOEHEMIN").TrimStart('0'),
                Hubraum = SAP.GetExportParameter("ZZHUBRAUM").TrimStart('0'),
                Kraftstoffart = SAP.GetExportParameter("ZZKRAFTSTOFF_TXT"),
                Kraftstoffcode = SAP.GetExportParameter("ZZCODE_KRAFTSTOF"),
                Laenge = SAP.GetExportParameter("ZZLAENGEMIN").TrimStart('0'),
                Leistung = SAP.GetExportParameter("ZZNENNLEISTUNG").TrimStart('0'),
                MaxAchslastAchse1 = SAP.GetExportParameter("ZZACHSL_A1_STA").TrimStart('0'),
                MaxAchslastAchse2 = SAP.GetExportParameter("ZZACHSL_A2_STA").TrimStart('0'),
                MaxAchslastAchse3 = SAP.GetExportParameter("ZZACHSL_A3_STA").TrimStart('0'),
                NationaleEmissionsklasseCode = SAP.GetExportParameter("ZZSLD"),
                NationaleEmissionsklasse = SAP.GetExportParameter("ZZNATIONALE_EMIK"),
                Standgeraeusch = SAP.GetExportParameter("ZZSTANDGERAEUSCH").TrimStart('0'),
                Typ = SAP.GetExportParameter("ZZKLARTEXT_TYP"),
                TypSchluessel = SAP.GetExportParameter("ZZTYP_SCHL"),
                UmdrehungenProMin = SAP.GetExportParameter("ZZBEIUMDREH").TrimStart('0'),
                Variante = SAP.GetExportParameter("ZZVARIANTE"),
                Version = SAP.GetExportParameter("ZZVERSION"),
                ZulGesamtgewicht = SAP.GetExportParameter("ZZZULGESGEW").TrimStart('0')
            };
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

        #endregion
    }
}
