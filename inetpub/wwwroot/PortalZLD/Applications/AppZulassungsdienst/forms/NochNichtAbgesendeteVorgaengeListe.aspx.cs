using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Linq;
using CKG.Base.Business;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    public partial class NochNichtAbgesendeteVorgaengeListe : System.Web.UI.Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
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

            if (Session["objNacherf"] == null)
            {
                //Session-Variable weg (Session vermutlich abgelaufen) -> zurück zur 1. Seite
                Response.Redirect("ChangeZLDSelect.aspx?AppID=" + Session["AppID"].ToString());
                return;
            }

            objNacherf = (NacherfZLD)Session["objNacherf"];

            if (!IsPostBack)
            {
                Fillgrid(0, "");
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
            try
            {
                if (e.CommandName == "OK")
                {
                    Int32 Index;
                    Int32.TryParse(e.CommandArgument.ToString(), out Index);

                    Label lblID = (Label)gvZuldienst.Rows[Index].FindControl("lblsapID");
                    Label lblIsSelected = (Label)gvZuldienst.Rows[Index].FindControl("lblIsSelected");

                    var newValue = !lblIsSelected.Text.XToBool();

                    objNacherf.SelectNochNichtAbgesendetenVorgang(lblID.Text, newValue);

                    lblIsSelected.Text = newValue.BoolToX();

                    if (objNacherf.ErrorOccured)
                    {
                        lblError.Text = objNacherf.Message;
                        return;
                    }

                    Session["objNacherf"] = objNacherf;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            objNacherf.SelectNochNichtAbgesendeteVorgaenge();

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            Session["objNacherf"] = objNacherf;

            Fillgrid(0, "");
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeZLDSelect.aspx?A=true&AppID=" + Session["AppID"].ToString());
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            objNacherf.SendNochNichtAbgesendeteVorgaengeToSap();

            tab1.Visible = true;

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = "Kommunikationfehler: Daten konnten nicht in SAP gespeichert werden! " + objNacherf.Message;
                return;
            }

            if (objNacherf.NochNichtAbgesendeteVorgaenge.Any(vg => !String.IsNullOrEmpty(vg.FehlerText) && vg.FehlerText != "OK"))
            {
                lblError.Text = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";

                Fillgrid(0, "");

                // Status
                gvZuldienst.Columns[0].Visible = true;
                // Übernehmen
                gvZuldienst.Columns[10].Visible = false;
            }
            else
            {
                tab1.Height = "250px";
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";

                Result.Visible = false;

                cmdOK.Enabled = false;
                cmdSend.Enabled = false;
                cmdContinue.Visible = true;
            }
        }

        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            objNacherf.DeleteNochNichtAbgesendeteVorgaengeOkFromList();

            if (objNacherf.NochNichtAbgesendeteVorgaenge.Count == 0)
            {
                Fillgrid(0, "");
                Result.Visible = false;
                cmdSend.Enabled = false;
                cmdOK.Enabled = false;
                lblError.Text = "Keine Daten zur bestehenden Selektion vorhanden!";
            }
            else
            {
                Result.Visible = true;
                cmdSend.Enabled = true;
                cmdOK.Enabled = true;
                Fillgrid(0, "");
            }
            cmdContinue.Visible = false;

            // Status
            gvZuldienst.Columns[0].Visible = false;
            // Übernehmen
            gvZuldienst.Columns[10].Visible = true;

            lblMessage.Visible = false;
        }

        #endregion

        #region Methods

        private void Fillgrid(Int32 intPageIndex, String strSort)
        {
            if (objNacherf.NochNichtAbgesendeteVorgaenge.None())
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
                    if ((Session["NacherfSort"] == null) || ((String)Session["NacherfSort"] == strTempSort))
                    {
                        if (Session["NacherfDirection"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)Session["NacherfDirection"];
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

                    Session["NacherfSort"] = strTempSort;
                    Session["NacherfDirection"] = strDirection;
                }
                else if (Session["NacherfSort"] != null)
                {
                    strTempSort = Session["NacherfSort"].ToString();
                    strDirection = Session["NacherfDirection"].ToString();
                }
                if (!String.IsNullOrEmpty(strTempSort))
                {
                    System.Reflection.PropertyInfo prop = typeof(NochNichtAbgesendeterVorgang).GetProperty(strTempSort);

                    if (strDirection == "asc")
                    {
                        gvZuldienst.DataSource = objNacherf.NochNichtAbgesendeteVorgaenge.OrderBy(v => prop.GetValue(v, null)).ToList();
                    }
                    else
                    {
                        gvZuldienst.DataSource = objNacherf.NochNichtAbgesendeteVorgaenge.OrderByDescending(v => prop.GetValue(v, null)).ToList();
                    }
                }
                else
                {
                    gvZuldienst.DataSource = objNacherf.NochNichtAbgesendeteVorgaenge.OrderBy(v => v.KundenNrAsSapKunnr).ThenBy(v => v.SapId.ToLong(0)).ToList();
                }

                gvZuldienst.PageIndex = intTempPageIndex;
                gvZuldienst.DataBind();

                lblAnzahl.Text = "Anzahl Vorgänge: " + objNacherf.NochNichtAbgesendeteVorgaenge.Count;
            }
        }

        #endregion
    }
}