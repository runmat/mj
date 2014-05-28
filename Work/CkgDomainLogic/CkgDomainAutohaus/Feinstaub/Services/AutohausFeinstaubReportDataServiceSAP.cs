using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Feinstaub.Contracts;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Feinstaub.Services
{
    public class AutohausFeinstaubReportDataServiceSAP : CkgGeneralDataServiceSAP, IAutohausFeinstaubReportDataService
    {
        public FeinstaubSuchparameter Suchparameter { get; set; }

        private IEnumerable<Kundenstammdaten> _kundenstamm;
        public List<Kundenstammdaten> Kundenstamm { get { return PropertyCacheGet(() => GetKundenstamm().ToList()); } }

        public List<FeinstaubVergabeInfo> VergabeInfos { get { return PropertyCacheGet(() => LoadVergabeInfosFromSap().ToList()); } }

        public string Kundennummer { get { return (Kundenstamm == null || Kundenstamm.Count == 0 ? "" : Kundenstamm[0].Kundennummer); } }

        public AutohausFeinstaubReportDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new FeinstaubSuchparameter();
        }

        public void MarkForRefreshKundenstamm()
        {
            PropertyCacheClear(this, m => m.Kundenstamm);
            _kundenstamm = null;
        }

        public void MarkForRefreshVergabeInfos()
        {
            PropertyCacheClear(this, m => m.VergabeInfos);
        }

        private IEnumerable<Kundenstammdaten> GetKundenstamm()
        {
            if (_kundenstamm == null)
            {
                LoadStammdatenFromSap();
            }
            return _kundenstamm;
        }

        private void LoadStammdatenFromSap()
        {
            SAP.Init("Z_ZLD_AH_KUNDE_MAT");

            SAP.SetImportParameter("I_VKORG", (LogonContext as ILogonContextDataServiceAutohaus).VkOrg);
            SAP.SetImportParameter("I_VKBUR", (LogonContext as ILogonContextDataServiceAutohaus).VkBur);
            SAP.SetImportParameter("I_GRUPPE", LogonContext.GroupName);

            SAP.Execute();

            var sapListKunden = Z_ZLD_AH_KUNDE_MAT.GT_DEB.GetExportList(SAP);

            _kundenstamm = Autohaus.Models.AppModelMappings.Z_ZLD_AH_KUNDE_MAT_GT_DEB_To_Kundenstammdaten.Copy(sapListKunden);
        }

        private IEnumerable<FeinstaubVergabeInfo> LoadVergabeInfosFromSap()
        {
            Z_ZLD_AH_FS_STATISTIK.Init(SAP);
            
            SAP.SetImportParameter("I_VKORG", (LogonContext as ILogonContextDataServiceAutohaus).VkOrg);
            SAP.SetImportParameter("I_VKBUR", (LogonContext as ILogonContextDataServiceAutohaus).VkBur);
            if (!String.IsNullOrEmpty(Suchparameter.KennzeichenTeil1) && !String.IsNullOrEmpty(Suchparameter.KennzeichenTeil2))
            {
                var kennzeichen = Suchparameter.KennzeichenTeil1 + "-" + Suchparameter.KennzeichenTeil2;
                SAP.SetImportParameter("I_KENNZ", kennzeichen.ToUpper());
            }
            SAP.SetImportParameter("I_KUNNR", Kundennummer.PadLeft(10, '0'));
            SAP.SetImportParameter("I_STANDORT", (LogonContext as ILogonContextDataServiceAutohaus).UserInfo.Department);
            SAP.SetImportParameter("I_VON", Suchparameter.ErfassungsdatumVon);
            SAP.SetImportParameter("I_BIS", Suchparameter.ErfassungsdatumBis);

            var sapList = Z_ZLD_AH_FS_STATISTIK.GT_FSP.GetExportListWithExecute(SAP);

            return Feinstaub.Models.AppModelMappings.Z_ZLD_AH_FS_STATISTIK_GT_FSP_To_FeinstaubVergabeInfo.Copy(sapList);
        }
    }
}
