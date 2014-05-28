using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel;
//using CKG.Services.PageElements;
using CKG.Base.Kernel.Common;
using System.Data;
using Leasing.lib;

namespace Leasing.forms
{
	public partial class Report_002_02 : System.Web.UI.Page
	{
		
//#Region " Vom Web Form Designer generierter Code "

//    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
//    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

//    End Sub

//    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
//        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
//        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
//        InitializeComponent()
//    End Sub

//#End Region

    private CKG.Base.Kernel.Security.App m_App; 
    private CKG.Base.Kernel.Security.User m_User;
    private DataTable m_objTable;
    private DataTable resultTable;

	

    private void Page_Load(object sender, EventArgs e) //Handles MyBase.Load
	{ 
		m_User = Common.GetUser(this);
        Common.FormAuth(this, m_User);
        if (Session["ResultTableNative"] == null)
		{
			Response.Redirect(Request.UrlReferrer.ToString());
        }
		else
		{
            m_objTable = (DataTable)Session["ResultTableNative"];
        }
        lblHead.Text = m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString();
        try
		{
            m_App = new CKG.Base.Kernel.Security.App(m_User);
            if(!IsPostBack)
			{
                showdata();
            }
        }
		catch (Exception ex)
		{
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
        }
    }

    private void showdata()
	{
        m_App = new CKG.Base.Kernel.Security.App(m_User);
        if (Session["ResultTableNative"] == null)
		{
			Response.Redirect(Request.UrlReferrer.ToString() + "?AppID=" + Session["AppID"].ToString());
        }
		else
		{
			resultTable = (DataTable)Session["ResultTableNative"];
        }
        //lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString
        lblHead.Text = m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"].ToString();
        //ucStyles.TitleText = lblHead.Text

        //Bemerkungsfelder nur editierbar, wenn "Klärfall"
        string kf;

        kf = Request.QueryString["kf"];

        txtBem1.Enabled = false;
        txtBem2.Enabled = false;
        txtBem3.Enabled = false;
        txtBem4.Enabled = false;
        btnSave.Visible = false;
        lblBemerkungen.Visible = false;

        //§§§ JVE 14.07.2006
        trBemerkungen.Visible = false;
        trBemerkungenShow.Visible = false;
        trBemerkungenddl.Visible = false;
        trBemerkungenErfassen.Visible = false;
        trtxtBem1.Visible = false;
        trtxtBem2.Visible = false;
        //lblBemerk.Visible = False
        //------------------------

        if (kf != null && kf != string.Empty) 
		{
            txtBem1.Enabled = true;
            txtBem2.Enabled = true;
            txtBem3.Enabled = true;
            txtBem4.Enabled = true;
            btnSave.Visible = true;
            lblBemerkungen.Visible = true;

            //§§§ JVE 14.07.2006
            //Nur dann die Bemerkungstabelle anzeigen    
            trBemerkungen.Visible = true;
            trBemerkungenShow.Visible = true;
            trBemerkungenddl.Visible = true;
            trBemerkungenErfassen.Visible = true;
            trtxtBem1.Visible = true;
            trtxtBem2.Visible = true;
            //lblBemerk.Visible = True
            //---------------------------------------------
        
		}
        //Daten filtern
        string equi;
        equi = Request["Equipment"];	//.Item["Equipment"];    //Equipment-Nr. holen
        DataTable data;
        int rowIndex = 0;
        DataRow selectedRow;

        data = (DataTable) Session["ResultTableNative"];
        while(data.Rows[rowIndex]["Equipment"].ToString() != equi)
		{
            rowIndex += 1;
        }
        selectedRow = data.Rows[rowIndex];


        //§§§ JVE 14.07.2006
        lblB1.Text = selectedRow["Bemerkung1"].ToString();
        lblB2.Text = selectedRow["Bemerkung2"].ToString();
        lblB3.Text = selectedRow["Bemerkung3"].ToString();
        lblB4.Text = selectedRow["Bemerkung4"].ToString();
        //-------------------

        //Langtext holen
        string strFileName = DateTime.Now.ToString() + m_User.UserName + ".xls";
        LP_03 m_Report = new LP_03(m_User, m_App, strFileName);
        DataTable langtext;

        try
		{
            langtext = m_Report.getLangText(Session["AppID"].ToString(), Session.SessionID.ToString(), this, equi);   //BAPI-Aufruf...
        }
		catch (Exception ex)
		{
            lblError.Text = "Fehler beim Laden des Langtextes.";
            return;
        }

        if(langtext != null)
		{
			if (langtext.Rows.Count > 0)
			{
                ddl1.Visible = true;
                ddl1.Items.Clear();
                ddl1.DataSource = langtext;
                ddl1.DataTextField = "Tdline";
                ddl1.DataBind();                
            }
			else
			{
				ddl1.Visible = false;
			}
			//####### Daten darstellen
			lblInfo.Text = selectedRow["Info"].ToString(); //Info
			//Vertragsnummer, Status,Antrags-Nr.
			lblLVNr.Text = selectedRow[2].ToString();    //LvNr.
			lblStatus.Text = selectedRow[10].ToString().Replace("Sicherungsschein", "");   //Status
			DateTime m_AnlDate = (DateTime) selectedRow[1];
			lblAntrag.Text = m_AnlDate.ToShortDateString();    //Angelegt.
			//lblAntrag2.Text = selectedRow(23).ToString   'Antrags-Nr.


			lblLBeginn.Text = DataRowToShortDateString(selectedRow,5); //Leasingbeginn
			lblLEnde.Text = DataRowToShortDateString(selectedRow,6);   //Leasingende

			//Leasingnehmer
			lblNameLN.Text = selectedRow[7].ToString(); //Leasingnehmer
			lblStrLN.Text = selectedRow["StrasseLN"].ToString(); //StrasseLN
			lblPLZLN.Text = selectedRow["PLZLeasingnehmer"].ToString(); //PLZLeasingnehmer
			lblOrtLN.Text = selectedRow["OrtLeasingnehmer"].ToString(); //OrtLeasingnehmer
			//        lblKonzern.Text = selectedRow(26).ToString 'Konzernname
			//lblKonzernID.Text = selectedRow("Kundennummer").ToString 'KonzernID

			//Versicherungsgeber
			lblNameVG.Text = selectedRow[9].ToString();    //Versicherung
			lblStrVG.Text = selectedRow["StrasseVG"].ToString();   //StrasseVG
			lblPLZVG.Text = selectedRow["PLZVersicherungsgeber"].ToString();   //PLZVersicherungsgeber
			lblOrtVG.Text = selectedRow["OrtVersicherungsgeber"].ToString();   //OrtVersicherungsgeber

			lblVBeginn.Text = DataRowToShortDateString(selectedRow,19); //Versicherungsbeginn
			lblVEnde.Text = DataRowToShortDateString(selectedRow,20);  //Versicherungsende
			lblVschein.Text = selectedRow[24].ToString();  //VersicherungsSchein-Nr

			//Versanddatum, Rückgabedatum, Zurück LN + VG
			
			lblVersandLN.Text = DataRowToShortDateString(selectedRow,11);	//VersandLN
			lblVersandVG.Text = DataRowToShortDateString(selectedRow,15); //RückgabeLN
			lblRueckLN.Text = DataRowToShortDateString(selectedRow,14);   //VersandVG
			lblRueckVG.Text = DataRowToShortDateString(selectedRow,18);  //RückgabeVG

			//JVE 21.03.2006
			//lblKonzs_ZK.Text = CType(selectedRow("Konzernschlüssel"), String)
			lblName1_ZK.Text = selectedRow["Name1"].ToString(); //§§§ JVE 27.10.2006
			lblName1.Text = selectedRow["Name1GP"].ToString();
			lblName2.Text = selectedRow["Name2"].ToString();
			lblName3.Text = selectedRow["Name3"].ToString();
			lblStras_ZO.Text = selectedRow["Strasse"].ToString();
			lblPstlz_ZO.Text = selectedRow["Postleitzahl"].ToString();
			lblOrt_ZO.Text = selectedRow["Ort"].ToString();
			//lblKonzs_ZO.Text = selectedRow["Halternr"].ToString();
			lblName2LN.Text = selectedRow["Name2LN"].ToString();
			lblName3LN.Text = selectedRow["Name3LN"].ToString();
			//--------------

			//§§§ JVE 16.05.2006
			txtBem1.Text = selectedRow["Text1"].ToString();
			txtBem2.Text = selectedRow["Text2"].ToString();
			txtBem3.Text = selectedRow["Text3"].ToString();
			txtBem4.Text = selectedRow["Text4"].ToString();

			//Versicherungsumfang
			lblVersUmf.Text = selectedRow["Versicherungsumfang"].ToString();

			string VersandLNunv = DataRowToShortDateString(selectedRow,27); //VersandLNunv
			string RueckversandLNunv = DataRowToShortDateString(selectedRow,29); //RueckversandLNunv
			string VersandLNunv2 = DataRowToShortDateString(selectedRow,28); //VersandLNunv2
			string RueckversandLNunv2 = DataRowToShortDateString(selectedRow,30); //RueckversandLNunv2

			if(VersandLNunv != string.Empty)
			{
				lblUnvLN.Items.Add(VersandLNunv + " - Erneuter Versand");
			}
			if(RueckversandLNunv != string.Empty)
			{
				lblUnvLN.Items.Add(RueckversandLNunv + " - Rückversand");
			}
			if(VersandLNunv2 != string.Empty)
			{
				lblUnvLN.Items.Add(VersandLNunv2 + " - Erneuter Versand 2");
			}
			if(RueckversandLNunv2 != string.Empty)
			{
				lblUnvLN.Items.Add(RueckversandLNunv2 + " - Rückversand 2");
			}

			string VersandVGunv = DataRowToShortDateString(selectedRow,31); //VersandVGunv
			string RueckversandVGunv = DataRowToShortDateString(selectedRow,33); //RueckversandVGunv
			string VersandVGunv2 = DataRowToShortDateString(selectedRow,32); //VersandVGunv2
			string RueckversandVGunv2 = DataRowToShortDateString(selectedRow,34); //RueckversandVGunv2

			if(VersandVGunv != string.Empty)
			{
				lblUnvVG.Items.Add(VersandVGunv + " - Erneuter Versand");
			}
			if(RueckversandVGunv != string.Empty)
			{
				lblUnvVG.Items.Add(RueckversandVGunv + " - Rückversand");
			}
			if(VersandVGunv2 != string.Empty)
			{
				lblUnvVG.Items.Add(VersandVGunv2 + " - Erneuter Versand 2");
			}
			if (RueckversandVGunv2 != string.Empty)
			{
				lblUnvVG.Items.Add(RueckversandVGunv2 + " - Rückversand 2");
			}
			//Wenn leer, Dropdownlisten verstecken...
			if (lblUnvLN.Items.Count == 0)
			{
				lblUnvLN.Visible = false;
			}
			if (lblUnvVG.Items.Count == 0)
			{
				lblUnvVG.Visible = false;
			}
			//Fahrzeugdaten
			
			lblEz.Text = DataRowToShortDateString(selectedRow,"Erstzulassung"); //Erstzulassung
			lblFzArt.Text = Convert.ToString(selectedRow[21]); //Fahrzeugart
			lblFGNr.Text = Convert.ToString(selectedRow[3]); //Fahrgestellnr
			lblHerst.Text = Convert.ToString(selectedRow[22]); //Hersteller_Typ
			lblKennz.Text = Convert.ToString(selectedRow[4]); //Kennzeichen
			//Mahndaten
			lblMahnsLN.Text = Convert.ToString(selectedRow[13]); //MahnstufeLN
			lblMadatLN.Text = DataRowToShortDateString(selectedRow,12); //MahndatumLN
			lblMahnsVG.Text = Convert.ToString(selectedRow[17]); //MahnstufeVG
			lblMadatVG.Text = DataRowToShortDateString(selectedRow,16); //MahndatumVG


		}
	}

