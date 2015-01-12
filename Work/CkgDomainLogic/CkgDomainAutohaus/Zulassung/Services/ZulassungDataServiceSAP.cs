using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Autohaus.Models.AppModelMappings;

namespace CkgDomainLogic.Autohaus.Services
{
    public class ZulassungDataServiceSAP : CkgGeneralDataServiceSAP, IZulassungDataService
    {
        public List<Kunde> Kunden { get { return PropertyCacheGet(() => LoadKundenFromSap().ToList()); } }

        public List<Domaenenfestwert> Fahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }

        public List<Material> Zulassungsarten { get { return PropertyCacheGet(() => LoadZulassungsartenFromSap().ToList()); } }

        public List<Zusatzdienstleistung> Zusatzdienstleistungen { get { return PropertyCacheGet(() => LoadZusatzdienstleistungenFromSap().ToList()); } }

        public List<Kennzeichengroesse> Kennzeichengroessen { get { return PropertyCacheGet(() => LoadKennzeichengroessenFromSql().ToList()); } }

        public string PfadAuftragszettel { get; private set; }

        private static ZulassungSqlDbContext CreateDbContext()
        {
            return new ZulassungSqlDbContext();
        }

        public ZulassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            PfadAuftragszettel = GeneralConfiguration.GetConfigValue("KroschkeAutohaus", "PfadAuftragszettel");
        }

        public void MarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Kunden);
            PropertyCacheClear(this, m => m.Fahrzeugarten);
            PropertyCacheClear(this, m => m.Zulassungsarten);
            PropertyCacheClear(this, m => m.Zusatzdienstleistungen);
            PropertyCacheClear(this, m => m.Kennzeichengroessen);
        }

        private IEnumerable<Kunde> LoadKundenFromSap()
        {
            Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.Init(SAP);

            var orgRef = ((ILogonContextDataService) LogonContext).Organization.OrganizationReference;

            SAP.SetImportParameter("I_KUNNR", (string.IsNullOrEmpty(orgRef) ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
            SAP.SetImportParameter("I_VKORG", ((ILogonContextDataService)LogonContext).Customer.AccountingArea.ToString());
            SAP.SetImportParameter("I_VKBUR", ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2);
            SAP.SetImportParameter("I_SPART", "01");

            var sapList = Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_Kunde.Copy(sapList).OrderBy(k => k.Name1);
        }

        public Bankdaten GetBankdaten(string iban)
        {
            var erg = new Bankdaten();

            Z_FI_CONV_IBAN_2_BANK_ACCOUNT.Init(SAP, "I_IBAN", iban);

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                erg.Swift = SAP.GetExportParameter("E_SWIFT");
                erg.KontoNr = SAP.GetExportParameter("E_BANK_ACCOUNT");
                erg.Bankleitzahl = SAP.GetExportParameter("E_BANK_NUMBER");
                erg.Geldinstitut = SAP.GetExportParameter("E_BANKA");
            }           

            return erg;
        }

        private IEnumerable<Domaenenfestwert> LoadFahrzeugartenFromSap()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE, SORTIEREN", "ZZLD_FAHRZ_ART", "DE", "X");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList);
        }

        private IEnumerable<Material> LoadZulassungsartenFromSap()
        {
            Z_ZLD_AH_MATERIAL.Init(SAP, "I_VKBUR", ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2);

            var sapList = Z_ZLD_AH_MATERIAL.GT_MAT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_ZLD_AH_MATERIAL_GT_MAT_To_Material.Copy(sapList);
        }

        public string GetZulassungskreis(Vorgang zulassung)
        {
            Z_ZLD_AH_ZULST_BY_PLZ.Init(SAP, "I_PLZ, I_ORT", zulassung.Halterdaten.PLZ, zulassung.Halterdaten.Ort);

            var sapList = Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST.GetExportListWithExecute(SAP);

            if (SAP.ResultCode == 0 && sapList.Count > 0)
                return sapList[0].ZKFZKZ;

            return "";
        }

        private IEnumerable<Zusatzdienstleistung> LoadZusatzdienstleistungenFromSap()
        {
            Z_DPM_READ_LV_001.Init(SAP, "I_VWAG", "X");

            var kroschkeKunde = (((ILogonContextDataService)LogonContext).Customer.AccountingArea == 1010);

            if (kroschkeKunde)
                SAP.SetImportParameter("I_EKORG", "0001");

            var importListAG = Z_DPM_READ_LV_001.GT_IN_AG.GetImportList(SAP);
            importListAG.Add(new Z_DPM_READ_LV_001.GT_IN_AG { AG = LogonContext.KundenNr.ToSapKunnr() });
            SAP.ApplyImport(importListAG);

            var sort1 = (kroschkeKunde ? "" : "1");

            var importListProcess = Z_DPM_READ_LV_001.GT_IN_PROZESS.GetImportList(SAP);
            importListProcess.Add(new Z_DPM_READ_LV_001.GT_IN_PROZESS { SORT1 = sort1 });
            SAP.ApplyImport(importListProcess);
            
            var sapList = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportListWithExecute(SAP)
                .Where(x => x.ASNUM.IsNotNullOrEmpty() && x.EXTGROUP == "1" && x.KTEXT1_H2.IsNullOrEmpty());

            return AppModelMappings.Z_DPM_READ_LV_001_GT_OUT_DL_To_Zusatzdienstleistung.Copy(sapList).OrderBy(x => x.Name);
        }

        private static IEnumerable<Kennzeichengroesse> LoadKennzeichengroessenFromSql()
        {
            var ct = CreateDbContext();

            return ct.GetKennzeichengroessen();
        }

        private bool GetSapId(Vorgang vorgang)
        {
            Z_ZLD_EXPORT_BELNR.Init(SAP);

            var tmpId = SAP.GetExportParameterWithExecute("E_BELN");

            if (string.IsNullOrEmpty(tmpId) || tmpId.TrimStart('0').Length == 0)
                return false;

            vorgang.BelegNr = tmpId;

            return true;
        }

        public string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart)
        {
            try
            {
                foreach (var vorgang in zulassungen)
                {
                    // Vorgang, Belegnummer für Hauptvorgang (GT_BAK)
                    var blnBelegNrLeer = (string.IsNullOrEmpty(vorgang.BelegNr) || vorgang.BelegNr.TrimStart('0').Length == 0);

                    if (blnBelegNrLeer && !GetSapId(vorgang))
                        return Localize.SaveFailed + ": " + Localize.UnableToRetrieveNewRecordIdFromSap;
                }

                Z_ZLD_AH_IMPORT_ERFASSUNG1.Init(SAP);

                SAP.SetImportParameter("I_TELNR", ((ILogonContextDataService)LogonContext).UserInfo.Telephone);
                SAP.SetImportParameter("I_FESTE_REFERENZEN", "X");

                if (saveDataToSap)
                    SAP.SetImportParameter("I_SPEICHERN", "X");

                SAP.SetImportParameter("I_FORMULAR", "X");
                SAP.SetImportParameter("I_ZUSATZFORMULARE", "X");

                var positionen = new List<Zusatzdienstleistung>();
                var adressen = new List<Adressdaten>();
                foreach (var vorgang in zulassungen)
                {
                    // Vorgang, Zusatzdienstleistungen (GT_POS)
                    positionen.Add(new Zusatzdienstleistung
                    {
                        BelegNr = vorgang.BelegNr,
                        PositionsNr = "10",
                        MaterialNr = vorgang.Zulassungsdaten.ZulassungsartMatNr,
                        Menge = "1"
                    });
                    vorgang.OptionenDienstleistungen.AlleDienstleistungen.ForEach(dl => dl.BelegNr = vorgang.BelegNr);
                    positionen.AddRange(vorgang.OptionenDienstleistungen.GewaehlteDienstleistungen);


                    // Vorgang, Adressen (GT_ADRS)
                    if (!string.IsNullOrEmpty(vorgang.BankAdressdaten.Rechnungsempfaenger.Name1))
                    {
                        vorgang.BankAdressdaten.Rechnungsempfaenger.BelegNr = vorgang.BelegNr;
                        vorgang.BankAdressdaten.Rechnungsempfaenger.Kennung = "RE";
                        adressen.Add(vorgang.BankAdressdaten.Rechnungsempfaenger);
                    }
                    // Halteradresse
                    adressen.Add(new Adressdaten().AdresseToAdressdaten(vorgang.BelegNr, "ZH", vorgang.Halterdaten));
                }

                var bakList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BAK_IN_From_Vorgang.CopyBack(zulassungen).ToList();
                SAP.ApplyImport(bakList);

                var posList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_POS_IN_From_Zusatzdienstleistung.CopyBack(positionen).ToList();
                SAP.ApplyImport(posList);

                if (adressen.Any())
                {
                    var adrsList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_ADRS_IN_From_Adressdaten.CopyBack(adressen).ToList();
                    SAP.ApplyImport(adrsList);
                }

                SAP.Execute();
            }
            catch (Exception e)
            {
                return e.FormatSapSaveException();
            }

            if (SAP.ResultCode != 0)
            {
                var errstring = "";
                var errList = Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_ERROR.GetExportList(SAP);
                if (errList.Count > 0 && errList.Any(e => e.MESSAGE != "OK"))
                    errstring = string.Join(", ", errList.Select(e => e.MESSAGE));

                return string.Format("{0}{1}", SAP.ResultMessage.FormatSapSaveResultMessage(), errstring.FormatIfNotNull(" ({this})")); 
            }

            // alle PDF Formulare abrufen:
            var fileNamesSap = Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_FILENAME.GetExportList(SAP);
            //XmlService.XmlSerializeToFile(fileNames, Path.Combine(AppSettings.DataPath, "GT_FILENAME.xml"));
            
            var fileNames = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_FILENAME_To_PdfFormular.Copy(fileNamesSap).ToListOrEmptyList();
            // alle relativen Pfade zu absoluten Pfaden konvertieren:
            fileNames.ForEach(f =>
                {
                    f.DateiPfad = Path.Combine(PfadAuftragszettel, SlashToBackslash(f.DateiPfad).SubstringTry(1));
                });

            var auftragsListePath = fileNames.FirstOrDefault(f => f.DateiPfad.NotNullOrEmpty().ToLower().Contains("auftragsliste"));
            foreach (var vorgang in zulassungen)
            {
                var bnr = vorgang.BelegNr;
                vorgang.Zusatzformulare.AddRange(fileNames.Where(f => f.Belegnummer == bnr));
                if (auftragsListePath != null)
                    vorgang.Zusatzformulare.Add(auftragsListePath);

                if (!saveFromShoppingCart && vorgang.BankAdressdaten.Cpdkunde)
                {
                    // NICHT Warenkorb (=> immer nur Einzel-Vorgang, also zulassungen.Count == 1)
                    try { vorgang.KundenformularPdf = SAP.GetExportParameterByte("E_PDF"); }
                    catch { vorgang.KundenformularPdf = null; }
                }
            }

            return "";
        }

        static string SlashToBackslash(string s)
        {
            return s.Replace('/', '\\');
        }


        #region Zulassungs Report

        public List<ZulassungsReportModel> GetZulassungsReportItems(ZulassungsReportSelektor selector, Action<string, string> addModelError)
        {
            var iKunnr = selector.KundenNr.ToSapKunnr();
            //var iGroup = ((ILogonContextDataService)LogonContext).Organization.OrganizationName;
            var iVkOrg = ((ILogonContextDataService) LogonContext).Customer.AccountingArea.ToString();
            var iVkBur = ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2;

            try
            {
                Z_ZLD_AH_ZULLISTE.Init(SAP);

                SAP.SetImportParameter("I_KUNNR", iKunnr);
                //SAP.SetImportParameter("I_GRUPPE", iGroup);
                SAP.SetImportParameter("I_VKORG", iVkOrg);
                SAP.SetImportParameter("I_VKBUR", iVkBur);

                SAP.SetImportParameter("I_ZZREFNR1", selector.Referenz1);
                SAP.SetImportParameter("I_ZZREFNR2", selector.Referenz2);
                SAP.SetImportParameter("I_ZZREFNR3", selector.Referenz3);
                SAP.SetImportParameter("I_ZZREFNR4", selector.Referenz4);

                SAP.SetImportParameter("I_ZZKENN", selector.Kennzeichen);
                SAP.SetImportParameter("I_LISTE", selector.AuftragsArt);

                if (selector.ZulassungsDatumRange.IsSelected)
                {
                    SAP.SetImportParameter("I_ZZZLDAT_VON", selector.ZulassungsDatumRange.StartDate);
                    SAP.SetImportParameter("I_ZZZLDAT_BIS", selector.ZulassungsDatumRange.EndDate);
                }
                if (selector.AuftragsDatumRange.IsSelected)
                {
                    SAP.SetImportParameter("I_ERDAT_VON", selector.AuftragsDatumRange.StartDate);
                    SAP.SetImportParameter("I_ERDAT_BIS", selector.AuftragsDatumRange.EndDate);
                }

                SAP.Execute();
            }
            catch (Exception e)
            {
                addModelError("", e.FormatSapSaveException());
            }

            if (SAP.ResultCode != 0)
                addModelError("", SAP.ResultMessage.FormatSapSaveResultMessage());

            var sapItems = Z_ZLD_AH_ZULLISTE.GT_OUT.GetExportList(SAP);
            var webItems = AppModelMappings.Z_ZLD_AH_ZULLISTE_GT_OUT_To_ZulassungsReportModel.Copy(sapItems).ToList();
            return webItems;
        }

        #endregion
    }
}
