using System;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Telerik.Web.UI;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// KVP-Bewertung
    /// </summary>
    public partial class KVPBewertung : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private KVP mObjKVP;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if ((Session["mObjKVP"] != null))
            {
                mObjKVP = (KVP)Session["mObjKVP"];
            }

            if (!IsPostBack)
            {
                mObjKVP = new KVP(ref m_User, ref m_App, Session["AppID"].ToString(), Session.SessionID, "");
                LoginKVPUser();
                Session["mObjKVP"] = mObjKVP;

                Title = lblHead.Text;
            }

            Session["LastPage"] = this;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (mObjKVP.Vorschlagsliste != null)
            {
                rgGrid1.DataSource = mObjKVP.Vorschlagsliste.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem gridRow = e.Item as GridDataItem;

                if (e.CommandName == "bewerten")
                {
                    mObjKVP.SelectKVPForBewertung(gridRow["KVPID"].Text);
                    LadeKVP();
                    Session["mObjKVP"] = mObjKVP;

                    Bewertungsliste.Visible = false;
                    Bewertung.Visible = true;
                }
            }
        }

        protected void btnLike_Click(object sender, EventArgs e)
        {
            mObjKVP.BewertungPositiv = true;
            mObjKVP.BewertungNegativ = false;
            Session["mObjKVP"] = mObjKVP;
            lblBewertung.Text = btnLike.Text;
            mpeConfirmBewertung.Show();
        }

        protected void btnDontLike_Click(object sender, EventArgs e)
        {
            mObjKVP.BewertungPositiv = false;
            mObjKVP.BewertungNegativ = true;
            Session["mObjKVP"] = mObjKVP;
            lblBewertung.Text = btnDontLike.Text;
            mpeConfirmBewertung.Show();
        }

        protected void btnPanelConfirmDeleteOK_Click(object sender, EventArgs e)
        {
            mpeConfirmBewertung.Hide();
            BewerteKVP();
        }

        protected void btnPanelConfirmDeleteCancel_Click(object sender, EventArgs e)
        {
            mpeConfirmBewertung.Hide();
            mObjKVP.BewertungPositiv = false;
            mObjKVP.BewertungNegativ = false;
            Session["mObjKVP"] = mObjKVP;
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Session["mObjKVP"] = null;
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

        private void LoginKVPUser()
        {
            try
            {
                mObjKVP.KVPLogin(m_User.Kostenstelle, m_User.UserName, m_User.LastName + ", " + m_User.FirstName, Session["AppID"].ToString(), Session.SessionID, this);

                if (mObjKVP.HasError)
                {
                    throw new Exception("Fehler: " + mObjKVP.Message);
                }

                LadeBewertungen();

                Session["mObjKVP"] = mObjKVP;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void LadeBewertungen()
        {
            mObjKVP.LoadBewertungen(Session["AppID"].ToString(), Session.SessionID, this);
            Session["mObjKVP"] = mObjKVP;

            if (mObjKVP.HasError)
            {
                lblError.Text = "Fehler beim Laden der Bewertungsliste: " + mObjKVP.Message;
            }
            else
            {
                FillGrid();
            }
        }

        private void FillGrid()
        {
            if ((mObjKVP.Vorschlagsliste != null) && (mObjKVP.Vorschlagsliste.Rows.Count > 0))
            {
                rgGrid1.Visible = true;
                rgGrid1.Rebind();
                // Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
            else
            {
                rgGrid1.Visible = false;
                lblNoData.Text = "Keine Daten gefunden";
            }
        }

        private void LadeKVP()
        {
            mObjKVP.LoadKVP(mObjKVP.AktuelleKVPId, Session["AppID"].ToString(), Session.SessionID, this, true);

            if (mObjKVP.HasError)
            {
                btnLike.Enabled = false;
                btnDontLike.Enabled = false;
                lblError.Text = "Fehler beim KVP-Laden: " + mObjKVP.Message;
            }
            else
            {
                txtBeschreibung.Text = mObjKVP.Kurzbeschreibung;
                txtSituation.Text = mObjKVP.SituationText;
                txtVeraenderung.Text = mObjKVP.VeraenderungText;
                txtVorteil.Text = mObjKVP.VorteilText;
                txtBewertungsfrist.Text = mObjKVP.Bewertungsfrist;
                btnLike.Enabled = true;
                btnDontLike.Enabled = true;
            }
        }

        private void BewerteKVP()
        {
            try
            {
                mObjKVP.SaveBewertung(Session["AppID"].ToString(), Session.SessionID, this);

                if (mObjKVP.HasError)
                {
                    lblError.Text = "Fehler beim Speichern der Bewertung: " + mObjKVP.Message;
                }
                else
                {
                    btnLike.Enabled = false;
                    btnDontLike.Enabled = false;
                    ClearKVPFields();
                    lblError.Text = "Bewertung erfolgreich gespeichert";
                    Session["mObjKVP"] = mObjKVP;
                    Bewertungsliste.Visible = true;
                    Bewertung.Visible = false;
                    LadeBewertungen();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Speichern der Bewertung: " + ex.Message;
            }
        }

        private void ClearKVPFields()
        {
            txtBeschreibung.Text = "";
            txtSituation.Text = "";
            txtVeraenderung.Text = "";
            txtVorteil.Text = "";
            txtBewertungsfrist.Text = "";
        }

        #endregion
    }
}
