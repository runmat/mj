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
    public class DispositionslisteDataServiceSAP : CkgGeneralDataServiceSAP, IDispositionslisteDataService
    {
        public DispositionslisteDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }
      
        public List<Dispositionsliste> GetDispositionsliste(DispositionslisteSelektor selector)
        {
            var ret = new List<Dispositionsliste>();
            ret.Add(new Dispositionsliste(){ Hersteller = "Audi (Test)" });
            return ret;
                      
            //Z_M_EC_AVM_ZULASSUNGEN.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            //if (selector.ZulassungsdatumRange.IsSelected)
            //{
            //    SAP.SetImportParameter("I_ZULDAT_VON", selector.ZulassungsdatumRange.StartDate);
            //    SAP.SetImportParameter("I_ZULDAT_BIS", selector.ZulassungsdatumRange.EndDate);
            //}

            //if (selector.PDINummer.IsNotEmpty())
            //    SAP.SetImportParameter("I_PDI", selector.PDINummer);

            //SAP.Execute();

            //var sapItemsEquis = Z_M_EC_AVM_ZULASSUNGEN.GT_WEB.GetExportList(SAP);
            //var webItemsEquis = AppModelMappings.Z_M_ECA_TAB_BESTAND_To_Zb2BestandSecurityFleet.Copy(sapItemsEquis).ToList();

            //return webItemsEquis;
            

        }
    }
}
