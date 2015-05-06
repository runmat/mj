// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.FzgModelle.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.FzgModelle.Services
{
    public class ModellIdDataServiceSAP : CkgGeneralDataServiceSAP, IModellIdDataService
    {
        public ModellIdDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<ModellId> GetModellIds()
        {
            Z_DPM_READ_MODELID_TAB.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            var sapItems = Z_DPM_READ_MODELID_TAB.GT_OUT.GetExportList(SAP);
            var webItems = AppModelMappings.Z_DPM_READ_MODELID_TAB__GT_OUT_To_ModellId.Copy(sapItems).ToList();

            return webItems;
        }

        public string SaveModellId(ModellId modellId)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_DPM_CHANGE_MODELID.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

                    AppModelMappings.Z_DPM_CHANGE_MODELID_From_ModellId(SAP, LogonContext, modellId);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    return "";
                });

            return error;
        }
    }
}
