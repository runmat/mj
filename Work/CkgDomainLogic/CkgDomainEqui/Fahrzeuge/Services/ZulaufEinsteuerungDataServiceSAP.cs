using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class ZulaufEinsteuerungDataServiceSAP : CkgGeneralDataServiceSAP, IZulaufEinsteuerungDataService
    {
        public ZulaufEinsteuerungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }
      
        public List<ZulaufEinsteuerung> GetZulaufEinsteuerung()
        {
            Z_M_EC_AVM_STATUS_ZUL.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
          
            SAP.Execute();

            var sapItemsEquis = Z_M_EC_AVM_STATUS_ZUL.GT_WEB.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_EC_AVM_STATUS_ZUL_GT_WEB_ToZulaufEinsteuerung.Copy(sapItemsEquis).ToList();

            return webItemsEquis;            
        }
    }
}
