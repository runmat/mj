using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.FzgModelle.Models.AppStatusEinsteuerungModelMappings;

namespace CkgDomainLogic.FzgModelle.Services
{
    public class StatusEinsteuerungDataServiceSAP : CkgGeneralDataServiceSAP, IStatusEinsteuerungDataService
    {
        public StatusEinsteuerungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<StatusEinsteuerung> GetStatusbericht()
        {                          	                 
            Z_M_EC_AVM_STATUS_EINSTEUERUNG.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr()); 
                   
            SAP.Execute();

            return AppModelMappings.Z_M_EC_AVM_STATUS_EINSTEUERUNG_GT_WEB_To_StatusEinsteuerung.Copy(Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB.GetExportList(SAP)).ToList();            
        }

        public List<StatusEinsteuerung> GetStatusEinsteuerung()
        {
            Z_M_EC_AVM_STATUS_BESTAND.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            return AppModelMappings.Z_M_EC_AVM_STATUS_BESTAND_GT_WEB_To_StatusEinsteuerung.Copy(Z_M_EC_AVM_STATUS_BESTAND.GT_WEB.GetExportList(SAP)).ToList();
        }

        public int GetZbIIOhneFzgCount()
        {
            Z_M_EC_AVM_NUR_BRIEF_VORH.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            return Z_M_EC_AVM_NUR_BRIEF_VORH.GT_WEB.GetExportList(SAP).Count();
        }
    }
}
