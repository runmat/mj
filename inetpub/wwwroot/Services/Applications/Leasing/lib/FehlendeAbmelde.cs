using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace Leasing.lib
{
    public class FehlendeAbmelde : CKG.Base.Business.DatenimportBase
    {
        #region "Declarations"

        private DataTable m_tblKundenadresse;

        #endregion

        #region "Properties"

        public DataTable tblKundenadresse
        {
            get { return m_tblKundenadresse; }
        }

        #endregion

        public FehlendeAbmelde(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
        : base(ref objUser, objApp, strFilename)
        { }

        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LeasePlan_R03.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Abm_Fehlende_Unterl_Slan", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();
                   
                DataTable tblTemp2   = myProxy.getExportTable("GT_WEB");

                foreach (DataRow tr in  tblTemp2.Rows)
	            {
                		switch (tr["ABCKZ"].ToString())
                        {
                            case "1":
                                    tr["Brief"] = "T";
                                    break;
                            case "2":
                                tr["Brief"] = "E";
                                break;
                            default: break;
                        }
	            }

                tblTemp2.AcceptChanges();

                CreateOutPut(tblTemp2, strAppID);


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
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }

        }

        public void FillArval(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "FehlendeAbmelde.FillArval";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Abm_Fehlende_Unterl_001", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                m_tblResult = myProxy.getExportTable("AUSGABE");
                   


            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }

        }

        public void FillKundenadresse(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "FehlendeAbmelde.FillKundenadresse";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_PARTNER_AUS_KNVP_LESEN", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.callBapi();

                m_tblKundenadresse = myProxy.getExportTable("AUSGABE");
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br/> " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }

        }

    }
}
