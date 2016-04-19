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
            Z_AHP_CRE_CHG_PARTNER.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_From_Partnerdaten.CopyBack(items).ToList();
            SAP.ApplyImport(importList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_AHP_CRE_CHG_PARTNER.GT_OUT_ERR.GetExportList(SAP);

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
            Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importList = AppModelMappings.Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_From_Fahrzeugdaten.CopyBack(items).ToList();
            SAP.ApplyImport(importList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

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
            #region Partner speichern

            var relevantePartner = items.Where(u => !string.IsNullOrEmpty(u.Halter.Name1)).Select(up => up.Halter);

            Z_AHP_CRE_CHG_PARTNER.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importListPartner = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_From_Partnerdaten.CopyBack(relevantePartner).ToList();
            SAP.ApplyImport(importListPartner);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var exportListPartner = Z_AHP_CRE_CHG_PARTNER.GT_OUT.GetExportList(SAP);
            var errListPartner = Z_AHP_CRE_CHG_PARTNER.GT_OUT_ERR.GetExportList(SAP);

            foreach (var item in items)
            {
                if (errListPartner.Any(e => string.Compare(e.REFKUNNR, item.Halter.Referenz1, true) == 0))
                {
                    item.SaveStatus = Localize.SaveFailed;
                }
                else
                {
                    item.SaveStatus = Localize.OK;

                    var savedPartner = exportListPartner.FirstOrDefault(e => string.Compare(e.REFKUNNR, item.Halter.Referenz1, true) == 0);
                    if (savedPartner != null)
                        item.Fahrzeug.Halter = savedPartner.KUNNR;
                }
            }

            #endregion

            #region Fahrzeuge speichern

            var relevanteFzg = items.Where(u => !string.IsNullOrEmpty(u.Fahrzeug.FahrgestellNr) && u.SaveStatus == Localize.OK).Select(uf => uf.Fahrzeug);

            Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP, "I_KUNNR, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var importListFzg = AppModelMappings.Z_AHP_CRE_CHG_FZG_AKT_BEST_GT_WEB_IMP_From_Fahrzeugdaten.CopyBack(relevanteFzg).ToList();
            SAP.ApplyImport(importListFzg);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var errListFzg = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

            foreach (var item in items)
            {
                if (errListFzg.Any(e => string.Compare(e.FIN, item.Fahrzeug.FahrgestellNr, true) == 0))
                    item.SaveStatus = Localize.SaveFailed;
            }

            #endregion

            return "";
        }

        #endregion
    }
}
