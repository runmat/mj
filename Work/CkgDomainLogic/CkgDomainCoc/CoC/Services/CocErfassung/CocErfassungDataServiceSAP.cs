using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.CoC.Models.AppModelMappings;

namespace CkgDomainLogic.CoC.Services
{
    public class CocErfassungDataServiceSAP : CkgGeneralDataServiceSAP, ICocErfassungDataService
    {
        private IEnumerable<Z_M_BLOCKEN_FARBEN.FARBE> SapFarben { get { return PropertyCacheGet(() => Z_M_BLOCKEN_FARBEN.FARBE.GetExportListWithInitExecute(SAP)); } }

        private IEnumerable<Z_DPM_READ_COC_01.GT_OUT> SapCocAuftraege { get { return PropertyCacheGet(() => GetSapCocAuftraege().ToList()); } }

        private IEnumerable<Z_DPM_READ_COC_01.GT_OUT> SapCocAuftraegeGedruckt { get { return SapCocAuftraege.Where(SapOrderIsPrinted); } }
        private IEnumerable<Z_DPM_READ_COC_01.GT_OUT> SapCocAuftraegeNichtGedruckt { get { return SapCocAuftraege.Where(SapOrderIsNotPrinted); } }

        private List<Z_DPM_TYPDATEN_02.GT_WEB> SapCocTypdaten { get { return PropertyCacheGet(() => new List<Z_DPM_TYPDATEN_02.GT_WEB>()); } }
        
        public List<CocEntity> CocAuftraege { get { return PropertyCacheGet(() => AppModelMappings.Z_DPM_READ_COC_01_GT_OUT_To_CocEntity.Copy(SapCocAuftraege).ToList()); } }


        public CocErfassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public bool SaveUploadCocOrdersWithProofReading(List<CsvUploadEntityDpmCoc> items)
        {
            SaveUploadCocOrders(items);

            //
            // prüflesen:
            //
            MarkForRefreshCocOrders();

            var failedSavedVins = new List<string>();
            items.ForEach(item =>
                {
                    if (SapCocAuftraegeNichtGedruckt.None(savedItem => item.VIN.NotNullOrEmpty().ToUpper() == savedItem.VIN.NotNullOrEmpty().ToUpper()))
                        failedSavedVins.Add(item.VIN);
                });

            return failedSavedVins.None();
        }

        private void SaveUploadCocOrders(List<CsvUploadEntityDpmCoc> items)
        {
            var sapItems = ModelMapping.Copy<CsvUploadEntityDpmCoc, Z_DPM_UPD_COC_01.GT_DAT>(items, AppModelMappings.MapCsvUploadEntityDpmCocToSAP).ToList();
            sapItems.ForEach(sapItem =>
            {
                var existingOrderUnprinted = SapCocAuftraegeNichtGedruckt.FirstOrDefault(nichtGedruckt => nichtGedruckt.VIN.NotNullOrEmpty().ToUpper() == sapItem.VIN.NotNullOrEmpty().ToUpper());

                sapItem.AKTION = existingOrderUnprinted != null ? "U" : "I";
                sapItem.VORG_NR = existingOrderUnprinted != null ? existingOrderUnprinted.VORG_NR : null;
            });

            //
            // speichern:
            //
            Z_DPM_UPD_COC_01.Init(SAP);
            SAP.ApplyImport(sapItems);
            SAP.Execute();
        }

        IEnumerable<Z_DPM_READ_COC_01.GT_OUT> GetSapCocAuftraege()
        {
            return Z_DPM_READ_COC_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());
        }

        Z_DPM_TYPDATEN_02.GT_WEB GetSapCocTypdaten(string herstellerCode, string typCode, string vvsCode)
        {
            Func<Z_DPM_TYPDATEN_02.GT_WEB, bool> filterTypdaten = (s => s.ZZHERSTELLER_SCH == herstellerCode && s.ZZTYP_SCHL == typCode && s.ZZVVS_SCHLUESSEL == vvsCode);
            if (SapCocTypdaten.None(filterTypdaten))
                SapCocTypdaten.AddRange(Z_DPM_TYPDATEN_02.GT_WEB.GetExportListWithInitExecute(SAP, "I_ZZHERSTELLER_SCH, I_ZZTYP_SCHL, I_ZZVVS_SCHLUESSEL", herstellerCode, typCode, vvsCode));

            return SapCocTypdaten.FirstOrDefault(filterTypdaten);
        }

        private static bool SapOrderIsPrinted(Z_DPM_READ_COC_01.GT_OUT sapItem)
        {
            return sapItem.AUFTRAG_DAT != null;
        }

