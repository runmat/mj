// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;



namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class TreuhandDataServiceSAP : CkgGeneralDataServiceSAP, ITreuhandDataService
    {
        public TreuhandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }


        void ValidateAblehnen(List<Treuhandbestand> items)
        { 
              foreach (var item in items)
                  if (!item.IsActionFreigeben && item.Ablehnungsgrund.IsNullOrEmpty())
                  {
                      item.ValidationErrors += "Ablehnungsgrnd erforderlich;";                  
                  }
        }

        public void FreigebenAblehnen(TreuhandverwaltungSelektor selector)
        {
            var items = TreuhandverwaltungSelektor.GetViewModel().TreuhandbestandsFiltered;

            foreach (var item in items)
                item.IsActionFreigeben = selector.IsActionFreigeben;

            ValidateAblehnen(items);

            Z_DPM_FREIG_VERSAND_SPERR_001.Init(SAP);

            SAP.SetImportParameter("I_FREIGABEUSER", LogonContext.UserName);
           
            var vgList = AppModelMappings.Z_DPM_FREIG_VERSAND_SPERR_001_GT_WEB_From_Treuhandbestand.CopyBack(items.Where(x => x.IsSelected && x.ValidationErrors.IsNullOrEmpty())).ToList();
            SAP.ApplyImport(vgList);
            
            SAP.Execute();

            var sapItems = Z_DPM_FREIG_VERSAND_SPERR_001.GT_WEB.GetExportList(SAP);                            
        }


        public List<Treuhandbestand> GetTreuhandfreigabeFromSap(TreuhandverwaltungSelektor selector)
        {
            Z_DPM_READ_VERSAND_SPERR_001.Init(SAP);

            if (selector.IsActionShowGesperrte)
                SAP.SetImportParameter("I_AKTION", "G");
            else
                SAP.SetImportParameter("I_AKTION", "A");
            
            SAP.SetImportParameter("I_TREU", selector.AGNummer);                        
            SAP.SetImportParameter("I_Name", LogonContext.LastName);
            SAP.SetImportParameter("I_Vorna", LogonContext.FirstName);
            string email = ((ILogonContextDataService)LogonContext).GetEmailAddressForUser();
            SAP.SetImportParameter("I_EMAIL", email);
            
            if (selector.AGNummer.IsNotNullOrEmpty())
            {
                SAP.SetImportParameter("I_AG", selector.TGNummer);
            }

            SAP.Execute();

            var sapItemsEquis = Z_DPM_READ_VERSAND_SPERR_001.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_READ_VERSAND_SPERR_001_GT_OUT_ToTreuhandfreigabe.Copy(sapItemsEquis).ToList();

            return webItemsEquis;

        }

        public List<Treuhandbestand> GetTreuhandbestandFromSap(TreuhandverwaltungSelektor selector)
        {            
            string agnummer = selector.AGNummer;

            Z_M_TH_BESTAND.Init(SAP);
           
            if (selector.Reporttype == ReportType.TG)
            {                                
                SAP.SetImportParameter("I_TG", LogonContext.KundenNr);

                if (selector.AGNummer.IsNotNullOrEmpty())
                {
                    SAP.SetImportParameter("I_AG", agnummer);
                }
            }
            else // AG
            {                
                SAP.SetImportParameter("I_AG", LogonContext.KundenNr);

                if (selector.TGNummer.IsNotNullOrEmpty())
                {
                    SAP.SetImportParameter("I_TG", agnummer);
                }
            }
                                                   
            SAP.SetImportParameter("I_EQTYP", "B");            
            
            SAP.Execute();

            var sapItemsEquis = Z_M_TH_BESTAND.GT_BESTAND.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_M_TH_BESTAND__GET_BESTAND_LIST_To_Treuhandbestand.Copy(sapItemsEquis).ToList();

            return webItemsEquis;
        }

        private List<TreuhandKunde> _treuhandKundenFromSap;
        public List<TreuhandKunde> GetTreuhandKundenFromSap(TreuhandverwaltungSelektor selector)
        {
            //if (_treuhandKundenFromSap == null)
                GetKundenFromSAP(selector, false);
            return _treuhandKundenFromSap; 
        } 
        
        private List<TreuhandKunde> _auftraggeberKundenFromSap;
        public List<TreuhandKunde> GetAuftraggeberFromSap(TreuhandverwaltungSelektor selector) 
        {
            //if (_auftraggeberKundenFromSap == null)
                GetKundenFromSAP(selector, true);
            return _auftraggeberKundenFromSap; 
        }
       

        private void GetKundenFromSAP(TreuhandverwaltungSelektor selector, bool isAuftraggeber)
        {
            Z_M_TH_GET_TREUH_AG.Init(SAP, "I_TREU", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_EQTYP", "B");

            if (isAuftraggeber)
                SAP.SetImportParameter("I_ALL_AG", "X");

            SAP.Execute();

            var sapItems = Z_M_TH_GET_TREUH_AG.GT_EXP.GetExportList(SAP);

            var tgList = sapItems.Where(x => x.ZSELECT == "TG").ToList();
            var agList = sapItems.Where(x => x.ZSELECT == "AG").ToList();

            _treuhandKundenFromSap = AppModelMappings.Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKunden.Copy(tgList).ToList();

            if (!isAuftraggeber && selector.Reporttype != ReportType.TG && tgList.Count == 0 && agList.Count > 0)
                _treuhandKundenFromSap = AppModelMappings.Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKundenAGServices.Copy(agList).ToList();
            
            _auftraggeberKundenFromSap = AppModelMappings.Z_M_TH_GET_TREUH_AG_GT_EXP_ToTreuhandKundenAG.Copy(agList).ToList();
                                               
        }


        public TreuhandverwaltungSelektor GetBerechtigungenFromSap(TreuhandverwaltungSelektor selector)
        {           
            string tgNummer = selector.TGNummer.ToSapKunnr();
            string agNummer = selector.AGNummer.ToSapKunnr(); 

            if(selector.GetAGViewServicesMode)
            {
                string temptg = tgNummer;
                tgNummer = agNummer;
                agNummer = temptg;              
            }
                       
            Z_DPM_TREUHAND_AUTHORITY.Init(SAP);
            SAP.SetImportParameter("I_KUNNR_TG", tgNummer);
            SAP.SetImportParameter("I_KUNNR_TN", agNummer); 
            SAP.SetImportParameter("I_NAME", LogonContext.LastName);
            SAP.SetImportParameter("I_VORNA", LogonContext.FirstName);
            string email = ((ILogonContextDataService)LogonContext).GetEmailAddressForUser();
            SAP.SetImportParameter("I_EMAIL", email);
            SAP.Execute();

            selector.Treuhandberechtigung = new Treuhandberechtigung()
            {
                Freigeben = SAP.GetExportParameter("E_FREIGABE"),              
                Sperren = SAP.GetExportParameter("E_SPERREN"),
                Entsperren = SAP.GetExportParameter("E_ENTSPERREN")
            };

            return selector;
        }


        public void ValidateUploadTreuhandverwaltung(TreuhandverwaltungSelektor selector)
        {
            var uploadItems = selector.UploadItems;

            // 1. single item validation
            uploadItems.ForEach(ValidateSingleUploadItem);

            // 2. overall items validation
            ValidateOverallUploadItems(selector);

        }


        void ValidateOverallUploadItems(TreuhandverwaltungSelektor selector)
        {

            var uploadItems = selector.UploadItems;

            // duplicate VIN check overall upload items:
            var duplicateVins = uploadItems.GroupBy(group => group.Fahrgestellnummer).Where(g => g.Count() > 1).Select(g => g.Key.NotNullOrEmpty().ToUpper()).ToList();
            uploadItems.Where(item => duplicateVins.Contains(item.Fahrgestellnummer.NotNullOrEmpty().ToUpper())).ToList().ForEach(item => item.ValidationErrors += ",VIN");
        
            TreuhandverwaltungCsvUpload uploadItem = uploadItems.FirstOrDefault();

            Z_DPM_CHECK_TH_CODE.Init(SAP);
                        
            SAP.SetImportParameter("I_AG", uploadItem.AGNummer);
            SAP.SetImportParameter("I_TREU", uploadItem.TGNummer);
            SAP.SetImportParameter("I_TREUH_VGA", uploadItem.IsSperren ? "S" : "F");

            var vgList = AppModelMappings.Z_DPM_CHECK_TH_CODE_GT_IN_From_TreuhandverwaltungCsvUpload.CopyBack(uploadItems).ToList();
            SAP.ApplyImport(vgList);

            SAP.Execute();

            //if (SAP.ResultCode != 0)
            //     SAP.ResultMessage;

            var outList = Z_DPM_CHECK_TH_CODE.GT_OUT.GetExportList(SAP);

            foreach (var item in outList)
            {
                var errorItem = uploadItems.Where(x => x.Fahrgestellnummer == item.EQUI_KEY && item.ERROR != "").ToList().FirstOrDefault();

                if (errorItem != null)
                    errorItem.ValidationErrors += item.ERROR_TEXT + ",";
            }

        }

        public bool SaveUploadItems(List<TreuhandverwaltungCsvUpload> uploadItems)
        {
            bool succsessfullySaved = true;
          
            TreuhandverwaltungCsvUpload uploadItem = uploadItems.FirstOrDefault();

            try
            {               
                Z_M_TH_INS_VORGANG.Init(SAP);
                SAP.SetImportParameter("I_TREU", uploadItem.TGNummer);
                SAP.SetImportParameter("I_EQTYP", "B");
                // inAltApp if kombi-upload = true -> fix in cmd_save
                SAP.SetImportParameter("I_KOMBI", "X");

                var vgList = AppModelMappings.Z_M_TH_INS_VORGANG_GT_IN_From_TreuhandverwaltungCsvUpload.CopyBack(uploadItems.Where(x => x.IsSelected)).ToList();
                SAP.ApplyImport(vgList);

                SAP.Execute();

                if (SAP.ResultCode != 0)
                    succsessfullySaved = false;                
            }
            catch { succsessfullySaved = false; }
            
            return succsessfullySaved;
        }


        void ValidateSingleUploadItem(TreuhandverwaltungCsvUpload item)
        {
            var errorList = ValidateUploadItem(item);
            item.ValidationErrors = string.Join(",", errorList);
        }

        IEnumerable<string> ValidateUploadItem(TreuhandverwaltungCsvUpload item)
        {
            //item.CustomerNo = LogonContext.KundenNr.ToSapKunnr();

            //
            // Step 1. Data Annotation Validation
            //
            var errorList = ValidationService.ValidateDataAnnotations(item).SelectMany(r => r.MemberNames).ToList();

            //
            // Step 2. Fluent Validaton
            //

            if (item.Fahrgestellnummer.IsNullOrEmpty())
                errorList.Add("Fahrgestellnummer");

            if (item.Sperrdatum == null)
                errorList.Add("Sperrdatum");

            return errorList;
        }



        /// <summary>
        /// Auzhorisierung (aktuell nur VW) ist noch nicht aktiviert und kann bei Bedarf zugeschaltet werden
        /// </summary>        
        bool CheckForPendingAuthorization(int appId, int customerID, string customerName, string fin, bool isTestUser, string initiator, int authorizationID)
        {

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connectionstring"].ToString());
            SqlCommand cmdTable = new SqlCommand();

            initiator = null;
            authorizationID = -1;

            cn.Open();

            cmdTable = new SqlCommand();
            cmdTable.Connection = cn;


            if (customerID == -22)
            {
                cmdTable.CommandText = @"SELECT InitializedBy,AuthorizationID FROM [Authorization] 
                    WHERE AppID = @AppID 
                    AND CustomerReference = @CustomerReference 
                    AND ProcessReference = @ProcessReference 
                    AND TestUser = @TestUser";
            }
            else
            {
                cmdTable.CommandText = @"SELECT InitializedBy,AuthorizationID FROM [Authorization] 
                    WHERE AppID = @AppID &
                    AND OrganizationID = @OrganizationID 
                    AND CustomerReference = @CustomerReference 
                    AND ProcessReference = @ProcessReference 
                    AND TestUser = @TestUser";
            }

            cmdTable.Parameters.AddWithValue("@AppID", appId);
            if (customerID != -22)
            {
                cmdTable.Parameters.AddWithValue("@OrganizationID", customerID);
            }

            cmdTable.Parameters.AddWithValue("@CustomerReference", customerName);
            cmdTable.Parameters.AddWithValue("@ProcessReference", fin);
            cmdTable.Parameters.AddWithValue("@TestUser", isTestUser);

            SqlDataReader reader = cmdTable.ExecuteReader();

            while (reader.Read())
            {
                initiator = reader.GetString(0);
                authorizationID = reader.GetInt32(1);
            }

            if (String.IsNullOrEmpty(initiator))
                return false;
            else
                return true;
        }

    }       
}
