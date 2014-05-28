using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using System.Web.UI.HtmlControls;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Wareneingangsprüfung Detailseite(Artikelanzeige).
    /// </summary>
    public partial class WareneingangDetails : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private clsWareneingang objWareneingang;
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
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objWareneingang"] != null)

            { objWareneingang = (clsWareneingang)Session["objWareneingang"]; }
            else
            { 
                lblError.Text = "Feher beim Laden der Artikel!";
                lbAbsenden.Visible = false;
                return;
            }
            
            if (IsPostBack != true)
            {
                lblBestellnummerLieferant.Text = objWareneingang.Lieferant;
                Fillgrid(0,"");
                GridView1.Columns[9].Visible = false;
            }
         }
        /// <summary>
        /// Binden der Artikel zu einer Bestellung an das Grid.
        /// </summary>
        /// <param name="intPageIndex">Seitenindex</param>
        /// <param name="strSort">Sortierung nach</param>
        private void Fillgrid(Int32 intPageIndex, String strSort)
        {

            DataView tmpDataView = new DataView();
            tmpDataView = objWareneingang.Bestellpositionen.DefaultView;


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
                GridViewRow row   = GridView1.HeaderRow;
                Image ImgButton ;
                ImgButton =  (Image)(row.FindControl("imgbAllVollstaendig"));
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

            DataView tmpDataView = new DataView();
            tmpDataView = objWareneingang.Bestellpositionen.DefaultView;


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

                GridView2.PageIndex = intTempPageIndex;
                GridView2.DataSource = tmpDataView;
                GridView2.DataBind();

                foreach (GridViewRow tmpRow in GridView2.Rows)
                {
                    Label lblEAN = (Label)tmpRow.FindControl("lblEAN");
                    DataRow tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];
                    if (tmpPosition["PositionLieferMenge"] == DBNull.Value)
                    {
                        Label lblLieferMenge = (Label)tmpRow.FindControl("lblLieferMenge");
                        lblLieferMenge.Text = tmpPosition["BestellteMenge"].ToString();
                    }

                
                }


            }
        }
        /// <summary>
        /// Bestellung im Grid als vollständig markieren. Markieren der Bestellung in der Positionstabelle.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkVollstaendig_CheckedChanged(object sender, EventArgs e)
        {
            Boolean tmpBool = false;
            CheckBox check =(CheckBox)sender;
            Label tmpLabel;
            GridViewRow tmpGridRow = (GridViewRow)(check.Parent.Parent);
            tmpLabel = (Label)tmpGridRow.FindControl("lblEAN");
            if (check.Checked)
            {
                objWareneingang.Bestellpositionen.Select("Bestellposition='" + tmpLabel.Text + "'")[0]["PositionVollstaendig"] = "X";
                objWareneingang.Bestellpositionen.Select("Bestellposition='" + tmpLabel.Text + "'")[0]["PositionAbgeschlossen"] = "J";
                tmpBool = false;
            }
            else 
            {
                objWareneingang.Bestellpositionen.Select("Bestellposition='" + tmpLabel.Text + "'")[0]["PositionVollstaendig"] = "";
                tmpBool = true; 
            
            }

            TextBox tmpTextbox;
            RadioButton tmpRadio;

            tmpTextbox = (TextBox)tmpGridRow.FindControl("txtPositionLieferMenge");
            tmpTextbox.Enabled = tmpBool;
            tmpRadio = (RadioButton)tmpGridRow.FindControl("rbPositionAbgeschlossenJA");
            tmpRadio.Enabled = tmpBool;
            tmpRadio.Checked = false;
            tmpRadio = (RadioButton)tmpGridRow.FindControl("rbPositionAbgeschlossenNEIN");
            tmpRadio.Enabled = tmpBool;
            tmpRadio.Checked = false;

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
                CheckBox tmpChkBox  = (CheckBox)e.Row.FindControl("chkVollstaendig");
                TextBox tmpTxtBox  = (TextBox)e.Row.FindControl("txtPositionLieferMenge");
                HtmlInputHidden tmphidden  = (HtmlInputHidden)e.Row.FindControl("txtPositionLieferMenge2");
                RadioButton tmprbJa = (RadioButton)e.Row.FindControl("rbPositionAbgeschlossenJA");
                RadioButton tmprbNein  = (RadioButton)e.Row.FindControl("rbPositionAbgeschlossenNEIN");
                
                tmpChkBox.Attributes.Add("onclick", "javascript:checkedRow('" + tmpChkBox.ClientID + "' , '" 
                                                + tmpTxtBox.ClientID + "','" + tmprbJa.ClientID + "','" + tmprbNein.ClientID + "')");
                tmpTxtBox.Attributes.Add("onkeyup", "javascript:MengeChanged('" + tmphidden.ClientID + "' , '" + tmpTxtBox.ClientID + "')");
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
        /// Prüfung auf Eingaben im Grid und Eingaben übernehmen. 
        /// </summary>
        private void Checkgrid() 
        {
            foreach (GridViewRow tmprow in GridView1.Rows)
            {
                Label lblEAN = (Label)tmprow.FindControl("lblEAN");
                DataRow tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];
                String tmpMenge = "";
                HtmlInputHidden tmphidden = (HtmlInputHidden)tmprow.FindControl("txtPositionLieferMenge2");
                tmpMenge = tmphidden.Value;

                if (tmpMenge == "")
                {
                    tmpPosition["PositionLieferMenge"] = DBNull.Value;
                }
                else 
                {
                    tmpPosition["PositionLieferMenge"] = tmpMenge;
                }

                String sVollAll="";
                String sVollRowJa="";

                CheckBox tmpChkBox = (CheckBox)tmprow.FindControl("chkVollstaendig");
                if (tmpChkBox.Checked)
                {
                    sVollAll = "X";
                }
                else
                {
                    sVollAll = "0";
                }
                RadioButton tmprbJa = (RadioButton)tmprow.FindControl("rbPositionAbgeschlossenJA");
                RadioButton tmprbNein = (RadioButton)tmprow.FindControl("rbPositionAbgeschlossenNEIN");
                if (tmprbJa.Checked)
                {
                    sVollRowJa = "J";
                }
                else
                {
                    sVollRowJa = "";
                    if (tmprbNein.Checked)
                    {
                        sVollRowJa = "N";
                    }
                    else
                    {
                        sVollRowJa = "";
                    }
                }
                tmpPosition["PositionVollstaendig"] = sVollAll;
                if (sVollAll == "X")
                {
                    sVollRowJa = "J";
                }

                objWareneingang.Bestellpositionen.AcceptChanges();

            }
        }
        /// <summary>
        /// Senden der Daten an SAP(Z_FIL_EFA_UML_STEP2).
        /// </summary>
        private void doSubmit() 
        {

            objWareneingang.sendUmlToSAP(Session["AppID"].ToString(), Session.SessionID, this, txtBelegdatum.Text);
            MPEWareneingangsbuchungResultat.Show();
            switch (objWareneingang.Status)
            { 
                case 0:
                    lblWareneingangsbuchungMeldung.ForeColor = System.Drawing.Color.Green;
                    lblWareneingangsbuchungMeldung.Text = "Ihre Wareneingangsbuchung war erfolgreich. ";
                    break;
                case -1:
                    lblWareneingangsbuchungMeldung.ForeColor = System.Drawing.Color.Red;
                    lblWareneingangsbuchungMeldung.Text = "Ihre Wareneingangsbuchung konnte nicht durchgeführt werden: <br><br> " + objWareneingang.Message;
                    break;
                case 134:
                    lblWareneingangsbuchungMeldung.ForeColor = System.Drawing.Color.Red;
                    lblWareneingangsbuchungMeldung.Text =  objWareneingang.Message;
                    break;
                default :
                    lblWareneingangsbuchungMeldung.ForeColor = System.Drawing.Color.Red;
                    lblWareneingangsbuchungMeldung.Text = "Bei der Wareneingangsprüfung ist ein Fehler aufgetreten:: <br><br> " + objWareneingang.Message;

                    break;
            }
            

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
            foreach (GridViewRow tmpRow in GridView1.Rows)
            {
                Label lblEAN = (Label)tmpRow.FindControl("lblEAN");
                DataRow tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];

                if (tmpPosition["PositionVollstaendig"].ToString() == "0" && tmpPosition["PositionLieferMenge"] == DBNull.Value)
                {
                    tmpValid = false;
                }
                else
                {
                    tmpPosition["PositionAbgeschlossen"] = "J";
                    tmpValid = true;
                }
            }

            if (ZLDCommon.IsDate(txtBelegdatum.Text))
            {
 
            }
            else
            {
                tmpValid = false;
                txtBelegdatum.BorderColor = System.Drawing.Color.Red;
            }
            Fillgrid(0,"");



            if (!tmpValid)
            {
                foreach (GridViewRow tmpRow in GridView1.Rows)
                {
                    Label lblEAN = (Label)tmpRow.FindControl("lblEAN");
                    DataRow tmpPosition = objWareneingang.Bestellpositionen.Select("Bestellposition='" + lblEAN.Text + "'")[0];

                    if (tmpPosition["PositionAbgeschlossen"].ToString() == "N" && tmpPosition["PositionLieferMenge"] == DBNull.Value ||
                        tmpPosition["PositionAbgeschlossen"].ToString() == "" && tmpPosition["PositionVollstaendig"].ToString() == "0")
                    {
                        tmpRow.BackColor = System.Drawing.Color.Red;
                    }
                    else if (tmpPosition["PositionVollstaendig"].ToString() == "0" && tmpPosition["PositionAbgeschlossen"].ToString() == "J"
                        && tmpPosition["PositionLieferMenge"] == DBNull.Value)
                    {
                        tmpRow.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        tmpRow.BorderColor = System.Drawing.Color.Empty;
                    }

                    if (tmpPosition["PositionAbgeschlossen"].ToString() == "0" && tmpPosition["PositionLieferMenge"] == DBNull.Value)
                    {
                        tmpRow.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        tmpRow.BorderColor = System.Drawing.Color.Empty;
                    }
                }
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wareneingang.aspx?AppID=" + Session["AppID"].ToString());
        }
        /// <summary>
        /// Wareneingang abgeschlossen? Zurück zur Auswahlseite.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWareneingangsbuchungFinalize_Click(object sender, EventArgs e)
        {
            Response.Redirect("Wareneingang.aspx?AppID=" + Session["AppID"].ToString());
        }
        /// <summary>
        /// Es soll ein Korrektur vorgenommen werden. Schliessen des Absendendialogs. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbWareneingangKorrektur_Click1(object sender, EventArgs e)
        {

        }


    }
}
