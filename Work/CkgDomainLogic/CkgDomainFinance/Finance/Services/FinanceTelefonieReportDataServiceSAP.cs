using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Finance.Models.AppModelMappings;

namespace CkgDomainLogic.Finance.Services
{
    public class FinanceTelefonieReportDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceTelefonieReportDataService
    {
        public TelefoniedatenSuchparameter Suchparameter { get; set; }

        public List<TelefoniedatenItem> Telefoniedaten { get { return PropertyCacheGet(() => LoadTelefoniedatenFromSap().ToList()); } }

        public FinanceTelefonieReportDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new TelefoniedatenSuchparameter{ Anrufart = "" };
        }

        public void MarkForRefreshTelefoniedaten()
        {
            PropertyCacheClear(this, m => m.Telefoniedaten);
        }

        private IEnumerable<TelefoniedatenItem> LoadTelefoniedatenFromSap()
        {
            Z_DPM_READ_PROT_TELEFONATE_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.Vertragsart != "alle")
                SAP.SetImportParameter("I_ZVERT_ART", Suchparameter.Vertragsart);

            if (Suchparameter.DatumRange.IsSelected)
            {
                if (Suchparameter.DatumRange.StartDate.HasValue)
                    SAP.SetImportParameter("I_ANRUFDATUM_VON", Suchparameter.DatumRange.StartDate.Value);

                if (Suchparameter.DatumRange.EndDate.HasValue)
                    SAP.SetImportParameter("I_ANRUFDATUM_BIS", Suchparameter.DatumRange.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(Suchparameter.Anrufart))
                SAP.SetImportParameter("I_ANRUFART", Suchparameter.Anrufart);

            var sapList = Z_DPM_READ_PROT_TELEFONATE_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_dpm_Read_Prot_Telefonate_01_GT_OUT_To_TelefoniedatenItem.Copy(sapList);
        }
    }
}
