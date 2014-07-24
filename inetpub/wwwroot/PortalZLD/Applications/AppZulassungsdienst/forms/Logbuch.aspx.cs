using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Logbuch;
using System.Web.UI.HtmlControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Filiallogbuch
    /// </summary>
    public partial class Logbuch : System.Web.UI.Page
    {

        #region "Enumeratoren"

        private enum ViewStatus
        {
            Unauthenticated,
            Gebietsleiter,
            FilialeProtokoll,
            FilialeAufgaben
        }

        #endregion

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private LogbuchClass mObjFilialbuch;
        private LongStringToSap mObjLongStringToSap;
        private bool bError;

        private ViewStatus curView;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";
            lblMessage.Text = "";

            if ((this.Session["mObjFilialbuch"] != null))
            {
                mObjFilialbuch = (LogbuchClass)Session["mObjFilialbuch"];
            }
            if ((this.Session["curView"] != null))
            {
                curView = (ViewStatus)Session["curView"];
            }
            if ((this.Session["mObjLongStringToSap"] != null))
            {
                mObjLongStringToSap = (LongStringToSap)Session["mObjLongStringToSap"];
            }

            if (!IsPostBack)
            {
                mObjFilialbuch = new LogbuchClass(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "");
                Session["mObjFilialbuch"] = mObjFilialbuch;

                curView = ViewStatus.Unauthenticated;
                // Filialbuch-Berechtigungen für ZLD-User aus SAP ziehen
                AuthenticateFilialbuchUser();
                Session["curView"] = curView;

                mObjLongStringToSap = new LongStringToSap(m_User, m_App, this);
                Session["mObjLongStringToSap"] = mObjLongStringToSap;

                Title = lblHead.Text;
                lblKostenstelle.Text = mObjFilialbuch.VkBur;
                //lblFilialname.Text = ""
                //lblKW.Text = New PM_Common_Functions.KW_Berechnung().KW(Date.Today).ToString()
                //lblLFB.Text = mObjFilialbuch.LFB
            }

            ViewControl(curView);

            Session["LastPage"] = this;
        }

        private void AuthenticateFilialbuchUser()
        {
            try
            {
                FilialbuchUser FilBuUser = mObjFilialbuch.LoginUser(Session["AppID"].ToString(), Session.SessionID, this, mObjFilialbuch.VkBur, m_User.UserName);
                if (mObjFilialbuch.Status != 0)
                {
                    throw new Exception(mObjFilialbuch.Status + ": " + mObjFilialbuch.Message);
                }
                else
                {
                    lblUser.Text = FilBuUser.Bedienername;
                    switch (FilBuUser.Rolle)
                    {
                        case LogbuchClass.Rolle.Zulassungsdienst:
                        case LogbuchClass.Rolle.Filiale:
                            curView = ViewStatus.FilialeAufgaben;
                            FillListAufgaben();
                            break;
                        case LogbuchClass.Rolle.Gebietsleiter:
                            curView = ViewStatus.Gebietsleiter;
                            FillListProtokoll();
                            break;
                        default:
                            curView = ViewStatus.Unauthenticated;
                            lblError.Text = "Der Benutzer ist keiner bekannten Rolle zugeordnet!";
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void FillListAufgaben()
        {
            mObjFilialbuch.GetEinträge(Session["AppID"].ToString(), Session.SessionID, this, mObjFilialbuch.UserLoggedIn, LogbuchClass.StatusFilter.Neu);
            if (mObjFilialbuch.Status != 0)
            {
                lblError.Text = mObjFilialbuch.Status + ": " + mObjFilialbuch.Message;
            }
            else
            {
                gvAufgaben.Visible = true;
                gvProtokollFiliale.Visible = false;
                gvProtokollGL.Visible = false;

                gvAufgaben.DataSource = mObjFilialbuch.Protokoll.CreateTable(
                    EntryStatus.Ausblenden, EntryStatus.Ausblenden,
                    EmpfängerStatus.Neu, EmpfängerStatus.Ausblenden);
                gvAufgaben.DataBind();
            }
        }

        private void FillListProtokoll()
        {
            if (txtDatumVon.Text.Trim() == string.Empty)
            {
                txtDatumVon.Text = DateTime.Today.AddMonths(-3).ToShortDateString();
            }
            if (txtDatumBis.Text.Trim() == string.Empty)
            {
                txtDatumBis.Text = DateTime.Today.ToShortDateString();
            }

            switch (curView)
            {
                case ViewStatus.Gebietsleiter:
                    mObjFilialbuch.GetEinträge(Session["AppID"].ToString(), Session.SessionID, this, mObjFilialbuch.UserLoggedIn, 
                        LogbuchClass.StatusFilter.Alle, mObjFilialbuch.VkBur, Convert.ToDateTime(txtDatumVon.Text), Convert.ToDateTime(txtDatumBis.Text));
                    break;
                default:
                    mObjFilialbuch.GetEinträge(Session["AppID"].ToString(), Session.SessionID, this, mObjFilialbuch.UserLoggedIn, 
                        LogbuchClass.StatusFilter.Alle, null, Convert.ToDateTime(txtDatumVon.Text), Convert.ToDateTime(txtDatumBis.Text));
                    break;
            }


            if (mObjFilialbuch.Status != 0)
            {
                lblError.Text = mObjFilialbuch.Status + ": " + mObjFilialbuch.Message;
            }
            else
            {
                switch (curView)
                {
                    case ViewStatus.FilialeProtokoll:
                        gvAufgaben.Visible = false;
                        gvProtokollFiliale.Visible = true;
                        gvProtokollGL.Visible = false;
                        if (ddlFilterFiliale.SelectedItem.Text == "Gelesen" ||
                            ddlFilterFiliale.SelectedItem.Text == "Beantwortet" ||
                            ddlFilterFiliale.SelectedItem.Text == "Erledigt" ||
                            ddlFilterFiliale.SelectedItem.Text == "Neu")
                        {
                            string sFilter = ddlFilterFiliale.SelectedValue.Trim('E');
                            DataTable dt = mObjFilialbuch.Protokoll.CreateTable(EntryStatus.Ausblenden,
                                                                                EntryStatus.Ausblenden,
                                                                                (EmpfängerStatus)Enum.Parse(typeof(EmpfängerStatus), sFilter),
                                                                                (EmpfängerStatus)Enum.Parse(typeof(EmpfängerStatus), sFilter));

                            gvProtokollFiliale.DataSource = FilterClosed(ref dt);
                        }
                        else if (ddlFilterFiliale.SelectedValue != "all")
                        {
                            DataTable dt =
                                mObjFilialbuch.Protokoll.CreateTable(
                                    (EntryStatus)Enum.Parse(typeof(EntryStatus), ddlFilter.SelectedValue),
                                    (EntryStatus)Enum.Parse(typeof(EntryStatus), ddlFilter.SelectedValue));
                            gvProtokollFiliale.DataSource = dt;
                        }
                        else
                        {
                            gvProtokollFiliale.DataSource = mObjFilialbuch.Protokoll.CreateTable();
                        }
                        gvProtokollFiliale.DataBind();
                        break;
                    case ViewStatus.Gebietsleiter:
                        gvAufgaben.Visible = false;
                        gvProtokollFiliale.Visible = false;
                        gvProtokollGL.Visible = true;
                        if (ddlFilter.SelectedItem.Text == "Gelesen" || ddlFilter.SelectedItem.Text == "Beantwortet" ||
                            ddlFilter.SelectedItem.Text == "Erledigt" || ddlFilter.SelectedItem.Text == "Neu")
                        {
                            string sFilter = ddlFilter.SelectedValue.Trim('E');
                            DataTable dt = mObjFilialbuch.Protokoll.CreateTable(EntryStatus.Ausblenden,
                                                                                EntryStatus.Ausblenden,
                                                                                (EmpfängerStatus)Enum.Parse(typeof(EmpfängerStatus), sFilter),
                                                                                (EmpfängerStatus)Enum.Parse(typeof(EmpfängerStatus), sFilter));

                            gvProtokollGL.DataSource = FilterClosed(ref dt);
                        }
                        else if (ddlFilter.SelectedValue != "all")
                        {
                            DataTable dt = mObjFilialbuch.Protokoll.CreateTable(EntryStatus.Ausblenden,
                                                                                (EntryStatus)Enum.Parse(typeof(EntryStatus), ddlFilter.SelectedValue),
                                                                                EmpfängerStatus.Ausblenden,
                                                                                EmpfängerStatus.Ausblenden);
                            gvProtokollGL.DataSource = dt;
                        }
                        else
                        {
                            gvProtokollGL.DataSource = mObjFilialbuch.Protokoll.CreateTable();
                        }
                        gvProtokollGL.DataBind();
                        break;
                }
            }
        }

        /// <summary>
        /// Löscht alle geschlossenen Datensätze aus der mitgegebenen Tabelle
        /// </summary>
        /// <param name="dt">Protokolltabelle</param>
        /// <returns>Protokolltabelle ohne geschlossene Sätze</returns>
        /// <remarks></remarks>
        private DataTable FilterClosed(ref DataTable dt)
        {
            //geschlossene Sätze filtern
            foreach (DataRow row in dt.Rows)
            {
                bool bDelete = false;
                if (row["I_STATUS"] != DBNull.Value)
                {
                    if (row["I_STATUS"].ToString() == "Geschlossen")
                    {
                        bDelete = true;
                    }
                }

                if (bDelete == false & row["O_STATUS"] != DBNull.Value)
                {
                    if (row["O_STATUS"].ToString() == "Geschlossen")
                    {
                        row.Delete();
                    }
                }

                if (bDelete)
                {
                    row.Delete();
                }
            }
            dt.AcceptChanges();

            return dt;
        }

        /// <summary>
        /// Ereignis das beim Entladen der Seite aufgerufen wird
        /// </summary>
        /// <param name="sender">Absender des Ereignisses</param>
        /// <param name="e">EventArgumente</param>
        /// <remarks></remarks>
        private void Filialbuch_Unload(object sender, System.EventArgs e)
        {
            // aktuellen Objekt-Status sichern
            Session["mObjFilialbuch"] = mObjFilialbuch;
            Session["curView"] = curView;
        }

        #region "ButtonEvents"

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            if (curView == ViewStatus.Unauthenticated)
            {
                Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                curView = ViewStatus.Unauthenticated;
                ViewControl(curView);
            }
        }

        protected void lbtAdd_Click(object sender, EventArgs e)
        {
            ShowPopUpNewEntry();
        }

        //protected void lbtFilialbesuch_Click(object sender, EventArgs e)
        //{
        //    mObjFilialbuch.NeuerEintrag(Session["AppID"].ToString(), Session.SessionID, this, "Filialbesuch durch Leiter Filialbetrieb",
        //                                "Filialbesuch durch Leiter Filialbetrieb am " +
        //                                DateTime.Today.ToShortDateString(), mObjFilialbuch.VkBur, "FILB", "");

        //    if (mObjFilialbuch.Status != 0)
        //    {
        //        lblError.Text = mObjFilialbuch.Status + ": " + mObjFilialbuch.Message;
        //    }

        //    DataTable dt = mObjFilialbuch.Protokoll.CreateTable(EntryStatus.Neu);

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if (dt.Rows[i]["O_ERDAT"].ToString() == DateTime.Today.ToShortDateString())
        //        {
        //            if (dt.Rows[i]["O_BETREFF"].ToString() == "Filialbesuch durch Leiter Filialbetrieb")
        //            {
        //                mObjFilialbuch.Protokoll.EintragAbschliessen(Session["AppID"].ToString(), Session.SessionID, this, i, 
        //                    EntryStatus.Geschlossen);
        //            }

        //              if (mObjFilialbuch.Protokoll.Status != 0)
        //              {
        //                  lblError.Text = mObjFilialbuch.Protokoll.Status + ": " + mObjFilialbuch.Protokoll.Message;
        //              }
        //        }
        //    }

        //    FillListProtokoll();
        //}

        protected void lbProtokoll_Click(object sender, EventArgs e)
        {
            if (curView == ViewStatus.FilialeAufgaben | curView == ViewStatus.FilialeProtokoll)
            {
                curView = ViewStatus.FilialeProtokoll;
            }
            else
            {
                curView = ViewStatus.Gebietsleiter;
            }
            ViewControl(curView);
            FillListProtokoll();
        }

        protected void lbAufgaben_Click(object sender, System.EventArgs e)
        {
            curView = ViewStatus.FilialeAufgaben;
            ViewControl(curView);
            FillListAufgaben();
        }

        protected void lbtnRefresh_Click(object sender, EventArgs e)
        {
            // # CurView bereits auf GL oder Filiale gesetzt, daher keine Änderung nötig
            FillListProtokoll();
        }

        #endregion

        /// <summary>
        /// Ansichtssteuerung für alle Elemente der Seite
        /// </summary>
        /// <param name="VS">Das anzuzeigende Seiten-Layout</param>
        /// <remarks></remarks>
        private void ViewControl(ViewStatus VS)
        {
            switch (VS)
            {
                case ViewStatus.FilialeProtokoll:
                    tblHeaderTabs.Visible = true;

                    lbAufgaben.Visible = true;
                    lbAufgaben.CssClass = "TabButtonBig Active";
                    lbProtokoll.CssClass = "TabButtonBig";

                    gvAufgaben.Visible = false;
                    gvProtokollFiliale.Visible = true;
                    gvProtokollGL.Visible = false;

                    divTimeSpan.Visible = true;
                    ddlFilter.Visible = false;
                    ddlFilterFiliale.Visible = true;

                    EditAufgabe2.Visible = false;
                    break;

                case ViewStatus.FilialeAufgaben:
                    tblHeaderTabs.Visible = true;

                    lbAufgaben.Visible = true;
                    lbAufgaben.CssClass = "TabButtonBig";
                    lbProtokoll.CssClass = "TabButtonBig Active";

                    gvAufgaben.Visible = true;
                    gvProtokollFiliale.Visible = false;
                    gvProtokollGL.Visible = false;

                    divTimeSpan.Visible = false;

                    EditAufgabe2.Visible = true;
                    lbtAdd.Text = "Anfrage";
                    break;

                case ViewStatus.Gebietsleiter:
                    tblHeaderTabs.Visible = true;

                    lbAufgaben.Visible = false;
                    lbAufgaben.CssClass = "TabButtonBig Active";
                    lbProtokoll.CssClass = "TabButtonBig";

                    gvAufgaben.Visible = false;
                    gvProtokollFiliale.Visible = false;
                    gvProtokollGL.Visible = true;

                    divTimeSpan.Visible = true;
                    ddlFilter.Visible = true;
                    ddlFilterFiliale.Visible = false;

                    EditAufgabe2.Visible = true;
                    lbtAdd.Text = "hinzufügen";
                    break;

                default:
                    tblHeaderTabs.Visible = true;

                    lbAufgaben.Visible = true;
                    lbAufgaben.CssClass = "TabButtonBig";
                    lbProtokoll.CssClass = "TabButtonBig";

                    gvAufgaben.Visible = false;
                    gvProtokollFiliale.Visible = false;
                    gvProtokollGL.Visible = false;

                    divTimeSpan.Visible = false;
                    ddlFilter.Visible = false;
                    ddlFilterFiliale.Visible = false;

                    EditAufgabe2.Visible = false;
                    break;
            }

        }

        #region "GridViewEvents"

        protected void gvAufgaben_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            SortDirection NewDir;

            DataRow[] tmpRows = mObjFilialbuch.Protokoll.ProtokollTabelle.Select("Rowindex='" + e.CommandArgument + "'");
            switch (e.CommandName)
            {
                case "ReadAufgabeText":
                    string Text = mObjLongStringToSap.ReadString(Convert.ToString(tmpRows[0]["I_LTXNR"]));
                    ShowPopUp(false, false, Convert.ToString(tmpRows[0]["I_BETREFF"]), Text, e.CommandArgument.ToString());
                    break;
                case "AnswerAufgabe":
                    ShowPopUp(true, false, "AW:" + Convert.ToString(tmpRows[0]["I_BETREFF"]), "", e.CommandArgument.ToString());
                    break;
                case "ErlAufgabe":
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EmpfängerStatus.Erledigt);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListAufgaben();
                    break;
                case "ReadAufgabe":
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EmpfängerStatus.Gelesen);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListAufgaben();
                    break;
                case "DatumEingangSort":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvAufgaben.Sort("I_DATUM", NewDir);
                    break;
                case "SortVon":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvAufgaben.Sort("I_VON", NewDir);
                    break;
                case "RückfrageAufgabe":
                    ShowPopUp(true, true, "RF:" + Convert.ToString(tmpRows[0]["I_BETREFF"]), "", e.CommandArgument.ToString());
                    break;
            }
        }

        protected void gvAufgaben_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            DataView View = mObjFilialbuch.Protokoll.ProtokollTabelle.DefaultView;
            string[] sortparts = e.SortExpression.Split(',');
            string sortString = "";

            for (int i = 0; i < sortparts.GetLength(0); i++)
            {
                sortString += sortparts[i];

                if (e.SortDirection == SortDirection.Ascending)
                {
                    sortString += " ASC";
                }
                else
                {
                    sortString += " DESC";
                }
                if (i < sortparts.GetLength(0) - 1)
                {
                    sortString += ",";
                }
            }
            View.Sort = sortString;
            gvAufgaben.DataSource = View;
            gvAufgaben.DataBind();
        }

        protected void gvProtokollFiliale_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            SortDirection NewDir;
            string Text;

            DataRow[] tmpRows = mObjFilialbuch.Protokoll.ProtokollTabelle.Select("Rowindex='" + e.CommandArgument + "'");
            switch (e.CommandName)
            {
                case "ReadAufgabeText":
                    Text = mObjLongStringToSap.ReadString(Convert.ToString(tmpRows[0]["I_LTXNR"]));
                    ShowPopUp(false, false, Convert.ToString(tmpRows[0]["I_BETREFF"]), Text, e.CommandArgument.ToString());
                    break;
                case "ReadAnswerText":
                    Text = mObjLongStringToSap.ReadString(Convert.ToString(tmpRows[0]["O_LTXNR"]));
                    ShowPopUp(false, false, Convert.ToString(tmpRows[0]["O_BETREFF"]), Text, e.CommandArgument.ToString());
                    break;
                case "AnswerAufgabe":
                    ShowPopUp(true, false, "AW:" + Convert.ToString(tmpRows[0]["I_BETREFF"]), "", e.CommandArgument.ToString());
                    break;
                case "ErlAufgabe":
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EmpfängerStatus.Erledigt);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "LoeAufgabe":
                    mObjFilialbuch.Protokoll.EintragAbschliessen(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EntryStatus.Gelöscht);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "CloseAufgabe":
                    mObjFilialbuch.Protokoll.EintragAbschliessen(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EntryStatus.Geschlossen);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "ReadAufgabe":
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EmpfängerStatus.Gelesen);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "DatumEingangSort":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollFiliale.Sort("I_DATUM", NewDir);
                    break;
                case "DatumAusgangSort":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollFiliale.Sort("O_DATUM", NewDir);
                    break;
                case "SortVon":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollFiliale.Sort("I_VON", NewDir);
                    break;
                case "SortAn":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollFiliale.Sort("O_AN", NewDir);
                    break;
            }
        }

        protected void gvProtokollFiliale_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            DataView View = mObjFilialbuch.Protokoll.ProtokollTabelle.DefaultView;
            string[] sortparts = e.SortExpression.Split(',');
            string sortString = "";

            for (int i = 0; i < sortparts.GetLength(0); i++)
            {
                sortString += sortparts[i];

                if (e.SortDirection == SortDirection.Ascending)
                {
                    sortString += " ASC";
                }
                else
                {
                    sortString += " DESC";
                }
                if (i < sortparts.GetLength(0) - 1)
                {
                    sortString += ",";
                }
            }
            View.Sort = sortString;
            gvProtokollFiliale.DataSource = View;
            gvProtokollFiliale.DataBind();
        }

        protected void gvProtokollGL_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            SortDirection NewDir;
            string Text;

            DataRow[] tmpRows = mObjFilialbuch.Protokoll.ProtokollTabelle.Select("Rowindex='" + e.CommandArgument + "'");
            switch (e.CommandName)
            {
                case "ReadAufgabeText":
                    Text = mObjLongStringToSap.ReadString(Convert.ToString(tmpRows[0]["I_LTXNR"]));
                    ShowPopUp(false, false, Convert.ToString(tmpRows[0]["I_BETREFF"]), Text, e.CommandArgument.ToString());
                    break;
                case "ReadAnswerText":
                    Text = mObjLongStringToSap.ReadString(Convert.ToString(tmpRows[0]["O_LTXNR"]));
                    ShowPopUp(false, false, Convert.ToString(tmpRows[0]["O_BETREFF"]), Text, e.CommandArgument.ToString());
                    break;
                case "AnswerAufgabe":
                    ShowPopUp(true, false, "AW:" + Convert.ToString(tmpRows[0]["I_BETREFF"]), "", e.CommandArgument.ToString());
                    break;
                case "ErlAufgabe":
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EmpfängerStatus.Erledigt);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "LoeAufgabe":
                    mObjFilialbuch.Protokoll.EintragAbschliessen(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EntryStatus.Gelöscht);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "CloseAufgabe":
                    mObjFilialbuch.Protokoll.EintragAbschliessen(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EntryStatus.Geschlossen);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "ReadAufgabe":
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Convert.ToInt32(e.CommandArgument), 
                        EmpfängerStatus.Gelesen);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                    break;
                case "DatumEingangSort":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollGL.Sort("I_DATUM", NewDir);
                    break;
                case "DatumAusgangSort":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollGL.Sort("O_DATUM", NewDir);
                    break;
                case "SortVon":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollGL.Sort("I_VON", NewDir);
                    break;
                case "SortAn":
                    NewDir = default(SortDirection);
                    if ((ViewState["SortDirection"] != null) && ((SortDirection)ViewState["SortDirection"] == SortDirection.Ascending))
                    {
                        NewDir = SortDirection.Descending;
                    }
                    else
                    {
                        NewDir = SortDirection.Ascending;
                    }
                    ViewState["SortDirection"] = NewDir;
                    gvProtokollGL.Sort("O_AN", NewDir);
                    break;
            }
        }

        protected void gvProtokollGL_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            DataView View = mObjFilialbuch.Protokoll.ProtokollTabelle.DefaultView;
            string[] sortparts = e.SortExpression.Split(',');
            string sortString = "";

            for (int i = 0; i < sortparts.GetLength(0); i++)
            {
                sortString += sortparts[i];

                if (e.SortDirection == SortDirection.Ascending)
                {
                    sortString += " ASC";
                }
                else
                {
                    sortString += " DESC";
                }
                if (i < sortparts.GetLength(0) - 1)
                {
                    sortString += ",";
                }
            }
            View.Sort = sortString;
            gvProtokollGL.DataSource = View;
            gvProtokollGL.DataBind();
        }

        #endregion

        #region "PopUpActions"

        private void ClosePopup()
        {
            txtBetreff.Text = "";
            txtText.Text = "";
            lblBetreff.Text = "";
            lblText.Text = "";
            chkEdit.Checked = false;
            chkIsRückfrage.Checked = false;

            mpeLangtext.Hide();
        }

        private void ClosePopupNewEntry()
        {
            txtNewBetreff.Text = "";
            txtNewText.Text = "";
            txtZuerledigenBis.Text = "";

            mpeNeuerText.Hide();
        }

        private void ShowPopUp(bool editmode, bool isrückfrage, string strBetreff, string strText, string rowindex,
                               string vorgid = "", string lfdnr = "", string an = "")
        {
            txtBetreff.Visible = editmode;
            txtText.Visible = editmode;

            lblBetreff.Text = (!editmode).ToString();
            lblText.Text = (!editmode).ToString();

            lblErrorLangtext.Text = "";
            lblVorgangsID.Text = vorgid;
            lblLFDNR.Text = lfdnr;
            lblAn.Text = an;
            lblRowIndex.Text = rowindex;

            if (editmode)
            {
                txtBetreff.Text = strBetreff;
                txtText.Text = strText;
                divText.Attributes["Style"] =
                    "margin: 0px 3px 10px 3px; font-size: 10px; overflow: visible; height: 230px; border: none;";
                lblBetreff.Text = "";
                lblText.Text = "";
                chkEdit.Checked = true;
                chkIsRückfrage.Checked = isrückfrage;
            }
            else
            {
                txtBetreff.Text = "";
                txtText.Text = "";
                divText.Attributes["Style"] =
                    "margin: 0px 3px 10px 3px; font-size: 10px; overflow: visible; height: 230px; border: solid 1px #dddddd;";
                lblBetreff.Text = strBetreff;
                lblText.Text = strText;
                chkEdit.Checked = false;
                chkIsRückfrage.Checked = false;
            }

            mpeLangtext.Show();
        }

        private void ShowPopUpNewEntry()
        {
            List<VorgangsartDetails> lstVorgangsarten = new List<VorgangsartDetails>(mObjFilialbuch.Vorgangsarten);

            for (int i = (lstVorgangsarten.Count - 1); i >= 0; i--)
            {
                VorgangsartDetails item = lstVorgangsarten[i];
                bool gefunden = false;

                // Nur die für die Rolle zulässigen Vorgangsarten zur Auswahl stellen
                foreach (VorgangsartRolleDetails itemRolle in mObjFilialbuch.VorgangsartenRolle)
                {
                    if (item.Vorgangsart == itemRolle.Vorgangsart)
                    {
                        gefunden = true;
                        break;
                    }
                }

                if ((!gefunden) || (item.Vorgangsart == "ANTW"))
                {
                    lstVorgangsarten.Remove(item);
                }
            }

            ddlVorgangsarten.DataSource = lstVorgangsarten;
            ddlVorgangsarten.DataValueField = "Vorgangsart";
            ddlVorgangsarten.DataTextField = "Bezeichnung";
            ddlVorgangsarten.DataBind();

            mpeNeuerText.Show();
        }

        protected void ibtnOkNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewBetreff.Text.TrimStart(',')))
            {
                lblErrorNewText.Text = "Geben Sie einen Betreff ein!";
                mpeNeuerText.Show();
                return;
            }

            string empfaenger = mObjFilialbuch.VkBur;

            // Wenn Absender Filialmitarbeiter -> Empfänger = Vorgesetzter
            if (ddlVorgangsarten.SelectedValue == "ZLDZ")
            {
                empfaenger = mObjFilialbuch.UserLoggedIn.NamePa;
            }

            mObjFilialbuch.NeuerEintrag(Session["AppID"].ToString(), Session.SessionID, this, txtNewBetreff.Text, txtNewText.Text, empfaenger,
                                        ddlVorgangsarten.SelectedValue, txtZuerledigenBis.Text);

            if (mObjFilialbuch.Status != 0)
            {
                lblError.Text = mObjFilialbuch.Status + ": " + mObjFilialbuch.Message;
            }

            FillListProtokoll();
            ClosePopupNewEntry();
        }

        protected void ibtnOK_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            if (chkEdit.Checked)
            {
                if (string.IsNullOrEmpty(txtBetreff.Text.TrimStart(',')))
                {
                    lblErrorLangtext.Text = "Geben Sie einen Betreff ein!";
                    mpeLangtext.Show();
                    return;
                }
                if (string.IsNullOrEmpty(txtText.Text.Trim()))
                {
                    lblErrorLangtext.Text = "Geben Sie einen Antworttext ein!";
                    mpeLangtext.Show();
                    return;
                }

                if (curView == ViewStatus.FilialeAufgaben)
                {
                    if (chkIsRückfrage.Checked)
                    {
                        mObjFilialbuch.Protokoll.Rückfrage(Session["AppID"].ToString(), Session.SessionID, this, Int32.Parse(lblRowIndex.Text), 
                            txtBetreff.Text, txtText.Text);
                    }
                    else
                    {
                        mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Int32.Parse(lblRowIndex.Text), 
                            txtBetreff.Text, txtText.Text);
                    }

                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListAufgaben();
                }
                else if (curView == ViewStatus.FilialeProtokoll)
                {
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Int32.Parse(lblRowIndex.Text), 
                        txtBetreff.Text, txtText.Text);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                }
                else if (curView == ViewStatus.Gebietsleiter)
                {
                    mObjFilialbuch.Protokoll.EintragBeantworten(Session["AppID"].ToString(), Session.SessionID, this, Int32.Parse(lblRowIndex.Text), 
                        txtBetreff.Text, txtText.Text);
                    if (mObjFilialbuch.Protokoll.Status != 0)
                    {
                        lblError.Text = "Es ist ein Fehler aufgetreten: " + mObjFilialbuch.Protokoll.Message;
                    }
                    FillListProtokoll();
                }
            }

            ClosePopup();
        }

        #endregion

        #region "HelpProcedures"

        protected void img_prerender(object sender, EventArgs e)
        {
            HtmlImage img = sender as HtmlImage;
            switch (img.Attributes["value"])
            {
                case "Neu":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/new.png";
                    img.Alt = "Neu";
                    img.Attributes.Add("Title", "Neu");
                    break;
                case "Geantwortet":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/email.png";
                    img.Alt = "Geantwortet";
                    img.Attributes.Add("Title", "Geantwortet");
                    break;
                case "AutomatischBeantwortet":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/email.png";
                    img.Alt = "Automatisch beantwortet";
                    img.Attributes.Add("Title", "Automatisch beantwortet");
                    break;
                case "Geschlossen":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/Lock.png";
                    img.Alt = "Geschlossen";
                    img.Attributes.Add("Title", "Geschlossen");
                    break;
                case "Gelöscht":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/bin_closed.png";
                    img.Alt = "Gelöscht";
                    img.Attributes.Add("Title", "Gelöscht");
                    break;
                case "Gesendet":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/Email_go.png";
                    img.Alt = "Gesendet";
                    img.Attributes.Add("Title", "Gesendet");
                    break;
                default:
                    img.Visible = false;
                    break;
            }
        }

        protected void img2_prerender(object sender, EventArgs e)
        {
            HtmlImage img = sender as HtmlImage;
            switch (img.Attributes["value"])
            {
                case "Neu":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/new.png";
                    img.Alt = "Neu";
                    img.Attributes.Add("Title", "Neu");
                    break;
                case ("Gelesen"):
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/eye.png";
                    img.Alt = "Gelesen";
                    img.Attributes.Add("Title", "Gelesen");
                    break;
                case "AutomatischBeantwortet":
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/email.png";
                    img.Alt = "Automatisch beantwortet";
                    img.Attributes.Add("Title", "Automatisch beantwortet");
                    break;
                case ("Geantwortet"):
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/email.png";
                    img.Alt = "Geantwortet";
                    img.Attributes.Add("Title", "Geantwortet");
                    break;
                case ("Gelöscht"):
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/bin_closed.png";
                    img.Alt = "Gelöscht";
                    img.Attributes.Add("Title", "Gelöscht");
                    break;
                case ("Erledigt"):
                    img.Visible = true;
                    img.Src = "/PortalZLD/images/haken_gruen.gif";
                    img.Alt = "Erledigt";
                    img.Attributes.Add("Title", "Erledigt");
                    break;
                default:
                    img.Visible = false;
                    break;
            }
        }

        protected void DivRender(object sender, EventArgs e)
        {
            HtmlControl div = sender as HtmlControl;
            switch (div.Attributes["value"])
            {
                case "True":
                    div.Attributes["Style"] =
                        "height:22px; margin-bottom:3px; white-space:nowrap; border-top:solid 1px #595959;";
                    break;
                default:
                    div.Attributes["Style"] = "height:22px; margin-bottom:3px; white-space:nowrap;";
                    break;
            }
        }

        public Logbuch()
        {
            Unload += Filialbuch_Unload;
            Load += Page_Load;
        }

        #endregion

    }
}
