using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.Models.DataModels;
using CkgDomainLogic.Leasing.Models.UIModels;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingAbweichWiedereingangDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingAbweichWiedereingangDataService
    {
        public LeasingAbweichWiedereingangDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }


        public List<AbweichungWiedereingang> LoadWiedereingaengeFromSap(AbweichWiedereingangSelektor selektor)
        {
                Z_DAD_CHANGES_WIEDEING_01.Init(SAP);
                Z_DAD_CHANGES_WIEDEING_01.SetImportParameter_I_AG(SAP,LogonContext.KundenNr.ToSapKunnr());
                Z_DAD_CHANGES_WIEDEING_01.SetImportParameter_I_QMART(SAP, "Z2");

            var importList =
                AppModelMappings.Z_DAD_CHANGES_WIEDEING_01_IT_RG_ERDAT_From_AbweichungWiedereingangSelektor.CopyBack(
                    new List<AbweichWiedereingangSelektor> {selektor});
            SAP.ApplyImport(importList);

            SAP.Execute();

            return AppModelMappings.Z_DAD_CHANGES_WIEDEING_01_ET_CHG_To_AbweichungWiedereingang.Copy(
                Z_DAD_CHANGES_WIEDEING_01.ET_CHG.GetExportList(SAP)).ToList();

        }
    }
}
