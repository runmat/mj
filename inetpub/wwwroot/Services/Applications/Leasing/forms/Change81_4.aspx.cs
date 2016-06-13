using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.DocumentGeneration;
using CKG.Base.Kernel.Logging;
using CKG.Base.Kernel.Security;
using CKG.Services;
using Leasing.lib;
using System.Globalization;
using GeneralTools.Services;

namespace Leasing.forms
{
    // ReSharper disable once InconsistentNaming
    public partial class Change81_4 : Page
    {
        private User _mUser;
        private App _mApp;
        private Lp02 _objDienstleistung;
        protected GridNavigation GridNavigation1;

        protected void Page_Load(object sender, EventArgs e)
        {
            _mUser = Common.GetUser(this);
            Common.FormAuth(this, _mUser);
            _mApp = new App(_mUser);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)_mUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            step1.NavigateUrl = "Change81.aspx?" + Request.QueryString;
            step2.NavigateUrl = "Change81_2.aspx?" + Request.QueryString;
            step2.NavigateUrl = "Change81_3.aspx?" + Request.QueryString;

            if (Session["objDienstleistung"] == null)
            { Response.Redirect("Change81.aspx?AppID=" + Session["AppID"]); }
            else { _objDienstleistung = (Lp02)Session["objDienstleistung"]; }

            var tmpDataView = _objDienstleistung.Fahrzeuge.DefaultView;
            tmpDataView.RowFilter = "MANDT = '99'";

            if (tmpDataView.Count == 0)
            {
                Response.Redirect("Change81.aspx?AppID=" + Session["AppID"]);
            }
            tmpDataView.RowFilter = "";

