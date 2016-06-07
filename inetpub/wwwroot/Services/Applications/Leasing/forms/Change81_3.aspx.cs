using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using GeneralTools.Models;
using Leasing.lib;

namespace Leasing.forms
{
    public partial class Change81_3 : Page
    {
        private User m_User;
        private App m_App;
        private Lp02 objDienstleistung;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            //lnkFahrzeugsuche.NavigateUrl = "Change81.aspx?AppID=" + Session["AppID"].ToString();
            step1.NavigateUrl = "Change81.aspx?" + Request.QueryString;
            step2.NavigateUrl = "Change81_2.aspx?" + Request.QueryString;

            if (Session["objDienstleistung"] == null)
            {
                Response.Redirect("Change81.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                objDienstleistung = (Lp02)Session["objDienstleistung"];
            }

            if (!IsPostBack)
            {
                txtHalterName1.Text = objDienstleistung.HalterName1;
                txtHalterName2.Text = objDienstleistung.HalterName2;
                txtHalterOrt.Text = objDienstleistung.HalterOrt;
                txtHalterPLZ.Text = objDienstleistung.HalterPLZ;
                txtHalterStrasse.Text = objDienstleistung.HalterStrasse;
                txtHalterHausnr.Text = objDienstleistung.HalterHausnr;
                txtStandortName1.Text = objDienstleistung.StandortName1;
                txtStandortName2.Text = objDienstleistung.StandortName2;
                txtStandortOrt.Text = objDienstleistung.StandortOrt;
                txtStandortPLZ.Text = objDienstleistung.StandortPLZ;
                txtStandortStrasse.Text = objDienstleistung.StandortStrasse;
                txtStandortHausnr.Text = objDienstleistung.StandortHausnr;
                txtKreis.Text = objDienstleistung.Kreis;
                lblKreis.Text = objDienstleistung.Kreis;
                txtWunschkennzeichen.Text = objDienstleistung.Wunschkennzeichen;
                txtWunschkennzeichen.Text = objDienstleistung.Wunschkennzeichen;
                txtReserviertAuf.Text = objDienstleistung.ReserviertAuf;
                txtVersicherungstraeger.Text = objDienstleistung.Versicherungstraeger;
                txtEmpfaengerName1.Text = objDienstleistung.EmpfaengerName1;
                txtEmpfaengerName2.Text = objDienstleistung.EmpfaengerName2;
                txtEmpfaengerOrt.Text = objDienstleistung.EmpfaengerOrt;
                txtEmpfaengerPLZ.Text = objDienstleistung.EmpfaengerPLZ;
                txtEmpfaengerStrasse.Text = objDienstleistung.EmpfaengerStrasse;
                txtEmpfaengerHausnr.Text = objDienstleistung.EmpfaengerHausnr;
                txtDurchfuehrungsDatum.Text = objDienstleistung.DurchfuehrungsDatum;
                txtBemerkung.Text = objDienstleistung.Bemerkung;

                if (!string.IsNullOrEmpty(objDienstleistung.EVBNr))
                {
                    var split = objDienstleistung.EVBNr.Split(' ');
                    txtEVBNummer.Text = split[0];
                    if (split.Length > 1)
                    {
                        txtEVBVon.Text = DateTime.ParseExact(split[1], "yyyyMMdd", null).ToShortDateString();
                        txtEVBBis.Text = DateTime.ParseExact(split[2], "yyyyMMdd", null).ToShortDateString();
                    }
                }
                else
                {
                    txtEVBNummer.Text = String.Empty;
                    txtEVBVon.Text = String.Empty;
                    txtEVBBis.Text = String.Empty;
                }

                HideAll();
                FillDates();
                FillDropDown();
                FillLaender();

                if (!string.IsNullOrEmpty(objDienstleistung.Auftragsgrund))
                {
                    ddlDienstleistung.SelectedValue = objDienstleistung.Auftragsgrund;
                    DisplaySelectedDienstleistung();
                }
            }

            cmdSave.Enabled = ddlDienstleistung.SelectedIndex > 0;
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void HideAll()
        {
            pnlHalter.Visible = false;
            pnlSonstiges.Visible = false;
            pnlNeuerStandort1.Visible = false;
            pnlEmpfaenger.Visible = false;
            pnlZulDaten.Visible = false;
            lnkStandortAnzeige.Text = "Anzeigen";
        }

        private void FillDates()
        {
            var tmpDataView = objDienstleistung.Fahrzeuge.DefaultView;
            tmpDataView.RowFilter = "MANDT = '99'";

            if (tmpDataView.Count == 0)
            {
                Response.Redirect("Change81.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                lblKreis.Text = tmpDataView[0]["Kennzeichen"].ToString().Split('-')[0];
            }

            tmpDataView.RowFilter = "";
            var dummyDate = objDienstleistung.SuggestionDay();
            txtDurchfuehrungsDatum.Text = dummyDate.ToShortDateString();
        }

        private void FillDropDown()
        {
            var item = new ListItem("-- keine Auswahl --", "kein");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Ummeldung ohne Kennzeichenwechsel", "2052");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Ummeldung mit Kennzeichenwechsel", "572");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Umkennzeichnung", "1294");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Ersatzfahrzeugschein", "2037");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Kennzeichen erneuern / Nachstempelung", "2076");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Technischer Eintrag", "1380-1");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Korrektur wegen Fehleintrag", "1380-2");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Abmeldung", "2900");
            ddlDienstleistung.Items.Add(item);