	private string DataRowToShortDateString(DataRow Row,int index)
		{
			string m_string = string.Empty;
			DateTime m_Date;
			try
			{
				m_Date = Row.Field<DateTime>(index);
				m_string = m_Date.ToShortDateString();
			}
			catch
			{
			}

			return m_string;
		}

		private string DataRowToShortDateString(DataRow Row,string index)
		{
			string m_string = string.Empty;
			DateTime m_Date;
			try
			{
				m_Date = Row.Field<DateTime>(index);
				m_string = m_Date.ToShortDateString();
			}
			catch
			{
			}

			return m_string;
		}

		private void btnSave_Click(object sender, EventArgs e) //Handles btnSave.Click
		{
			string strFileName = DateTime.Now.ToString() + m_User.UserName + ".xls";
			LP_03 m_Report = new LP_03(m_User, m_App, strFileName);
			string equi;
			equi = Request["Equipment"];	//.Item["Equipment"];    //Equipment-Nr. holen

			try
			{
				m_Report.saveComments(Session["AppID"].ToString(), Session.SessionID.ToString(),this,
                                  equi, txtBem1.Text, txtBem2.Text, txtBem3.Text, txtBem4.Text);
				lblError.Text = "Die Eingaben wurden gespeichert.";
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}

		}

		private void Page_PreRender(object sender, EventArgs e)
		{
			Common.SetEndASPXAccess(this);
		}

		private void Page_Unload(object sender, EventArgs e) 
		{
			Common.SetEndASPXAccess(this);
		}
	}
}