            if (!IsPostBack)
            {
                InitialLoad();
            }
        }

 
        // ReSharper disable once FunctionComplexityOverflow
        protected void DownloadSummary(object sender, EventArgs e)
        {
            var head = new DataTable("Kopf");
            Array.ForEach(new[] { "Dienstleistung", "Username", "Datum", "Auftragsnummer" }, c => head.Columns.Add(c, typeof(string)));
            var r = head.NewRow();
            r["Dienstleistung"] = _objDienstleistung.BeauftragungKlartext;
            r["Username"] = _mUser.UserName;
            r["Datum"] = DateTime.Now.ToShortDateString();
            r["Auftragsnummer"] = _objDienstleistung.Auftragsnummer;
            head.Rows.Add(r);
            head.AcceptChanges();

            var details = new DataTable("Details");
            Array.ForEach(new[] { "Titel", "Wert" }, c => details.Columns.Add(c, typeof(string)));
            if (pnlHalter.Visible)
            {
                var h = details.NewRow();
                h["Titel"] = "Halter";
                var text = "Adresse: " + Environment.NewLine + _objDienstleistung.HalterName1;
                if (!string.IsNullOrEmpty(_objDienstleistung.HalterName2))
                    text += " " + _objDienstleistung.HalterName2;
                text += Environment.NewLine + _objDienstleistung.HalterPlz + " " + _objDienstleistung.HalterOrt;
                text += Environment.NewLine + _objDienstleistung.HalterStrasse + " " + _objDienstleistung.HalterHausnr;
                h["Wert"] = text;
                details.Rows.Add(h);
            }
            if (pnlZulDaten.Visible)
            {
                var z = details.NewRow();
                z["Titel"] = "Zulassungsdaten";
                var text = "Wunschkennzeichen " + _objDienstleistung.Kreis + "-" + _objDienstleistung.Wunschkennzeichen;
                text += Environment.NewLine + "reserviert auf " + _objDienstleistung.ReserviertAuf;
                if (trVersicherungstr.Visible) text += Environment.NewLine + "Versicherungsträger: " + _objDienstleistung.Versicherungstraeger;
                if (!string.IsNullOrEmpty(_objDienstleistung.EvbNr))
                {
                    var evbParts = _objDienstleistung.EvbNr.Split(' ');
                    if (trEvbNr.Visible) text += Environment.NewLine + "eVB-Nummer: " + evbParts.FirstOrDefault();
                    DateTime von, bis;
                    if (evbParts.Length >= 3 && DateTime.TryParseExact(evbParts.ElementAt(1), "yyyyMMdd", null, DateTimeStyles.AssumeLocal, out von) &&
                        DateTime.TryParseExact(evbParts.ElementAt(2), "yyyyMMdd", null, DateTimeStyles.AssumeLocal, out bis))
                    {
                        text += Environment.NewLine + "Gültigkeit: gültig von " + von.ToShortDateString() + " bis " + bis.ToShortDateString();
                    }
                }
                z["Wert"] = text;
                details.Rows.Add(z);
            }
            if (pnlEmpfaenger.Visible)
            {
                var em = details.NewRow();
                em["Titel"] = "Empfänger Schein/Schilder";
                var text = "Adresse: " + Environment.NewLine + _objDienstleistung.EmpfaengerName1;
                if (!string.IsNullOrEmpty(_objDienstleistung.EmpfaengerName2)) text += " " + _objDienstleistung.EmpfaengerName2;
                text += Environment.NewLine + _objDienstleistung.EmpfaengerPlz + " " + _objDienstleistung.EmpfaengerOrt;
                text += Environment.NewLine + _objDienstleistung.EmpfaengerStrasse + " " + _objDienstleistung.EmpfaengerHausnr;
                em["Wert"] = text;
                details.Rows.Add(em);
            }
            if (pnlSonstiges.Visible)
            {
                var s = details.NewRow();
                s["Titel"] = "Sonstiges";
                var text = "gew. Durchführungsdatum: " + _objDienstleistung.DurchfuehrungsDatum;
                text += Environment.NewLine + "Bemerkung: " + _objDienstleistung.Bemerkung;
                s["Wert"] = text;
                details.Rows.Add(s);
            }
            details.AcceptChanges();

            var fzg = new DataTable("Fahrzeuge");
            Array.ForEach(new[] { "Fahrgestellnummer", "Leasingnummer", "ZBII", "Kennzeichen" }, c => fzg.Columns.Add(c, typeof(string)));
            var rows = _objDienstleistung.Fahrzeuge.Select("MANDT = '99'");
            foreach (var row in rows)
            {
                var f = fzg.NewRow();
                f["Fahrgestellnummer"] = row["Fahrgestellnummer"];
                f["Leasingnummer"] = row["Leasingnummer"];
                f["ZBII"] = row["NummerZB2"];
                f["Kennzeichen"] = row["Kennzeichen"];
                fzg.Rows.Add(f);
            }
            fzg.AcceptChanges();
            var ds = new DataSet();
            ds.Tables.Add(details);
            ds.Tables.Add(fzg);

            var factory = new WordDocumentFactory(new DataTable("dt"), null);
            factory.CreateDocumentDataset("Sonstige Dienstleistungen " + DateTime.Now.ToString("yyyyMMdd-HHmmss"), this, "\\Applications\\Leasing\\docu\\Summary.doc", head, ds);
        }

