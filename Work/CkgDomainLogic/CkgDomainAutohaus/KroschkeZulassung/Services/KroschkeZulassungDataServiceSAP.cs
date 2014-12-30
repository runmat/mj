using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.KroschkeZulassung.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.KroschkeZulassung.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.KroschkeZulassung.Models.AppModelMappings;

namespace CkgDomainLogic.KroschkeZulassung.Services
{
    public class KroschkeZulassungDataServiceSAP : CkgGeneralDataServiceSAP, IKroschkeZulassungDataService
    {
        public List<Kunde> Kunden { get { return PropertyCacheGet(() => LoadKundenFromSap().ToList()); } }

        public List<Domaenenfestwert> Fahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }

        public List<Material> Zulassungsarten { get { return PropertyCacheGet(() => LoadZulassungsartenFromSap().ToList()); } }

        public List<Zusatzdienstleistung> Zusatzdienstleistungen { get { return PropertyCacheGet(() => LoadZusatzdienstleistungenFromSap().ToList()); } }

        public List<Kennzeichengroesse> Kennzeichengroessen { get { return PropertyCacheGet(() => LoadKennzeichengroessenFromSql().ToList()); } }

        public string PfadAuftragszettel { get; private set; }

        private static KroschkeZulassungSqlDbContext CreateDbContext()
        {
            return new KroschkeZulassungSqlDbContext();
        }

        public KroschkeZulassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            PfadAuftragszettel = GeneralTools.Services.GeneralConfiguration.GetConfigValue("KroschkeAutohaus", "PfadAuftragszettel");
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

