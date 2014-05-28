using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppMBB.lib
{
    public class Abmeldung : DatenimportBase
    {


       public string DatumVon { set; get; }
       public string DatumBis { set; get; }

        public Abmeldung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            
                : base(ref objUser, objApp, strFilename)
            {
            }


        public void getAbmeldung(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Abmeldung.getAbmeldung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ABM_EQUI_ABMDAT", ref m_objApp, ref m_objUser, ref page);



                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EXPIRY_DATE_VON", DatumVon);
                myProxy.setImportParameter("I_EXPIRY_DATE_BIS", DatumBis);

                myProxy.callBapi();

                ResultTable = myProxy.getExportTable("GT_DATEN");


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch (Exception ex)
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }
        }

             public void getData(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Abmeldung.getData";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ABM_FEHLENDE_UNTERL_002", ref m_objApp, ref m_objUser, ref page);



                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));


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


             public void getBriefbestand(String strAppID, String strSessionID, System.Web.UI.Page page)
             {

                 m_strClassAndMethod = "Abmeldung.getBriefbestand";
                 m_strAppID = strAppID;
                 m_strSessionID = strSessionID;
                 m_intStatus = 0;
                 m_strMessage = String.Empty;

                 try
                 {
                     DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_BRIEFBESTAND_001", ref m_objApp, ref m_objUser, ref page);



                     myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));


                     myProxy.callBapi();

                     ResultTable = myProxy.getExportTable("GT_DATEN");


                     WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
                 }
                 catch (Exception ex)
                 {
                     m_intStatus = -1111;
                     m_strMessage = "Keine Informationen gefunden.";

                 }


             }


             public void getEndgVersendet(String strAppID, String strSessionID, System.Web.UI.Page page)
             {

                 m_strClassAndMethod = "Abmeldung.getEndgVersendet";
                 m_strAppID = strAppID;
                 m_strSessionID = strSessionID;
                 m_intStatus = 0;
                 m_strMessage = String.Empty;

                 try
                 {
                     DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ABM_EQUI_ENDGLT_VERS", ref m_objApp, ref m_objUser, ref page);



                     myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                     myProxy.setImportParameter("I_ZZTMPDT_VON", DatumVon);
                     myProxy.setImportParameter("I_ZZTMPDT_BIS", DatumBis);

                     myProxy.callBapi();

                     ResultTable = myProxy.getExportTable("GT_DATEN");


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