        private void InitialLoad()
        {
            HideAll();
            switch (_objDienstleistung.Auftragsgrund)
            {
                case "2052":
                    pnlHalter.Visible = true;
                    pnlSonstiges.Visible = true;
                    pnlEmpfaenger.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = true;
                    trEvbNr.Visible = true;
                    trHinweis.Visible = false;
                    break;
                case "572":
                    pnlHalter.Visible = true;
                    pnlSonstiges.Visible = true;
                    pnlEmpfaenger.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = true;
                    trEvbNr.Visible = true;
                    trHinweis.Visible = false;
                    break;
                case "1294":
                    pnlHalter.Visible = false;
                    pnlSonstiges.Visible = true;
                    pnlEmpfaenger.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = false;
                    trEvbNr.Visible = false;
                    trHinweis.Visible = false;
                    break;
                case "2037": // "Ersatzfahrzeugschein"
                case "2076": // "Kennzeichen erneuern / Nachstempelung"
                    pnlEmpfaenger.Visible = true;
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = true;
                    lblHinweis.Text = "Bitte Verlusterklärung / eidestattliche Versicherung im Original an DAD senden.";
                    break;
                case "1380-1":
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = true;
                    lblHinweis.Text = "Bitte Gutachten im Original an DAD senden.";
                    break;
                case "1380-2":
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = false;
                    break;
                case "1380-3":
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = true;
                    lblHinweis.Text = "Bitte ZB1 im Original und Kennzeichen an DAD senden.";
                    break;
                case "1462":
                    pnlSonstiges.Visible = true;
                    trWunschkennzeichen.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = true;
                    trEvbNr.Visible = true;
                    trHinweis.Visible = false;
                    break;
            }
            lblBeauftragteDienstleistungAnzeige.Text = _objDienstleistung.BeauftragungKlartext;
            lblAdresseName.Text = _objDienstleistung.HalterName1;
            if (!string.IsNullOrEmpty(_objDienstleistung.HalterName2))
            { lblAdresseName.Text += " " + _objDienstleistung.HalterName2; }
            lblAdressePLZOrt.Text += _objDienstleistung.HalterPlz + " ";
            lblAdressePLZOrt.Text += _objDienstleistung.HalterOrt;
            lblAdresseStrasseNr.Text += _objDienstleistung.HalterStrasse + " ";
            lblAdresseStrasseNr.Text += _objDienstleistung.HalterHausnr;

            lblKreis.Text = _objDienstleistung.Kreis;
            lblWunschkennzeichen.Text = _objDienstleistung.Wunschkennzeichen;
            lblWunschkennzeichen.Text = _objDienstleistung.Wunschkennzeichen;
            lblReserviertAuf.Text = _objDienstleistung.ReserviertAuf;
            lblVersicherungstraeger.Text = _objDienstleistung.Versicherungstraeger;
            lblAdresseNameEmpf.Text = _objDienstleistung.EmpfaengerName1;
            if (!string.IsNullOrEmpty(_objDienstleistung.EmpfaengerName2))
            { lblAdresseNameEmpf.Text += " " + _objDienstleistung.EmpfaengerName2; }
            lblAdressePLZOrtEmpf.Text += _objDienstleistung.EmpfaengerPlz + " ";
            lblAdressePLZOrtEmpf.Text += _objDienstleistung.EmpfaengerOrt;
            lblAdresseStrasseNrEmpf.Text += _objDienstleistung.EmpfaengerStrasse + " ";
            lblAdresseStrasseNrEmpf.Text += _objDienstleistung.EmpfaengerHausnr;

            lblDurchfuehrungsDatum.Text = _objDienstleistung.DurchfuehrungsDatum;
            lblBemerkung.Text = _objDienstleistung.Bemerkung;

            if (!string.IsNullOrEmpty(_objDienstleistung.EvbNr))
            {
                var split = _objDienstleistung.EvbNr.Split(' ');
                lblEVB.Text = split[0];
                if (split.Length > 1)
                {
                    lblDatumVON.Text = HelpProcedures.MakeDateStandard(split[1]).ToShortDateString();
                    lblDatumBis.Text = HelpProcedures.MakeDateStandard(split[2]).ToShortDateString();
                }
            }
            else
            {
                lblEVB.Text = String.Empty;
                lblDatumVON.Text = String.Empty;
                lblDatumBis.Text = String.Empty;
            }
            Fillgrid("");
        }

        private void HideAll()
        {
            pnlHalter.Visible = false;
            pnlSonstiges.Visible = false;
            pnlEmpfaenger.Visible = false;
            pnlZulDaten.Visible = false;
        }

        private void Fillgrid(String strSort)
        {
            Result.Visible = true;
            var tmpDataView = _objDienstleistung.Fahrzeuge.DefaultView;

            String strTempSort = "";
            String strDirection = null;

            if (strSort.Trim(' ').Length > 0)
            {
                strTempSort = strSort.Trim(' ');
                if ((ViewState["Sort"] == null) || ((String)ViewState["Sort"] == strTempSort))
                {
                    if (ViewState["Direction"] == null)
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = (String)ViewState["Direction"];
                    }
                }
                else
                {
                    strDirection = "desc";
                }

                strDirection = strDirection == "asc" ? "desc" : "asc";

                ViewState["Sort"] = strTempSort;
                ViewState["Direction"] = strDirection;
            }

