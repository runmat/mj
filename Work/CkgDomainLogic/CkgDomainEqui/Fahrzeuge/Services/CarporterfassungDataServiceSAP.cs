using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class CarporterfassungDataServiceSAP : CkgGeneralDataServiceSAP, ICarporterfassungDataService
    {
        public CarporterfassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public IDictionary<string, string> GetCarportPdis(string adressKennung)
        {
            Z_DPM_READ_CARPID_01.Init(SAP);

            Z_DPM_READ_CARPID_01.SetImportParameter_I_AG(SAP, LogonContext.KundenNr.ToSapKunnr());

            return Z_DPM_READ_CARPID_01.GT_TAB.GetExportListWithExecute(SAP).OrderBy(s => s.KUNPDI.NotNullOrEmpty().Replace(adressKennung, "").ToInt(0))
                .ToDictionary(s => s.KUNPDI, s => string.Format("{0} - {1}", s.KUNPDI, s.NAME1));
        }

        public List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items, ref string errorMessage, bool nacherfassung)
        {
            try
            {
                if (nacherfassung)
                {
                    Z_DPM_INS_CARPORT_NACHLIEF_01.Init(SAP);

                    Z_DPM_INS_CARPORT_NACHLIEF_01.SetImportParameter_I_WEB_USER(SAP, LogonContext.UserName);

                    var sapItems = AppModelMappings.Z_DPM_INS_CARPORT_NACHLIEF_01_GT_WEB_From_CarporterfassungModel.CopyBack(items);
                    SAP.ApplyImport(sapItems);

                    return AppModelMappings.Z_DPM_INS_CARPORT_NACHLIEF_01_GT_WEB_To_CarporterfassungModel.Copy(Z_DPM_INS_CARPORT_NACHLIEF_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
                }
                else
                {
                    Z_DPM_IMP_CARPORT_MELD_01.Init(SAP);

                    Z_DPM_IMP_CARPORT_MELD_01.SetImportParameter_I_WEB_USER(SAP, LogonContext.UserName);

                    var sapItems = AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_From_CarporterfassungModel.CopyBack(items);
                    SAP.ApplyImport(sapItems);

                    return AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel.Copy(Z_DPM_IMP_CARPORT_MELD_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return items;
        }

        public List<CarportInfo> GetCarportAdressen(string adressKennung)
        {
            Z_DPM_READ_AUFTR_006.Init(SAP);

            Z_DPM_READ_AUFTR_006.SetImportParameter_I_KUNNR(SAP, LogonContext.KundenNr.ToSapKunnr());
            Z_DPM_READ_AUFTR_006.SetImportParameter_I_KENNUNG(SAP, adressKennung);

            return AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_To_CarportInfo.Copy(Z_DPM_READ_AUFTR_006.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }

        public List<CarporterfassungModel> GetFahrzeuge(CarportnacherfassungSelektor selektor)
        {
            Z_DPM_READ_CARPORT_MELD_01.Init(SAP);

            Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_KUNNR_AG(SAP, LogonContext.KundenNr.ToSapKunnr());
            Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_CARPORT_ID_AG(SAP, selektor.UserCarportId);
            Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_NUR_OFF_NL(SAP, "X");

            if (!selektor.UserAllOrganizations)
                Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_ORGANISATION(SAP, selektor.UserOrganization);

            if (!string.IsNullOrEmpty(selektor.BestandsNr))
                Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_MVA_NUMMER(SAP, selektor.BestandsNr);

            if (!string.IsNullOrEmpty(selektor.AuftragsNr))
                Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_AUFTRAGS_NR(SAP, selektor.AuftragsNr);

            if (!string.IsNullOrEmpty(selektor.FahrgestellNr))
                Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_CHASSIS_NUM(SAP, selektor.FahrgestellNr);

            if (!string.IsNullOrEmpty(selektor.Kennzeichen))
                Z_DPM_READ_CARPORT_MELD_01.SetImportParameter_I_LICENSE_NUM(SAP, selektor.Kennzeichen);

            return AppModelMappings.Z_DPM_READ_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel.Copy(Z_DPM_READ_CARPORT_MELD_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public IDictionary<string, string> GetCustomerOrganizations()
        {
            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                return dbContext.Organizations
                    .Where(o => o.CustomerID == LogonContext.User.CustomerID)
                    .OrderBy(o => o.OrganizationName)
                    .ToDictionary(o => o.OrganizationReference, o => o.OrganizationName);
            }
        }
    }
}
