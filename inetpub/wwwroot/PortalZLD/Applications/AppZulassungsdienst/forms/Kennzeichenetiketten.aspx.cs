using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    public partial class Kennzeichenetiketten : System.Web.UI.Page
    {
        private User m_User;
        private DruckKennzeichenetiketten mObjDruckKennzeichenetiketten;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if ((Session["mObjDruckKennzeichenetiketten"] != null))
            {
                mObjDruckKennzeichenetiketten = (DruckKennzeichenetiketten)Session["mObjDruckKennzeichenetiketten"];
            }

            if (!IsPostBack)
            {
                SetAttributes();

                mObjDruckKennzeichenetiketten = new DruckKennzeichenetiketten(m_User.Reference);
                Session["mObjDruckKennzeichenetiketten"] = mObjDruckKennzeichenetiketten;

                Title = lblHead.Text;
                FillForm();
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            Panel1.Visible = !Panel1.Visible;
            cmdCreate.Visible = !cmdCreate.Visible;
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        protected void cmdPrint_Click(object sender, EventArgs e)
        {
            PrintEtiketten();
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            rgGrid1.DataSource = mObjDruckKennzeichenetiketten.Vorgaenge;
        }

        protected void cbxAuswahl_CheckedChanged(object sender, EventArgs e)
        {
            var cbx = (CheckBox)sender;
            var item = (GridDataItem)cbx.NamingContainer;
            var sapId = item.GetDataKeyValue("SapId").ToString();

            mObjDruckKennzeichenetiketten.SelectVorgang(sapId, cbx.Checked);
            Session["mObjDruckKennzeichenetiketten"] = mObjDruckKennzeichenetiketten;
        }

        #endregion

        #region Methods

        private void FillForm()
        {
            txtDruckerpositionZeile.Text = "1";
            txtDruckerpositionSpalte.Text = "1";
        }

        private void SetAttributes()
        {
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
        }

        private void DoSubmit()
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtZulDate.Text))
            {
                lblError.Text = "Bitte geben Sie ein Zulassungsdatum ein!";
                return;
            }
            if (txtZulDate.Text.Trim(' ').Length < 6)
            {
                lblError.Text = "Bitte geben Sie das Zulassungsdatum 6-stellig ein!";
                return;
            }
            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return;
            }

            if (String.IsNullOrEmpty(txtDruckerpositionZeile.Text) || !txtDruckerpositionZeile.Text.IsNumeric() || txtDruckerpositionZeile.Text.ToInt() < 1)
            {
                lblError.Text = "Bitte geben Sie eine Zeilennummer > 0 ein!";
                return;
            }

            if (String.IsNullOrEmpty(txtDruckerpositionSpalte.Text) || !txtDruckerpositionSpalte.Text.IsNumeric() || txtDruckerpositionSpalte.Text.ToInt() < 1)
            {
                lblError.Text = "Bitte geben Sie eine Spaltennummer > 0 ein!";
                return;
            }

            mObjDruckKennzeichenetiketten.Zulassungsdatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
            mObjDruckKennzeichenetiketten.SapId = txtID.Text;
            mObjDruckKennzeichenetiketten.Kennzeichen = txtKennzeichen.Text.ToUpper();
            mObjDruckKennzeichenetiketten.DruckAbZeile = txtDruckerpositionZeile.Text.ToInt();
            mObjDruckKennzeichenetiketten.DruckAbSpalte = txtDruckerpositionSpalte.Text.ToInt();
            mObjDruckKennzeichenetiketten.Deltaliste = (rbAnsicht.SelectedValue == "0");

            mObjDruckKennzeichenetiketten.LoadVorgaenge();
            Session["mObjDruckKennzeichenetiketten"] = mObjDruckKennzeichenetiketten;

            if (mObjDruckKennzeichenetiketten.ErrorOccured)
            {
                lblError.Text = "Fehler: " + mObjDruckKennzeichenetiketten.Message;
            }
            else
            {
                FillGrid();
            }
        }

        private void FillGrid()
        {
            if (mObjDruckKennzeichenetiketten.Vorgaenge.AnyAndNotNull())
            {
                Panel1.Visible = false;
                cmdCreate.Visible = false;
                Panel2.Visible = true;
                rgGrid1.Visible = true;
                cmdPrint.Visible = true;
                rgGrid1.Rebind();
                // Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
            else
            {
                Panel2.Visible = false;
                rgGrid1.Visible = false;
                cmdPrint.Visible = false;
                lblError.Text = "Keine Daten gefunden";
            }
        }

        private void PrintEtiketten()
        {
            lblError.Text = "";

            if (mObjDruckKennzeichenetiketten.Vorgaenge.None(v => v.IsSelected))
            {
                lblError.Text = "Fehler: Es wurden keine Vorgänge selektiert";
                return;
            }

            mObjDruckKennzeichenetiketten.PrintEtiketten();

            if (mObjDruckKennzeichenetiketten.ErrorOccured)
            {
                lblError.Text = "Fehler: " + mObjDruckKennzeichenetiketten.Message;
            }
            else
            {
                if ((mObjDruckKennzeichenetiketten.PDFXString != null) && (mObjDruckKennzeichenetiketten.PDFXString.Length > 0))
                {
                    Session["PDFXString"] = mObjDruckKennzeichenetiketten.PDFXString;

                    if (mObjDruckKennzeichenetiketten.Deltaliste)
                    {
                        mObjDruckKennzeichenetiketten.Vorgaenge.RemoveAll(v => v.IsSelected);
                        Session["mObjDruckKennzeichenetiketten"] = mObjDruckKennzeichenetiketten;
                        FillGrid();
                    }

                    ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");
                }
                else
                {
                    lblError.Text = "PDF-Generierung fehlgeschlagen.";
                }
            }
        }

        #endregion
    }
}
