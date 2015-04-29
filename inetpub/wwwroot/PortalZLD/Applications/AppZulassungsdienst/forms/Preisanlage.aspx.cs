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
        private clsPreisanlage objPreisanlage;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
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
            if (!IsPostBack)
            {
                objPreisanlage = new clsPreisanlage(m_User.Reference);
                fillForm();
            }
            else
            {
                objPreisanlage = (clsPreisanlage)Session["objPreisanlage"];
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

            switch (e.CommandName)
            {
                case "proLandkr":
                    objPreisanlage.NeueKundenNr = e.CommandArgument.ToString();
                    objPreisanlage.NeueKundenName = objPreisanlage.tblNeueKunden.Select("KUNNR = '" + objPreisanlage.NeueKundenNr + "'")[0]["NAME1"].ToString();
                    Session["objPreisanlage"] = objPreisanlage;
                    Response.Redirect("Preisanlage_2.aspx?AppID=" + Session["AppID"].ToString());
                    break;

                case "ohneLandkr":
                    objPreisanlage.NeueKundenNr = e.CommandArgument.ToString();
                    objPreisanlage.NeueKundenName = objPreisanlage.tblNeueKunden.Select("KUNNR = '" + objPreisanlage.NeueKundenNr + "'")[0]["NAME1"].ToString();
                    Session["objPreisanlage"] = objPreisanlage;
                    Response.Redirect("Preisanlage_3.aspx?AppID=" + Session["AppID"].ToString());
                    break;

                case "OK":
                    lblError.Text = "";
                    lblMessage.Text = "";
                    objPreisanlage.NeueKundenNr = e.CommandArgument.ToString();
                    DataRow[] NewRow = objPreisanlage.tblNeueKunden.Select("KUNNR = '" + objPreisanlage.NeueKundenNr + "'");
                    if (NewRow.Length > 0)
                    {
                        objPreisanlage.SaveNeueKunden(NewRow[0]);
                        if (objPreisanlage.ErrorOccured)
                        {
                            lblError.Text = "Fehler beim Speichern der Daten!";
                        }
                        else
                        {
                            objPreisanlage.tblNeueKunden.Rows.Remove(NewRow[0]);
                            Session["objPreisanlage"] = objPreisanlage;
                            lblMessage.Visible = true;
                            lblMessage.Text = "Daten erfolgreich gespeichert!";
                            Fillgrid();

                        }
                    }
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Laden neuer angelegter Kunden aus SAP.
        /// </summary>
        private void fillForm()
        {
            objPreisanlage.getNeueKunden();
            Session["objPreisanlage"] = objPreisanlage;

            if (objPreisanlage.ErrorOccured)
            {
                lblError.Text = objPreisanlage.Message;
                return;
            }

            Fillgrid();
        }

        /// <summary>
        /// Füllen des Grid mit neu angelegten Kunden.
        /// </summary>
        private void Fillgrid()
        {
            DataView tmpDataView = new DataView(objPreisanlage.tblNeueKunden);
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
                GridView1.PageIndex = 0;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
            }
        }

        #endregion
    }
}
