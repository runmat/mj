using System;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    public partial class TaggleicheMeldungDAD : Page
    {
        private User m_User;
        private MeldungDAD objMeldungDAD;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

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

            if (!IsPostBack)
            {
                objMeldungDAD = new MeldungDAD(m_User.Reference);
                Session["objMeldungDAD"] = objMeldungDAD;

                Title = lblHead.Text;

                lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulassungsdatum.ClientID + "'); return false;");
                lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulassungsdatum.ClientID + "'); return false;");
                lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulassungsdatum.ClientID + "'); return false;");
            }
            else if (Session["objMeldungDAD"] != null)
            {
                objMeldungDAD = (MeldungDAD)Session["objMeldungDAD"];
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Panel1.Visible = !Panel1.Visible;
            cmdCreate.Visible = !cmdCreate.Visible;
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        protected void cmdFrachtbriefnummer_Click(object sender, EventArgs e)
        {
            trFrachtbriefnummer.Visible = true;
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            SaveMeldung();
        }

        #endregion

        #region Methods

        private void DoSubmit()
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtFahrgestellnummer.Text) && String.IsNullOrEmpty(txtBriefnummer.Text))
            {
                lblError.Text = "Bitte geben Sie eine Fahrgestell- oder Briefnummer an!";
                return;
            }

            objMeldungDAD.IDSuche = txtBarcode.Text.Trim();
            objMeldungDAD.FahrgestellnummerSuche = txtFahrgestellnummer.Text.Trim().ToUpper();
            objMeldungDAD.BriefnummerSuche = txtBriefnummer.Text.Trim().ToUpper();

            objMeldungDAD.LoadVorgang();

            Session["objMeldungDAD"] = objMeldungDAD;

            if (objMeldungDAD.ErrorOccured)
            {
                lblError.Text = "Fehler: " + objMeldungDAD.Message;
            }
            else
            {
                FillForm();
            }
        }

        private void FillForm()
        {
            Panel1.Visible = false;
            cmdCreate.Visible = false;

            lblIDDisplay.Text = objMeldungDAD.ID;
            lblBestellnummerDisplay.Text = objMeldungDAD.Bestellnummer;
            lblFahrgestellnummerDisplay.Text = objMeldungDAD.Fahrgestellnummer;
            lblBriefnummerDisplay.Text = objMeldungDAD.Briefnummer;

            if (!String.IsNullOrEmpty(objMeldungDAD.Zulassungsdatum))
            {
                String tmpDate = objMeldungDAD.Zulassungsdatum;
                txtZulassungsdatum.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);
            }
            else
            {
                txtZulassungsdatum.Text = "";
            }

            string tmpKennz1;
            string tmpKennz2;
            ZLDCommon.KennzeichenAufteilen(objMeldungDAD.Kennzeichen, out tmpKennz1, out tmpKennz2);
            txtKennz1.Text = tmpKennz1;
            txtKennz2.Text = tmpKennz2;

            txtGebuehr.Text = "";
            cbxAuslieferung.Checked = false;
            txtFrachtbriefnummer.Text = objMeldungDAD.Frachtbriefnummer;

            Panel2.Visible = true;
            cmdSend.Visible = true;
        }

        private void SaveMeldung()
        {
            try
            {
                if (String.IsNullOrEmpty(txtZulassungsdatum.Text))
                {
                    lblError.Text = "Bitte geben Sie ein Zulassungsdatum ein.";
                    return;
                }
                objMeldungDAD.Zulassungsdatum = ZLDCommon.toShortDateStr(txtZulassungsdatum.Text);
                if (String.IsNullOrEmpty(objMeldungDAD.Zulassungsdatum))
                {
                    lblError.Text = "Bitte geben Sie ein gültiges Zulassungsdatum ein.";
                    return;
                }

                if (String.IsNullOrEmpty(txtKennz1.Text) || String.IsNullOrEmpty(txtKennz2.Text))
                {
                    lblError.Text = "Bitte geben Sie ein vollständiges Kennzeichen ein.";
                    return;
                }
                objMeldungDAD.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();

                if (!String.IsNullOrEmpty(txtGebuehr.Text) && !txtGebuehr.Text.Trim().IsDecimal())
                {
                    lblError.Text = "Bitte geben Sie einen Zahlenwert für die Gebühr ein.";
                    return;
                }
                objMeldungDAD.Gebuehr = txtGebuehr.Text.Trim();

                objMeldungDAD.Auslieferung = cbxAuslieferung.Checked;

                if (trFrachtbriefnummer.Visible)
                {
                    objMeldungDAD.Frachtbriefnummer = txtFrachtbriefnummer.Text;
                }

                objMeldungDAD.SaveVorgang(m_User.UserName);

                if (objMeldungDAD.ErrorOccured)
                {
                    lblError.Text = "Fehler beim Absenden: " + objMeldungDAD.Message;
                }
                else
                {
                    lblError.Text = "Die Meldung wurde erfolgreich abgesendet!";
                    Panel2.Visible = false;
                    cmdSend.Visible = false;
                    Panel1.Visible = true;
                    objMeldungDAD.ClearFields();
                    txtBarcode.Text = "";
                    txtFahrgestellnummer.Text = "";
                    txtBriefnummer.Text = "";
                    trFrachtbriefnummer.Visible = false;
                    cmdCreate.Visible = true;
                }

                Session["objMeldungDAD"] = objMeldungDAD;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Absenden: " + ex.Message;
            }
        }

        #endregion
    }
}
