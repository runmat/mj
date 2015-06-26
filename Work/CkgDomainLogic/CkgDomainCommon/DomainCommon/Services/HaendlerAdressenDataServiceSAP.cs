using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
// ReSharper disable RedundantUsingDirective
using CkgDomainLogic.DomainCommon.Contracts;
using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class HaendlerAdressenDataServiceSAP : CkgGeneralDataServiceSAP, IHaendlerAdressenDataService
    {
        public HaendlerAdressenDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<HaendlerAdresse> GetHaendlerAdressen()
        {
            Z_DPM_READ_REM_VERS_VORG_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            var sapItems = Z_DPM_READ_REM_VERS_VORG_01.GT_OUT.GetExportList(SAP);
            var webItems = AppModelMappings.Z_DPM_READ_MODELID_TAB__GT_OUT_To_HaendlerAdresse.Copy(sapItems).ToList();

            return webItems;
        }

        public string SaveHaendlerAdresse(HaendlerAdresse haendlerAdresse)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_DPM_SAVE_REM_VERS_VORG_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

                    var sapItems = AppModelMappings.Z_DPM_READ_MODELID_TAB__GT_OUT_To_HaendlerAdresse
                        .CopyBack(new List<HaendlerAdresse> {haendlerAdresse}).ToList();
                    
                    SAP.ApplyImport(sapItems);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    var exportList = Z_DPM_SAVE_REM_VERS_VORG_01.GT_TAB.GetExportList(SAP);
                    if (exportList != null && exportList.Any() && exportList.First().BEM.IsNotNullOrEmpty())
                        return exportList.First().BEM;

                    return "";
                });

            return error;
        }

        public List<SelectItem> GetLaenderList()
        {
            Z_DPM_READ_LAND_02.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            var sapItems = Z_DPM_READ_LAND_02.GT_OUT.GetExportList(SAP);
            var webItems = AppModelMappings.Z_DPM_READ_LAND_02__GT_OUT_To_SelectItem.Copy(sapItems).ToList();

            return webItems;
        }
    }
}
