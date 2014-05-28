using System;
using System.Globalization;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Services;


namespace AppZulassungsdienst.forms
{
    public partial class Disposition : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private clsDisposition objDispo;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";
            lblMessage.Text = "";

            if (Session["objDispo"] == null)
            {
                objDispo = new clsDisposition(ref m_User, m_App, Session["AppID"].ToString(), Session.SessionID, "", this);
                Session["objDispo"] = objDispo;
            }
            else
            {
                objDispo = (clsDisposition) Session["objDispo"];
            }

            if (!IsPostBack)
            {
                FillForm();
                ZulDateChanged();
            }
            else
            {
                if (Request.Params["__EVENTTARGET"] == "txtZulDate")
                {
                    ZulDateChanged();
                }
            }
        }

        private void FillForm()
        {
            // Morgiges Datum als Default-Wert setzen, dabei ggf. Wochenenden/Feiertage überspringen
            DateTime morgen = SkipWeekendsAndHolidays(DateTime.Today.AddDays(1));
            txtZulDate.Text = morgen.ToString("ddMMyy");
            
            // Bei Änderung des gewählten Zulassungsdatums Daten neu einlesen
            txtZulDate.Attributes.Add("onblur", "__doPostBack('txtZulDate', '');");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "');__doPostBack('txtZulDate', '');");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "');__doPostBack('txtZulDate', '');");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "');__doPostBack('txtZulDate', '');");
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Fillgrid()
        {
            DataView tmpDataView = objDispo.Dispositionen.DefaultView;

            if (tmpDataView.Count == 0)
            {
                cmdSave.Enabled = false;
                cmdSend.Enabled = false;

                lblError.Text = "Keine disponierbaren Vorgänge für den " + objDispo.ZulDat + " vorhanden!";
            }
            else
            {
                cmdSave.Enabled = true;
                cmdSend.Enabled = true;
            }

            gvDispositionen.DataSource = tmpDataView;
            gvDispositionen.DataBind();
        }

        /// <summary>
        /// Für jede Gridzeile die Fahrer-Dropdownliste füllen/initialisieren
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDispositionen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lbl = (Label) e.Row.FindControl("lblAmt");
                DropDownList ddl = (DropDownList) e.Row.FindControl("ddlFahrer");
                DataView tmpView = objDispo.Fahrerliste.DefaultView;
                tmpView.Sort = "MOBUSER";
                ddl.DataSource = tmpView;
                ddl.DataValueField = "MOBUSER";
                ddl.DataTextField = "NAME";
                ddl.DataBind();
                DataRow[] dRows = objDispo.Dispositionen.Select("AMT = '" + lbl.Text + "'");
                if (dRows.Length > 0)
                {
                    string mobUser = dRows[0]["MOBUSER"].ToString();
                    if (String.IsNullOrEmpty(mobUser))
                    {
                        ddl.SelectedValue = "0";
                    }
                    else
                    {
                        ddl.SelectedValue = mobUser;
                    }
                }
            }
        }

        /// <summary>
        /// Zulassungsdatum aus Textbox in Objekt übernehmen
        /// </summary>
        private bool ApplyZulDate()
        {
            DateTime tmpDatum;
            if (!DateTime.TryParseExact(txtZulDate.Text, "ddMMyy", System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDatum))
            {
                return false;
            }
            objDispo.ZulDat = tmpDatum.ToString("dd.MM.yyyy");

            return true;
        }

        private DateTime SkipWeekendsAndHolidays(DateTime datum)
        {
            DateTime tmpdat = datum;
            DeutscheFeiertageEinesJahres dfej = new DeutscheFeiertageEinesJahres(datum.Year);

            for (int i = 0; i < 5; i++)
            {
                if ((tmpdat.DayOfWeek == DayOfWeek.Saturday) || (tmpdat.DayOfWeek == DayOfWeek.Sunday) || (IstFeiertag(tmpdat)))
                {
                    tmpdat = tmpdat.AddDays(1);
                }
                else
                {
                    break;
                }
            }
            return tmpdat;
        }

        private bool IstFeiertag(DateTime datum)
        {
            bool erg = false;

            DeutscheFeiertageEinesJahres dfej = new DeutscheFeiertageEinesJahres(datum.Year);

            foreach (Feiertag ft in dfej.Feiertage)
            {
                if (ft.Datum.Date == datum.Date)
                {
                    erg = true;
                }
            }

            return erg;
        }

        private void ZulDateChanged()
        {
            if (ApplyZulDate())
            {
                objDispo.LoadDispos(Session["AppID"].ToString(), Session.SessionID, this);
                Session["objDispo"] = objDispo;
                Fillgrid();
            }
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            ZulDateChanged();
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            DatenSpeichern(false);
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            DatenSpeichern(true);
        }

        private void DatenSpeichern(bool senden)
        {
            Label lbl;
            DropDownList ddl;
            DataRow[] dRows;
            int disponierte = 0;
            string nichtDisponierte = "";

            // Eingaben aus Grid übernehmen
            foreach (GridViewRow gRow in gvDispositionen.Rows)
            {
                lbl = (Label)gRow.FindControl("lblAmt");
                ddl = (DropDownList)gRow.FindControl("ddlFahrer");
                dRows = objDispo.Dispositionen.Select("AMT = '" + lbl.Text + "'");
                if ((dRows.Length > 0) && (ddl.SelectedItem != null))
                {
                    if (ddl.SelectedItem.Value == "0")
                    {
                        dRows[0]["MOBUSER"] = "";
                        dRows[0]["NAME"] = "";
                        if (String.IsNullOrEmpty(nichtDisponierte))
                        {
                            nichtDisponierte = lbl.Text;
                        }
                        else
                        {
                            nichtDisponierte += ", " + lbl.Text;
                        }
                    }
                    else
                    {
                        dRows[0]["MOBUSER"] = ddl.SelectedItem.Value;
                        dRows[0]["NAME"] = ddl.SelectedItem.Text;
                        disponierte++;
                    }
                }
            }

            // Daten speichern (nur dann wirklich an SAP senden, wenn disponiert)
            bool datenAnSapSenden = ((senden) && (disponierte > 0));

            objDispo.SaveDispos(Session["AppID"].ToString(), Session.SessionID, this, datenAnSapSenden);

            if (objDispo.Status != 0)
            {
                lblError.Text = "Fehler beim " + (datenAnSapSenden ? "Absenden" : "Speichern") + " der Daten: " + objDispo.Message;
            }
            else
            {
                lblMessage.Text = "Daten erfolgreich " + (datenAnSapSenden ? "an SAP übertragen" : "gespeichert");

                if (datenAnSapSenden)
                {
                    if (!String.IsNullOrEmpty(nichtDisponierte))
                    {
                        lblError.Text = "Achtung! Für die Ämter " + nichtDisponierte + " wurde noch kein Fahrer ausgewählt";
                    }
                    objDispo.LoadDispos(Session["AppID"].ToString(), Session.SessionID, this);
                    Fillgrid();
                }
                else if (senden)
                {
                    lblError.Text = "Es wurden keine Sätze an SAP übertragen, weil für keines der Ämter ein Fahrer ausgewählt wurde";
                }
            }

            Session["objDispo"] = objDispo;
        }
    }
}