        private static bool SapOrderIsNotPrinted(Z_DPM_READ_COC_01.GT_OUT sapItem)
        {
            return !SapOrderIsPrinted(sapItem);
        }

        public void MarkForRefreshCocOrders()
        {
            PropertyCacheClear(this, m => m.SapCocAuftraege);
            PropertyCacheClear(this, m => m.CocAuftraege);

            PropertyCacheClear(this, m => m.SapCocTypdaten);
        }

        public CocEntity SaveCocOrder(CocEntity cocTyp, Action<string, string> addModelError)
        {
            cocTyp.ZBII_D_1 = cocTyp.COC_0_1;
            cocTyp.ZBII_D_2_TYP = cocTyp.COC_0_2_TYP;
            cocTyp.ZBII_D_2_VARIANTE = cocTyp.COC_0_2_VAR;
            cocTyp.ZBII_D_2_VERSION = cocTyp.COC_0_2_VERS;
            cocTyp.ZBII_J = cocTyp.COC_0_4;
            cocTyp.ZBII_4 = cocTyp.COC_38;
            cocTyp.ZBII_K = cocTyp.COC_EG_TYP_GEN.NotNullOrEmpty();
            cocTyp.ZBII_6 = cocTyp.COC_EG_TYP_GEN_DAT.NotNullOrEmptyToString();
            cocTyp.ERDAT = DateTime.Today;

            return StoreToSap(cocTyp, addModelError, false);
        }

        IEnumerable<CocEntity> LoadFromSap(string typ = null, string var = null, string vers = null)
        {
            var cocOrders = GetSapCocAuftraege()
                .Where(o => o.COC_0_2_TYP == typ && o.COC_0_2_VAR == var && o.COC_0_2_VERS == vers)
                .OrderByDescending(c => c.VORG_NR);

            return AppModelMappings.Z_DPM_READ_COC_01_GT_OUT_To_CocEntity.Copy(cocOrders);
        }

