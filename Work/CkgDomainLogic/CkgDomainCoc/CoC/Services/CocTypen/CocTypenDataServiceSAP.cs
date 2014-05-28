using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.CoC.Services
{
    public class CocTypenDataServiceSAP : CkgGeneralDataServiceSAP, ICocTypenDataService
    {
        public List<CocEntity> CocTypen { get { return PropertyCacheGet(() => new List<CocEntity>()); } }


        public CocTypenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void CocTypenMarkForRefresh()
        {
            PropertyCacheSet(this, m => m.CocTypen, LoadFromSap());
        }

        public CocEntity SaveCocTyp(CocEntity cocTyp, Action<string, string> addModelError)
        {
            var storedItem = StoreToSap(cocTyp, addModelError, false);
            return ModelMapping.Copy(storedItem, CocTypen.First(c => c.ID == storedItem.ID));
        }

        public void DeleteCocTyp(CocEntity cocTyp)
        {
            StoreToSap(cocTyp, null, true);
            CocTypen.RemoveAll(c => c.ID == cocTyp.ID);
        }

        List<CocEntity> LoadFromSap(string typ = null, string var = null, string vers = null)
        {
            Z_DPM_COC_TYPDATEN.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_VERKZ", "L");

            if (typ.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_TYP", typ);
            if (var.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_VARIANTE", var);
            if (vers.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_VERSION", vers);

            var sapList = Z_DPM_COC_TYPDATEN.GT_WEB.GetExportListWithExecute(SAP);

            // Nur "Nicht-Vorlagen" selektieren:
            sapList = sapList.Where(s => s.VORLAGE.IsNullOrEmpty()).ToList();

            return ModelMapping.Copy<Z_DPM_COC_TYPDATEN.GT_WEB, CocEntity>(sapList, AppModelMappings.MapCocTypdatenFromSAP).ToList();
        }

        CocEntity StoreToSap(CocEntity cocTyp, Action<string, string> addModelError, bool deleteOnly)
        {
            var sapCocTyp = ModelMapping.Copy(cocTyp, new Z_DPM_COC_TYPDATEN.GT_WEB(), AppModelMappings.MapCocTypdatenToSAP);
            if (sapCocTyp.KUNNR.IsNullOrEmpty())
                sapCocTyp.KUNNR = LogonContext.KundenNr.ToSapKunnr();

            Z_DPM_COC_TYPDATEN.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_VERKZ", (deleteOnly ? "D" : "U"));

            try
            {
                var importList = Z_DPM_COC_TYPDATEN.GT_WEB.GetImportList(SAP);
                importList.Add(sapCocTyp);
                SAP.ApplyImport(importList);
                SAP.Execute();
            }
            catch (Exception e)
            {
                if (addModelError != null)
                {
                    var errorPropertyName = e.Message.GetPartEnclosedBy('\'');
                    if (errorPropertyName.IsNullOrEmpty())
                        errorPropertyName = "SapError";

                    addModelError(errorPropertyName, e.Message);
                }
            }

            //
            // prüflesen:
            //
            if (!deleteOnly)
            {
                var savedID = cocTyp.ID;
                var savedCocTyp = LoadFromSap(cocTyp.COC_0_2_TYP, cocTyp.COC_0_2_VAR, cocTyp.COC_0_2_VERS).FirstOrDefault();
                if (savedCocTyp != null)
                    savedCocTyp.ID = savedID;

                if (addModelError != null)
                    ModelMapping.Differences(cocTyp, savedCocTyp).ForEach(differentPropertyName => addModelError(differentPropertyName, "Wert wurde aus unbekannten Gründen nicht gespeichert."));

                //cocTyp = savedCocTyp;
            }

            return cocTyp;
        }
    }
}
