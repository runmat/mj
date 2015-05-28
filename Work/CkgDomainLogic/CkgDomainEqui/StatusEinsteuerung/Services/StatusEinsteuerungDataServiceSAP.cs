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
	       
            Z_M_EC_AVM_STATUS_EINSTEUERUNG.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());                    
            SAP.Execute();
            var sapItemsEinst = Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB.GetExportList(SAP);
            
            // Sperren
            Z_M_EC_AVM_STATUS_BESTAND.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());;
            SAP.Execute();
            var sapItemsBest = Z_M_EC_AVM_STATUS_BESTAND.GT_WEB.GetExportList(SAP);

            var result =
                from s in sapItemsEinst
                join b in sapItemsBest on new {s.ZZCARPORT, s.ZZMODELL} equals new {b.ZZCARPORT, b.ZZMODELL}
                select new Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB
                {
                    ZZCARPORT = s.ZZCARPORT,
                    ZNAME1 = s.ZNAME1,
                    ZFZG_GROUP = s.ZFZG_GROUP,
                    ZZHERST = s.ZZHERST,
                    ZKLTXT = s.ZKLTXT,
                    ZZMODELL = s.ZZMODELL,
                    ZZBEZEI = s.ZZBEZEI,
                    FZG_EING_GES = s.FZG_EING_GES,
                    FZG_AUS_VJ = s.FZG_AUS_VJ,
                    ZUL_VM = s.ZUL_VM,
                    ZUL_LFD_M = s.ZUL_LFD_M,
                    ZUL_GES_M = s.ZUL_GES_M,
                    ZUL_PZ_LFD_M = s.ZUL_PZ_LFD_M,
                    ZUL_PZ_FM = s.ZUL_PZ_FM,
                    FZG_BEST = s.FZG_BEST,
                    FZG_AUSGER = s.FZG_AUSGER,
                    FZG_M_BRIEF = s.FZG_M_BRIEF,
                    FZG_ZUL_BER = s.FZG_ZUL_BER,
                    FZG_OHNE_UNIT = s.FZG_OHNE_UNIT,
                    ZSIPP_CODE = s.ZSIPP_CODE,
                    FZG_GESP = b.FZG_GESP,
                };
            
            var webItemsEquis = AppModelMappings.Z_M_EC_AVM_STATUS_EINSTEUERUNG_GT_WEB_To_StatusEinsteuerung.Copy(result).ToList();

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
