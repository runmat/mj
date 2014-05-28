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
using System.Collections.Generic;

namespace Leasing.forms
{
    public partial class Change81_4 : Page
    {
        private User m_User;
        private App m_App;
        private LP_02 objDienstleistung;
        protected GridNavigation GridNavigation1;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += GridView1_PageIndexChanged;
            GridNavigation1.PageSizeChanged += GridView1_ddlPageSizeChanged;

            //lnkFahrzeugsuche.NavigateUrl = "Change81.aspx?AppID=" + Session["AppID"].ToString();
            //lnkFahrzeugauswahl.NavigateUrl = "Change81_2.aspx?AppID=" + Session["AppID"].ToString();
            //lnkAdressen.NavigateUrl = "Change81_3.aspx?AppID=" + Session["AppID"].ToString();
            step1.NavigateUrl = "Change81.aspx?" + Request.QueryString;
            step2.NavigateUrl = "Change81_2.aspx?" + Request.QueryString;
            step2.NavigateUrl = "Change81_3.aspx?" + Request.QueryString;

            if (Session["objDienstleistung"] == null)
            { Response.Redirect("Change81.aspx?AppID=" + Session["AppID"].ToString()); }
            else { objDienstleistung = (LP_02)Session["objDienstleistung"]; }

            var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;
            tmpDataView.RowFilter = "MANDT = '99'";

            if (tmpDataView.Count == 0)
            {
                Response.Redirect("Change81.aspx?AppID=" + Session["AppID"].ToString());
            }
            tmpDataView.RowFilter = "";

            if (!IsPostBack)
            {
                InitialLoad();
            }
        }

        //public override void ProcessRequest(HttpContext context)
        //{
        //    if (context.Request.QueryString.AllKeys.Contains("Download"))
        //    {
        //        var f = (string)context.Session["DLFile"];

        //        if (!string.IsNullOrEmpty(f) && File.Exists(f))
        //        {
        //            var downloadFile = new FileInfo(f);
        //            var fileName = Path.GetFileNameWithoutExtension(f).Split('_').First() + Path.GetExtension(f);

        //            context.Response.Clear();
        //            context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", fileName));
        //            context.Response.AddHeader("Content-Length", downloadFile.Length.ToString());
        //            context.Response.ContentType = "application/octet-stream";
        //            context.Response.WriteFile(downloadFile.FullName);
        //            context.Response.Flush();
        //            context.Response.End();
        //        }
        //    }
        //    else
        //    {
        //        base.ProcessRequest(context);
        //    }
        //}

