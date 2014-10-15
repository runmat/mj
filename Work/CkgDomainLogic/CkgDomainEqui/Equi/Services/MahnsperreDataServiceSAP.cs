using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class MahnsperreDataServiceSAP : CkgGeneralDataServiceSAP, IMahnsperreDataService
    {
        public MahnsperreSuchparameter Suchparameter { get; set; }

        public List<EquiMahnsperre> MahnsperreEquis { get { return PropertyCacheGet(() => LoadMahnsperreEquisFromSap().ToList()); } }

        public MahnsperreDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new MahnsperreSuchparameter();
        }

        public void MarkForRefreshMahnsperreEquis()
        {
            PropertyCacheClear(this, m => m.MahnsperreEquis);
        }

        private IEnumerable<EquiMahnsperre> LoadMahnsperreEquisFromSap()
        {
            Z_DPM_READ_TEMP_VERS_EQUI_01.Init(SAP, "I_AG, I_EQTYP", LogonContext.KundenNr.ToSapKunnr(), "B");

            if (!string.IsNullOrEmpty(Suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_CHASSIS_NUM", Suchparameter.FahrgestellNr.ToUpper());

            if (!string.IsNullOrEmpty(Suchparameter.VertragsNr))
                SAP.SetImportParameter("I_LIZNR", Suchparameter.VertragsNr.ToUpper());

            if (!string.IsNullOrEmpty(Suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_LICENSE_NUM", Suchparameter.Kennzeichen.ToUpper());

            if (!string.IsNullOrEmpty(Suchparameter.BriefNr))
                SAP.SetImportParameter("I_TIDNR", Suchparameter.BriefNr.ToUpper());

            var sapList = Z_DPM_READ_TEMP_VERS_EQUI_01.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_TEMP_VERS_EQUI_01_GT_WEB_To_EquiMahnsperre.Copy(sapList);
        }

        public List<EquiMahnsperre> SaveMahnsperreEquis(List<EquiMahnsperre> equis, ref string message)
        {
            List<EquiMahnsperre> erg = new List<EquiMahnsperre>();

            Z_DPM_CHANGE_MAHNSP_EQUI_01.Init(SAP, "I_AG, I_QMNAM", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var vList = AppModelMappings.Z_DPM_CHANGE_MAHNSP_EQUI_01_GT_WEB_From_EquiMahnsperre.CopyBack(equis).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var outList = Z_DPM_CHANGE_MAHNSP_EQUI_01.GT_WEB.GetExportList(SAP);

                foreach (var eq in equis)
                {
                    var outItem = outList.Find(e => e.CHASSIS_NUM == eq.FahrgestellNr);

                    if (String.IsNullOrEmpty(outItem.RET_BEM))
                    {
                        eq.Status = Localize.OK;
                    }
                    else
                    {
                        eq.Status = outItem.RET_BEM;
                    }

                    erg.Add(eq);
                }
            }
            else
            {
                message = Localize.SaveFailed + ": " + SAP.ResultMessage;
            }

            return erg;
        }
    }
}