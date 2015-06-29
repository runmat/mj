using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Autohaus.Models.AppModelMappings;

namespace CkgDomainLogic.Autohaus.Services
{
    public class ZulassungDataServiceSAP : CkgGeneralDataServiceSAP, IZulassungDataService
    {
        public List<Kunde> Kunden { get { return PropertyCacheGet(() => LoadKundenFromSap().ToList()); } }

        public List<Domaenenfestwert> Fahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }

        public List<Material> Zulassungsarten { get { return PropertyCacheGet(() => LoadZulassungsAbmeldeArtenFromSap().Where(m => !m.IstAbmeldung).ToList()); } }

        public List<Material> Abmeldearten { get { return PropertyCacheGet(() => LoadZulassungsAbmeldeArtenFromSap().Where(m => m.IstAbmeldung).ToList()); } }

        public List<Zusatzdienstleistung> Zusatzdienstleistungen { get { return PropertyCacheGet(() => LoadZusatzdienstleistungenFromSap().ToList()); } }

        public List<Kennzeichengroesse> Kennzeichengroessen { get { return PropertyCacheGet(() => LoadKennzeichengroessenFromSql().ToList()); } }

        public List<Zulassungskreis> Zulassungskreise { get { return PropertyCacheGet(() => LoadZulassungskreiseFromSap().ToList()); } }

        public List<Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST> ZulassungskreisKennzeichen { get { return PropertyCacheGet(() => LoadZulassungskreisKennzeichenFromSap().ToList()); } }
        

        private static ZulassungSqlDbContext CreateDbContext()
        {
            return new ZulassungSqlDbContext();
        }

