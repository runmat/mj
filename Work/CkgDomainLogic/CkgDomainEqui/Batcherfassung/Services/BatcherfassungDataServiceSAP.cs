// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.FzgModelle.Models.BatchModelMappings;

namespace CkgDomainLogic.FzgModelle.Services
{
    public class BatcherfassungDataServiceSAP : CkgGeneralDataServiceSAP, IBatcherfassungDataService
    {
        public BatcherfassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Batcherfassung> GetBatches(BatcherfassungSelektor selector)
        {
            Z_M_EC_AVM_BATCH_SELECT.Init(SAP);
                               
            var vgList = AppModelMappings.Z_M_EC_AVM_BATCH_SELECT_GT_IN_From_Batcherfassung.CopyBack(new List<BatcherfassungSelektor>(){ selector });
            SAP.ApplyImport(vgList);

            SAP.Execute();
           
            var outList = Z_M_EC_AVM_BATCH_SELECT.GT_OUT.GetExportList(SAP);
            var weblist = AppModelMappings.Z_M_EC_AVM_BATCH_SELECT_GT_OUT_To_Batcherfassung.Copy(outList).ToList();
           
            return weblist;
        }

        public List<FzgByUnitnummer> GetUnitnummern(string batchId)
        {
            Z_M_EC_AVM_BATCH_UNIT_SELECT.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_BATCH_ID", batchId);
            var outList = Z_M_EC_AVM_BATCH_UNIT_SELECT.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_M_EC_AVM_BATCH_UNIT_SELECT_GT_OUT_To_Unitnummer.Copy(outList).ToList();
        }

        public List<Auftragsnummer> GetAuftragsnummern()
        {
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_KENNUNG", "AUFTRAGSNUMMER");            
            var outList = Z_DPM_READ_AUFTR_006.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_To_Auftragsnummer.Copy(outList).ToList();
        }

        public List<ModelHersteller> GetModelHersteller()
        {
            Z_DPM_READ_MODELID_TAB.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.Execute();

            var sapItems = Z_DPM_READ_MODELID_TAB.GT_OUT.GetExportList(SAP);
            var webItems = AppModelMappings.Z_DPM_READ_MODELID_TAB_GT_OUT_To_ModelHersteller.Copy(sapItems).ToList();

            return webItems;

        }
        
        public string SaveBatches(Batcherfassung batcherfassung)
        {
            var error = SAP.ExecuteAndCatchErrors(
                
                // exception safe SAP action:
                () =>
                {                    
                    Z_M_EC_AVM_BATCH_INSERT.Init(SAP);
                    var vgList = AppModelMappings.Z_M_EC_AVM_BATCH_INSERT_ZBATCH_IN_From_Batcherfassung.CopyBack(new List<Batcherfassung>() { batcherfassung });
                    SAP.ApplyImport(vgList);

                    SAP.Execute();

                    var outList = Z_M_EC_AVM_BATCH_INSERT.GT_IN.GetExportList(SAP);

                    // TODO -> wie geht das mit den Unitnummern??
                    //batcherfassung.ForEach(x => { outList.Where(x => x.ZUNIT_NR = 

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
