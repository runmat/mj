using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models;
using CkgDomainLogic.General.Contracts;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Services
{
    public class UploadPartnerUndFahrzeugdatenDataServiceSap : CkgGeneralDataServiceSAP, IUploadPartnerUndFahrzeugdatenDataService
    {
        public UploadPartnerUndFahrzeugdatenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public List<IUploadItem> UploadItems { get; set; }

        public void LoadTypdaten(IEnumerable<Fahrzeugdaten> fahrzeuge)
        {
            Z_AHP_READ_TYPDAT_BESTAND.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            var fzgList = AppModelMappings.Z_AHP_READ_TYPDAT_BESTAND_GT_WEB_IMP_MASS_From_Fahrzeugdaten.CopyBack(fahrzeuge).ToList();
            SAP.ApplyImport(fzgList);

            var sapList = Z_AHP_READ_TYPDAT_BESTAND.GT_WEB_TYPDATEN.GetExportListWithExecute(SAP);
            var typList = Fahrzeugbestand.Models.AppModelMappings.Z_AHP_READ_TYPDAT_BESTAND_GT_TYPDATEN_To_FahrzeugAkteBestand.Copy(sapList);

            foreach (var item in fahrzeuge)
            {
                var typItem = typList.FirstOrDefault(t => string.Compare(t.FIN, item.FahrgestellNr, true) == 0);

                if (typItem != null)
                {
                    item.TypdatenGefunden = true;
                    item.HerstellerSchluessel = typItem.HerstellerSchluessel;
                    item.TypSchluessel = typItem.TypSchluessel;
                    item.VvsSchluessel = typItem.VvsSchluessel;
                    item.VvsPruefziffer = typItem.VvsPruefZiffer;
                    item.FabrikName = typItem.FabrikName;
                    item.HandelsName = typItem.HandelsName;
                }
                else
                {
                    item.TypdatenGefunden = false;
                    item.FabrikName = "";
                    item.HandelsName = "";
                }
            }
        }

        public string SaveUploadItems()
        {
            if (UploadItems.Any())
            {
                if (UploadItems.First() is UploadPartnerdaten)
                    return SaveUploadPartnerdatenItems(UploadItems.Select(u => (UploadPartnerdaten)u));

                if (UploadItems.First() is UploadFahrzeugdaten)
                    return SaveUploadFahrzeugdatenItems(UploadItems.Select(u => (UploadFahrzeugdaten)u));

                if (UploadItems.First() is UploadPartnerUndFahrzeugdaten)
                    return SaveUploadPartnerUndFahrzeugdatenItems(UploadItems.Select(u => (UploadPartnerUndFahrzeugdaten)u));
            }

            return string.Format("{0} ({1})", Localize.ErrorsOccuredOnSaving, Localize.InvalidObjectType);
        }

        #region Partner

        private string SaveUploadPartnerdatenItems(IEnumerable<UploadPartnerdaten> items)
        {
            Z_AHP_CRE_CHG_PARTNER_FZGDATEN.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_IMP_From_Partnerdaten.CopyBack(items).ToList();
            SAP.ApplyImport(importList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_ERR.GetExportList(SAP);

            foreach (var item in items)
            {
                if (errList.Any(e => string.Compare(e.REFKUNNR, item.Referenz1, true) == 0))
                    item.SaveStatus = Localize.SaveFailed;
                else
                    item.SaveStatus = Localize.OK;
            }

            return "";
        }

        #endregion


        #region Fahrzeug

        private string SaveUploadFahrzeugdatenItems(IEnumerable<UploadFahrzeugdaten> items)
        {
            Z_AHP_CRE_CHG_PARTNER_FZGDATEN.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_IMP_From_Fahrzeugdaten.CopyBack(items).ToList();
            SAP.ApplyImport(importList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_ERR.GetExportList(SAP);

            foreach (var item in items)
            {
                if (errList.Any(e => string.Compare(e.FIN, item.FahrgestellNr, true) == 0))
                    item.SaveStatus = Localize.SaveFailed;
                else
                    item.SaveStatus = Localize.OK;
            }

            return "";
        }

        #endregion


        #region Partner & Fahrzeug

        private string SaveUploadPartnerUndFahrzeugdatenItems(IEnumerable<UploadPartnerUndFahrzeugdaten> items)
        {
            Z_AHP_CRE_CHG_PARTNER_FZGDATEN.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importListPartner = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_IMP_From_Partnerdaten
                .CopyBack(items.Where(u => !string.IsNullOrEmpty(u.Halter.Name1)).Select(up => up.Halter)).ToList();
            SAP.ApplyImport(importListPartner);

            var importListFzg = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_IMP_From_Fahrzeugdaten
                .CopyBack(items.Where(u => !string.IsNullOrEmpty(u.Fahrzeug.FahrgestellNr)).Select(uf => uf.Fahrzeug)).ToList();
            SAP.ApplyImport(importListFzg);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errListPartner = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_ERR.GetExportList(SAP);
            var errListFzg = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_ERR.GetExportList(SAP);

            foreach (var item in items)
            {
                if (errListPartner.Any(e => string.Compare(e.REFKUNNR, item.Halter.Referenz1, true) == 0) || errListFzg.Any(e => string.Compare(e.FIN, item.Fahrzeug.FahrgestellNr, true) == 0))
                    item.SaveStatus = Localize.SaveFailed;
                else
                    item.SaveStatus = Localize.OK;
            }

            return "";
        }

        #endregion
    }
}
