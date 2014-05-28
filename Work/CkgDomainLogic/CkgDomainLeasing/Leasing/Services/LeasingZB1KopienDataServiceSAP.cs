using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingZB1KopienDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingZB1KopienDataService
    {
        public ZB1KopieSuchparameter Suchparameter { get; set; }
        public List<ZB1Kopie> ZB1Kopien { get { return PropertyCacheGet(() => LoadZB1KopienFromSap().ToList()); } }    

        public LeasingZB1KopienDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new ZB1KopieSuchparameter();
        }

        public void MarkForRefreshZB1Kopien()
        {
            PropertyCacheClear(this, m => m.ZB1Kopien);
        }

        private IEnumerable<ZB1Kopie> LoadZB1KopienFromSap()
        {
            Z_DPM_READ_SCHEINKOPIEN.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_DATUM_VON", DateTime.ParseExact(Suchparameter.DatumVon, "dd.MM.yyyy", CultureInfo.CurrentCulture));
            SAP.SetImportParameter("I_DATUM_BIS", DateTime.ParseExact(Suchparameter.DatumBis, "dd.MM.yyyy", CultureInfo.CurrentCulture));

            if (Suchparameter.Kunde == 1)
            {
                SAP.SetImportParameter("I_KUNDE_PG", "X");
            }
            else
            {
                SAP.SetImportParameter("I_KUNDE_OTHER", "X");
            }

            if (Suchparameter.Klaerfaelle)
                SAP.SetImportParameter("I_KLAERFALL", "X");

            var sapList = Z_DPM_READ_SCHEINKOPIEN.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_SCHEINKOPIEN_GT_OUT_To_ZB1Kopie.Copy(sapList);
        }
    }
}
