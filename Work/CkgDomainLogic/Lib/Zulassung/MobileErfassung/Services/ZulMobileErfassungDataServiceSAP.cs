using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Zulassung.MobileErfassung.Contracts;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;
using GeneralTools.Contracts;
using SapORM.Contracts;
using SapORM.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Services
{
    public class ZulMobileErfassungDataServiceSAP : IZulMobileErfassungDataService
    {
        public ISapDataService SAP { get; set; }

        public IAppSettings AppSettings { get; private set; }

        public ILogonContextDataServiceZLDMobile LogonContext { get; private set; }

        public ZulMobileErfassungDataServiceSAP(ISapDataService sap, IAppSettings appSettings, ILogonContextDataServiceZLDMobile logonContext)
        {
            SAP = sap;
            AppSettings = appSettings;
            LogonContext = logonContext;
        }

        public List<Anwendung> GetAnwendungen()
        {
            List<Anwendung> liste = new List<Anwendung> { new Anwendung("ZLD-Vorgänge bearbeiten", "EditZLDVorgaenge", "ErfassungMobil") };

            return liste;
        }

        public Stammdatencontainer GetStammdaten()
        {
            Stammdatencontainer stda = new Stammdatencontainer();

            SAP.InitExecute("Z_ZLD_MOB_STAMMD");

            // Ämter
            var sapItems_amt = Z_ZLD_MOB_STAMMD.GT_KREISKZ.GetExportList(SAP);
            var webItems_amt = AppModelMappings.Z_ZLD_MOB_STAMMD_GT_KREISKZ_To_Amt.Copy(sapItems_amt).OrderBy(w => w.KurzBez).ToList();

            // Dienstleistungen
            var sapItems_dl = Z_ZLD_MOB_STAMMD.GT_MAT.GetExportList(SAP);
            var webItems_dl = AppModelMappings.Z_ZLD_MOB_STAMMD_GT_MAT_To_Dienstleistung.Copy(sapItems_dl).OrderBy(w => w.Bezeichnung).ToList();

            stda.Aemter = webItems_amt;
            stda.Dienstleistungen = webItems_dl;

            return stda;
        }

        public void GetAemterMitVorgaengen(out List<AmtVorgaenge> aemterMitVorgaengen, out List<Vorgang> vorgaenge)
        {
            SAP.InitExecute("Z_ZLD_MOB_USER_GET_VG", "I_VKORG, I_VKBUR, I_MOBUSER", LogonContext.VkOrg, LogonContext.VkBur, LogonContext.UserName.ToUpper());

            // GT_VGANZ
            var sapItems_vganz = Z_ZLD_MOB_USER_GET_VG.GT_VGANZ.GetExportList(SAP);
            var webItems_vganz = AppModelMappings.Z_ZLD_MOB_USER_GET_VG_GT_VGANZ_To_AmtVorgaenge.Copy(sapItems_vganz).OrderBy(w => w.KurzBez).ToList();

            aemterMitVorgaengen = webItems_vganz;

            // GT_VG_KOPF
            var sapItems_vgkopf = Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF.GetExportList(SAP);
            var webItems_vgkopf = AppModelMappings.Z_ZLD_MOB_USER_GET_VG_GT_VG_KOPF_To_Vorgang.Copy(sapItems_vgkopf).OrderBy(w => w.Id).ToList();

            // GT_VG_POS
            var sapItems_vgpos = Z_ZLD_MOB_USER_GET_VG.GT_VG_POS.GetExportList(SAP);
            var webItems_vgpos = AppModelMappings.Z_ZLD_MOB_USER_GET_VG_GT_VG_POS_To_VorgangPosition.Copy(sapItems_vgpos).OrderBy(w => w.KopfId).ThenBy(w => w.PosNr).ToList();
            
            // Positionen den Kopfsätzen zuordnen
            webItems_vgkopf.ForEach(item =>
                {
                    item.Positionen = webItems_vgpos.Where(w => w.KopfId == item.Id).ToList();
                }
            );

            vorgaenge = webItems_vgkopf;
        }

        public string SaveVorgaenge(List<Vorgang> vorgaenge)
        {
            string erg;

            try
            {
                SAP.Init("Z_ZLD_MOB_USER_PUT_VG");

                List<VorgangPosition> positionen = new List<VorgangPosition>();
                foreach (Vorgang vg in vorgaenge)
                {
                    positionen.AddRange(vg.Positionen);
                }

                var positionList = AppModelMappings.Z_ZLD_MOB_USER_PUT_VG_GT_VG_POS_To_VorgangPosition.CopyBack(positionen).ToList();
                var vorgangList = AppModelMappings.Z_ZLD_MOB_USER_PUT_VG_GT_VG_KOPF_To_Vorgang.CopyBack(vorgaenge).ToList();

                SAP.ApplyImport(positionList);
                SAP.ApplyImport(vorgangList);

                SAP.Execute();

                erg = SAP.ResultMessage;
            }
            catch (Exception ex)
            {
                erg = "Fehler beim Speichern in SAP: " + ex.Message;
            }

            return erg;
        }

        public List<VorgangStatus> GetVorgangBebStatus(List<string> vorgIds)
        {
            List<VorgangStatus> liste = new List<VorgangStatus>();

            SAP.Init("Z_ZLD_MOB_CHECK_BEB_STATUS");

            foreach (string vorgId in vorgIds)
            {
                liste.Add(new VorgangStatus(vorgId, ""));
            }

            var vorgList = AppModelMappings.Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_To_VorgangStatus.CopyBack(liste).ToList();

            SAP.ApplyImport(vorgList);

            SAP.Execute();

            var sapItems_vorgaenge = Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS.GetExportList(SAP);
            var webItems_vorgaenge = AppModelMappings.Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_To_VorgangStatus.Copy(sapItems_vorgaenge).OrderBy(w => w.Id).ToList();

            liste = webItems_vorgaenge;

            return liste;
        }
    }
}
