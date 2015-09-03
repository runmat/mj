using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Wareneingangsprüfung Detailseite(Artikelanzeige).
    /// </summary>
    public partial class WareneingangDetails : System.Web.UI.Page
    {
        private User m_User;
        private clsWareneingang objWareneingang;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// Aufruf FillGrid().
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objWareneingang"] != null)
            {
                objWareneingang = (clsWareneingang)Session["objWareneingang"];
            }
            else
            {
                lblError.Text = "Fehler beim Laden der Artikel!";
                lbAbsenden.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                lblBestellnummerLieferant.Text = objWareneingang.Lieferant;
                Fillgrid(0, "");
                TrLiefernr.Visible = (!objWareneingang.IstUmlagerung);
                GridView1.Columns[9].Visible = (!objWareneingang.IstUmlagerung);
            }
        }

        /// <summary>
        /// Bestellung im Grid als vollständig markieren. Markieren der Bestellung in der Positionstabelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkVollstaendig_CheckedChanged(object sender, EventArgs e)
        {
            var chkVollst = (CheckBox)sender;

            var tmprow = chkVollst.Parent.Parent;

            var lblEAN = (Label)tmprow.FindControl("lblEAN");
            var txtMenge = (TextBox)tmprow.FindControl("txtPositionLieferMenge");
            var rbJa = (RadioButton)tmprow.FindControl("rbPositionAbgeschlossenJA");
            var rbNein = (RadioButton)tmprow.FindControl("rbPositionAbgeschlossenNEIN");

            var tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];

            var vollst = chkVollst.Checked;

            if (vollst)
            {
                tmpPosition["PositionVollstaendig"] = "X";
                tmpPosition["PositionAbgeschlossen"] = "J";
                rbJa.Checked = true;
                tmpPosition["PositionLieferMenge"] = tmpPosition["BestellteMenge"];
                txtMenge.Text = tmpPosition["BestellteMenge"].ToString();
            }
            else
            {
                tmpPosition["PositionVollstaendig"] = "";
            }

            txtMenge.Enabled = !vollst;
            rbJa.Enabled = !vollst;
            rbNein.Enabled = !vollst;
        }

        /// <summary>
        /// Binden der Checkboxes im Gridviewheader an Jacascriptfunktionen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox tmpChkBox = (CheckBox)e.Row.FindControl("chkVollstaendig");
                TextBox tmpTxtBox = (TextBox)e.Row.FindControl("txtPositionLieferMenge");
                RadioButton tmprbJa = (RadioButton)e.Row.FindControl("rbPositionAbgeschlossenJA");
                RadioButton tmprbNein = (RadioButton)e.Row.FindControl("rbPositionAbgeschlossenNEIN");

                tmpChkBox.Attributes.Add("onclick", "javascript:checkedRow('" + tmpChkBox.ClientID + "' , '"
                                                + tmpTxtBox.ClientID + "','" + tmprbJa.ClientID + "','" + tmprbNein.ClientID + "')");
            }
        }

        /// <summary>
        /// Sortieren nach. Prüfung auf Eingaben im Grid und. Eingaben übernehmen. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Checkgrid();
            Fillgrid(GridView1.PageIndex, e.SortExpression);
        }

        /// <summary>
        /// Prüfung auf Eingaben im Grid. Evtl. Fehler anzeigen. Kein Fehler = Übersicht zum Absenden anzeigen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbAbsenden_Click(object sender, EventArgs e)
        {
            Boolean tmpValid = true;

            Checkgrid();

            if (!objWareneingang.IstUmlagerung && String.IsNullOrEmpty(txtLieferscheinnummer.Text))
            {
                tmpValid = false;
                txtLieferscheinnummer.BorderColor = System.Drawing.Color.Red;
            }
            else
            {
                txtLieferscheinnummer.BorderColor = System.Drawing.Color.Empty;
            }

            if (!txtBelegdatum.Text.IsDate())
            {
                tmpValid = false;
                txtBelegdatum.BorderColor = System.Drawing.Color.Red;
            }
            else
            {
                txtBelegdatum.BorderColor = System.Drawing.Color.Empty;
            }

            Fillgrid(0, "");

            foreach (GridViewRow tmpRow in GridView1.Rows)
            {
                Label lblEAN = (Label)tmpRow.FindControl("lblEAN");
                DataRow tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];

                if ((tmpPosition["PositionAbgeschlossen"].ToString() == "" && !objWareneingang.IstUmlagerung) || tmpPosition["PositionLieferMenge"] == DBNull.Value)
                {
                    tmpValid = false;
                    tmpRow.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    tmpRow.BorderColor = System.Drawing.Color.Empty;
                }
            }

            if (!tmpValid)
            {
                lblError.Text = "Bitte prüfen Sie rot markierte Positionen";
            }
            else
            {
                foreach (GridViewRow tmpRow in GridView1.Rows)
                {
                    tmpRow.BorderColor = System.Drawing.Color.Empty;
                }
                Fillgrid2(0, "");
                mpeWareneingangsCheck.Show();
            }
        }

        /// <summary>
        /// Sortieren der Übersicht zum Absenden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewSortEventArgs</param>
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid2(0, e.SortExpression);
            tblGrid2.Visible = true;
            tblGrid1.Visible = false;
            tblAnzeigeVersandDaten.Visible = false;
            lbAbsenden.Visible = false;
        }

        /// <summary>
        /// Schliessen des Absendendialogs.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbWareneingangKorrektur_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Wareneingang abschliessen. Funktionsaufruf doSubmit().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWareneingangOk_Click(object sender, EventArgs e)
        {
            mpeWareneingangsCheck.Hide();
            doSubmit();
        }

        /// <summary>
        /// Zurück zur Auswahlseite.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wareneingang.aspx?AppID=" + Session["AppID"].ToString() + "&BackToList=B");
        }

        /// <summary>
        /// Wareneingang abgeschlossen? Zurück zur Auswahlseite.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWareneingangsbuchungFinalize_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wareneingang.aspx?AppID=" + Session["AppID"].ToString() + "&BackToList=C");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binden der Artikel zu einer Bestellung an das Grid.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            DataView tmpDataView = new DataView(objWareneingang.Bestellpositionen);

            if (tmpDataView.Count == 0)
            {
                GridView1.Visible = false;
                lblNoData.Visible = true;
            }
            else
            {
                lblNoData.Visible = false;
                GridView1.Visible = true;
                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (!String.IsNullOrEmpty(strSort))
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["Sort"] == null) || ((String)Session["Sort"] == strTempSort))
                    {
                        if (Session["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["Direction"];
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

                    Session["Sort"] = strTempSort;
                    Session["Direction"] = strDirection;
                }

                if (!String.IsNullOrEmpty(strTempSort))
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView1.PageIndex = intTempPageIndex;
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                GridViewRow row = GridView1.HeaderRow;
                Image ImgButton;
                ImgButton = (Image)(row.FindControl("imgbAllVollstaendig"));
                ImgButton.Attributes.Add("onclick", "javascript:SelectRbandChk('" + GridView1.Rows.Count + "', true)");
                ImgButton = (Image)(row.FindControl("imgbAlleUnvollstaendig"));
                ImgButton.Attributes.Add("onclick", "javascript:SelectRbandChk('" + GridView1.Rows.Count + "', false)");
            }
        }

        /// <summary>
        /// Binden der Artikel an das Gridview2. Prüfung der Daten durch Benutzer.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid2(Int32 intPageIndex, String strSort)
        {
            DataView tmpDataView = new DataView(objWareneingang.Bestellpositionen);

            if (tmpDataView.Count == 0)
            {
                GridView2.Visible = false;
                lblNoData.Visible = true;
            }
            else
            {
                lblNoData.Visible = false;
                GridView2.Visible = true;
                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (!String.IsNullOrEmpty(strSort))
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["Sort"] == null) || ((String)Session["Sort"] == strTempSort))
                    {
                        if (Session["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["Direction"];
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

                    Session["Sort"] = strTempSort;
                    Session["Direction"] = strDirection;
                }

                if (!String.IsNullOrEmpty(strTempSort))
                {
                    tmpDataView.Sort = strTempSort + " " + strDirection;
                }

                GridView2.PageIndex = intTempPageIndex;
                GridView2.DataSource = tmpDataView;
                GridView2.DataBind();
            }
        }

        /// <summary>
        /// Prüfung auf Eingaben im Grid und Eingaben übernehmen. 
        /// </summary>
        private void Checkgrid()
        {
            foreach (GridViewRow tmprow in GridView1.Rows)
            {
                var lblEAN = (Label)tmprow.FindControl("lblEAN");
                var txtMenge = (TextBox)tmprow.FindControl("txtPositionLieferMenge");
                var chkVollst = (CheckBox)tmprow.FindControl("chkVollstaendig");
                var rbJa = (RadioButton)tmprow.FindControl("rbPositionAbgeschlossenJA");
                var rbNein = (RadioButton)tmprow.FindControl("rbPositionAbgeschlossenNEIN");

                var tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];

                if (!String.IsNullOrEmpty(txtMenge.Text))
                    tmpPosition["PositionLieferMenge"] = txtMenge.Text;
                else
                    tmpPosition["PositionLieferMenge"] = DBNull.Value;

                tmpPosition["PositionVollstaendig"] = (chkVollst.Checked ? "X" : "0");
                tmpPosition["PositionAbgeschlossen"] = (rbJa.Checked || chkVollst.Checked ? "J" : rbNein.Checked ? "N" : "");

                objWareneingang.Bestellpositionen.AcceptChanges();
            }
        }

        /// <summary>
        /// Senden der Daten an SAP.
        /// </summary>
        private void doSubmit()
        {
            if (objWareneingang.IstUmlagerung)
                objWareneingang.sendUmlToSAP(txtBelegdatum.Text);
            else
                objWareneingang.sendOrderCheckToSAP(txtLieferscheinnummer.Text, txtBelegdatum.Text);

            MPEWareneingangsbuchungResultat.Show();

            if (objWareneingang.ErrorOccured)
            {
                lblWareneingangsbuchungMeldung.ForeColor = System.Drawing.Color.Red;
                lblWareneingangsbuchungMeldung.Text = "Bei der Wareneingangsbuchung ist ein Fehler aufgetreten: <br><br> " + objWareneingang.Message;
            }
            else
            {
                lblWareneingangsbuchungMeldung.ForeColor = System.Drawing.Color.Green;
                lblWareneingangsbuchungMeldung.Text = "Ihre Wareneingangsbuchung war erfolgreich.";
            }
        }

        #endregion
    }
}