        private CocEntity StoreToSap(CocEntity cocTyp, Action<string, string> addModelError, bool deleteOnly)
        {
            var sapCocTyp = AppModelMappings.Z_DPM_UPD_COC_01_GT_DAT_From_CocEntity.CopyBack(cocTyp);
            sapCocTyp.AKTION = (sapCocTyp.VORG_NR.IsNullOrEmpty() ? "I" : "U");

            if (sapCocTyp.KUNNR_AG.IsNullOrEmpty())
                sapCocTyp.KUNNR_AG = LogonContext.KundenNr.ToSapKunnr();

            try
            {
                var importList = Z_DPM_UPD_COC_01.GT_DAT.GetImportListWithInit(SAP);
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
            MarkForRefreshCocOrders();

            if (!deleteOnly)
            {
                var savedID = cocTyp.ID;
                var savedCocTypen = LoadFromSap(cocTyp.COC_0_2_TYP, cocTyp.COC_0_2_VAR, cocTyp.COC_0_2_VERS);
                var savedCocTyp = savedCocTypen.FirstOrDefault();
                if (sapCocTyp.VORG_NR.IsNotNullOrEmpty())
                    savedCocTyp = savedCocTypen.FirstOrDefault(c => c.VORG_NR == sapCocTyp.VORG_NR);

                if (savedCocTyp != null)
                    savedCocTyp.ID = savedID;

                if (addModelError != null)
                    ModelMapping.Differences(cocTyp, savedCocTyp).ForEach(differentPropertyName => addModelError(differentPropertyName, "Wert wurde aus unbekannten Gründen nicht gespeichert."));

                //cocTyp = savedCocTyp;
            }

            if (cocTyp != null)
                cocTyp.IsCocOrder = true;

            return cocTyp;
        }

        #region Validation

        public void ValidateUploadCocOrders(List<CsvUploadEntityDpmCoc> uploadItems)
        {
            MarkForRefreshCocOrders();

            // 1. single item validation
            uploadItems.ForEach(ValidateSingleUploadItem);

            // 2. overall items validation
            ValidateOverallUploadItems(uploadItems);
        }

        static void ValidateOverallUploadItems(List<CsvUploadEntityDpmCoc> uploadItems)
        {
            // duplicate VIN check overall upload items:
            var duplicateVins = uploadItems.GroupBy(group => group.VIN).Where(g => g.Count() > 1).Select(g => g.Key.NotNullOrEmpty().ToUpper()).ToList();
            uploadItems.Where(item => duplicateVins.Contains(item.VIN.NotNullOrEmpty().ToUpper())).ToList().ForEach(item => item.ValidationErrors += ",VIN");
        }

        void ValidateSingleUploadItem(CsvUploadEntityDpmCoc item)
        {
            var errorList = ValidateUploadItem(item);
            item.ValidationErrors = string.Join(",", errorList);
        }

        public IEnumerable<string> ValidateUploadItem(CsvUploadEntityDpmCoc item)
        {
            item.CustomerNo = LogonContext.KundenNr.ToSapKunnr();

            //
            // Step 1. Data Annotation Validation
            //
            var errorList = ValidationService.ValidateDataAnnotations(item).SelectMany(r => r.MemberNames).ToList();

            //
            // Step 2. Fluent Validaton
            //

            // country check:
            if (!IsValidCountryCode(item.Country))
                errorList.Add("Country");

            // color check:
            if (item.Color.IsNumeric() && !IsValidColorCode(item.Color))
                errorList.Add("Color");
            if (!item.Color.IsNumeric() && !IsValidColorName(item.Color))
                errorList.Add("Color");

            // order check:
            if (item.OrderID.IsNullOrEmpty())
                errorList.Add("OrderID");

            // VIN check:
            // Nur wenn nicht bereits mit selber VIN vorhanden mit gesetzter Auftrags-Nr (bzw. bereits gedruckt)
            if (SapCocAuftraegeGedruckt.Any(auftragGedruckt => auftragGedruckt.VIN.NotNullOrEmpty().ToUpper() == item.VIN.NotNullOrEmpty().ToUpper()))
                errorList.Add("VIN");
            
            // moved to step 1, see above (Data Annotation Validation)
            //if (item.VIN.IsNullOrEmpty())
            //    errorList.Add("VIN");


            // Typdaten check:
            // Nur wenn es diesen Typ bereits gibt
            var sapItem = ModelMapping.Copy(item, new Z_DPM_UPD_COC_01.GT_DAT(), AppModelMappings.MapCsvUploadEntityDpmCocToSAP);

            var existingTyp = GetSapCocTypdaten(sapItem.ZBII_2_1, sapItem.ZBII_2_2_TYP, sapItem.ZBII_2_2_VVS);
            if (existingTyp == null)
            {
                errorList.Add("CodeManufacturer");
                errorList.Add("CodeTypeVersion");
            }

            return errorList;
        }

        bool IsValidColorCode(string colorCode)
        {
            return SapFarben.Any(farbe => farbe.DOMVALUE_L == colorCode);
        }

        bool IsValidColorName(string colorName)
        {
            return SapFarben.Any(farbe => farbe.DDTEXT.ToUpper() == colorName.NotNullOrEmpty().ToUpper());
        }

        bool IsValidCountryCode(string countryCode)
        {
            return SapLaender.Any(land => land.LAND1.ToUpper() == countryCode.NotNullOrEmpty().ToUpper());
        }

        #endregion

        
        #region Versand Beauftragung

        public string SaveVersandBeauftragung(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen)
        {
            return SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () => StoreVersandBeauftragungToSap(cocAuftraege, druckOptionen),

                // SAP custom error handling:
                () => "");
        }

