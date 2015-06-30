using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Database.Services;
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
            List<Anwendung> liste = new List<Anwendung>
                {
                    new Anwendung("ZLD-Vorgänge bearbeiten", "EditZLDVorgaenge", "ErfassungMobil"),
                    new Anwendung("Neuen Vorgang erfassen", "CreateZLDVorgang", "ErfassungMobil")
                };

            return liste;
        }

        public Stammdatencontainer GetStammdaten()
        {
            Z_ZLD_MOB_STAMMD.Init(SAP);

            SAP.Execute();

            return new Stammdatencontainer
                {
                    Aemter = AppModelMappings.Z_ZLD_MOB_STAMMD_GT_KREISKZ_To_Amt.Copy(Z_ZLD_MOB_STAMMD.GT_KREISKZ.GetExportList(SAP)).OrderBy(w => w.KurzBez).ToList(),
                    Dienstleistungen = AppModelMappings.Z_ZLD_MOB_STAMMD_GT_MAT_To_Dienstleistung.Copy(Z_ZLD_MOB_STAMMD.GT_MAT.GetExportList(SAP)).OrderBy(w => w.Bezeichnung).ToList()
                };
        }

        public void GetAemterMitVorgaengen(out List<AmtVorgaenge> aemterMitVorgaengen, out List<Vorgang> vorgaenge)
        {
            Z_ZLD_MOB_USER_GET_VG.Init(SAP, "I_VKORG, I_VKBUR, I_MOBUSER", LogonContext.VkOrg, LogonContext.VkBur, LogonContext.UserName.ToUpper());

            SAP.Execute();

            aemterMitVorgaengen = AppModelMappings.Z_ZLD_MOB_USER_GET_VG_GT_VGANZ_To_AmtVorgaenge.Copy(Z_ZLD_MOB_USER_GET_VG.GT_VGANZ.GetExportList(SAP)).OrderBy(w => w.KurzBez).ToList();

            vorgaenge = AppModelMappings.Z_ZLD_MOB_USER_GET_VG_GT_VG_KOPF_To_Vorgang.Copy(Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF.GetExportList(SAP)).OrderBy(w => w.Id).ToList();

            var positionen = AppModelMappings.Z_ZLD_MOB_USER_GET_VG_GT_VG_POS_To_VorgangPosition.Copy(Z_ZLD_MOB_USER_GET_VG.GT_VG_POS.GetExportList(SAP)).OrderBy(w => w.KopfId).ThenBy(w => w.PosNr).ToList();
            
            var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName);

            vorgaenge.ForEach(item =>
                {
                    // Positionen den Kopfsätzen zuordnen
                    item.Positionen = positionen.Where(p => p.KopfId == item.Id).OrderBy(p => p.PosNr).ToList();

                    // User-Infos aus SQL lesen
                    var vorerfUser = dbContext.GetCkgUserInfo(item.VorerfasserUser);
                    if (vorerfUser != null)
                    {
                        item.VorerfasserAnrede = vorerfUser.Title;
                        item.VorerfasserName1 = vorerfUser.FirstName;
                        item.VorerfasserName2 = vorerfUser.LastName;
                    }
                }
            );
        }

        public string SaveVorgaenge(List<Vorgang> vorgaenge)
        {
            string erg;

            try
            {
                Z_ZLD_MOB_USER_PUT_VG.Init(SAP, "I_VKORG, I_VKBUR, I_MOBUSER", LogonContext.VkOrg, LogonContext.VkBur, LogonContext.UserName.ToUpper());

                List<VorgangPosition> positionen = new List<VorgangPosition>();
                foreach (Vorgang vg in vorgaenge)
                {
                    positionen.AddRange(vg.Positionen);
                }

                var positionList = AppModelMappings.Z_ZLD_MOB_USER_PUT_VG_GT_VG_POS_From_VorgangPosition.CopyBack(positionen).ToList();
                var vorgangList = AppModelMappings.Z_ZLD_MOB_USER_PUT_VG_GT_VG_KOPF_From_Vorgang.CopyBack(vorgaenge).ToList();

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
            Z_ZLD_MOB_CHECK_BEB_STATUS.Init(SAP);

            var liste = new List<VorgangStatus>();
            foreach (var vorgId in vorgIds)
            {
                liste.Add(new VorgangStatus(vorgId, ""));
            }

            var sapList = AppModelMappings.Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_From_VorgangStatus.CopyBack(liste).ToList();

            SAP.ApplyImport(sapList);

            SAP.Execute();

            return AppModelMappings.Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_To_VorgangStatus.Copy(Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS.GetExportList(SAP)).OrderBy(w => w.Id).ToList();
        }

        public List<string> GetVkBurs()
        {
            Z_ZLD_MOB_GET_USER_AEMTER.Init(SAP, "I_VKORG, I_MOBUSER", LogonContext.VkOrg, LogonContext.UserName.ToUpper());

            return Z_ZLD_MOB_GET_USER_AEMTER.GT_MOBUSER.GetExportListWithExecute(SAP).OrderBy(a => a.VKBUR).Select(a => a.VKBUR).ToList();
        }

        public void GetStammdatenKundenUndHauptdienstleistungen(string vkBur, out List<Kunde> kunden, out List<Dienstleistung> dienstleistungen)
        {
            Z_ZLD_EXPORT_KUNDE_MAT.Init(SAP, "I_VKORG, I_VKBUR", LogonContext.VkOrg, vkBur);

            SAP.Execute();

            kunden = AppModelMappings.Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_KUNDE_To_Kunde.Copy(Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE.GetExportList(SAP)).OrderBy(w => w.Name).ToList();
            dienstleistungen = AppModelMappings.Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_MATERIAL_To_Dienstleistung.Copy(Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL.GetExportList(SAP)).OrderBy(w => w.Bezeichnung).ToList();
        }

        public List<Amt> GetStammdatenAemter()
        {
            Z_ZLD_EXPORT_ZULSTEL.Init(SAP);

            SAP.Execute();

            return AppModelMappings.Z_ZLD_EXPORT_ZULSTEL_GT_EX_ZULSTELL_To_Amt.Copy(Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL.GetExportList(SAP)).OrderBy(w => w.KurzBez).ToList();
        }
    }
}
