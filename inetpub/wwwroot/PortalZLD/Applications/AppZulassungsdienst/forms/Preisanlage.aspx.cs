using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;

namespace AppZulassungsdienst
{
    /// <summary>
    /// Preisanlage für Neukunden und Preisfindung.
    /// </summary>
    public partial class Preisanlage : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private VoerfZLD objVorerf;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
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
            if (IsPostBack != true)
            {
                objVorerf = new VoerfZLD(ref m_User, m_App, "");
                fillForm();

            }
            else 
            {
                objVorerf = (VoerfZLD)Session["objVorerf"];
            }
        }

        /// <summary>
        /// Laden neuer angelegter Kunden aus SAP. Z_ZLD_EXPORT_NEW_DEBI.
        /// </summary>
        private void fillForm()
        {

            objVorerf.VKBUR = m_User.Reference.Substring(4, 4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);
            
            objVorerf.getNeueKunden(Session["AppID"].ToString(), Session.SessionID, this);
            Session["objVorerf"] = objVorerf;
            if (objVorerf.Status > 0)
            {
                lblError.Text = objVorerf.Message;
                return;
            }

            Fillgrid(0, "");
        }

        /// <summary>
        /// Füllen des Grid mit neu angelegten Kunden.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            DataView tmpDataView = objVorerf.tblNeueKunden.DefaultView;
            String strFilter = "";
            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                lblError.Text = "Kein neuen Kunden gefunden!";
            }
            else
            {
                GridView1.Visible = true;
                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((this.ViewState["Sort"] == null) || ((String)this.ViewState["Sort"] == strTempSort))
                    {
                        if (this.ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)this.ViewState["Direction"];
                        }
                    }
                    else
                    {
                        strDirection = "desc";
                    }

                    if (strDirection == "asc")
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = "asc";
                    }

                    this.ViewState["Sort"] = strTempSort;
                    this.ViewState["Direction"] = strDirection;
                }

                if (strTempSort.Length != 0)
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Setzen des Preispflege Status("OK"). Weiterleiten an Preiserfassung pro Landkreis("proLandkr").
        /// Weiterleiten an Preiserfassung ohne Landkreis("ohneLandkr").
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblError.Text = "";
            lblMessage.Visible = false;
            if (e.CommandName == "proLandkr")
            {
                objVorerf.NeueKundenNr = e.CommandArgument.ToString();
                objVorerf.NeueKundenName = objVorerf.tblNeueKunden.Select("KUNNR = '" + objVorerf.NeueKundenNr + "'")[0]["NAME1"].ToString();
                Session["objVorerf"] = objVorerf;
                Response.Redirect("Preisanlage_2.aspx?AppID=" + Session["AppID"].ToString());
            }
            if (e.CommandName == "ohneLandkr")
            {
                objVorerf.NeueKundenNr = e.CommandArgument.ToString();
                objVorerf.NeueKundenName = objVorerf.tblNeueKunden.Select("KUNNR = '" + objVorerf.NeueKundenNr + "'")[0]["NAME1"].ToString();
                Session["objVorerf"] = objVorerf;
                Response.Redirect("Preisanlage_3.aspx?AppID=" + Session["AppID"].ToString());
            }
            if (e.CommandName == "OK")
            {
                lblError.Text = "";
                lblMessage.Text = "";
                objVorerf.NeueKundenNr = e.CommandArgument.ToString();
                DataRow[] NewRow = objVorerf.tblNeueKunden.Select("KUNNR = '" + objVorerf.NeueKundenNr + "'");
                if (NewRow.Length == 1) 
                {
                    objVorerf.SaveNeueKunden(Session["AppID"].ToString(), Session.SessionID, this, NewRow[0]);
                    if (objVorerf.Status != 0)
                    {
                        lblError.Text = "Fehler beim Speichern der Daten!";
                    }
                    else 
                    {
                        objVorerf.tblNeueKunden.Rows.Remove(NewRow[0]);
                        Session["objVorerf"] = objVorerf;
                        lblMessage.Visible = true;
                        lblMessage.Text="Daten erfolgreich gespeichert!";
                        Fillgrid(0, "");

                    }
                }
            }
        }

    }
}
