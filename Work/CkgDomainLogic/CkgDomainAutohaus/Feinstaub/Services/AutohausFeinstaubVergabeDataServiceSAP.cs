using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Feinstaub.Contracts;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Feinstaub.Services
{
    public class AutohausFeinstaubVergabeDataServiceSAP : CkgGeneralDataServiceSAP, IAutohausFeinstaubVergabeDataService
    {
        private IEnumerable<Kundenstammdaten> _kundenstamm;
        public List<Kundenstammdaten> Kundenstamm { get { return PropertyCacheGet(() => GetKundenstamm().ToList()); } }

        public List<Domaenenfestwert> Kraftstoffcodes { get { return PropertyCacheGet(() => LoadKraftstoffcodesFromSap().ToList()); } }

        public List<Domaenenfestwert> Plakettenarten { get { return PropertyCacheGet(() => LoadPlakettenartenFromSap().ToList()); } }

        public string Kundennummer { get { return (Kundenstamm == null || Kundenstamm.Count == 0 ? "" : Kundenstamm[0].Kundennummer); } }

        public AutohausFeinstaubVergabeDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshKundenstamm()
        {
            PropertyCacheClear(this, m => m.Kundenstamm);
            _kundenstamm = null;
        }

        public void MarkForRefreshKraftstoffcodes()
        {
            PropertyCacheClear(this, m => m.Kraftstoffcodes);
        }

        public void MarkForRefreshPlakettenarten()
        {
            PropertyCacheClear(this, m => m.Plakettenarten);
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

        private IEnumerable<Domaenenfestwert> LoadKraftstoffcodesFromSap()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE", "ZZ_CODE_KRAFTSTOFF2", "DE");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList);
        }

        private IEnumerable<Domaenenfestwert> LoadPlakettenartenFromSap()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE", "ZZ_PLAKART", "DE");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList);
        }

        public string CheckFeinstaubVergabe(FeinstaubCheck pruefKriterien, out string plakettenart)
        {
            var erg = "";

            try
            {
                Z_ZLD_AH_FS_CHECK.Init(SAP);

                SAP.SetImportParameter("I_FAHRZEUGKLASSE", pruefKriterien.Fahrzeugklasse);
                SAP.SetImportParameter("I_CODE_AUFBAU", pruefKriterien.CodeAufbau);
                SAP.SetImportParameter("I_CODE_KRAFTSTOFF", pruefKriterien.Kraftstoffcode);
                SAP.SetImportParameter("I_SLD2", pruefKriterien.Emissionsschluesselnummer);

                SAP.Execute();

                if (SAP.ResultCode != 0)
                {
                    erg = SAP.ResultMessage;
                    plakettenart = "";
                }
                else
                {
                    plakettenart = SAP.GetExportParameter("E_PLAKART");
                }
            }
            catch (Exception ex)
            {
                plakettenart = "";
                erg = "Fehler bei der Prüfung: " + ex.Message;
            }
            
            return erg;
        }

        public string SaveFeinstaubVergabe(FeinstaubVergabe feinstaubDaten)
        {
            var erg = "";

            try
            {
                Z_ZLD_AH_FS_SAVE.Init(SAP);

                SAP.SetImportParameter("I_VKORG", (LogonContext as ILogonContextDataServiceAutohaus).VkOrg);
                SAP.SetImportParameter("I_VKBUR", (LogonContext as ILogonContextDataServiceAutohaus).VkBur);
                SAP.SetImportParameter("I_KENNZ", feinstaubDaten.Kennzeichen.ToUpper());
                SAP.SetImportParameter("I_KUNNR", Kundennummer.PadLeft(10, '0'));
                SAP.SetImportParameter("I_PLAKART", feinstaubDaten.Plakettenart);
                SAP.SetImportParameter("I_WEB_USER", LogonContext.UserName.ToUpper());
                SAP.SetImportParameter("I_STANDORT", (LogonContext as ILogonContextDataServiceAutohaus).UserInfo.Department);

                SAP.Execute();

                if (SAP.ResultCode != 0)
                {
                    erg = SAP.ResultMessage;
                }
            }
            catch (Exception ex)
            {
                erg = "Fehler beim Speichern: " + ex.Message;
            }
            
            return erg;
        }
    }
}