            if (strTempSort.Length != 0)
            {
                tmpDataView.Sort = strTempSort + " " + strDirection;
            }
            tmpDataView.RowFilter = "MANDT = '99'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            
        }

        private void GridView1_PageIndexChanged(Int32 pageindex)
        {
            Fillgrid("");
        }

        private void GridView1_ddlPageSizeChanged()
        {
            Fillgrid("");
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            Fillgrid(e.SortExpression);
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            cmdContinue.Visible = false;
            //lnkAdressen.Enabled = false;
            //lnkFahrzeugauswahl.Enabled = false;
            step2.Enabled = false;
            step3.Enabled = false;
            DoSubmit();
        }

        private void DoSubmit()
        {

            string bapiName = ApplicationConfiguration.GetApplicationConfigValue("SonstDienstleistungSpeicherBapi", Session["AppID"].ToString(), _mUser.Customer.CustomerId);

            //default, falls gar nichts konfiguriert wurde
            if (String.IsNullOrEmpty(bapiName))
            {
                AnfordernDefault();
 
            }
            else if (bapiName.ToUpper() == "Z_M_DEZDIENSTL_001")
            {
                 AnfordernDefault();
            }

            else
            {
                AnfordernCustom(); 
            }


        }

        private void AnfordernDefault()
        {
            Trace logApp;

            if (Session["logObj"] != null)
            {
                logApp = (Trace)(Session["logObj"]);
            }
            else
            {
                logApp = new Trace(_mApp.Connectionstring, _mApp.SaveLogAccessSAP, _mApp.LogLevel);
            }

            lblMessage.Visible = false;
            btnSummary.Visible = false;

            try
            {

                var rows = _objDienstleistung.Fahrzeuge.Select("MANDT = '99'");
                foreach (var row in rows)
                {
                    _objDienstleistung.Equimpent = row["EQUNR"].ToString();
                    _objDienstleistung.Fahrgestellnummer = row["Fahrgestellnummer"].ToString();

                    _objDienstleistung.Anfordern(Session["AppID"].ToString(), Session.SessionID, this);
                    row["STATUS"] = _objDienstleistung.Auftragsstatus;

                    if (string.IsNullOrEmpty(_objDienstleistung.Auftragsnummer) || _objDienstleistung.Status != 0)
                        break;
                }

                if (!string.IsNullOrEmpty(_objDienstleistung.Auftragsnummer) && _objDienstleistung.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Ihre Auftragsnummer: " + _objDienstleistung.Auftragsnummer;
                    btnSummary.Visible = true;
                }
                else if (!string.IsNullOrEmpty(_objDienstleistung.Message))
                {
                    lblError.Text = _objDienstleistung.Message;
                }

                var tmpDataView = _objDienstleistung.Fahrzeuge.DefaultView;
                tmpDataView.RowFilter = "MANDT = '99'";
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                logApp.UpdateEntry("APP", Session["AppID"].ToString(), "Beauftragung sonstiger Dienstleistungen");
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler: " + ex.Message;
                logApp.UpdateEntry("ERR", Session["AppID"].ToString(), "Fehler bei der Briefanforderung zu Equipment: " + _objDienstleistung.Equimpent + "Fehler: " + ex.Message);
                throw;
            }
        }

        private void AnfordernCustom()
        {
            Trace logApp;

            if (Session["logObj"] != null)
            {
                logApp = (Trace)(Session["logObj"]);
            }
            else
            {
                logApp = new Trace(_mApp.Connectionstring, _mApp.SaveLogAccessSAP, _mApp.LogLevel);
            }

            lblMessage.Visible = false;
            btnSummary.Visible = false;

            try
            {
                _objDienstleistung.AnfordernCustom(Session["AppID"].ToString(), Session.SessionID, this);

                if (!string.IsNullOrEmpty(_objDienstleistung.Auftragsnummer))
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Ihre Auftragsnummer: " + _objDienstleistung.Auftragsnummer;
                    btnSummary.Visible = true;
                }
                else if (!string.IsNullOrEmpty(_objDienstleistung.Message))
                {
                    lblError.Text = _objDienstleistung.Message;
                }


                var tmpDataView = _objDienstleistung.Fahrzeuge.DefaultView;
                tmpDataView.RowFilter = "MANDT = '99'";
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                logApp.UpdateEntry("APP", Session["AppID"].ToString(), "Beauftragung sonstiger Dienstleistungen");
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler: " + ex.Message;
                logApp.UpdateEntry("ERR", Session["AppID"].ToString(), "Fehler bei der Briefanforderung zu Equipment: " + _objDienstleistung.Equimpent + "Fehler: " + ex.Message);
                throw;
            }
            
        }


    }
}
