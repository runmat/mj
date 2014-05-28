using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Web.UI;
using System.Reflection;
using System.Data;

namespace Vermieter.lib
{
    public class KFZSteuerAvisierung : DatenimportBase
    {

        string equNr = string.Empty;
        string chassisNum = string.Empty;
        string zeitraumMon = string.Empty;
        string flagZulZeitr = string.Empty;

        string fzg_Art = string.Empty;
        string erst_Zul_Dat = string.Empty;
        string krafftstoff = string.Empty;
        string zzhubraum = string.Empty;
        string zzCo2Kombi = string.Empty;
        string zzSld = string.Empty;
        string zzZulGesGew = string.Empty;

        public string ZulassungVon { get; set; }
        public string ZulassungBis { get; set; }
        public string Fahrgestellnummer { get; set; }
        public string Kennzeichen { get; set; }
        public string EquipmentNr { get; set; }

        const string BAPI_STEUERAVIS = "Z_DPM_KFZ_STEUER_02";
       // const string BAPI_STEUER = "Z_DPM_KFZ_STEUER_001";
        
        public KFZSteuerAvisierung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }

        public void Fill(Page page)
        {
            var mi = MethodInfo.GetCurrentMethod();
            m_strClassAndMethod = mi.DeclaringType.Name + "." + mi.Name;

            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {

                var myProxy = DynSapProxy.getProxy(BAPI_STEUERAVIS, ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_REPLA_DATE_VON",ZulassungVon);
                myProxy.setImportParameter("I_REPLA_DATE_BIS",ZulassungBis);

                DataTable tabImport = myProxy.getImportTable("GT_IN");
                tabImport.Rows.Add(new string[3] {Fahrgestellnummer,Kennzeichen,""});
        
                myProxy.callBapi();

                //string subrc = myProxy.getExportParameter("E_SUBRC");
                m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                m_tblResult = myProxy.getExportTable("GT_OUT");

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
                return;
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