        protected void DownloadSummary(object sender, EventArgs e)
        {
            var head = new DataTable("Kopf");
            Array.ForEach(new[] { "Dienstleistung", "Username", "Datum", "Auftragsnummer" }, (c) => head.Columns.Add(c, typeof(string)));
            var r = head.NewRow();
            r["Dienstleistung"] = objDienstleistung.BeauftragungKlartext;
            r["Username"] = m_User.UserName;
            r["Datum"] = DateTime.Now.ToShortDateString();
            r["Auftragsnummer"] = objDienstleistung.Auftragsnummer;
            head.Rows.Add(r);
            head.AcceptChanges();

            var details = new DataTable("Details");
            Array.ForEach(new[] { "Titel", "Wert" }, (c) => details.Columns.Add(c, typeof(string)));
            if (pnlHalter.Visible)
            {
                var h = details.NewRow();
                h["Titel"] = "Halter";
                var text = "Adresse: " + Environment.NewLine + objDienstleistung.HalterName1;
                if (!string.IsNullOrEmpty(objDienstleistung.HalterName2))
                    text += " " + objDienstleistung.HalterName2;
                text += Environment.NewLine + objDienstleistung.HalterPLZ + " " + objDienstleistung.HalterOrt;
                text += Environment.NewLine + objDienstleistung.HalterStrasse + " " + objDienstleistung.HalterHausnr;
                h["Wert"] = text;
                details.Rows.Add(h);
            }
            if (pnlZulDaten.Visible)
            {
                var z = details.NewRow();
                z["Titel"] = "Zulassungsdaten";
                var text = "Wunschkennzeichen " + objDienstleistung.Kreis + "-" + objDienstleistung.Wunschkennzeichen;
                text += Environment.NewLine + "reserviert auf " + objDienstleistung.ReserviertAuf;
                if (trVersicherungstr.Visible) text += Environment.NewLine + "Versicherungsträger: " + objDienstleistung.Versicherungstraeger;
                if (!string.IsNullOrEmpty(objDienstleistung.EVBNr))
                {
                    var evbParts = objDienstleistung.EVBNr.Split(' ');
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
                var text = "Adresse: " + Environment.NewLine + objDienstleistung.EmpfaengerName1;
                if (!string.IsNullOrEmpty(objDienstleistung.EmpfaengerName2)) text += " " + objDienstleistung.EmpfaengerName2;
                text += Environment.NewLine + objDienstleistung.EmpfaengerPLZ + " " + objDienstleistung.EmpfaengerOrt;
                text += Environment.NewLine + objDienstleistung.EmpfaengerStrasse + " " + objDienstleistung.EmpfaengerHausnr;
                em["Wert"] = text;
                details.Rows.Add(em);
            }
            if (pnlSonstiges.Visible)
            {
                var s = details.NewRow();
                s["Titel"] = "Sonstiges";
                var text = "gew. Durchführungsdatum: " + objDienstleistung.DurchfuehrungsDatum;
                text += Environment.NewLine + "Bemerkung: " + objDienstleistung.Bemerkung;
                s["Wert"] = text;
                details.Rows.Add(s);
            }
            details.AcceptChanges();

            var fzg = new DataTable("Fahrzeuge");
            Array.ForEach(new[] { "Fahrgestellnummer", "Leasingnummer", "ZBII", "Kennzeichen" }, (c) => fzg.Columns.Add(c, typeof(string)));
            var rows = objDienstleistung.Fahrzeuge.Select("MANDT = '99'");
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
            switch (objDienstleistung.Auftragsgrund)
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
                default:
                    break;
            }
            lblBeauftragteDienstleistungAnzeige.Text = objDienstleistung.BeauftragungKlartext;
            lblAdresseName.Text = objDienstleistung.HalterName1;
            if (!string.IsNullOrEmpty(objDienstleistung.HalterName2))
            { lblAdresseName.Text += " " + objDienstleistung.HalterName2; }
            lblAdressePLZOrt.Text += objDienstleistung.HalterPLZ + " ";
            lblAdressePLZOrt.Text += objDienstleistung.HalterOrt;
            lblAdresseStrasseNr.Text += objDienstleistung.HalterStrasse + " ";
            lblAdresseStrasseNr.Text += objDienstleistung.HalterHausnr;

            lblKreis.Text = objDienstleistung.Kreis;
            lblWunschkennzeichen.Text = objDienstleistung.Wunschkennzeichen;
            lblWunschkennzeichen.Text = objDienstleistung.Wunschkennzeichen;
            lblReserviertAuf.Text = objDienstleistung.ReserviertAuf;
            lblVersicherungstraeger.Text = objDienstleistung.Versicherungstraeger;
            lblAdresseNameEmpf.Text = objDienstleistung.EmpfaengerName1;
            if (!string.IsNullOrEmpty(objDienstleistung.EmpfaengerName2))
            { lblAdresseNameEmpf.Text += " " + objDienstleistung.EmpfaengerName2; }
            lblAdressePLZOrtEmpf.Text += objDienstleistung.EmpfaengerPLZ + " ";
            lblAdressePLZOrtEmpf.Text += objDienstleistung.EmpfaengerOrt;
            lblAdresseStrasseNrEmpf.Text += objDienstleistung.EmpfaengerStrasse + " ";
            lblAdresseStrasseNrEmpf.Text += objDienstleistung.EmpfaengerHausnr;

            lblDurchfuehrungsDatum.Text = objDienstleistung.DurchfuehrungsDatum;
            lblBemerkung.Text = objDienstleistung.Bemerkung;

            if (!string.IsNullOrEmpty(objDienstleistung.EVBNr))
            {
                var split = objDienstleistung.EVBNr.Split(' ');
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
            var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;

            String strTempSort = "";
            String strDirection = null;

            if (strSort.Trim(' ').Length > 0)
            {
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
            tmpDataView.RowFilter = "MANDT = '99'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();

            //lblMessage.Text = tmpDataView.Count.ToString();
            //Int32 intZaehl0099 = 0;
            //lblMessage.Text = "";//Anforderungen zählen
            //foreach (DataRow row in tmpDataView.Table.Rows)
            //{
            //    if (row["MANDT"].ToString() == "99")
            //    {
            //        intZaehl0099 += 1;
            //    }
            //}
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

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void cmdContinue_Click(object sender, EventArgs e)
        {
            cmdContinue.Enabled = false;
            //lnkAdressen.Enabled = false;
            //lnkFahrzeugauswahl.Enabled = false;
            step2.Enabled = false;
            step3.Enabled = false;
            DoSubmit();
        }

        private void DoSubmit()
        {
            Trace logApp;

            if (Session["logObj"] != null)
            {
                logApp = (Trace)(Session["logObj"]);
            }
            else
            {
                logApp = new Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel);
            }

            lblMessage.Visible = false;
            btnSummary.Visible = false;

            try
            {
                var rows = objDienstleistung.Fahrzeuge.Select("MANDT = '99'");
                foreach (var row in rows)
                {
                    objDienstleistung.Equimpent = row["EQUNR"].ToString();
                    objDienstleistung.Fahrgestellnummer = row["Fahrgestellnummer"].ToString();

                    objDienstleistung.Anfordern(Session["AppID"].ToString(), Session.SessionID.ToString(), this);
                    row["STATUS"] = objDienstleistung.Auftragsstatus;

                    if (string.IsNullOrEmpty(objDienstleistung.Auftragsnummer) || objDienstleistung.Status != 0)
                        break;
                }

                if (!string.IsNullOrEmpty(objDienstleistung.Auftragsnummer) && objDienstleistung.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Ihre Auftragsnummer: " + objDienstleistung.Auftragsnummer;
                    btnSummary.Visible = true;
                }
                else if (!string.IsNullOrEmpty(objDienstleistung.Message))
                {
                    lblError.Text = objDienstleistung.Message;
                }

                var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;
                tmpDataView.RowFilter = "MANDT = '99'";
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                logApp.UpdateEntry("APP", Session["AppID"].ToString(), "Beauftragung sonstiger Dienstleistungen", null);
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler: " + ex.Message;
                logApp.UpdateEntry("ERR", Session["AppID"].ToString(), "Fehler bei der Briefanforderung zu Equipment: " + objDienstleistung.Equimpent + "Fehler: " + ex.Message, null);
                throw;
            }
        }
    }
}
