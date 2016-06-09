using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.Contracts;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Remarketing.Models.AppModelMappings;

namespace CkgDomainLogic.Remarketing.Services
{
    public class FehlendeDatenDataServiceSAP : RemarketingDataServiceSAP, IFehlendeDatenDataService
    {
        public FehlendeDatenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
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
