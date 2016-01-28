using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Autohaus.Services
{
    public class CocAnforderungDataServiceSAP : CkgGeneralDataServiceSAP, ICocAnforderungDataService
    {
        public CocAnforderungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public string GetEmpfaengerEmailAdresse()
        {
            Z_DPM_READ_ZDAD_AUFTR_006.Init(SAP, "I_KENNUNG", "COC-ANFORDERUNG");

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var sapItem = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportList(SAP).FirstOrDefault();

                if (sapItem != null)
                    return sapItem.EMAIL;
            }

            return "";
        }
    }
}