        public ZulassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Kunden);
            PropertyCacheClear(this, m => m.Fahrzeugarten);
            PropertyCacheClear(this, m => m.Zulassungsarten);
            PropertyCacheClear(this, m => m.Abmeldearten);
            PropertyCacheClear(this, m => m.Zusatzdienstleistungen);
            PropertyCacheClear(this, m => m.Kennzeichengroessen);
            PropertyCacheClear(this, m => m.Zulassungskreise);
            PropertyCacheClear(this, m => m.ZulassungskreisKennzeichen);
        }

        private IEnumerable<Kunde> LoadKundenFromSap()
        {
            Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.Init(SAP);

            var orgRef = ((ILogonContextDataService) LogonContext).Organization.OrganizationReference;

            SAP.SetImportParameter("I_KUNNR", (orgRef.IsNullOrEmpty() ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
            SAP.SetImportParameter("I_VKORG", ((ILogonContextDataService)LogonContext).Customer.AccountingArea.ToString());
            SAP.SetImportParameter("I_VKBUR", ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2);
            SAP.SetImportParameter("I_SPART", "01");

            var sapList = Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_Kunde.Copy(sapList).OrderBy(k => k.Adresse.Name1);
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

        private IEnumerable<Material> LoadZulassungsAbmeldeArtenFromSap()
        {
            Z_ZLD_AH_MATERIAL.Init(SAP, "I_VKBUR", ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2);

            var sapList = Z_ZLD_AH_MATERIAL.GT_MAT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_ZLD_AH_MATERIAL_GT_MAT_To_Material.Copy(sapList);
        }

        static string GetKreis(Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST sapItem)
        {
            return (sapItem.KREISKZ.IsNotNullOrEmpty() ? sapItem.KREISKZ : sapItem.ZKFZKZ).ToUpper();
        }

        public void GetZulassungskreisUndKennzeichen(Vorgang zulassung, out string kreis, out string kennzeichen)
        {
            kreis = "";
            kennzeichen = "";
            if (zulassung == null || zulassung.Halter == null)
                return;

            Z_ZLD_AH_ZULST_BY_PLZ.Init(SAP, "I_PLZ, I_ORT", zulassung.Halter.Adresse.PLZ, zulassung.Halter.Adresse.Ort);

            var sapList = Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST.GetExportListWithExecute(SAP);

            kreis = kennzeichen = "";
            if (SAP.ResultCode == 0 && sapList.Count > 0)
            {
                var sapItem = sapList[0];
                kreis = GetKreis(sapItem);
                kennzeichen = sapItem.ZKFZKZ;
            }
        }

        public void GetZulassungsKennzeichen(string kreis, out string kennzeichen)
        {
            var sapList = ZulassungskreisKennzeichen;

            kennzeichen = "";
            var sapItem = sapList.FirstOrDefault(s => GetKreis(s) == kreis.NotNullOrEmpty().ToUpper());
            if (sapItem != null)
                kennzeichen = sapItem.ZKFZKZ;
        }

        public Adresse GetLieferantZuKreis(string kreis)
        {
            Z_ZLD_EXPORT_INFOPOOL.Init(SAP, "I_KREISKZ", kreis);

            return AppModelMappings.Z_ZLD_EXPORT_INFOPOOL_GT_EX_ZUSTLIEF_To_Adresse.Copy(Z_ZLD_EXPORT_INFOPOOL.GT_EX_ZUSTLIEF.GetExportListWithExecute(SAP)).FirstOrDefault();
        }

        private IEnumerable<Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST> LoadZulassungskreisKennzeichenFromSap()
        {
            return Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST.GetExportListWithInitExecute(SAP);
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

            if (tmpId.IsNullOrEmpty() || tmpId.TrimStart('0').Length == 0)
                return false;

            vorgang.BelegNr = tmpId;

            return true;
        }

        string GetVorgangNummern(List<Vorgang> zulassungen)
        {
            foreach (var vorgang in zulassungen)
            {
                // Vorgang, Belegnummer für Hauptvorgang (GT_BAK)
                var blnBelegNrLeer = (vorgang.BelegNr.IsNullOrEmpty() || vorgang.BelegNr.TrimStart('0').Length == 0);

                if (blnBelegNrLeer && !GetSapId(vorgang))
                    return Localize.SaveFailed + ": " + Localize.UnableToRetrieveNewRecordIdFromSap;
            }

            return "";
        }

        public string Check48hExpress(Vorgang zulassung)
        {
            Z_ZLD_CHECK_48H.Init(SAP);

            SAP.SetImportParameter("I_KREISKZ", zulassung.Zulassungsdaten.Zulassungskreis);
            SAP.SetImportParameter("I_LIFNR", zulassung.VersandAdresse.Adresse.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_DATUM_AUSFUEHRUNG", zulassung.Zulassungsdaten.Zulassungsdatum);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var checkResults = Z_ZLD_CHECK_48H.ES_VERSAND_48H.GetExportList(SAP);
            if (checkResults.Any())
            {
                var item = checkResults.First();

                zulassung.Ist48hZulassung = item.Z48H.XToBool();
                zulassung.LieferuhrzeitBis = item.LIFUHRBIS;

                // Abweichende Versandadresse?
                if (!String.IsNullOrEmpty(item.NAME1))
                {
                    var adr = zulassung.VersandAdresse.Adresse;
                    adr.Name1 = item.NAME1;
                    adr.Name2 = item.NAME2;
                    adr.Strasse = item.STREET;
                    adr.HausNr = "";
                    adr.PLZ = item.POST_CODE1;
                    adr.Ort = item.CITY1;
                }
            }

            return "";
        }

        public string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart, bool modusAbmeldung)
        {
            try
            {
                var errorMessage = GetVorgangNummern(zulassungen);
                if (errorMessage.IsNotNullOrEmpty())
                    return errorMessage;

                Z_ZLD_AH_IMPORT_ERFASSUNG1.Init(SAP);

                SAP.SetImportParameter("I_TELNR", ((ILogonContextDataService)LogonContext).UserInfo.Telephone);
                SAP.SetImportParameter("I_FESTE_REFERENZEN", "X");

                if (saveDataToSap)
                    SAP.SetImportParameter("I_SPEICHERN", "X");

                SAP.SetImportParameter("I_FORMULAR", "X");
                if (!modusAbmeldung)
                    SAP.SetImportParameter("I_ZUSATZFORMULARE", "X");

                var positionen = new List<Zusatzdienstleistung>();
                var adressen = new List<Adressdaten>();
                var zusBankdaten = new List<Bankdaten>();

                foreach (var vorgang in zulassungen)
                {
                    // Zusatzdienstleistungen (GT_POS_IN)
                    positionen.Add(new Zusatzdienstleistung
                    {
                        BelegNr = vorgang.BelegNr,
                        PositionsNr = "10",
                        MaterialNr = vorgang.Zulassungsdaten.ZulassungsartMatNr,
                        Menge = "1"
                    });
                    vorgang.OptionenDienstleistungen.AlleDienstleistungen.ForEach(dl => dl.BelegNr = vorgang.BelegNr);
                    positionen.AddRange(vorgang.OptionenDienstleistungen.GewaehlteDienstleistungen);

                    // Adressen (GT_ADRS_IN)
                    vorgang.BankAdressdaten.Adressdaten.BelegNr = vorgang.BelegNr;
                    adressen.Add(vorgang.BankAdressdaten.Adressdaten);

                    vorgang.Halter.BelegNr = vorgang.BelegNr;
                    adressen.Add(vorgang.Halter);

                    vorgang.ZahlerKfzSteuer.Adressdaten.BelegNr = vorgang.BelegNr;
                    adressen.Add(vorgang.ZahlerKfzSteuer.Adressdaten);

                    vorgang.AuslieferAdressen.ForEach(a =>
                        {
                            a.Adressdaten.BelegNr = vorgang.BelegNr;
                            if (a.ZugeordneteMaterialien.AnyAndNotNull())
                            {
                                var tmpBem = a.Adressdaten.Bemerkung;
                                a.Adressdaten.Bemerkung = String.Join(";", a.ZugeordneteMaterialien);
                                a.Adressdaten.Bemerkung = a.Adressdaten.Bemerkung.Replace("Sonstiges", String.Format("Sonstiges: {0}", tmpBem));
                            }
                            adressen.Add(a.Adressdaten);
                        });

                    vorgang.VersandAdresse.BelegNr = vorgang.BelegNr;
                    adressen.Add(vorgang.VersandAdresse);

                    // zus. Bankdaten (GT_BANK_IN)
                    vorgang.ZahlerKfzSteuer.Bankdaten.BelegNr = vorgang.BelegNr;
                    zusBankdaten.Add(vorgang.ZahlerKfzSteuer.Bankdaten);
                }

                var bakList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BAK_IN_From_Vorgang.CopyBack(zulassungen).ToList();
                SAP.ApplyImport(bakList);

                var posList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_POS_IN_From_Zusatzdienstleistung.CopyBack(positionen).ToList();
                SAP.ApplyImport(posList);

                var relevanteAdressen = adressen.Where(a => a.Adresse.Name1.IsNotNullOrEmpty()).ToList();
                if (relevanteAdressen.Any())
                {
                    var adrsList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_ADRS_IN_From_Adressdaten.CopyBack(relevanteAdressen).ToList();
                    SAP.ApplyImport(adrsList);
                }

                var relevanteBankdaten = zusBankdaten.Where(b => b.Iban.IsNotNullOrEmpty());
                if (relevanteBankdaten.Any())
                {
                    var bankList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BANK_IN_From_Bankdaten.CopyBack(relevanteBankdaten).ToList();
                    SAP.ApplyImport(bankList);
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
            
            var fileNames = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_FILENAME_To_PdfFormular.Copy(fileNamesSap).ToListOrEmptyList();
            // alle relativen Pfade zu absoluten Pfaden konvertieren:
            fileNames.ForEach(f =>
                {
                    f.DateiPfad = Path.Combine(KroschkeZulassungViewModel.PfadAuftragszettel, f.DateiPfad.SlashToBackslash().SubstringTry(1));
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


        #region Zulassungs Report

        public List<ZulassungsReportModel> GetZulassungsReportItems(ZulassungsReportSelektor selector, List<Kunde> kunden, Action<string, string> addModelError)
        {
            var iKunnr = selector.KundenNr;
            var iGroup = ((ILogonContextDataService)LogonContext).Organization.OrganizationName;
            var iVkOrg = ((ILogonContextDataService) LogonContext).Customer.AccountingArea.ToString();
            var iVkBur = ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2;

            try
            {
                Z_ZLD_AH_ZULLISTE.Init(SAP);

                SAP.SetImportParameter("I_KUNNR", iKunnr.ToSapKunnr());
                if (iKunnr.IsNullOrEmpty())
                {
                    SAP.SetImportParameter("I_KUNNR", "");
                    SAP.SetImportParameter("I_GRUPPE", iGroup);
                }

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
                if (addModelError != null)
                    addModelError("", e.FormatSapSaveException());
            }

            if (SAP.ResultCode != 0)
                if (addModelError != null)
                    addModelError("", SAP.ResultMessage.FormatSapSaveResultMessage());

            var sapItems = Z_ZLD_AH_ZULLISTE.GT_OUT.GetExportList(SAP);
            var webItems = AppModelMappings.Z_ZLD_AH_ZULLISTE_GT_OUT_To_ZulassungsReportModel.Copy(sapItems).ToList();
            var sapKunden = Z_ZLD_AH_ZULLISTE.GT_KUN.GetExportList(SAP).ToListOrEmptyList();
            webItems.ForEach(item =>
                {
                    if (iKunnr.IsNotNullOrEmpty())
                    {
                        var kunde = kunden.FirstOrDefault(k => k.KundenNr == item.KundenNr);
                        item.KundenNrAndName = (kunde == null ? item.KundenNr : string.Format("{0} - {1}, {2}, {3}", kunde.KundenNr, kunde.Adresse.Name1, kunde.Adresse.Name2, kunde.Adresse.Ort));
                    }
                    else
                    {
                        var sapKunde = sapKunden.FirstOrDefault(k => k.KUNNR.ToSapKunnr() == item.KundenNr.ToSapKunnr());
                        item.KundenNrAndName = (sapKunde == null ? item.KundenNr : string.Format("{0} - {1}", sapKunde.KUNNR, sapKunde.NAME1));
                    }
                } 
            );
            return webItems;
        }

        #endregion


        #region Dokumentencenter Formulare

        private IEnumerable<Zulassungskreis> LoadZulassungskreiseFromSap()
        {
            var sapList = Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL.GetExportListWithInitExecute(SAP);

            return AppModelMappings.Z_ZLD_EXPORT_ZULSTEL_GT_EX_ZULSTELL_To_Zulassungskreis.Copy(sapList)
                .Where(z => z.KreisKz.IsNotNullOrEmpty() && !z.KreisKz.IsNumeric()).OrderBy(z => z.KreisKz);
        }

        public List<PdfFormular> GetFormulare(FormulareSelektor selector, Action<string, string> addModelError)
        {
            try
            {
                Z_ZLD_AH_AUSGABE_ZULFORMS.Init(SAP, "I_KUNNR_AG, I_KREISKZ", LogonContext.KundenNr.ToSapKunnr(), selector.Zulassungskreis);

                SAP.Execute();
            }
            catch (Exception e)
            {
                addModelError("", e.FormatSapSaveException());
            }

            if (SAP.ResultCode != 0)
                addModelError("", SAP.ResultMessage.FormatSapSaveResultMessage());

            var sapItems = Z_ZLD_AH_AUSGABE_ZULFORMS.GT_FILENAME.GetExportList(SAP);

            return AppModelMappings.Z_ZLD_AH_AUSGABE_ZULFORMS_GT_FILENAME_To_PdfFormular.Copy(sapItems).OrderBy(f => f.Typ).ToList();
        }

        #endregion
    }
}
