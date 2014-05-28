using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;

namespace appCheckServices.lib
{
    public class clsInfo : CKG.Base.Business.DatenimportBase
    {
       
#region "Declarations"
    String m_strDatumVon;
    String m_strDatumBis;
    DataTable m_tblInfo;
#endregion

#region "Properties"
    public DataTable tblInfo
    {
        get { return m_tblInfo; }
        set { m_tblInfo = value; }
    }

    public String DatumVon
    {
        get { return m_strDatumVon; }
        set { m_strDatumVon = value; }
    }

    public String DatumBis
    {
        get { return m_strDatumBis; }
        set { m_strDatumBis = value; }
    }

#endregion

# region "Methods"


    public clsInfo(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
                                        : base(ref objUser, objApp, strFilename)
	    {
           

	    }

    public void FILL(String strAppID, String strSessionID, System.Web.UI.Page page)

    {
        m_strClassAndMethod = "clsInfo.FILL";
        m_strAppID = strAppID;
        m_strSessionID = strSessionID;
        m_intStatus = 0;
        m_strMessage = String.Empty;

        try
        {
            DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_WEB_FSCHEIN", ref m_objApp, ref m_objUser, ref page);


            myProxy.setImportParameter("I_ERDAT_VON", m_strDatumVon);
            myProxy.setImportParameter("I_ERDAT_BIS", m_strDatumBis);

            myProxy.setImportParameter("I_KUNNR_WE", m_objUser.Reference.PadLeft(10, '0'));

            myProxy.setImportParameter("I_KUNNR_AG", m_objUser.Organization.OrganizationReference.PadLeft(10, '0'));

            myProxy.callBapi();

            m_tblInfo = myProxy.getExportTable("GT_FSCHEIN");

            WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
        }
        catch (Exception ex)
        {
            switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            {
                case "NO_DATA":
                    m_intStatus = -1111;
                    m_strMessage = "Keine Informationen gefunden.";
                   break;

                default:
                    m_intStatus = -9999;

                    break;
            }


        }

    }

    public void Change(String strAppID, String strSessionID, System.Web.UI.Page page, String sBelegnr, String sErdat)
    {
        m_strClassAndMethod = "clsInfo.Change";
        m_strAppID = strAppID;
        m_strSessionID = strSessionID;
        m_intStatus = 0;
        m_strMessage = String.Empty;

        try
        {
            DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_WEB_FSCHEIN_DWNL", ref m_objApp, ref m_objUser, ref page);


            DataTable Saptable = myProxy.getImportTable("GT_FSCHEIN_DWNL");


            DataRow SapRow;

            SapRow = Saptable.NewRow();

            SapRow["FBELN"] = sBelegnr;
            SapRow["KUNNR_AG"] = m_objUser.Organization.OrganizationReference.PadLeft(10, '0');
            SapRow["KUNNR_WE"] = m_objUser.Reference.PadLeft(10, '0');
            SapRow["ERDAT"] = sErdat;
            SapRow["DOWNLDAT"] = System.DateTime.Now.ToShortDateString();
            SapRow["DOWNLUSR"] = m_objUser.UserName.PadLeft( 12);

            Saptable.Rows.Add(SapRow);
            myProxy.callBapi();

           
            WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
        }
        catch (Exception ex)
        {
            switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            {
                case "NO_DATA":
                    m_intStatus = -1111;
                    m_strMessage = "Keine Informationen gefunden.";
                    break;
                case "ERR_UPDATE":
                    m_intStatus = -9999;
                    m_strMessage = "Downloadinformationen konnten nicht gespeichert werden.";
                    break;
                default:
                    m_intStatus = -9999;

                    break;
            }


        }

    }
    # endregion
    }
}
