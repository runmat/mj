using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.Contracts;
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
	        //  Z_M_EC_AVM_STATUS_BESTAND

            Z_M_EC_AVM_STATUS_EINSTEUERUNG.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    
            SAP.Execute();

            var sapItemsEquis = Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_EC_AVM_STATUS_EINSTEUERUNG_GT_WEB_To_StatusEinsteuerung.Copy(sapItemsEquis).ToList();

            return webItemsEquis;            
        }


        public int GetZbIIOhneFzgCount()
        {
            Z_M_EC_AVM_NUR_BRIEF_VORH.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            return Z_M_EC_AVM_NUR_BRIEF_VORH.GT_WEB.GetExportList(SAP).Count();
        }

    }
}
