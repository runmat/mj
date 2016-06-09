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

            var sapItems = Z_M_BRIEFLEBENSLAUF_001.GT_WEB.GetExportListWithExecute(SAP);
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

            hist.Typdaten = (GetTypdaten(hist.Equipmentnummer) ?? new EquiTypdaten());

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

            var sapItems = Z_DPM_FAHRZEUGHISTORIE_AVM.GT_UEBER.GetExportListWithExecute(SAP);
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

        #region Remarketing

        public List<EquiHistorieRemarketingInfo> GetHistorieRemarketingInfos(EquiHistorieSuchparameter suchparameter)
        {
            Z_DPM_REM_FAHRZEUGHIST_02.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_KENNZ", suchparameter.Kennzeichen.ToUpper());

            if (!string.IsNullOrEmpty(suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_FAHRGNR", suchparameter.FahrgestellNr.ToUpper());

            return AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_DATEN_To_EquiHistorieRemarketingInfo.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }

        public EquiHistorieRemarketing GetHistorieRemarketingDetail(string fin)
        {
            Z_DPM_REM_FAHRZEUGHIST_02.Init(SAP, "I_KUNNR_AG, I_FAHRGNR", LogonContext.KundenNr.ToSapKunnr(), fin);

            SAP.Execute();

            var sapItems = Z_DPM_REM_FAHRZEUGHIST_02.GT_DATEN.GetExportList(SAP);
            if (sapItems.None())
                return new EquiHistorieRemarketing();

            var hist = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_DATEN_To_EquiHistorieRemarketing.Copy(sapItems).ToList().First();

            hist.HistorieInfo = (AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_DATEN_To_EquiHistorieRemarketingInfo.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_DATEN.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiHistorieRemarketingInfo());

            hist.Gutachten = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_GUTA_To_EquiGutachten.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_GUTA.GetExportList(SAP)).ToList();

            hist.Versanddaten = (AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_VERS_To_EquiVersanddaten.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_VERS.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiVersanddaten());

            hist.LebenslaufBrief = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_LEB_B_To_EquiLebenslaufBrief.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_LEB_B.GetExportList(SAP)).ToList();

            hist.LebenslaufSchluessel = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_LEB_T_To_EquiLebenslaufSchluessel.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_LEB_T.GetExportList(SAP)).ToList();

            hist.Adressen = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_ADDR_B_To_SelectItem.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_ADDR_B.GetExportList(SAP)).ToList();

            hist.Belastungsanzeige = (AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_BELAS_To_EquiBelastungsanzeige.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_BELAS.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiBelastungsanzeige());

            hist.Rechnungsdaten = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_RECHNG_To_EquiRechnungsdaten.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_RECHNG.GetExportList(SAP)).ToList();

            hist.Vorschaeden = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_SCHADEN_To_EquiVorschaden.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_SCHADEN.GetExportList(SAP)).ToList();

            hist.Zusatzdaten = (AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_DATEN2_To_EquiZusatzdatenRemarketing.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_DATEN2.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiZusatzdatenRemarketing());

            hist.Ausstattungen = AppModelMappings.Z_DPM_REM_FAHRZEUGHIST_02_GT_AUSST_To_EquiAusstattung.Copy(Z_DPM_REM_FAHRZEUGHIST_02.GT_AUSST.GetExportList(SAP)).ToList();

            hist.Schadenrechnung = (GetSchadenrechnung(fin) ?? new EquiSchadenrechnung());

            hist.Typdaten = (GetTypdaten(hist.Equipmentnummer) ?? new EquiTypdaten());

            hist.Lebenslauf = GetLebenslauf(hist).OrderBy(l => l.Datum).ToList();

            hist.UploaddatumSchadensgutachten = GetUploaddatumSchadensgutachten(fin);

            hist.TuevGutachtenBaseUrl = ApplicationConfiguration.GetApplicationConfigValue("TuevGutachtenUrl", "0", LogonContext.CustomerID);

            return hist;
        }

        private EquiSchadenrechnung GetSchadenrechnung(string fin)
        {
            Z_DPM_REM_SCHADENRG_01.Init(SAP, "I_KUNNR, I_FIN, I_STATU", LogonContext.KundenNr.ToSapKunnr(), fin, "A");

            SAP.Execute();

            return AppModelMappings.Z_DPM_REM_SCHADENRG_01_GT_OUT_To_EquiSchadenrechnung.Copy(Z_DPM_REM_SCHADENRG_01.GT_OUT.GetExportList(SAP)).FirstOrDefault();
        }

        private IEnumerable<EquiLebenslauf> GetLebenslauf(EquiHistorieRemarketing hist)
        {
            var liste = new List<EquiLebenslauf>();

            if (hist.Auslieferdatum.HasValue)
                liste.Add(new EquiLebenslauf(hist.Auslieferdatum.Value, string.Format("{0} {1}",
                    Localize.DeliveryToCarRentalCompany,
                    hist.Vermieter)));

            if (hist.Zulassungsdatum.HasValue)
                liste.Add(new EquiLebenslauf(hist.Zulassungsdatum.Value, string.Format("{0} ({1})",
                    Localize.Registered,
                    hist.HistorieInfo.Kennzeichen)));

            if (hist.HcEingang.HasValue)
                liste.Add(new EquiLebenslauf(hist.HcEingang.Value, string.Format("{0} {1}, {2} {3}",
                    Localize.ReceivedAtTakeInCenter,
                    (string.IsNullOrEmpty(hist.Hereinnahmecenter) ? hist.HcOrt : string.Format("{0} ({1})", hist.HcOrt, hist.Hereinnahmecenter)),
                    Localize.Mileage,
                    hist.KmStand)));

            if (hist.EingangSchluessel.HasValue)
                liste.Add(new EquiLebenslauf(hist.EingangSchluessel.Value, Localize.KeyReceived));

            if (hist.EingangZb2.HasValue)
                liste.Add(new EquiLebenslauf(hist.EingangZb2.Value, Localize.Zb2Received));

            hist.Gutachten.ForEach(guta =>
            {
                if (guta.EingangGutachten.HasValue)
                    liste.Add(new EquiLebenslauf(guta.EingangGutachten.Value, Localize.SurveyReceived));
            });

            if (hist.Zusatzdaten.HcAusgang.HasValue)
                liste.Add(new EquiLebenslauf(hist.Zusatzdaten.HcAusgang.Value, string.Format("{0} {1}",
                    Localize.DispatchedByTakeInCenter,
                    (string.IsNullOrEmpty(hist.Hereinnahmecenter) ? hist.HcOrt : string.Format("{0} ({1})", hist.HcOrt, hist.Hereinnahmecenter)))));

            if (hist.Zusatzdaten.GutachtenAuftragsdatum.HasValue)
                liste.Add(new EquiLebenslauf(hist.Zusatzdaten.GutachtenAuftragsdatum.Value, 
                    string.Format(Localize.SurveyOrderedFromSurveyor, hist.Gutachten.FirstOrDefault(new EquiGutachten()).Gutachter)));

            hist.LebenslaufSchluessel.ForEach(lebT =>
            {
                if (lebT.AusgangSchluessel.HasValue)
                    liste.Add(new EquiLebenslauf(lebT.AusgangSchluessel.Value, Localize.KeyDispatched));
            });

            hist.LebenslaufBrief.ForEach(lebB =>
            {
                if (lebB.AusgangZb2.HasValue)
                    liste.Add(new EquiLebenslauf(lebB.AusgangZb2.Value, Localize.Zb2Dispatched));
            });

            hist.Vorschaeden.ForEach(schad =>
            {
                if (schad.Erstellungsdatum.HasValue && schad.Preis.HasValue)
                    liste.Add(new EquiLebenslauf(schad.Erstellungsdatum.Value, string.Format("{0}, {1}",
                        Localize.PreviousDamageReported,
                        schad.Preis.ToString("c"))));
            });

            if (hist.Belastungsanzeige.DatumSchadenrechnung.HasValue)
                liste.Add(new EquiLebenslauf(hist.Belastungsanzeige.DatumSchadenrechnung.Value, Localize.BillPrinted));

            hist.Rechnungsdaten.ForEach(rech =>
            {
                if (rech.Rechnungsdatum.HasValue)
                {
                    switch (rech.BelegArt)
                    {
                        case "1":
                            liste.Add(new EquiLebenslauf(rech.Rechnungsdatum.Value, Localize.CreditCreated));
                            break;

                        case "2":
                            liste.Add(new EquiLebenslauf(rech.Rechnungsdatum.Value, string.Format("{0} {1}",
                                Localize.AdditionalChargeCreatedTo,
                                rech.Empfaenger)));
                            break;
                    }
                }
            });

            return liste;
        }

        private string GetUploaddatumSchadensgutachten(string fin)
        {
            Z_DPM_REM_SET_SCHADENDAT_PDF.Init(SAP, "I_KUNNR, I_READ", LogonContext.KundenNr.ToSapKunnr(), "X");

            var finList = new List<Z_DPM_REM_SET_SCHADENDAT_PDF.GT_IN> { new Z_DPM_REM_SET_SCHADENDAT_PDF.GT_IN { FIN = fin } };
            SAP.ApplyImport(finList);

            SAP.Execute();

            var ergItem = Z_DPM_REM_SET_SCHADENDAT_PDF.GT_OUT.GetExportList(SAP).FirstOrDefault();

            return (ergItem != null ? ergItem.SDPDF_DAT : "");
        }

        #endregion

        #region Common

        private EquiTypdaten GetTypdaten(string equiNr)
        {
            if (equiNr.IsNullOrEmpty())
                return new EquiTypdaten();

            Z_M_ABEZUFZG_NEU.Init(SAP, "ZZKUNNR, ZZEQUNR", LogonContext.KundenNr.ToSapKunnr(), equiNr.PadLeft0(18));

            return AppModelMappings.Z_M_ABEZUFZG_NEU_E_ABE_DATEN_To_EquiTypdaten.Copy(Z_M_ABEZUFZG_NEU.E_ABE_DATEN.GetExportListWithExecute(SAP)).FirstOrDefault();
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
