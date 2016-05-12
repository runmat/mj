using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;
using System;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.Services
{
    public class DokumenteOhneDatenDataServiceSAP : CkgGeneralDataServiceSAP, IDokumenteOhneDatenDataService
    {
        public List<DokumentOhneDaten> DokumenteOhneDaten { get { return PropertyCacheGet(() => LoadDokumenteOhneDatenFromSap().ToList()); } }

        public DokumenteOhneDatenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshDokumenteOhneDaten()
        {
            PropertyCacheClear(this, m => m.DokumenteOhneDaten);
        }

        private IEnumerable<DokumentOhneDaten> LoadDokumenteOhneDatenFromSap()
        {
            var sapList = Z_DPM_DOKUMENT_OHNE_DAT_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_DOKUMENT_OHNE_DAT_01_GT_OUT_To_DokumentOhneDaten.Copy(sapList);
        }

        public string SaveSperrvermerk(DokumentOhneDaten model)
        {
            string error = "";

            try
            {
                Z_DPM_SET_EQUI_REFERENZ.Init(SAP, "IMP_AG", LogonContext.KundenNr.ToSapKunnr());
                Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_FIN(SAP, model.Fahrgestellnummer);
                Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_REFERENZ1(SAP, model.Sperrvermerk);
                if(model.Sperrvermerk.IsNotNullOrEmpty())
                    Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_REFERENZ2(SAP, model.Referenz);
                else
                    Z_DPM_SET_EQUI_REFERENZ.SetImportParameter_IMP_REFERENZ2(SAP, "");


                SAP.Execute();
            }
            catch (Exception e)
            {
                error = "SAP error: " + e.Message;
            }            
            return error;
        }
    }
}
