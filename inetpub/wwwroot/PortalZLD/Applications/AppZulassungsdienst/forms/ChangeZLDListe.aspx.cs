using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Linq;
using CKG.Base.Business;

namespace AppZulassungsdienst.forms
{
    public partial class ChangeZLDListe : System.Web.UI.Page
    {
        private User m_User;
        private VorerfZLD objVorerf;
        private ZLDCommon objCommon;

        #region Events

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

            if (Session["objVorerf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString());
                return;
            }

            objVorerf = (VorerfZLD)Session["objVorerf"];

            if (!IsPostBack)
            {
                objVorerf.LoadVorgaengeFromSql(objCommon.KundenStamm, m_User.UserName);
                Session["objVorerf"] = objVorerf;
                if (objVorerf.ErrorOccured)
                {
                    tab1.Visible = true;
                    tab1.Height = "250px";
                    lblError.Text = objVorerf.Message;
                    cmdSend.Visible = false;
                }
                else
                {
                    Fillgrid(0, "");
                }
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            HelpProcedures.FixedGridViewCols(gvZuldienst);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void gvZuldienst_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(0, e.SortExpression);
        }

        protected void gvZuldienst_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 Index;
            Label lblID;

            switch (e.CommandName)
            {
                case "Bearbeiten":
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    lblID = (Label)gvZuldienst.Rows[Index].FindControl("lblsapID");

                    Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + lblID.Text + "&B=true");
                    break;

                case "Loeschen":
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);
                    lblID = (Label)gvZuldienst.Rows[Index].FindControl("lblsapID");
                    Label lblIDPos = (Label)gvZuldienst.Rows[Index].FindControl("lblid_pos");

                    objVorerf.DeleteVorgangPosition(lblID.Text, lblIDPos.Text);

                    if (objVorerf.ErrorOccured)
                    {
                        lblError.Text = objVorerf.Message;
                    }

                    Session["objVorerf"] = objVorerf;

                    Fillgrid(0, "");
                    break;
            }
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("Change01ZLD.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            objVorerf.SendVorgaengeToSap(objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);

            tab1.Visible = true;

            if (objVorerf.ErrorOccured)
            {
                lblError.Text = "Kommunikationfehler: Daten konnten nicht in SAP gespeichert werden! " + objVorerf.Message;
                return;
            }

            if (objVorerf.Vorgangsliste.Any(vg => !String.IsNullOrEmpty(vg.FehlerText) && vg.FehlerText != "OK"))
            {
                lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";

                Fillgrid(0, "");

                // Status
                gvZuldienst.Columns[0].Visible = true;
                // Bearbeiten
                gvZuldienst.Columns[1].Visible = false;
                // Löschen
                gvZuldienst.Columns[2].Visible = false;
                // Zulassungsdatum
                if (gvZuldienst.Columns[8] != null)
                    gvZuldienst.Columns[8].Visible = false;
                // Referenz 1
                if (gvZuldienst.Columns[9] != null)
                    gvZuldienst.Columns[9].Visible = false;
                // Referenz 2
                if (gvZuldienst.Columns[10] != null)
                    gvZuldienst.Columns[10].Visible = false;
            }
            else
            {
                tab1.Height = "250px";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";

                Result.Visible = false;

                cmdSend.Enabled = false;
            }
        }

        #endregion

        #region Methods

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            if (objVorerf.Vorgangsliste.Count == 0)
            {
                gvZuldienst.Visible = false;
                Result.Visible = false;
            }
            else
            {
                Result.Visible = true;
                gvZuldienst.Visible = true;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (!String.IsNullOrEmpty(strSort))
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((Session["VorerfSort"] == null) || ((String)Session["VorerfSort"] == strTempSort))
                    {
                        if (Session["VorerfDirection"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["VorerfDirection"];
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

                    Session["VorerfSort"] = strTempSort;
                    Session["VorerfDirection"] = strDirection;
                }
                else if (Session["VorerfSort"] != null)
                {
                    strTempSort = Session["VorerfSort"].ToString();
                    strDirection = Session["VorerfDirection"].ToString();
                }
                if (!String.IsNullOrEmpty(strTempSort))
                {
                    System.Reflection.PropertyInfo prop = typeof(ZLDVorgangUIVorerfassung).GetProperty(strTempSort);

                    if (strDirection == "asc")
                    {
                        gvZuldienst.DataSource = objVorerf.Vorgangsliste.OrderBy(v => prop.GetValue(v, null)).ToList();
                    }
                    else
                    {
                        gvZuldienst.DataSource = objVorerf.Vorgangsliste.OrderByDescending(v => prop.GetValue(v, null)).ToList();
                    }
                }
                else
                {
                    gvZuldienst.DataSource = objVorerf.Vorgangsliste.OrderBy(v => v.KundenName).ThenBy(v => v.SapId).ThenBy(v => v.PositionsNr).ToList();
                }

                gvZuldienst.PageIndex = intTempPageIndex;
                gvZuldienst.DataBind();

                // Zeilen mit gleicher ID gleich färben
                if (gvZuldienst.DataKeys.Count > 0 && gvZuldienst.DataKeys[0] != null)
                {
                    String myId = gvZuldienst.DataKeys[0]["SapId"].ToString();
                    String Css = "ItemStyle";
                    foreach (GridViewRow row in gvZuldienst.Rows)
                    {
                        if (gvZuldienst.DataKeys[row.RowIndex] != null)
                        {
                            if (gvZuldienst.DataKeys[row.RowIndex]["SapId"].ToString() == myId)
                            {
                                row.CssClass = Css;
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

                                myId = gvZuldienst.DataKeys[row.RowIndex]["SapId"].ToString();
                            }
                        }
                    }
                }

                lblAnzahl.Text = "Anzahl Vorgänge: " + objVorerf.IDCount;
            }
        }

        #endregion
    }
}