using System;
using System.Linq;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Gruppen für Lieferscheinselektion pflegen. 
    /// </summary>
    public partial class Kundengruppe : System.Web.UI.Page
    {
        private User m_User;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(m_User.Reference);
                objCommon.getSAPDatenStamm();
                objCommon.getSAPZulStellen();
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

            if (String.IsNullOrEmpty(txtKunnr.Text))
            {
                lblErrorGroup.Text = "Kein Kunde ausgewählt.";
            }
            else
            {
                objCommon.SetKunden_TourenZuordnung(txtKunnr.Text, "I");

                if (objCommon.ErrorOccured)
                {
                    lblErrorGroup.Text = objCommon.Message;
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
        /// Neue Gruppe anlegen(objCommon.SetKunden_Touren).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnInsertGroup_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtGruppe.Text))
            {
                lblErrorInsert.Text = "Bitte geben Sie eine Gruppenbezeichnung ein.";
            }
            else
            {
                objCommon.Bezeichnung = txtGruppe.Text;
                String sAction = "I";
                if (String.IsNullOrEmpty(lblGroupIDEdit.Text))
                {
                    objCommon.GroupOrTourID = "";
                }
                else
                {
                    objCommon.GroupOrTourID = lblGroupIDEdit.Text.PadLeft(10, '0');
                    sAction = "C";
                }
                objCommon.SetKunden_Touren("K", sAction);

                if (objCommon.ErrorOccured)
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
                case "Del":
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');
                    objCommon.SetKunden_Touren("K", "D");

                    if (objCommon.ErrorOccured)
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

                case "Insert":
                    lblGroupID.Text = e.CommandArgument.ToString();
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');

                    var gruppe = objCommon.Kundengruppen.FirstOrDefault(k => k.Gruppe == lblGroupID.Text);
                    if (gruppe != null)
                        lblGruppeShow.Text = gruppe.GruppenName;

                    FillCustomerTable();
                    pnlQuery.Visible = false;
                    Panel2.Visible = false;
                    Panel1.Visible = true;
                    break;

                case "Edt":
                    lblGroupIDEdit.Text = e.CommandArgument.ToString();
                    objCommon.GroupOrTourID = e.CommandArgument.ToString().PadLeft(10, '0');

                    var gruppeEdit = objCommon.Kundengruppen.FirstOrDefault(k => k.Gruppe == lblGroupIDEdit.Text);
                    if (gruppeEdit != null)
                        txtGruppe.Text = gruppeEdit.GruppenName;

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
            if (e.CommandName == "Del")
            {
                objCommon.SetKunden_TourenZuordnung(e.CommandArgument.ToString(), "D");

                if (objCommon.ErrorOccured)
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

        #endregion

        #region Methods

        /// <summary>
        /// Tabelle mit bereits angelegten Gruppen laden und anzeigen.
        /// </summary>
        private void FillGroupTable()
        {
            lblError.Text = "";
            objCommon.GetGruppen_Touren("K");

            if (objCommon.ErrorOccured)
            {
                lblError.Text = objCommon.Message;
                GridView1.Visible = false;
                return;
            }

            if (objCommon.Kundengruppen.Any(k => k.Gruppe != "0"))
            {
                GridView1.Visible = true;
                GridView1.DataSource = objCommon.Kundengruppen.Where(k => k.Gruppe != "0").ToList();
                GridView1.DataBind();
            }
            else
            {
                GridView1.Visible = false;
                lblError.Text = "Es sind noch keine Gruppen angelegt!";
            }
        }

        /// <summary>
        /// Kundentabelle der ausgewählten Gruppe laden und anzeigen. 
        /// objCommon.GetKunden_TourenZuordnung
        /// </summary>
        private void FillCustomerTable()
        {
            lblErrorGroup.Text = "";
            objCommon.GetKunden_TourenZuordnung();

            if (objCommon.ErrorOccured)
            {
                lblErrorGroup.Text = objCommon.Message;
                GridView2.Visible = false;
                return;
            }

            if (objCommon.KundenZurGruppe.Any())
            {
                GridView2.Visible = true;
                GridView2.DataSource = objCommon.KundenZurGruppe;
                GridView2.DataBind();
            }
            else
            {
                GridView2.Visible = false;
                lblError.Text = "Es sind noch keine Zuordnungen angelegt!";
            }
        }

        /// <summary>
        /// Dropdown mit Kundenstamm daten laden.
        /// </summary>
        private void fillDropDown()
        {
            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv).ToList();
            ddlKunnr.DataValueField = "KundenNr";
            ddlKunnr.DataTextField = "Name";
            ddlKunnr.DataBind();

            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            ddlKunnr.Attributes.Add("onchange", "SetTexttValue(" + ddlKunnr.ClientID + "," + txtKunnr.ClientID + ")");
        }

        #endregion
    }
}