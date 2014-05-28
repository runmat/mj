using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using CKG.Base.Business;
using System.Data;

namespace Leasing.lib
{
    public class Klaerfaelle : CKG.Base.Business.DatenimportBase
    {

        public Klaerfaelle(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }


        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Klaerfaelle.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Klaerfaellevw", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                DataTable tmpTable = myProxy.getExportTable("GT_WEB");


                DataTable tmpNewTable = tmpTable.Clone();


                tmpNewTable.Columns["TDLINE1"].MaxLength = 660;


                foreach (DataRow dr in tmpTable.Rows)
                {

                    DataRow NewRow = tmpNewTable.NewRow();


                    NewRow["KUNNR"] = dr["KUNNR"];
                    NewRow["VBELN"] = dr["VBELN"];
                    NewRow["ZZFAHRG"] = dr["ZZFAHRG"];
                    NewRow["ZZKENN"] = dr["ZZKENN"];
                    NewRow["ZZREFNR"] = dr["ZZREFNR"];
                    NewRow["TDLINE1"] = dr["TDLINE1"];

                    if (dr["TDLINE2"].ToString().Length > 0)
                    {
                        NewRow["TDLINE1"] += " " + dr["TDLINE2"].ToString();
                    }

                    if (dr["TDLINE3"].ToString().Length > 0)
                    {
                        NewRow["TDLINE1"] += " " + dr["TDLINE3"].ToString();
                    }

                    if (dr["TDLINE4"].ToString().Length > 0)
                    {
                        NewRow["TDLINE1"] += " " + dr["TDLINE4"].ToString();
                    }

                    if (dr["TDLINE5"].ToString().Length > 0)
                    {
                        NewRow["TDLINE1"] += " " + dr["TDLINE5"].ToString();
                    }


                    NewRow["AUDAT"] = dr["AUDAT"];


                    tmpNewTable.Rows.Add(NewRow);


                }





                CreateOutPut(tmpNewTable, strAppID);
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);  

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Fahrzeuge gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);  
            }

        }
    }
}
