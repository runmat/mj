using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.Zanf.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Zanf.Models.AppModelMappings;

namespace CkgDomainLogic.Zanf.Services
{
    public class ZulassungsunterlagenDataServiceSap : CkgGeneralDataServiceSAP, IZulassungsunterlagenDataService
    {
        public ZulassungsUnterlagenSuchparameter Suchparameter { get; set; }

        public List<ZulassungsUnterlagen> ZulassungsUnterlagen { get { return PropertyCacheGet(() => LoadZulassungsUnterlagenFromSap().ToList()); } }

        public ZulassungsunterlagenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new ZulassungsUnterlagenSuchparameter { Auswahl = "A" };
        }

        public void MarkForRefreshZulassungsUnterlagen()
        {
            PropertyCacheClear(this, m => m.ZulassungsUnterlagen);
        }

        private IEnumerable<ZulassungsUnterlagen> LoadZulassungsUnterlagenFromSap()
        {
            Z_DPM_READ_ZULDOK_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(Suchparameter.ExterneKundenNr))
                SAP.SetImportParameter("I_ZKUNNR_EXT", Suchparameter.ExterneKundenNr);

            if (!String.IsNullOrEmpty(Suchparameter.HalterName))
                SAP.SetImportParameter("I_NAME_HALTER", String.Format("*{0}*", Suchparameter.HalterName));

            switch (Suchparameter.Auswahl)
            {
                case "V":
                    SAP.SetImportParameter("I_NUR_VOLLST", "X");
                    break;

                case "U":
                    SAP.SetImportParameter("I_NUR_UNVOLLST", "X");
                    break;
            }

            var sapList = Z_DPM_READ_ZULDOK_01.GT_DATEN.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_ZULDOK_01_GT_DATEN_To_ZulassungsUnterlagen.Copy(sapList);
        }

        public byte[] GetZulassungsUnterlagenAsPdf(string documentId)
        {
            Z_DPM_READ_DOK_ARCHIV_01.Init(SAP, "I_DOC_ID, I_ARC_ID", documentId, "ZU");

            SAP.Execute();

            if (SAP.ResultCode == 0)
                return SAP.GetExportParameterByte("E_XSTRING");

            return null;
        }

        public string SaveZulassungsUnterlagen(ZulassungsUnterlagen zu)
        {
            var erg = "";

            var liste = new List<ZulassungsUnterlagen> { zu };

            Z_DPM_SAVE_ZULDOK_01.Init(SAP, "I_AG, I_WEB_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var vList = AppModelMappings.Z_DPM_SAVE_ZULDOK_01_GT_DATEN_From_ZulassungsUnterlagen.CopyBack(liste).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            var ergList = Z_DPM_SAVE_ZULDOK_01.GT_DATEN.GetExportList(SAP);

            if ((ergList.Count > 0) && (!String.IsNullOrEmpty(ergList[0].RET)))
                erg = ergList[0].RET;

            return erg;
        }
    }
}