            SAP.SetImportParameter("I_KUNNR", (String.IsNullOrEmpty(orgRef) ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
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

            if (String.IsNullOrEmpty(tmpId) || tmpId.TrimStart('0').Length == 0)
                return false;

            vorgang.BelegNr = tmpId;

            return true;
        }

        public string SaveZulassungen(List<Vorgang> zulassungen, bool saveDataInSap)
        {
            var blnCpdKunde = zulassungen.First().Rechnungsdaten.Kunde.Cpdkunde;

            Z_ZLD_AH_IMPORT_ERFASSUNG1.Init(SAP);
            SAP.SetImportParameter("I_TELNR", ((ILogonContextDataService)LogonContext).UserInfo.Telephone);
            SAP.SetImportParameter("I_FESTE_REFERENZEN", "X");

            if (saveDataInSap)
                SAP.SetImportParameter("I_SPEICHERN", "X");

            if (blnCpdKunde)
                SAP.SetImportParameter("I_FORMULAR", "X");

            var positionen = new List<Zusatzdienstleistung>();
            var adressen = new List<Adressdaten>();
            foreach (var vorgang in zulassungen)
            {
                // Vorgang, Belegnummer für Hauptvorgang (GT_BAK)
                var blnBelegNrLeer = (String.IsNullOrEmpty(vorgang.BelegNr) || vorgang.BelegNr.TrimStart('0').Length == 0);

                if (blnBelegNrLeer && !GetSapId(vorgang))
                    return Localize.SaveFailed + ": " + Localize.UnableToRetrieveNewRecordIdFromSap;


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
                if (!String.IsNullOrEmpty(vorgang.BankAdressdaten.Rechnungsempfaenger.Name1))
                {
                    vorgang.BankAdressdaten.Rechnungsempfaenger.BelegNr = vorgang.BelegNr;
                    adressen.Add(vorgang.BankAdressdaten.Rechnungsempfaenger);
                }
            }

            var bakList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BAK_IN_From_Vorgang.CopyBack(zulassungen).ToList();
            SAP.ApplyImport(bakList);

            var posList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_POS_IN_From_Zusatzdienstleistung.CopyBack(positionen).ToList();
            SAP.ApplyImport(posList);

            var adrsList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_ADRS_IN_From_Adressdaten.CopyBack(adressen).ToList();
            SAP.ApplyImport(adrsList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
            {
                var errString = "";
                var errList = Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_ERROR.GetExportList(SAP);
                if (errList.Count > 0 && errList.Any(e => e.MESSAGE != "OK"))
                    errString = String.Join(", ", errList.Select(e => e.MESSAGE));

                return String.Format("{0}: {1}{2}", Localize.SaveFailed, SAP.ResultMessage, (String.IsNullOrEmpty(errString) ? "" : String.Format(" ({0})", errString)));
            }

            var fileNames = Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_FILENAME.GetExportList(SAP);

            var i = 0;
            foreach (var vorgang in zulassungen)
            {
                if (saveDataInSap && fileNames.Count > i)
                    vorgang.AuftragszettelPdfPfad = String.Format("{0}{1}\\{2}.pdf", PfadAuftragszettel,
                                                                    fileNames[i].FILENAME.Split('/')[1],
                                                                    vorgang.BelegNr.PadLeft(10, '0'));
                else
                    vorgang.AuftragszettelPdfPfad = "";

                // ToDo: Pro Vorgang ein eigenes Kundenformular, am besten Export Parameter ersetzen durch Export-Tabelleneintrag!)
                if (blnCpdKunde)
                    vorgang.KundenformularPdf = SAP.GetExportParameterByte("E_PDF");

                i++;
            }

            return "";
        }

        public string SaveZulassung(Vorgang zulassung, bool saveDataInSap)
        {
            var blnCpdKunde = zulassung.Rechnungsdaten.Kunde.Cpdkunde;

            var blnBelegNrLeer = (String.IsNullOrEmpty(zulassung.BelegNr) || zulassung.BelegNr.TrimStart('0').Length == 0);

            var vorgaenge = new List<Vorgang> { zulassung };

            if (blnBelegNrLeer && !GetSapId(zulassung))
                return Localize.SaveFailed + ": " + Localize.UnableToRetrieveNewRecordIdFromSap;

            Z_ZLD_AH_IMPORT_ERFASSUNG1.Init(SAP);

            SAP.SetImportParameter("I_TELNR", ((ILogonContextDataService)LogonContext).UserInfo.Telephone);
            SAP.SetImportParameter("I_FESTE_REFERENZEN", "X");

            if (saveDataInSap)
                SAP.SetImportParameter("I_SPEICHERN", "X");

            if (blnCpdKunde)
                SAP.SetImportParameter("I_FORMULAR", "X");

            var bakList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BAK_IN_From_Vorgang.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(bakList);

            var positionen = new List<Zusatzdienstleistung> { new Zusatzdienstleistung
                {
                    BelegNr = zulassung.BelegNr,
                    PositionsNr = "10",
                    MaterialNr = zulassung.Zulassungsdaten.ZulassungsartMatNr,
                    Menge = "1"
                } };

            zulassung.OptionenDienstleistungen.AlleDienstleistungen.ForEach(dl => dl.BelegNr = zulassung.BelegNr);
            positionen.AddRange(zulassung.OptionenDienstleistungen.GewaehlteDienstleistungen);

            var posList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_POS_IN_From_Zusatzdienstleistung.CopyBack(positionen).ToList();
            SAP.ApplyImport(posList);

            if (!String.IsNullOrEmpty(zulassung.BankAdressdaten.Rechnungsempfaenger.Name1))
            {
                zulassung.BankAdressdaten.Rechnungsempfaenger.BelegNr = zulassung.BelegNr;
                var adressen = new List<Adressdaten> { zulassung.BankAdressdaten.Rechnungsempfaenger };

                var adrsList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_ADRS_IN_From_Adressdaten.CopyBack(adressen).ToList();
                SAP.ApplyImport(adrsList);
            }

            SAP.Execute();

            if (SAP.ResultCode != 0)
            {
                var errString = "";
                var errList = Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_ERROR.GetExportList(SAP);
                if (errList.Count > 0 && errList.Any(e => e.MESSAGE != "OK"))
                    errString = String.Join(", ", errList.Select(e => e.MESSAGE));

                return String.Format("{0}: {1}{2}", Localize.SaveFailed, SAP.ResultMessage, (String.IsNullOrEmpty(errString) ? "" : String.Format(" ({0})", errString)));
            }

            var fileNames = Z_ZLD_AH_IMPORT_ERFASSUNG1.GT_FILENAME.GetExportList(SAP);

            if (saveDataInSap && fileNames.Count > 0)
                zulassung.AuftragszettelPdfPfad = String.Format("{0}{1}\\{2}.pdf", PfadAuftragszettel, fileNames[0].FILENAME.Split('/')[1], zulassung.BelegNr.PadLeft(10, '0'));
            else
                zulassung.AuftragszettelPdfPfad = "";

            if (blnCpdKunde)
                zulassung.KundenformularPdf = SAP.GetExportParameterByte("E_PDF");
            else
                zulassung.KundenformularPdf = null;

            return "";
        }
    }
}
