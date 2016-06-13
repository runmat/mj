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
    // ReSharper disable once InconsistentNaming
    public partial class Change81_3 : Page
    {
        private User _mUser;

        private Lp02 _objDienstleistung;

        // ReSharper disable once FunctionComplexityOverflow
        protected void Page_Load(object sender, EventArgs e)
        {
            _mUser = Common.GetUser(this);
            Common.FormAuth(this, _mUser);

            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)_mUser.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            
            step1.NavigateUrl = "Change81.aspx?" + Request.QueryString;
            step2.NavigateUrl = "Change81_2.aspx?" + Request.QueryString;

            if (Session["objDienstleistung"] == null)
            {
                Response.Redirect("Change81.aspx?AppID=" + Session["AppID"]);
            }
            else
            {
                _objDienstleistung = (Lp02)Session["objDienstleistung"];
            }

            if (!IsPostBack)
            {
                txtHalterName1.Text = _objDienstleistung.HalterName1;
                txtHalterName2.Text = _objDienstleistung.HalterName2;
                txtHalterOrt.Text = _objDienstleistung.HalterOrt;
                txtHalterPLZ.Text = _objDienstleistung.HalterPlz;
                txtHalterStrasse.Text = _objDienstleistung.HalterStrasse;
                txtHalterHausnr.Text = _objDienstleistung.HalterHausnr;
                txtStandortName1.Text = _objDienstleistung.StandortName1;
                txtStandortName2.Text = _objDienstleistung.StandortName2;
                txtStandortOrt.Text = _objDienstleistung.StandortOrt;
                txtStandortPLZ.Text = _objDienstleistung.StandortPlz;
                txtStandortStrasse.Text = _objDienstleistung.StandortStrasse;
                txtStandortHausnr.Text = _objDienstleistung.StandortHausnr;
                txtKreis.Text = _objDienstleistung.Kreis;
                lblKreis.Text = _objDienstleistung.Kreis;
                txtWunschkennzeichen.Text = _objDienstleistung.Wunschkennzeichen;
                txtWunschkennzeichen.Text = _objDienstleistung.Wunschkennzeichen;
                txtReserviertAuf.Text = _objDienstleistung.ReserviertAuf;
                txtVersicherungstraeger.Text = _objDienstleistung.Versicherungstraeger;
                txtEmpfaengerName1.Text = _objDienstleistung.EmpfaengerName1;
                txtEmpfaengerName2.Text = _objDienstleistung.EmpfaengerName2;
                txtEmpfaengerOrt.Text = _objDienstleistung.EmpfaengerOrt;
                txtEmpfaengerPLZ.Text = _objDienstleistung.EmpfaengerPlz;
                txtEmpfaengerStrasse.Text = _objDienstleistung.EmpfaengerStrasse;
                txtEmpfaengerHausnr.Text = _objDienstleistung.EmpfaengerHausnr;
                txtDurchfuehrungsDatum.Text = _objDienstleistung.DurchfuehrungsDatum;
                txtBemerkung.Text = _objDienstleistung.Bemerkung;

                if (!string.IsNullOrEmpty(_objDienstleistung.EvbNr))
                {
                    var split = _objDienstleistung.EvbNr.Split(' ');
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

                if (!string.IsNullOrEmpty(_objDienstleistung.Auftragsgrund))
                {
                    ddlDienstleistung.SelectedValue = _objDienstleistung.Auftragsgrund;
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
            var tmpDataView = _objDienstleistung.Fahrzeuge.DefaultView;
            tmpDataView.RowFilter = "MANDT = '99'";

            if (tmpDataView.Count == 0)
            {
                Response.Redirect("Change81.aspx?AppID=" + Session["AppID"]);
            }
            else
            {
                lblKreis.Text = tmpDataView[0]["Kennzeichen"].ToString().Split('-')[0];
            }

            tmpDataView.RowFilter = "";
            var dummyDate = _objDienstleistung.SuggestionDay();
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
            BindLandDdl(ddlHalterLand, revHalterPLZ, _objDienstleistung.LaenderPlz);
            BindLandDdl(ddlEmpfaengerLand, revEmpfaengerPLZ, _objDienstleistung.LaenderPlz);
        }

        private void BindLandDdl(DropDownList ddl, RegularExpressionValidator rev, DataTable table)
        {
            if (table == null) throw new ArgumentNullException("table");
            ddl.DataSource = _objDienstleistung.LaenderPlz;
            ddl.DataTextField = "FullDesc";
            ddl.DataValueField = "Land1";
            ddl.DataBind();

            var firstLanguage = Request["HTTP_ACCEPT_Language"].Split(',').First();
            var region = firstLanguage.Split('-').Last().ToUpper();

            var item = ddl.Items.FindByValue(region);

            if (item != null) item.Selected = true;

            ApplyPlzFormat(ddl, rev);
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
                // nichts..
            }
        }

        // ReSharper disable once FunctionComplexityOverflow
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (ddlDienstleistung.SelectedIndex == 0)
            {
                lblError.Text = "Bitte Dienstleistung auswählen!";
                lblError.Visible = true;
                return;
            }

            _objDienstleistung.Auftragsgrund = ddlDienstleistung.SelectedValue;
            _objDienstleistung.BeauftragungKlartext = ddlDienstleistung.SelectedItem.Text;
            if (pnlHalter.Visible)
            {
                _objDienstleistung.HalterName1 = txtHalterName1.Text;
                _objDienstleistung.HalterName2 = txtHalterName2.Text;
                _objDienstleistung.HalterOrt = txtHalterOrt.Text;
                _objDienstleistung.HalterPlz = txtHalterPLZ.Text;
                _objDienstleistung.HalterStrasse = txtHalterStrasse.Text;
                _objDienstleistung.HalterHausnr = txtHalterHausnr.Text;
            }
            else
            {
                txtHalterName1.Text = "";
                txtHalterName2.Text = "";
                txtHalterOrt.Text = "";
                txtHalterPLZ.Text = "";
                txtHalterStrasse.Text = "";
                txtHalterHausnr.Text = "";
                _objDienstleistung.HalterName1 = "";
                _objDienstleistung.HalterName2 = "";
                _objDienstleistung.HalterOrt = "";
                _objDienstleistung.HalterPlz = "";
                _objDienstleistung.HalterStrasse = "";
                _objDienstleistung.HalterHausnr = "";
            }

            if (pnlZulDaten.Visible)
            {
                if (trWunschkennzeichen.Visible)
                {
                    _objDienstleistung.Kreis = txtKreis.Text;
                    _objDienstleistung.Wunschkennzeichen = txtWunschkennzeichen.Text;
                }
                else
                {
                    txtKreis.Text = "";
                    txtWunschkennzeichen.Text = "";
                }

                if (trReserviertAuf.Visible)
                {
                    _objDienstleistung.ReserviertAuf = txtReserviertAuf.Text;

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
                        if (txtEVBNummer.Text.Length > 6 && _objDienstleistung.IsAlphaNumeric(txtEVBNummer.Text) && HelpProcedures.checkDate(ref txtEVBVon, ref txtEVBBis, ref strFehlermeldung, true))
                        {
                            _objDienstleistung.Versicherungstraeger = txtVersicherungstraeger.Text;
                            if (txtEVBVon.Text != String.Empty)
                            {
                                _objDienstleistung.EvbNr = txtEVBNummer.Text + " " + HelpProcedures.MakeDateSAP(txtEVBVon.Text) + " " + HelpProcedures.MakeDateSAP(txtEVBBis.Text);
                                _objDienstleistung.EvbNrSingle = txtEVBNummer.Text;

                                DateTime tmp;

                                var von = DateTime.TryParse(txtEVBVon.Text, out tmp) ? tmp : (DateTime?)null;
                                var bis = DateTime.TryParse(txtEVBBis.Text, out tmp) ? tmp : (DateTime?)null;


                                _objDienstleistung.EvbGueltigVon = von;
                                _objDienstleistung.EvbGueltigBis = bis;
                            }
                            else
                            {
                                _objDienstleistung.EvbNr = txtEVBNummer.Text;
                                _objDienstleistung.EvbNrSingle = txtEVBNummer.Text;
                            }
                        }
                        else if (txtEVBNummer.Text.Length < 7 || !_objDienstleistung.IsAlphaNumeric(txtEVBNummer.Text))
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
                    _objDienstleistung.Versicherungstraeger = "";
                    _objDienstleistung.EvbNr = "";
                    _objDienstleistung.EvbNrSingle = "";
                }
            }

            if (pnlEmpfaenger.Visible)
            {
                _objDienstleistung.EmpfaengerName1 = txtEmpfaengerName1.Text;
                _objDienstleistung.EmpfaengerName2 = txtEmpfaengerName2.Text;
                _objDienstleistung.EmpfaengerOrt = txtEmpfaengerOrt.Text;
                _objDienstleistung.EmpfaengerPlz = txtEmpfaengerPLZ.Text;
                _objDienstleistung.EmpfaengerStrasse = txtEmpfaengerStrasse.Text;
                _objDienstleistung.EmpfaengerHausnr = txtEmpfaengerHausnr.Text;
            }
            else
            {
                txtEmpfaengerName1.Text = "";
                txtEmpfaengerName2.Text = "";
                txtEmpfaengerOrt.Text = "";
                txtEmpfaengerPLZ.Text = "";
                txtEmpfaengerStrasse.Text = "";
                txtEmpfaengerHausnr.Text = "";
                _objDienstleistung.EmpfaengerName1 = "";
                _objDienstleistung.EmpfaengerName2 = "";
                _objDienstleistung.EmpfaengerOrt = "";
                _objDienstleistung.EmpfaengerPlz = "";
                _objDienstleistung.EmpfaengerStrasse = "";
                _objDienstleistung.EmpfaengerHausnr = "";
            }

            if (pnlSonstiges.Visible)
            {
                //if (objDienstleistung.IsDate(txtDurchfuehrungsDatum.Text))
                DateTime val;
                if (DateTime.TryParse(txtDurchfuehrungsDatum.Text,out val))
                {
                    if (DateTime.Parse(txtDurchfuehrungsDatum.Text) < DateTime.Today)
                    {
                        lblError.Text = "Das gew. Durchführungsdatum darf nicht in der Vergangenheit liegen.";
                        return;
                    }

                    _objDienstleistung.DurchfuehrungsDatum = txtDurchfuehrungsDatum.Text;
                }
                else
                {
                    lblError.Text = "Bitte geben Sie ein gültiges Datum ein.";
                    return;
                }
                _objDienstleistung.Bemerkung = txtBemerkung.Text;
            }

            Session["objDienstleistung"] = _objDienstleistung;
            Response.Redirect("Change81_4.aspx?AppID=" + Session["AppID"]);
        }

        protected void SucheZulassungskreis(object sender, EventArgs e)
        {
            

            if (txtHalterPLZ.Text.Length != 5)
            {
                lblError.Text = "Bitte geben Sie eine fünfstellige PLZ beim Halter ein.";
                lblError.Visible = true;
                return;
            }

            String zulassungskreis = _objDienstleistung.KreisSuche(Session["AppID"].ToString(),
                Session.SessionID, this, txtHalterPLZ.Text);

            if (zulassungskreis.IsNullOrEmpty())
            {
                lblError.Text = "Der Zulassungskreis konnte nicht ermittelt werden.";
                lblError.Visible = true;
            }
            else
            {
                _objDienstleistung.Kreis = zulassungskreis;
                txtKreis.Text = zulassungskreis;
            }

            Session["objDienstleistung"] = _objDienstleistung;

        }

        protected void HalterLandChanged(object sender, EventArgs e)
        {
            ApplyPlzFormat(ddlHalterLand, revHalterPLZ);
        }

        protected void EmpfaengerLandChanged(object sender, EventArgs e)
        {
            ApplyPlzFormat(ddlEmpfaengerLand, revEmpfaengerPLZ);
        }

        private void ApplyPlzFormat(DropDownList ddl, RegularExpressionValidator rev)
        {
            var row = _objDienstleistung.LaenderPlz.Select("Land1='" + ddl.SelectedValue + "'").FirstOrDefault();
            
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
