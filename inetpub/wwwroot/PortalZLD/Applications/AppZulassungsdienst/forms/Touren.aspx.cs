using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Touren für Lieferscheinselektion pflegen.
    /// </summary>
    public partial class Touren : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);

                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                
                objCommon = (ZLDCommon)Session["objCommon"];
                

            }
            if (IsPostBack != true)
            {
                FillTourTable();

            }
        }

        /// <summary>
        ///  Tabelle mit bereits angelegten Touren laden und anzeigen (Z_ZLD_GET_GRUPPE).
        /// </summary>
        private void FillTourTable()
        {
            lblError.Text = "";
            objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T");
            if (objCommon.Message.Length > 0)
            {
                lblError.Text = objCommon.Message;
                GridView1.Visible = false;
            }
            else if (objCommon.tblGruppeTouren.Rows.Count > 0)
            {
                DataView tmpDataView = new DataView();
                tmpDataView = objCommon.tblGruppeTouren.DefaultView;
                tmpDataView.Sort = "GRUPPE";
                if (tmpDataView.Count == 0)
                {
                    GridView1.Visible = false;

                }
                else
                {
                    GridView1.Visible = true;
                    GridView1.DataSource = tmpDataView;
                    GridView1.DataBind();

                }
            }
            else
            {
                lblError.Text = "Es sind noch keine Gruppen angelegt!";
            }


        }
        /// <summary>
        /// Kundentabelle der ausgewählten Gruppe laden und anzeigen(Z_ZLD_GET_GRUPPE_KDZU). 
        /// </summary>
        private void FillCustomerTable()
        {
            lblErrorTour.Text = "";
            objCommon.GetKunden_TourenZuordnung(Session["AppID"].ToString(), Session.SessionID, this);
            if (objCommon.Message.Length > 0)
            {
                lblErrorTour.Text = objCommon.Message;
                GridView2.Visible = false;

            }
            else if (objCommon.tblKundeGruppe.Rows.Count > 0)
            {
                DataView tmpDataView = new DataView();
                tmpDataView = objCommon.tblKundeGruppe.DefaultView;
                if (tmpDataView.Count == 0)
                {
                    GridView2.Visible = false;
                }
                else
                {
                    GridView2.Visible = true;
                    GridView2.DataSource = tmpDataView;
                    GridView2.DataBind();
                }
            }
            else
            {
                lblError.Text = "Es sind noch keine Zuordnungen angelegt!";
            }


        }

        /// <summary>
        ///  Dropdown mit Kundenstamm daten laden.
        /// </summary>
        private void fillDropDown()
        {

            DataView tmpDView = new DataView();
            tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();
            ddlKunnr.SelectedValue = "0";
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            ddlKunnr.Attributes.Add("onchange", "SetTexttValue(" + ddlKunnr.ClientID + "," + txtKunnr.ClientID + ")");
        }
        /// <summary>
        /// Neue Gruppe anlegen. Panels für die Eingabe sichtbar machen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e"></param>
        protected void lbtnNew_Click(object sender, EventArgs e)
        {
            lblTourIDEdit.Text = "";
            txtGruppe.Text = "";
            pnlQuery.Visible = false;
            Panel1.Visible = false;
            Panel2.Visible = true;
        }
        /// <summary>
        /// Kunden zur Tour hinzufügen(Z_ZLD_SET_GRUPPE_KDZU).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnInsert_Click(object sender, EventArgs e)
        {
            lblErrorTour.Text = "";
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblErrorTour.Text = "Kein Kunde ausgewählt.";
            }
            else
            {
                objCommon.SetKunden_TourenZuordnung(Session["AppID"].ToString(), Session.SessionID, this, ddlKunnr.SelectedValue, "I");
                if (objCommon.Message.Length > 0)
                {
                    lblErrorTour.Text = objCommon.Message;
                }
                else
                {
                    FillCustomerTable();
                    ddlKunnr.SelectedIndex = 0;
                    txtKunnr.Text = "";
                }

            }
        }
        /// <summary>
        /// Neue Tour anlegen( Z_ZLD_SET_GRUPPE).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnInsertTour_Click(object sender, EventArgs e)
        {
            if (txtGruppe.Text.Length == 0)
            {
                lblErrorInsert.Text = "Bitte geben Sie eine Gruppenbezeichnung ein.";
            }
            else
            {
                objCommon.Bezeichnung = txtGruppe.Text;
                String sAction = "I";
                if (lblTourIDEdit.Text == "")
                {
                    objCommon.GroupOrTourID = "";
                }
                else
                {
                    objCommon.GroupOrTourID = lblTourIDEdit.Text.PadLeft(10, '0'); ;
                    sAction = "C";
                }
                objCommon.SetKunden_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T", sAction);
                if (objCommon.Message.Length > 0)
                {
                    lblErrorInsert.Text = objCommon.Message;
                }
                else
                {
                    FillTourTable();
                    pnlQuery.Visible = true;
                    Panel1.Visible = false;
                    Panel2.Visible = false;
                }

            }

        }
        /// <summary>
        /// Löschen, Bearbeiten von Touren und Kunden zur Tour hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            switch (e.CommandName)
            {
                case ("Del"):
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0'); ;
                    objCommon.SetKunden_Touren(Session["AppID"].ToString(), Session.SessionID, this, "T", "D");
                    if (objCommon.Message.Length > 0)
                    {
                        lblError.Text = objCommon.Message;
                    }
                    else
                    {
                        FillTourTable();
                        pnlQuery.Visible = true;
                        Panel1.Visible = false;
                        Panel2.Visible = false;
                    }
                    break;
                case ("Insert"):
                    lblTourID.Text = e.CommandArgument.ToString();
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');
                    lblTourShow.Text = objCommon.tblGruppeTouren.Select("GRUPPE = '" + lblTourID.Text + "'")[0]["BEZEI"].ToString();
                    fillDropDown();
                    FillCustomerTable();
                    pnlQuery.Visible = false;
                    Panel2.Visible = false;
                    Panel1.Visible = true;
                    break;
                case ("Edt"):
                    lblTourIDEdit.Text = e.CommandArgument.ToString();
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');
                    txtGruppe.Text = objCommon.tblGruppeTouren.Select("GRUPPE = '" + lblTourIDEdit.Text + "'")[0]["BEZEI"].ToString();
                    pnlQuery.Visible = false;
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    break;
                default:
                    break;
            }



        }
        /// <summary>
        /// Löschen eines Kunden in einer Tour.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case ("Del"):

                    objCommon.SetKunden_TourenZuordnung(Session["AppID"].ToString(), Session.SessionID, this, e.CommandArgument.ToString(), "D");
                    if (objCommon.Message.Length > 0)
                    {
                        lblError.Text = objCommon.Message;
                    }
                    else
                    {
                        FillCustomerTable();
                        pnlQuery.Visible = false;
                        Panel1.Visible = true;
                        Panel2.Visible = false;
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Bearbeiten einer Tour abbrechen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnCancelTour_Click(object sender, EventArgs e)
        {
            pnlQuery.Visible = true;
            Panel1.Visible = false;
            Panel2.Visible = false;
        }
        /// <summary>
        /// Zurück zur Startseite.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }
    }
}