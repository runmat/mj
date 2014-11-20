using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using System.Data;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Listenansicht nach Selektion Vorerfassuung Versanzulassung erfasst durch Autohaus.
    /// </summary>
    public partial class AHVersandListe : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User, "");
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"];

            objNacherf = (NacherfZLD)Session["objNacherf"];
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
                Fillgrid(0, "", null);
            }
        }

        /// <summary>
        /// Binden der Rückgabetabelle(objNacherf.AHVersandListe) an das Gridview1.
        /// </summary>
        /// <param name="intPageIndex">Index der Gridviewseite></param>
        /// <param name="strSort">Sortierung nach</param>
        /// <param name="Rowfilter">Filterkriterien</param>
        private void Fillgrid(Int32 intPageIndex, String strSort, String Rowfilter)
        {

            DataView tmpDataView = objNacherf.AHVersandListe.DefaultView;
            String strFilter = "";
            if (Rowfilter != null)
            {
                ViewState["Rowfilter"] = Rowfilter;
                strFilter = Rowfilter;
            }
            else if (ViewState["Rowfilter"] != null)
            {
                strFilter = (String)this.ViewState["Rowfilter"];
            }

            tmpDataView.RowFilter = strFilter;

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                Result.Visible = false;
            }
            else
            {
                Result.Visible = true;
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
                                String myId = GridView1.DataKeys[0]["ZULBELN"].ToString();
                String Css = "ItemStyle";
                foreach (GridViewRow row in GridView1.Rows)
                {

                    if (GridView1.DataKeys[row.RowIndex]["ZULBELN"].ToString() == myId)
                    {
                        row.CssClass = Css;
                        myId = GridView1.DataKeys[row.RowIndex]["ZULBELN"].ToString();
                    }
                    else
                    {
                        if (Css == "ItemStyle")
                        {
                            Css = "GridTableAlternate2";
                        }
                        else
                        {
                            Css = "ItemStyle";
                        }
                        row.CssClass = Css;
                        myId = GridView1.DataKeys[row.RowIndex]["ZULBELN"].ToString();

                    }
                }
  
            }
        }

        /// <summary>
        /// Nach bestimmter Spalte sortieren. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(0, e.SortExpression, null);
        }

        /// <summary>
        /// Zurück zur Listenansicht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("AHVersandSelect.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Auswahl eines Vorganges zum Bearbeiten(AHVersandChange) bzw. Löschen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edt") 
            {
                Response.Redirect("AHVersandChange.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + e.CommandArgument);
            }
            if (e.CommandName == "Del")
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
                Int32 Index;
                Int32.TryParse(e.CommandArgument.ToString(), out Index);
                Label lblID = (Label)GridView1.Rows[Index].FindControl("lblID");
                Label lblLoeschKZ = (Label)GridView1.Rows[Index].FindControl("lblLoeschKZ");
                String Loeschkz = "";
                Int32 IDSatz;
                Int32.TryParse(lblID.Text, out IDSatz);
                if (lblLoeschKZ.Text == "L")
                {
                    lblLoeschKZ.Text = Loeschkz;
                }
                else
                {
                    Loeschkz = "L";
                    lblLoeschKZ.Text = Loeschkz;

                }
                DataRow[] RowsEdit = objNacherf.AHVersandListe.Select("ZULBELN = '" + IDSatz + "'");
                foreach (DataRow Row in RowsEdit)
                {
                    Row["toDelete"] = Loeschkz;
                }
                Session["objNacherf"] = objNacherf;
            }
        }

        /// <summary>
        /// Setzen des Löschkennzeichens der markierten Vorgänge in SAP. Bapi: Z_ZLD_SET_LOEKZ
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSend_Click(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            DataRow[] RowsEdit = objNacherf.AHVersandListe.Select("toDelete = 'L'");
            if (RowsEdit.Length > 0) 
            {
                objNacherf.setSAPLOEKZ(Session["AppID"].ToString(), Session.SessionID, this);
                if (objNacherf.Status == 0)
                {
                    foreach (DataRow dRow in RowsEdit)
                    {
                        objNacherf.AHVersandListe.Rows.Remove(dRow);
                    }
                    Fillgrid(0, "","");

                }
                else
                {
                   lblError.Text += objNacherf.Message;
                }
            }
        }
        
    }
}