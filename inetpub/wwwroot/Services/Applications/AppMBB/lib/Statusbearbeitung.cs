using System;
using System.Data;
using System.Web.UI;
using CKG.Base.Common;
using CKG.Base.Kernel.Security;
using System.Globalization;

namespace AppMBB.lib
{
    internal sealed class Statusbearbeitung
    {
        private User user;
        private App app;
        private Page page;

        internal Statusbearbeitung(User user, App app, Page page)
        {
            this.user = user;
            this.app = app;
            this.page = page;
        }

        internal DataTable GetStatuses(string application, string chassisNumber, string contractNumber, string licenceNumber, 
            DateTime? wiedervorlageVon, DateTime? wiedervorlageBis, string abteilung, string rechnungsempfaenger)
        {
            try
            {
                var proxy = DynSapProxy.getProxy("Z_DPM_READ_ABM_STATUS_03", ref this.app, ref this.user, ref this.page);

                proxy.setImportParameter("I_KUNNR_AG", this.user.KUNNR.PadLeft(10, '0'));
                proxy.setImportParameter("I_ANWENDUNG", application);

                if (!String.IsNullOrEmpty(chassisNumber))
                {
                    proxy.setImportParameter("I_CHASSIS_NUM", chassisNumber);
                }

                if (!String.IsNullOrEmpty(contractNumber))
                {
                    proxy.setImportParameter("I_VERTRAGSNR", contractNumber);
                }

                if (!String.IsNullOrEmpty(licenceNumber))
                {
                    proxy.setImportParameter("I_LICENSE_NUM", licenceNumber);
                }

                if (wiedervorlageVon.HasValue)
                {
                    if (application == "C")
                    {
                        proxy.setImportParameter("I_WDKDAT_AB", wiedervorlageVon.Value.ToShortDateString());
                    }
                    else
                    {
                        proxy.setImportParameter("I_WDVDAT_AB", wiedervorlageVon.Value.ToShortDateString());
                    }
                }

                if (wiedervorlageBis.HasValue)
                {
                    if (application == "C")
                    {
                        proxy.setImportParameter("I_WDKDAT_BIS", wiedervorlageBis.Value.ToShortDateString());
                    }
                    else
                    {
                        proxy.setImportParameter("I_WDVDAT_BIS", wiedervorlageBis.Value.ToShortDateString());
                    }
                }

                if (!String.IsNullOrEmpty(abteilung))
                {
                    proxy.setImportParameter("I_ABTEILUNG", abteilung);
                }

                if (!String.IsNullOrEmpty(rechnungsempfaenger))
                {
                    proxy.setImportParameter("I_KUNNR_RE", rechnungsempfaenger.PadLeft(10, '0'));
                }

                proxy.callBapi();

                return proxy.getExportTable("GT_WEB");
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal DataTable GetHistory(string chassisNumber)
        {
            try
            {
                var proxy = DynSapProxy.getProxy("Z_DPM_READ_ABM_HIST_01", ref this.app, ref this.user, ref this.page);

                proxy.setImportParameter("I_KUNNR_AG", this.user.KUNNR.PadLeft(10, '0'));
                proxy.setImportParameter("I_CHASSIS_NUM", chassisNumber);

                proxy.callBapi();

                var result = proxy.getExportTable("GT_WEB");
                foreach (DataRow row in result.Rows)
                {
                    var status = row["STATUS"] as string;
                    DateTime dtStatus;
                    if (!string.IsNullOrEmpty(status) && DateTime.TryParseExact(status, "yyyyMMdd", null, DateTimeStyles.AssumeLocal, out dtStatus))
                        row["STATUS"] = dtStatus.ToShortDateString();
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal DataTable GetRechercheprotokoll(string vorgangsNr)
        {
            try
            {
                var proxy = DynSapProxy.getProxy("Z_DPM_READ_RECHERCHE_PROTOKOLL", ref this.app, ref this.user, ref this.page);

                proxy.setImportParameter("I_KUNNR_AG", this.user.KUNNR.PadLeft(10, '0'));
                proxy.setImportParameter("I_VORGANGS_NR", vorgangsNr);

                proxy.callBapi();

                return proxy.getExportTable("GT_OUT");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal DataTable GetStatusDetails(string chassisNumber, string licenceNumber)
        {
            try
            {
                var proxy = DynSapProxy.getProxy("Z_DPM_READ_ABM_STATUS_02", ref this.app, ref this.user, ref this.page);

                proxy.setImportParameter("I_KUNNR_AG", this.user.KUNNR.PadLeft(10, '0'));
                proxy.setImportParameter("I_CHASSIS_NUM", chassisNumber);
                proxy.setImportParameter("I_LICENSE_NUM", licenceNumber);
                proxy.callBapi();

                return proxy.getExportTable("GT_WEB");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal DataTable SaveStatus(string chassisNumber, string licenseNumber, string identification, string status, string comment)
        {
            try
            {
                var proxy = DynSapProxy.getProxy("Z_DPM_SAVE_ABM_STATUS_01", ref this.app, ref this.user, ref this.page);

                proxy.setImportParameter("I_KUNNR_AG", this.user.KUNNR.PadLeft(10, '0'));

                var impTable = proxy.getImportTable("GT_WEB");
                var dr = impTable.NewRow();

                dr["CHASSIS_NUM"] = chassisNumber;
                dr["KENNUNG"] = identification;
                dr["STATUS"] = status;
                dr["TEXT"] = comment;
                dr["KENNZ"] = licenseNumber;

                impTable.Rows.Add(dr);

                proxy.callBapi();

                return proxy.getExportTable("GT_WEB");
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal DataTable GetAbteilungen()
        {
            DataTable retTable = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref this.app, ref this.user, ref this.page);

                myProxy.setImportParameter("I_KUNNR", user.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "ABT");

                myProxy.callBapi();

                retTable = myProxy.getExportTable("GT_WEB");
            }
            catch (Exception ex)
            {
                return null;
            }

            return retTable;
        }

        internal DataTable GetRechnungsempfaenger()
        {
            DataTable retTable = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref this.app, ref this.user, ref this.page);

                myProxy.setImportParameter("I_KUNNR", user.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "RG");

                myProxy.callBapi();

                retTable = myProxy.getExportTable("GT_WEB");
            }
            catch (Exception ex)
            {
                return null;
            }

            return retTable;
        }
    }
}