            item = new ListItem("Wiederzulassung", "1462");
            ddlDienstleistung.Items.Add(item);
        }

        private void FillLaender()
        {
            BindLandDDL(ddlHalterLand, revHalterPLZ, objDienstleistung.LaenderPLZ);
            BindLandDDL(ddlEmpfaengerLand, revEmpfaengerPLZ, objDienstleistung.LaenderPLZ);
        }

        private void BindLandDDL(DropDownList ddl, RegularExpressionValidator rev, DataTable table)
        {
            ddl.DataSource = objDienstleistung.LaenderPLZ;
            ddl.DataTextField = "FullDesc";
            ddl.DataValueField = "Land1";
            ddl.DataBind();

            var firstLanguage = Request["HTTP_ACCEPT_Language"].Split(',').First();
            var region = firstLanguage.Split('-').Last().ToUpper();

            var item = ddl.Items.FindByValue(region);

            if (item != null) item.Selected = true;

            ApplyPLZFormat(ddl, rev);
        }

        protected void ddlDienstleistung_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySelectedDienstleistung();
        }

        private void DisplaySelectedDienstleistung()
        {
            HideAll();
            switch (ddlDienstleistung.SelectedValue)
            {
                case "2052": // "Ummeldung innerorts"
                    pnlHalter.Visible = true;
                    pnlSonstiges.Visible = true;
                    pnlEmpfaenger.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = true;
                    trEvbNr.Visible = true;
                    txtKreis.Text = lblKreis.Text;
                    lblKreis.Visible = true;
                    txtKreis.Visible = false;
                    trHinweis.Visible = false;
                    btnZulkreis.Visible = false;
                    break;
                case "572": // "Ummeldung ausserorts"
                    pnlHalter.Visible = true;
                    pnlSonstiges.Visible = true;
                    pnlEmpfaenger.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = true;
                    trEvbNr.Visible = true;
                    trHinweis.Visible = false;
                    txtKreis.Text = lblKreis.Text;
                    lblKreis.Visible = false;
                    txtKreis.Visible = true;
                    btnZulkreis.Visible = true;
                    break;
                case "1294": // "Umkennzeichnung"
                    pnlHalter.Visible = false;
                    pnlSonstiges.Visible = true;
                    pnlEmpfaenger.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = false;
                    trEvbNr.Visible = false;
                    trHinweis.Visible = false;
                    txtKreis.Text = lblKreis.Text;
                    lblKreis.Visible = true;
                    txtKreis.Visible = false;
                    btnZulkreis.Visible = false;
                    break;
                case "2037": // "Ersatzfahrzeugschein"
                case "2076": // "Kennzeichen erneuern / Nachstempelung"
                    pnlEmpfaenger.Visible = true;
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = true;
                    lblHinweis.Text = "Bitte Verlusterklärung / eidestattliche Versicherung im Original an DAD senden.";
                    txtKreis.Text = "";
                    btnZulkreis.Visible = false;
                    break;
                case "1380-1": // "Technischer Eintrag"
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = true;
                    lblHinweis.Text = "Bitte Gutachten im Original an DAD senden.";
                    txtKreis.Text = "";
                    btnZulkreis.Visible = false;
                    break;
                case "1380-2": // "Korrektur wegen Fehleintrag"
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = false;
                    txtKreis.Text = "";
                    btnZulkreis.Visible = false;
                    break;
                case "1380-3": // "Abmeldung"
                    pnlSonstiges.Visible = true;
                    trHinweis.Visible = true;
                    lblHinweis.Text = "Bitte ZB1 im Original und Kennzeichen an DAD senden.";
                    txtKreis.Text = "";
                    btnZulkreis.Visible = false;
                    break;
                case "1462": // "Wiederzulassung"
                    pnlEmpfaenger.Visible = true;
                    pnlSonstiges.Visible = true;
                    trWunschkennzeichen.Visible = true;
                    pnlZulDaten.Visible = true;
                    trVersicherungstr.Visible = true;
                    trEvbNr.Visible = true;
                    trHinweis.Visible = false;
                    txtKreis.Text = "";
                    btnZulkreis.Visible = false;
                    break;
                case "kein":
                default:
                    // nichts..
                    break;
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (ddlDienstleistung.SelectedIndex == 0)
            {
                lblError.Text = "Bitte Dienstleistung auswählen!";
                lblError.Visible = true;
                return;
            }

            objDienstleistung.Auftragsgrund = ddlDienstleistung.SelectedValue;
            objDienstleistung.BeauftragungKlartext = ddlDienstleistung.SelectedItem.Text;
            if (pnlHalter.Visible)
            {
                objDienstleistung.HalterName1 = txtHalterName1.Text;
                objDienstleistung.HalterName2 = txtHalterName2.Text;
                objDienstleistung.HalterOrt = txtHalterOrt.Text;
                objDienstleistung.HalterPLZ = txtHalterPLZ.Text;
                objDienstleistung.HalterStrasse = txtHalterStrasse.Text;
                objDienstleistung.HalterHausnr = txtHalterHausnr.Text;
            }
            else
            {
                txtHalterName1.Text = "";
                txtHalterName2.Text = "";
                txtHalterOrt.Text = "";
                txtHalterPLZ.Text = "";
                txtHalterStrasse.Text = "";
                txtHalterHausnr.Text = "";
                objDienstleistung.HalterName1 = "";
                objDienstleistung.HalterName2 = "";
                objDienstleistung.HalterOrt = "";
                objDienstleistung.HalterPLZ = "";
                objDienstleistung.HalterStrasse = "";
                objDienstleistung.HalterHausnr = "";
            }

            if (pnlZulDaten.Visible)
            {
                if (trWunschkennzeichen.Visible)
                {
                    objDienstleistung.Kreis = txtKreis.Text;
                    objDienstleistung.Wunschkennzeichen = txtWunschkennzeichen.Text;
                }
                else
                {
                    txtKreis.Text = "";
                    txtWunschkennzeichen.Text = "";
                }

                if (trReserviertAuf.Visible)
                {
                    objDienstleistung.ReserviertAuf = txtReserviertAuf.Text;

                }
                else
                {
                    txtReserviertAuf.Text = "";
                }

                if (trVersicherungstr.Visible)
                {
                    if (txtEVBNummer.Text != String.Empty)
                    {
                        var strFehlermeldung = "";
                        if (txtEVBNummer.Text.Length > 6 && objDienstleistung.IsAlphaNumeric(txtEVBNummer.Text) && HelpProcedures.checkDate(ref txtEVBVon, ref txtEVBBis, ref strFehlermeldung, true, 0))
                        {
                            objDienstleistung.Versicherungstraeger = txtVersicherungstraeger.Text;
                            if (txtEVBVon.Text != String.Empty)
                            {
                                objDienstleistung.EVBNr = txtEVBNummer.Text + " " + HelpProcedures.MakeDateSAP(txtEVBVon.Text) + " " + HelpProcedures.MakeDateSAP(txtEVBBis.Text);
                                objDienstleistung.EvbNrSingle = txtEVBNummer.Text;

                                DateTime tmp;

                                var von = DateTime.TryParse(txtEVBVon.Text, out tmp) ? tmp : (DateTime?)null;
                                var bis = DateTime.TryParse(txtEVBBis.Text, out tmp) ? tmp : (DateTime?)null;


                                objDienstleistung.EvbGueltigVon = von;
                                objDienstleistung.EvbGueltigBis = bis;
                            }
                            else
                            {
                                objDienstleistung.EVBNr = txtEVBNummer.Text;
                                objDienstleistung.EvbNrSingle = txtEVBNummer.Text;
                            }
                        }
                        else if (txtEVBNummer.Text.Length < 7 || !objDienstleistung.IsAlphaNumeric(txtEVBNummer.Text))
                        {
                            lblError.Text = "EVB-Nummer ungültig"; return;
                        }
                        else { lblError.Text = strFehlermeldung; return; }
                    }
                }
                else
                {
                    txtVersicherungstraeger.Text = "";
                    txtEVBNummer.Text = "";
                    txtEVBVon.Text = "";
                    txtEVBBis.Text = "";
                    objDienstleistung.Versicherungstraeger = "";
                    objDienstleistung.EVBNr = "";
                    objDienstleistung.EvbNrSingle = "";
                }
            }

            if (pnlEmpfaenger.Visible)
            {
                objDienstleistung.EmpfaengerName1 = txtEmpfaengerName1.Text;
                objDienstleistung.EmpfaengerName2 = txtEmpfaengerName2.Text;
                objDienstleistung.EmpfaengerOrt = txtEmpfaengerOrt.Text;
                objDienstleistung.EmpfaengerPLZ = txtEmpfaengerPLZ.Text;
                objDienstleistung.EmpfaengerStrasse = txtEmpfaengerStrasse.Text;
                objDienstleistung.EmpfaengerHausnr = txtEmpfaengerHausnr.Text;
            }
            else
            {
                txtEmpfaengerName1.Text = "";
                txtEmpfaengerName2.Text = "";
                txtEmpfaengerOrt.Text = "";
                txtEmpfaengerPLZ.Text = "";
                txtEmpfaengerStrasse.Text = "";
                txtEmpfaengerHausnr.Text = "";
                objDienstleistung.EmpfaengerName1 = "";
                objDienstleistung.EmpfaengerName2 = "";
                objDienstleistung.EmpfaengerOrt = "";
                objDienstleistung.EmpfaengerPLZ = "";
                objDienstleistung.EmpfaengerStrasse = "";
                objDienstleistung.EmpfaengerHausnr = "";
            }

            if (pnlSonstiges.Visible)
            {
                if (objDienstleistung.IsDate(txtDurchfuehrungsDatum.Text))
                {
                    if (DateTime.Parse(txtDurchfuehrungsDatum.Text) < DateTime.Today)
                    {
                        lblError.Text = "Das gew. Durchführungsdatum darf nicht in der Vergangenheit liegen.";
                        return;
                    }

                    objDienstleistung.DurchfuehrungsDatum = txtDurchfuehrungsDatum.Text;
                }
                else
                {
                    lblError.Text = "Bitte geben Sie ein gültiges Datum ein.";
                    return;
                }
                objDienstleistung.Bemerkung = txtBemerkung.Text;
            }

            Session["objDienstleistung"] = objDienstleistung;
            Response.Redirect("Change81_4.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void SucheZulassungskreis(object sender, EventArgs e)
        {
            

            if (txtHalterPLZ.Text.Length != 5)
            {
                lblError.Text = "Bitte geben Sie eine fünfstellige PLZ beim Halter ein.";
                lblError.Visible = true;
                return;
            }

            String zulassungskreis = objDienstleistung.KreisSuche(Session["AppID"].ToString(),
                Session.SessionID, this, txtHalterPLZ.Text);

            if (zulassungskreis.IsNullOrEmpty())
            {
                lblError.Text = "Der Zulassungskreis konnte nicht ermittelt werden.";
                lblError.Visible = true;
            }
            else
            {
                objDienstleistung.Kreis = zulassungskreis;
                txtKreis.Text = zulassungskreis;
            }

            Session["objDienstleistung"] = objDienstleistung;

        }

        protected void HalterLandChanged(object sender, EventArgs e)
        {
            ApplyPLZFormat(ddlHalterLand, revHalterPLZ);
        }

        protected void EmpfaengerLandChanged(object sender, EventArgs e)
        {
            ApplyPLZFormat(ddlEmpfaengerLand, revEmpfaengerPLZ);
        }

        private void ApplyPLZFormat(DropDownList ddl, RegularExpressionValidator rev)
        {
            var row = objDienstleistung.LaenderPLZ.Select("Land1='" + ddl.SelectedValue + "'").FirstOrDefault();
            
            if (row != null)
            {
                int lnPlz;
                if (int.TryParse((string)row["LNPLZ"], out lnPlz))
                {
                    rev.ValidationExpression = "\\d{" + lnPlz + "}";
                    return;
                }
            }

            rev.ValidationExpression = "\\d+";
        }
    }
}
