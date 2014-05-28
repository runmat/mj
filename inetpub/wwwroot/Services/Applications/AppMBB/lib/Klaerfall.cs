using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppMBB.lib
{
    public class Klaerfall : DatenimportBase
    {

        public string Status { get; set; }
        public string Fahrgestellnummer { get; set; }
        public string Kennzeichen { get; set; }
        public string Vertragsnummer { get; set; }
        public string Kennung { get; set; }
        public string Bemerkung { get; set; }

        public Klaerfall(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            
                : base(ref objUser, objApp, strFilename)
            {
            }

        public void GetKlaerfaelle(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Klaerfall.GetKlaerfaelle";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_ABM_STATUS_01", ref m_objApp, ref m_objUser, ref page);



                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                myProxy.setImportParameter("I_AB_STATUS", Status);
                myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer);
                myProxy.setImportParameter("I_VERTRAGSNR", Kennzeichen);
                myProxy.setImportParameter("I_LICENSE_NUM", Vertragsnummer);
               

                myProxy.callBapi();

                 ResultTable = myProxy.getExportTable("GT_WEB");
            

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch (Exception ex)
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }


        }


        public void GetStatus(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Klaerfall.GetStatus";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_ABM_STATUS_02", ref m_objApp, ref m_objUser, ref page);



                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer);
               
                myProxy.callBapi();

                ResultTable = myProxy.getExportTable("GT_WEB");


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch (Exception ex)
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }


        }


        public void GetHistory(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Klaerfall.GetHistory";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_ABM_HIST_01", ref m_objApp, ref m_objUser, ref page);



                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer);
                myProxy.setImportParameter("I_KENNUNG", Kennung);

                myProxy.callBapi();

                ResultTable = myProxy.getExportTable("GT_WEB");


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch (Exception ex)
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }


        }

        public void SaveStatus(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Klaerfall.SaveStatus";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_SAVE_ABM_STATUS_01", ref m_objApp, ref m_objUser, ref page);



                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable impTable = myProxy.getImportTable("GT_WEB");

                DataRow dr = impTable.NewRow();

                dr["CHASSIS_NUM"] = Fahrgestellnummer;
                dr["KENNUNG"] = Kennung;
                dr["STATUS"] = Status;
                dr["TEXT"] = Bemerkung;

                impTable.Rows.Add(dr);

                myProxy.callBapi();



                string message = myProxy.getExportParameter("E_MESSAGE");

                ResultTable = myProxy.getExportTable("GT_WEB");


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch (Exception ex)
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }


        }

    }
}