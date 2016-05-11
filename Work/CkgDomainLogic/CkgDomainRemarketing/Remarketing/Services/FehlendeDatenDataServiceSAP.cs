using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Remarketing.Models.AppModelMappings;

namespace CkgDomainLogic.Remarketing.Services
{
    public class FehlendeDatenDataServiceSAP : CkgGeneralDataServiceSAP, IFehlendeDatenDataService
    {
        public FehlendeDatenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Vermieter> GetVermieter()
        {
            Z_DPM_READ_AUFTR6_001.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), "VERMIETER");

            return AppModelMappings.Z_DPM_READ_AUFTR6_001_GT_WEB_To_Vermieter.Copy(Z_DPM_READ_AUFTR6_001.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public List<FehlendeDaten> GetFehlendeDaten(FehlendeDatenSelektor selektor, List<SimpleUploadItem> uploadList)
        {
            Z_DPM_READ_REM_002.Init(SAP, "I_KUNNR_AG, I_BESTANDSART", LogonContext.KundenNr.ToSapKunnr(), selektor.Bestandsart);

            switch (selektor.Auswahl)
            {
                case "A":
                    Z_DPM_READ_REM_002.SetImportParameter_I_AVNR(SAP, selektor.Vermieter);

                    if (selektor.DatumRange.IsSelected)
                    {
                        Z_DPM_READ_REM_002.SetImportParameter_I_EGZB2_VON(SAP, selektor.DatumRange.StartDate);
                        Z_DPM_READ_REM_002.SetImportParameter_I_EGZB2_BIS(SAP, selektor.DatumRange.EndDate);
                    }
                    break;

                case "F":
                    var finList = new List<Z_DPM_READ_REM_002.GT_IN_FIN>();
                    uploadList.ForEach(fin => finList.Add(new Z_DPM_READ_REM_002.GT_IN_FIN { CHASSIS_NUM = fin.Wert }));
                    SAP.ApplyImport(finList);
                    break;

                case "K":
                    var kennzList = new List<Z_DPM_READ_REM_002.GT_IN_KENNZ>();
                    uploadList.ForEach(kennz => kennzList.Add(new Z_DPM_READ_REM_002.GT_IN_KENNZ { LICENSE_NUM = kennz.Wert }));
                    SAP.ApplyImport(kennzList);
                    break;
            }

            return AppModelMappings.Z_DPM_READ_REM_002_GT_WEB_To_FehlendeDaten.Copy(Z_DPM_READ_REM_002.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }
    }
}