        public string SaveVersandDuplikatDruckBeauftragung(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen)
        {
            return SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () => StoreVersandDuplikatDruckBeauftragungToSap(cocAuftraege, druckOptionen),

                // SAP custom error handling:
                () => "");
        }

        public string SaveZulassung(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen, string land)
        {
            return SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () => StoreZulassungToSap(cocAuftraege, druckOptionen, land),

                // SAP custom error handling:
                () => "");
        }

        private void StoreCocAuftraegeToSap(List<CocEntity> cocAuftraege, Action<CocEntity> cocUpdateFunction, bool insertMode = false)
        {
            var sapReadItems = SapCocAuftraege.Where(s => cocAuftraege.Any(c => s.VORG_NR.NotNullOrEmpty().ToUpper() == c.VORG_NR.NotNullOrEmpty().ToUpper()));
            //if (sapReadItems.Count() != cocAuftraege.Count())
            //    throw new Exception(string.Format("Mindestens eine der markierten VINs wurde nicht über Bapi Z_DPM_READ_COC_01 gefunden"));
            if (cocAuftraege.Count > sapReadItems.Count(s => cocAuftraege.Any(c => s.VORG_NR == c.VORG_NR)))
                throw new Exception(string.Format("Mindestens eine der markierten VINs wurde nicht über Bapi Z_DPM_READ_COC_01 gefunden"));

            var sapUpdItemList = Z_DPM_UPD_COC_01.GT_DAT.GetImportListWithInit(SAP);

            sapReadItems.ToList().ForEach(sapReadItem =>
            {
                var cocTyp = AppModelMappings.Z_DPM_READ_COC_01_GT_OUT_To_CocEntity.Copy(sapReadItem);
                cocUpdateFunction(cocTyp);
                var sapUpdItem = AppModelMappings.Z_DPM_UPD_COC_01_GT_DAT_From_CocEntity.CopyBack(cocTyp);

                sapUpdItem.AKTION = insertMode ? "I" : "U";
                sapUpdItem.VORG_NR = insertMode ? null : sapReadItem.VORG_NR;

                sapUpdItemList.Add(sapUpdItem);
            });


            // Coc Aufträge speichern  
            Z_DPM_UPD_COC_01.Init(SAP);
            SAP.ApplyImport(sapUpdItemList);
            SAP.Execute();


            // Coc Auftrags cache leeren
            MarkForRefreshCocOrders();
        }

        private void StoreVersandBeauftragungToSap(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen)
        {
            StoreCocAuftraegeToSap(cocAuftraege, cocItem =>
            {

                //
                if (druckOptionen.DruckCoc)
                    cocItem.COC_DRUCK_ORIG = "X";

                if (druckOptionen.DruckZBII)
                    cocItem.ZBII_DRUCK = "X";

                cocItem.VERSAND = "1";

                cocItem.AUFTRAG_DAT = DateTime.Today;
                //

            });
        }

        private void StoreVersandDuplikatDruckBeauftragungToSap(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen)
        {
            StoreCocAuftraegeToSap(cocAuftraege, cocItem =>
            {

                //

                cocItem.AUFTRAG_DAT = null;
                cocItem.COC_KD_ORIG = null;
                cocItem.COC_KD_KOPIE = null;
                cocItem.COC_DRUCK_ORIG = null;
                cocItem.COC_DRUCK_KOPIE = null;
                cocItem.COC_DRUCK_DATUM = null;
                cocItem.COC_DRUCK_ZEIT = null;
                cocItem.ZBII_DRUCK = null;
                cocItem.ZBII_DRUCK_DATUM = null;
                cocItem.ZBII_DRUCK_ZEIT = null;
                cocItem.ZBII_KBA_MELD = null;
                cocItem.VERSAND = null;
                cocItem.ZUL_DEZ = null;
                cocItem.ZUL_AUSLAND = null;
                cocItem.ZUL_EXPORT = null;

                if (druckOptionen.ModusEigenDruck)
                {
                    cocItem.COC_KD_KOPIE = "X";
                    cocItem.COC_DRUCK_DATUM = DateTime.Today;
                    cocItem.COC_DRUCK_ZEIT = DateTime.Now.ToString("HHmmss");
                }
                else
                {
                    cocItem.COC_DRUCK_KOPIE = "X";
                    cocItem.VERSAND = "1";
                }

                cocItem.AUFTRAG_DAT = DateTime.Today;
                //

            }, 
            
            // set "insertMode" here, so we will duplicate existing items
            insertMode: true);
        }

        private void StoreZulassungToSap(List<CocEntity> cocAuftraege, DruckOptionen druckOptionen, string land)
        {
            StoreCocAuftraegeToSap(cocAuftraege, cocItem =>
            {

                //
                if (druckOptionen.DruckCoc)
                    cocItem.COC_DRUCK_ORIG = "X";

                if (druckOptionen.DruckZBII)
                    cocItem.ZBII_DRUCK = "X";

                if (land.NotNullOrEmpty().ToUpper() == "DE")
                    cocItem.ZUL_DEZ = "1";
                else
                    cocItem.ZUL_AUSLAND = "1";

                cocItem.AUFTRAG_DAT = DateTime.Today;
                //

            });
        }

        public byte[] GetCocAsPdf(string vorgangNr, string vorlage)
        {
            SAP.InitExecute("Z_DPM_Print_Coc_01", "I_AG, I_VORG_NR, I_VKZ, I_PDF", LogonContext.KundenNr.ToSapKunnr(), vorgangNr, vorlage.ToUpper(), "X");
            var pdfAsByteArray = SAP.GetExportParameterByte("E_PDF");
            if (SAP.ResultCode != 0)
                return null;

            return pdfAsByteArray;
        }

        public void StoreCocPdfSelbstDruck(CocEntity cocEntity, string vorlage)
        {
            StoreCocAuftraegeToSap(new List<CocEntity> { cocEntity }, cocItem =>
            {

                //
                if (vorlage.ToUpper() == "O")
                {
                    // Original Druck
                    cocItem.COC_KD_ORIG = "X";
                }

                cocItem.COC_DRUCK_DATUM = DateTime.Today;
                cocItem.COC_DRUCK_ZEIT = DateTime.Now.ToString("HHmmss");

                cocItem.AUFTRAG_DAT = DateTime.Today;
                //

            });
        }
        #endregion
    }
}
