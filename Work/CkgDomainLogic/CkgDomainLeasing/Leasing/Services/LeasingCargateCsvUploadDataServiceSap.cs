using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingCargateCsvUploadDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingCargateCsvUploadDataService
    {
        public LeasingCargateCsvUploadDataServiceSAP(ISapDataService sap) : base(sap)
        {
        }

        public List<LeasingCargateDisplayModel> GetCargateDisplayModel()
        {
            Z_DPM_READ_RUECKL_01.Init(SAP);
            SAP.Execute();
            var toReturn = Z_DPM_READ_RUECKL_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG", "10048516".ToSapKunnr());
            return ModelMapping.Copy<Z_DPM_READ_RUECKL_01.GT_OUT, LeasingCargateDisplayModel>(toReturn).ToList();
        }

        public void ValidateUploadCsv(List<LeasingCargateCsvUploadModel> uploadItems)
        {
            MarkForRefreshCsvUpload();
            uploadItems.ForEach(ValidateSingleUploadItem);
        }

        public bool SaveLeasingCargateCsvUpload(List<LeasingCargateCsvUploadModel> uploadItems)
        {
            var sapItems = ModelMapping.Copy<LeasingCargateCsvUploadModel, Z_DPM_SAVE_DAT_IN_RUECKL_01.GT_IN>(uploadItems, AppModelMappings.MapCarGateLeasingCsvUploadEntityToSAP).ToList();
            
            //
            // speichern:
            //
            var importList = Z_DPM_SAVE_DAT_IN_RUECKL_01.GT_IN.GetImportListWithInit(SAP, "I_AG", "10048516".ToSapKunnr());   
            importList.AddRange(sapItems);
            SAP.ApplyImport(sapItems);
            SAP.Execute();

            //
            // prüflesen:
            //
            MarkForRefreshCsvUpload();

            return true;
        }

        void ValidateSingleUploadItem(LeasingCargateCsvUploadModel item)
        {
            var errorList = ValidateUploadItem(item);
            item.ValidationErrors = errorList;
        }

        public IEnumerable<ValidationResult> ValidateUploadItem(LeasingCargateCsvUploadModel item)
        {
            //
            // Step 1. Data Annotation Validation
            //
            var errorList = new List<ValidationResult>();

            //
            // Step 2. Fluent Validaton
            //

            // Fin ist das wirklich eine vin?
            if (string.IsNullOrEmpty(item.Fin) || item.Fin.Length != 17)
                errorList.Add(new ValidationResult(Localize.VinInvalid, new[] { "Fin" }));

            // Standort : muss Hamburg, duisburg, oder Kiel beinhalten
            string[] standorte = new[]{"Hamburg", "Duisburg", "Kiel"};
            if (!standorte.Any(s => item.Standort.ToUpper().Contains(s.ToUpper())))
            {
                errorList.Add(new ValidationResult(string.Format(Localize.GivenLocationNotListed, string.Join(", ", standorte)), new[] { "Standort" }));
            }

            return errorList;
        }

        public void MarkForRefreshCsvUpload()
        {
        }
    }
}
