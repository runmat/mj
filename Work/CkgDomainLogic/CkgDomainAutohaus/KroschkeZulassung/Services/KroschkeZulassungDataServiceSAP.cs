using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.KroschkeZulassung.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.KroschkeZulassung.Contracts;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.KroschkeZulassung.Models.AppModelMappings;

namespace CkgDomainLogic.KroschkeZulassung.Services
{
    public class KroschkeZulassungDataServiceSAP : CkgGeneralDataServiceSAP, IKroschkeZulassungDataService
    {
        public Vorgang Zulassung { get; set; }

        public List<Kunde> Kunden { get { return PropertyCacheGet(() => LoadKundenFromSap().ToList()); } }

        public List<Domaenenfestwert> Fahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }

        public List<Material> Zulassungsarten { get { return PropertyCacheGet(() => LoadZulassungsartenFromSap().ToList()); } }

        public List<Zusatzdienstleistung> Zusatzdienstleistungen
        {
            get
            {
                return PropertyCacheGet(() =>
                    AppModelMappings.Z_DPM_READ_LV_001_GT_OUT_DL_To_Zusatzdienstleistung.Copy(SapZulassungsDienstleistungen)
                        .OrderBy(w => w.Name).ToList());
            }
        }

        public List<Kennzeichengroesse> Kennzeichengroessen { get { return PropertyCacheGet(() => LoadKennzeichengroessenFromSql().ToList()); } }

        private static KroschkeZulassungSqlDbContext CreateDbContext()
        {
            return new KroschkeZulassungSqlDbContext();
        }

        public KroschkeZulassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefresh()
        {
            Zulassung = new Vorgang
                {
                    VkOrg = ((LogonContextDataServiceBase)LogonContext).Customer.AccountingArea.ToString(),
                    VkBur = ((LogonContextDataServiceBase)LogonContext).Organization.OrganizationReference2,
                    Vorerfasser = LogonContext.UserName,
                    VorgangsStatus = "1"
                };
            PropertyCacheClear(this, m => m.Kunden);
            PropertyCacheClear(this, m => m.Fahrzeugarten);
            PropertyCacheClear(this, m => m.Zulassungsarten);
            PropertyCacheClear(this, m => m.Zusatzdienstleistungen);
            PropertyCacheClear(this, m => m.Kennzeichengroessen);
            Zulassung.OptionenDienstleistungen.InitDienstleistungen(Zusatzdienstleistungen);
        }

        private IEnumerable<Kunde> LoadKundenFromSap()
        {
            Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.Init(SAP);

            var orgRef = ((LogonContextDataServiceBase) LogonContext).Organization.OrganizationReference;

            SAP.SetImportParameter("I_KUNNR", (String.IsNullOrEmpty(orgRef) ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
            SAP.SetImportParameter("I_VKORG", ((LogonContextDataServiceBase)LogonContext).Customer.AccountingArea.ToString());
            SAP.SetImportParameter("I_VKBUR", ((LogonContextDataServiceBase)LogonContext).Organization.OrganizationReference2);
            SAP.SetImportParameter("I_SPART", "01");

            var sapList = Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_Kunde.Copy(sapList);
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
            Z_ZLD_AH_MATERIAL.Init(SAP, "I_VKBUR", ((LogonContextDataServiceBase)LogonContext).Organization.OrganizationReference2);

            var sapList = Z_ZLD_AH_MATERIAL.GT_MAT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_ZLD_AH_MATERIAL_GT_MAT_To_Material.Copy(sapList);
        }

        public string GetZulassungskreis()
        {
            Z_ZLD_AH_ZULST_BY_PLZ.Init(SAP, "I_PLZ, I_ORT", Zulassung.Halterdaten.PLZ, Zulassung.Halterdaten.Ort);

            var sapList = Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST.GetExportListWithExecute(SAP);

            if (SAP.ResultCode != 0)
                return Localize.UnableToRetrieveRegistrationDistrictFromSap + ": " + SAP.ResultMessage;

            if (sapList.Count > 0)
                return sapList[0].ZKFZKZ;

            return "";
        }

        private IEnumerable<Kennzeichengroesse> LoadKennzeichengroessenFromSql()
        {
            var ct = CreateDbContext();

            return ct.GetKennzeichengroessen();
        }

        private bool GetSapId()
        {
            Z_ZLD_EXPORT_BELNR.Init(SAP);

            var tmpId = SAP.GetExportParameterWithExecute("E_BELN");

            if (String.IsNullOrEmpty(tmpId) || tmpId.TrimStart('0').Length == 0)
                return false;

            Zulassung.BelegNr = tmpId;

            return true;
        }

        public string SaveZulassung(bool saveDataInSap, bool mitKundenformular)
        {
            var blnBelegNrLeer = (String.IsNullOrEmpty(Zulassung.BelegNr) || Zulassung.BelegNr.TrimStart('0').Length == 0);

            if (blnBelegNrLeer && !GetSapId())
                return Localize.SaveFailed + ": " + Localize.UnableToRetrieveNewRecordIdFromSap;

            Z_ZLD_AH_IMPORT_ERFASSUNG1.Init(SAP);

            SAP.SetImportParameter("I_TELNR", ((LogonContextDataServiceBase)LogonContext).UserInfo.Telephone);
            SAP.SetImportParameter("I_FESTE_REFERENZEN", "X");

            if (saveDataInSap)
                SAP.SetImportParameter("I_SPEICHERN", "X");

            if (mitKundenformular)
                SAP.SetImportParameter("I_FORMULAR", "X");

            var vorgaenge = new List<Vorgang> { Zulassung };

            var bakList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_BAK_IN_From_Vorgang.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(bakList);

            Zulassung.OptionenDienstleistungen.AlleDienstleistungen.ForEach(dl => dl.BelegNr = Zulassung.BelegNr);
            var posList = AppModelMappings.Z_ZLD_AH_IMPORT_ERFASSUNG1_GT_POS_IN_From_Zusatzdienstleistung.CopyBack(Zulassung.OptionenDienstleistungen.GewaehlteDienstleistungen).ToList();
            SAP.ApplyImport(posList);

            if (!String.IsNullOrEmpty(Zulassung.BankAdressdaten.Rechnungsempfaenger.Name1))
            {
                Zulassung.BankAdressdaten.Rechnungsempfaenger.BelegNr = Zulassung.BelegNr;
                var adressen = new List<Adressdaten> { Zulassung.BankAdressdaten.Rechnungsempfaenger };

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

            if (mitKundenformular)
                Zulassung.KundenformularPdf = SAP.GetExportParameterByte("E_PDF");
            else
                Zulassung.KundenformularPdf = null;

            return "";
        }
    }
}
