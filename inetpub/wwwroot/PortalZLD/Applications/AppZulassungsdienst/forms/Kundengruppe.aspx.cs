using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Gruppen für Lieferscheinselektion pflegen. 
    /// </summary>
    public partial class Kundengruppe : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private ZLDCommon objCommon;
        
        protected void Page_Init(object sender, EventArgs e)
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

            fillDropDown();
            }

        protected void Page_Load(object sender, EventArgs e)
            {
            if (!IsPostBack)
            {
                FillGroupTable();
            }
        }

        /// <summary>
        /// Tabelle mit bereits angelegten Gruppen laden und anzeigen (Z_ZLD_GET_GRUPPE).
        /// </summary>
        private void FillGroupTable() 
        {
            lblError.Text = "";
            objCommon.GetGruppen_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K");
            if (objCommon.Message.Length > 0)
            {
                lblError.Text = objCommon.Message;
                GridView1.Visible = false;
            }
            else if (objCommon.tblGruppeTouren.Rows.Count > 0)
            {
                DataView tmpDataView = objCommon.tblGruppeTouren.DefaultView;
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
        /// objCommon.GetKunden_TourenZuordnung
        /// </summary>
        private void FillCustomerTable()
        {
            lblErrorGroup.Text = "";
            objCommon.GetKunden_TourenZuordnung(Session["AppID"].ToString(), Session.SessionID, this);
            if (objCommon.Message.Length > 0)
            {
                lblErrorGroup.Text = objCommon.Message;
                GridView2.Visible = false;

            }
            else if (objCommon.tblKundeGruppe.Rows.Count > 0)
            {
                DataView tmpDataView = objCommon.tblKundeGruppe.DefaultView;
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
        /// Dropdown mit Kundenstamm daten laden.
        /// </summary>
        private void fillDropDown()
        {
            DataView tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            ddlKunnr.Attributes.Add("onchange", "SetTexttValue(" + ddlKunnr.ClientID + "," + txtKunnr.ClientID + ")");
        }

        /// <summary>
        /// Neue Gruppe anlegen. Panels für die Eingabe sichtbar machen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnNew_Click(object sender, EventArgs e)
        {
            lblGroupIDEdit.Text = "";
            txtGruppe.Text = "";
            pnlQuery.Visible = false;
            Panel1.Visible = false;
            Panel2.Visible = true;
        }

        /// <summary>
        /// Kunden zur Gruppe hinzufügen(objCommon.SetKunden_TourenZuordnung).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">,</param>
        protected void lbtnInsert_Click(object sender, EventArgs e)
        {
            lblErrorGroup.Text = "";
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblErrorGroup.Text = "Kein Kunde ausgewählt.";
            }
            else 
            {
                objCommon.SetKunden_TourenZuordnung(Session["AppID"].ToString(), Session.SessionID, this, ddlKunnr.SelectedValue, "I");
                if (objCommon.Message.Length > 0)
                {
                    lblErrorGroup.Text = objCommon.Message;
                }
                else
                {
                    FillCustomerTable();
                    ddlKunnr.SelectedIndex = 0;
                    txtKunnr.Text = "";
                    //pnlQuery.Visible = true;
                    //Panel1.Visible = false;
                    //Panel2.Visible = false;
                }     

            }
        }

        /// <summary>
        /// Neue Gruppe anlegen(objCommon.SetKunden_Touren).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnInsertGroup_Click(object sender, EventArgs e)
        {
            if (txtGruppe.Text.Length == 0 )
            {
                lblErrorInsert.Text = "Bitte geben Sie eine Gruppenbezeichnung ein.";
            }
            else
            {
                objCommon.Bezeichnung = txtGruppe.Text;
                String sAction = "I";
                if (lblGroupIDEdit.Text == "")
                {
                    objCommon.GroupOrTourID = "";
                }
                else 
                {
                    objCommon.GroupOrTourID = lblGroupIDEdit.Text.PadLeft(10, '0');
                    sAction = "C";
                }
                objCommon.SetKunden_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K", sAction);
                if (objCommon.Message.Length > 0)
                {
                    lblErrorInsert.Text = objCommon.Message;
                }
                else
                {
                    FillGroupTable();
                    pnlQuery.Visible = true;
                    Panel1.Visible = false;
                    Panel2.Visible = false;
                }
            }
        }

        /// <summary>
        /// Löschen, Bearbeiten von Gruppen und Kunden zur Gruppe hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case("Del"):
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');
                    objCommon.SetKunden_Touren(Session["AppID"].ToString(), Session.SessionID, this, "K", "D");
                    if (objCommon.Message.Length > 0)
                    {
                        lblError.Text = objCommon.Message;
                    }
                    else
                    {
                        FillGroupTable();
                        pnlQuery.Visible = true;
                        Panel1.Visible = false;
                        Panel2.Visible = false;
                    }
                    break;

                case ("Insert"):
                        lblGroupID.Text = e.CommandArgument.ToString();
                        objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10,'0');
                        lblGruppeShow.Text = objCommon.tblGruppeTouren.Select("GRUPPE = '" + lblGroupID.Text + "'")[0]["BEZEI"].ToString();
                        FillCustomerTable();
                        pnlQuery.Visible = false;
                        Panel2.Visible = false;
                        Panel1.Visible = true;
                    break;

                case ("Edt"):
                        lblGroupIDEdit.Text = e.CommandArgument.ToString();
                        objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');
                        txtGruppe.Text = objCommon.tblGruppeTouren.Select("GRUPPE = '" + lblGroupIDEdit.Text + "'")[0]["BEZEI"].ToString();
                        pnlQuery.Visible = false;
                        Panel1.Visible = false;
                        Panel2.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Löschen eines Kunden in einer Gruppe.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case("Del"):
                    
                    objCommon.SetKunden_TourenZuordnung(Session["AppID"].ToString(), Session.SessionID, this,  e.CommandArgument.ToString(), "D");
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
            }
        }

        /// <summary>
        /// Bearbeiten einer Gruppe abbrechen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnCancelGroup_Click(object sender, EventArgs e)
        {
            pnlQuery.Visible = true;
            Panel1.Visible = false;
            Panel2.Visible = false;
        }

        /// <summary>
        /// Zurück zur Startseite.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }
    }
}