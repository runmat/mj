// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FehlteilEtikettenDataServiceSAP : CkgGeneralDataServiceSAP, IFehlteilEtikettenDataService
    {
        public FehlteilEtikettenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }


        private void MarkDataForRefresh()
        {
        }


        public void ValidateUploadItems(List<FehlteilEtikett> uploadItems)
        {
            MarkDataForRefresh();

            // 1. single item validation
            uploadItems.ForEach(ValidateSingleUploadItem);

            // 2. overall items validation
            ValidateOverallUploadItems(uploadItems);
        }

        private static void ValidateOverallUploadItems(List<FehlteilEtikett> uploadItems)
        {
            // duplicate VIN check overall upload items:
            var duplicateVins =
                uploadItems.GroupBy(group => group.VIN)
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key.NotNullOrEmpty().ToUpper())
                           .ToList();
            uploadItems.Where(item => duplicateVins.Contains(item.VIN.NotNullOrEmpty().ToUpper()))
                       .ToList()
                       .ForEach(item => item.ValidationErrors += ",VIN");
        }

        private void ValidateSingleUploadItem(FehlteilEtikett item)
        {
            var errorList = ValidateUploadItem(item);
            item.ValidationErrors = string.Join(",", errorList);
        }

        public IEnumerable<string> ValidateUploadItem(FehlteilEtikett item)
        {
            item.CustomerNo = LogonContext.KundenNr.ToSapKunnr();

            //
            // Step 1. Data Annotation Validation
            //
            var errorList = ValidationService.ValidateDataAnnotations(item).SelectMany(r => r.MemberNames).ToList();

            //
            // Step 2. Fluent Validaton
            //
            // propbably implemented later on

            return errorList;
        }

        public string InsertItems(List<FehlteilEtikett> items)
        {
            return SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                    {
                        var sapItems =
                            AppModelMappings.Z_DPM_IMP_FEHLTEILETIK_01__GT_DATEN_To_FehlteilEtikett.CopyBack(items)
                                            .ToList();
                        // speichern:
                        Z_DPM_IMP_FEHLTEILETIK_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                        SAP.ApplyImport(sapItems);
                        SAP.Execute();
                    });
        }

        public FehlteilEtikett SaveItem(FehlteilEtikett item, Action<string, string> addModelError)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                    {
                        var sapItem = AppModelMappings.Z_DPM_DRUCK_FEHLTEILETIK_GT_ETIKETT_To_FehlteilEtikett.CopyBack(item);
                        // speichern:
                        Z_DPM_DRUCK_FEHLTEILETIK.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                        SAP.SetImportParameter("I_VERART", "S");

                        SAP.ApplyImport(new List<Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT> {sapItem});
                        
                        SAP.Execute();
                    });

            if (error.IsNotNullOrEmpty())
                addModelError("", error);

            return item;
        }

        public FehlteilEtikett LoadItem(string vin)
        {
            var resultCode = 101;
            while (resultCode == 101)
            {
                var error = SAP.ExecuteAndCatchErrors(

                    // exception load SAP action:
                    () =>
                        {
                            Z_DPM_DRUCK_FEHLTEILETIK.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                            SAP.SetImportParameter("I_VERART", "L");
                            //SAP.SetImportParameter("I_POSITION", "1");

                            var importList = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetImportList(SAP);
                            importList.Add(new Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT { CHASSIS_NUM = vin.NotNullOrEmpty().ToUpper() });
                            SAP.ApplyImport(importList);

                            SAP.Execute();
                        });

                resultCode = SAP.ResultCode;
                if (resultCode == 101)
                {
                    // Code 101 => Datensatz nicht gefunden, also legen wir hier einen Datensatz an!
                    InsertItems(new List<FehlteilEtikett> { new FehlteilEtikett { VIN = vin } });
                }
                else if (error.IsNotNullOrEmpty())
                    return null;
            }

            var sapItems = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetExportList(SAP);
            if (sapItems == null || sapItems.None())
                return null;

            return AppModelMappings.Z_DPM_DRUCK_FEHLTEILETIK_GT_ETIKETT_To_FehlteilEtikett.Copy(sapItems[0]);
        }

        public void GetEtikettAsPdf(FehlteilEtikett item, out string errorMessage, out byte[] pdfBytes)
        {
            errorMessage = SAP.ExecuteAndCatchErrors(

                // exception load SAP action:
                () =>
                {
                    Z_DPM_DRUCK_FEHLTEILETIK.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_VERART", "D");
                    SAP.SetImportParameter("I_POSITION", item.LayoutPosition.ToString());

                    var importList = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetImportList(SAP);
                    var sapItem = AppModelMappings.Z_DPM_DRUCK_FEHLTEILETIK_GT_ETIKETT_To_FehlteilEtikett.CopyBack(item);
                    importList.Add(sapItem);
                    SAP.ApplyImport(importList);

                    SAP.Execute();
                });


            pdfBytes = SAP.GetExportParameterByte("E_PDF");
        }
    }
}
