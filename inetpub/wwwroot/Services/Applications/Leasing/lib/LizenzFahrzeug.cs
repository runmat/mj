using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Reflection;
using CKG.Base.Common;
using CKG.Base.Business;
using System.Data;

namespace Leasing.lib
{
    public class LizenzFahrzeug : CKG.Base.Business.DatenimportBase
    {
        const string BAPI_AUFTRDAT = "Z_M_IMP_AUFTRDAT_007";
        const string BAPI_LIZENZ_FZG = "Z_DPM_LIZENZ_FZG_BESTAND";

        public LizenzFahrzeug(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }

        public void Fill(Page page)
        {
            var mi = MethodInfo.GetCurrentMethod();
            m_strClassAndMethod = mi.DeclaringType.Name + "." + mi.Name;

            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable lizenzgeber;
            try
            {
                var myProxy = DynSapProxy.getProxy(BAPI_AUFTRDAT, ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "LIZENZGEBER");

                myProxy.callBapi();

                lizenzgeber = myProxy.getExportTable("GT_WEB");
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Kein Lizenzgeber gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
                return;
            }

            if (lizenzgeber == null || lizenzgeber.Rows.Count == 0)
            {
                m_intStatus = -1111;
                m_strMessage = "Kein Lizenzgeber gefunden.";
                return;
            }

            var lizenzgeberNr = lizenzgeber.Rows.Cast<DataRow>().Where(r => (string)r["KENNUNG"] == "LIZENZGEBER").Select(r => (string)r["POS_KURZTEXT"]).ToList();
            if (lizenzgeberNr.Count == 0)
            {
                m_intStatus = -1111;
                m_strMessage = "Kein Lizenzgeber gefunden.";
                return;
            }

            try
            {
                var myProxy = DynSapProxy.getProxy(BAPI_LIZENZ_FZG, ref m_objApp, ref m_objUser, ref page);
                
                myProxy.setImportParameter("KUNNR1", lizenzgeberNr.First().PadLeft(10, '0'));
                myProxy.setImportParameter("PARVW1", "AG");
                myProxy.setImportParameter("KUNNR2", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("PARVW2", "ZC");

                myProxy.callBapi();

                // why, oh why..? 
                //  m_tblResult    -> Result
                //  m_tableResult  -> ResultTable
                m_tblResult = m_tableResult = myProxy.getExportTable("GT_OUT");
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Lizenzfahrzeuge gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }
        }

        internal sealed class ResultColumn
        {
            private readonly string sapName;
            private readonly string name;

            private ResultColumn(string sap, string n)
            {
                sapName = sap;
                name = n;
            }

            public string SapName { get { return sapName; } }
            public string Name { get { return name; } }

            //public static ResultColumn 
        }
    }
}