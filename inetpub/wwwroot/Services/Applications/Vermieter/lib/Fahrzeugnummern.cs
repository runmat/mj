using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace Vermieter.lib
{
    public class Fahrzeugnummern : BankBase
    {

        #region " Declarations"
            private string mE_SUBRC;
            private string mE_MESSAGE;
            private string m_strSperrEnsperr;
            private DataTable m_tblUpload;
            private DataTable m_tblFahrzeuge;
            private DataTable m_tblBestand;
            private string m_strTreunehmer;
            private string m_strFilename2;
            private string m_strReferenceforAut;
            private string m_strErdatvon;
            private string m_strErdatbis;
            private string m_Treugeber;

            private string m_alleTreugeber;
        #endregion


#region " Properties "
    public DataTable tblUpload {
	    get { return m_tblUpload; }
	    set { m_tblUpload = value; }
    }
    public DataTable Fahrzeuge {
	    get { return m_tblFahrzeuge; }
	    set { m_tblFahrzeuge = value; }
    }

    public string E_SUBRC {
	    get { return mE_SUBRC; }
	    set { mE_SUBRC = value; }
    }

    public string E_MESSAGE {
	    get { return mE_MESSAGE; }
	    set { mE_MESSAGE = value; }
    }
    public string SperrEnsperr {
	    get { return m_strSperrEnsperr; }
	    set { m_strSperrEnsperr = value; }
    }

    public DataTable Bestand {
	    get { return m_tblBestand; }
	    set { m_tblBestand = value; }
    }
    public string Treunehmer {
	    get { return m_strTreunehmer; }
	    set { m_strTreunehmer = value; }
    }
    public string ReferenceforAut {
	    get { return m_strReferenceforAut; }
	    set { m_strReferenceforAut = value; }
    }
    public string Erdatvon {
	    get { return m_strErdatvon; }
	    set { m_strErdatvon = value; }
    }
    public string Erdatbis {
	    get { return m_strErdatbis; }
	    set { m_strErdatbis = value; }
    }
    public string Treugeber {
	    get { return m_Treugeber; }
	    set { m_Treugeber = value; }
    }

    public string alleTreugeber {
	    get { return m_alleTreugeber; }
	    set { m_alleTreugeber = value; }
    }
#endregion

#region "Methods"

    public Fahrzeugnummern(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            this.m_strFilename2 = strFilename;

	    }


    public override void Change()
    {

    }

    public override void Show()
    {

    }


    public void GetCustomer(System.Web.UI.Page page, string strAppID, string strSessionID)
{
	m_strClassAndMethod = "SperreFreigabe.FILL";
	m_strAppID = strAppID;
	m_strSessionID = strSessionID;

	try {
		DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_TH_GET_TREUH_AG",ref m_objApp, ref m_objUser,ref page);

		myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, '0'));
		myProxy.setImportParameter("I_EQTYP", "B");
		myProxy.setImportParameter("I_ALL_AG", m_alleTreugeber);
		myProxy.callBapi();

		Result = myProxy.getExportTable("GT_EXP");

		mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
		mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

		WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

	} catch (Exception ex) {
		switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)) {
			default:
				m_intStatus = -9999;
				mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
				break;
		}

		WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "),ref m_tblResult);

	}

}

    public void GiveCars(System.Web.UI.Page page, string strAppID, string strSessionID)
{
	m_strClassAndMethod = "SperreFreigabe.GiveCars";
	m_strAppID = strAppID;
	m_strSessionID = strSessionID;


	try {
		DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_TH_INS_VORGANG",ref m_objApp,ref m_objUser, ref page);

		myProxy.setImportParameter("I_TREU", Treugeber.PadLeft(10, '0'));
		myProxy.setImportParameter("I_EQTYP", "B");

		DataTable tbltemp = myProxy.getImportTable("GT_IN");

		foreach (DataRow uploadRow in tblUpload.Rows) {
			if (uploadRow["SELECT"].ToString() == "99") {
				DataRow NewDatarow = tbltemp.NewRow();
				NewDatarow["AG"] = uploadRow["AG"];
				NewDatarow["EQUI_KEY"] = uploadRow["EQUI_KEY"];
                NewDatarow["ERNAM"] = uploadRow["ERNAM"].ToString().Substring(0,12);
				NewDatarow["ERDAT"] = uploadRow["ERDAT"];
				NewDatarow["SPERRDAT"] = uploadRow["SPERRDAT"];
				NewDatarow["TREUH_VGA"] = uploadRow["TREUH_VGA"];
				NewDatarow["SUBRC"] = "";
				NewDatarow["MESSAGE"] = "";
				tbltemp.Rows.Add(NewDatarow);
			}

		}

		myProxy.callBapi();

		m_tblFahrzeuge = myProxy.getExportTable("GT_IN");
		m_tblFahrzeuge.Columns.Add("SELECT", typeof(System.String));
		HelpProcedures.killAllDBNullValuesInDataTable(ref m_tblFahrzeuge);
		E_SUBRC = myProxy.getExportParameter("E_SUBRC");
		E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

	} catch (Exception ex) {
		switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)) {
			default:
				E_MESSAGE = "Fehler beim Lesen der Daten.";
				E_SUBRC = "-9999";
				break;
		}
	}
}

    public void AutorizeCar(System.Web.UI.Page page, string strAppID, string strSessionID)
{
    m_strClassAndMethod = "Fahrzeugnummern.AutorizeCar";
	m_strAppID = strAppID;
	m_strSessionID = strSessionID;


	try {
		DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_TH_INS_VORGANG",ref m_objApp,ref m_objUser,ref page);

		myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, '0'));
		myProxy.setImportParameter("I_EQTYP", "B");

		DataTable tbltemp = myProxy.getImportTable("GT_IN");

		foreach (DataRow uploadRow in tblUpload.Rows) {
			if (uploadRow["SELECT"].ToString() == "99" && uploadRow["EQUI_KEY"].ToString() == m_strReferenceforAut) {
				DataRow NewDatarow = tbltemp.NewRow();
				NewDatarow["AG"] = uploadRow["AG"];
				NewDatarow["EQUI_KEY"] = uploadRow["EQUI_KEY"];
                NewDatarow["ERNAM"] = uploadRow["ERNAM"].ToString().Substring(0, 12);
				NewDatarow["ERDAT"] = uploadRow["ERDAT"];
				NewDatarow["SPERRDAT"] = uploadRow["SPERRDAT"];
				NewDatarow["TREUH_VGA"] = uploadRow["TREUH_VGA"];
				NewDatarow["SUBRC"] = "";
				NewDatarow["MESSAGE"] = "";
				tbltemp.Rows.Add(NewDatarow);
			}

		}

		myProxy.callBapi();

		m_tblFahrzeuge = myProxy.getExportTable("GT_IN");
		m_tblFahrzeuge.Columns.Add("SELECT", typeof(System.String));
		HelpProcedures.killAllDBNullValuesInDataTable(ref m_tblFahrzeuge);
		E_SUBRC = myProxy.getExportParameter("E_SUBRC");
		E_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

	} catch (Exception ex) {
		switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)) {
			default:
				E_MESSAGE = "Fehler beim Lesen der Daten.";
				E_SUBRC = "-9999";
				break;
		}
	}
}
public DataTable GiveResultStructure()
{
	if (tblUpload == null) {
		tblUpload = new DataTable();
		var _with1 = tblUpload.Columns;
		_with1.Add("AG", typeof(System.String));
		_with1.Add("EQUI_KEY", typeof(System.String));
		_with1.Add("ERNAM", typeof(System.String));
		_with1.Add("ERDAT", typeof(System.String));
		_with1.Add("SPERRDAT", typeof(System.String));
		_with1.Add("TREUH_VGA", typeof(System.String));
		_with1.Add("SUBRC", typeof(System.String));
		_with1.Add("MESSAGE", typeof(System.String));
		_with1.Add("SELECT", typeof(System.String));
	}
	return Fahrzeuge;
}

public void GetTreuhandBestand(System.Web.UI.Page page, string strAppID, string strSessionID)
{
	m_strClassAndMethod = "SperreFreigabe.GetTreuhandBestand";
	m_strAppID = strAppID;
	m_strSessionID = strSessionID;
	mE_SUBRC = "";
	mE_MESSAGE = "";
	try {
		DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_TREUHANDBESTAND",ref m_objApp,ref m_objUser,ref page);

		myProxy.setImportParameter("KUNNR_AG", m_strTreunehmer.PadLeft(10, '0'));
		myProxy.setImportParameter("ERDAT_VON", m_strErdatvon);
		myProxy.setImportParameter("ERDAT_BIS", m_strErdatbis);

		myProxy.callBapi();

		DataTable tblTemp = myProxy.getExportTable("GT_WEB");

		mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
		mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

		m_tblBestand = CreateOutPut(tblTemp, m_strAppID);


		WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblBestand);

	} catch (Exception ex) {
		switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)) {
			default:
				m_intStatus = -9999;
				mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
				break;
		}

		WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "),ref m_tblBestand);

	}

}
#endregion





    }
}
