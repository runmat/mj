using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Insurance.Services
{
    public class VertragsverlaengerungDataServiceSap : CkgGeneralDataServiceSAP, IVertragsverlaengerungDataService
    {
        public List<VertragsverlaengerungModel> Vertragsdaten { get { return PropertyCacheGet(() => LoadVertragsdatenFromSap().ToList()); } }

        public VertragsverlaengerungDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshVertragsdaten()
        {
            PropertyCacheClear(this, m => m.Vertragsdaten);
        }

        private IEnumerable<VertragsverlaengerungModel> LoadVertragsdatenFromSap()
        {
            Z_DPM_ASSIST_READ_VTRAGVERL_01.Init(SAP);
            SAP.SetImportParameter("I_KUNNR_AH", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_KUNNR_FIL", (LogonContext as ILogonContextDataService).User.Reference.NotNullOrEmpty().ToSapKunnr());

            var sapList = Z_DPM_ASSIST_READ_VTRAGVERL_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_ASSIST_READ_VTRAGVERL_01_GT_OUT_To_VertragsverlaengerungModel.Copy(sapList);
        }

        public List<VertragsverlaengerungModel> SaveVertragsdaten(List<VertragsverlaengerungModel> vertraege, ref string message)
        {
            List<VertragsverlaengerungModel> erg = new List<VertragsverlaengerungModel>();

            Z_DPM_ASSIST_CHG_VTRAGVERL_01.Init(SAP);
            SAP.SetImportParameter("I_KUNNR_AH", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_KUNNR_FIL", (LogonContext as ILogonContextDataService).User.Reference.NotNullOrEmpty().ToSapKunnr());

            var vvlList = AppModelMappings.Z_DPM_ASSIST_CHG_VTRAGVERL_01_GT_IN_From_VertragsverlaengerungModel.CopyBack(vertraege).ToList();
            SAP.ApplyImport(vvlList);

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var errorList = Z_DPM_ASSIST_CHG_VTRAGVERL_01.GT_ERR.GetExportList(SAP);

                if (errorList.Any())
                {
                    message = Localize.ErrorsOccuredOnSaving;
                }

                foreach (VertragsverlaengerungModel vvl in vertraege)
                {
                    if (errorList.Any(e => e.ID == vvl.ID))
                    {
                        vvl.Status = Localize.SaveFailed;
                    }
                    else
                    {
                        vvl.Status = Localize.SaveSuccessful;
                    }

                    erg.Add(vvl);
                }
            }
            else
            {
                message = Localize.SaveFailed + ": " + SAP.ResultMessage;
            }

            return erg;
        }
    }
}
