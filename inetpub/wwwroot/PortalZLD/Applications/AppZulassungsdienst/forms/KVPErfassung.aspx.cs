using System;
using CKG.Base.Business;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// KVP-Erfassung
    /// </summary>
    public partial class KVPErfassung : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private KVP mObjKVP;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if ((this.Session["mObjKVP"] != null))
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

        private void LoginKVPUser()
        {
            try
            {
                mObjKVP.KVPLogin(m_User.Kostenstelle, m_User.UserName, m_User.LastName + ", " + m_User.FirstName, Session["AppID"].ToString(), Session.SessionID, this);

                if (mObjKVP.HasError)
                {
                    throw new Exception("Fehler: " + mObjKVP.Message);
                }

                txtVorschlagVon.Text = mObjKVP.Benutzername;
                txtStandort.Text = mObjKVP.Standort;
                txtVorgesetzter.Text = mObjKVP.Vorgesetzter;
                txtKST.Text = mObjKVP.Kostenstelle;

                if ((!String.IsNullOrEmpty(mObjKVP.GeparkteKVPId)) && (mObjKVP.GeparkteKVPId.Trim().Length > 0))
                {
                    mObjKVP.LoadKVP(mObjKVP.GeparkteKVPId, Session["AppID"].ToString(), Session.SessionID, this);

                    if (mObjKVP.HasError)
                    {
                        lblError.Text = "Fehler beim KVP-Laden: " + mObjKVP.Message;
                    }
                    else
                    {
                        txtBeschreibung.Text = mObjKVP.Kurzbeschreibung;
                        txtSituation.Text = mObjKVP.SituationText;
                        txtVeraenderung.Text = mObjKVP.VeraenderungText;
                        txtVorteil.Text = mObjKVP.VorteilText;
                        btnVerwerfen.Visible = true;
                    }
                }

                Session["mObjKVP"] = mObjKVP;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnParken_Click(object sender, EventArgs e)
        {
            if (SaveKVP(true))
            {
                Session["mObjKVP"] = null;
                Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
            }
        }

        protected void btnVerwerfen_Click(object sender, EventArgs e)
        {
            mpeConfirmDelete.Show();
        }

        protected void btnSenden_Click(object sender, EventArgs e)
        {
            SaveKVP();
        }

        private bool SaveKVP(bool nurParken = false)
        {
            try
            {
                if (!nurParken)
                {
                    // Zum Absenden müssen alle Textfelder gefüllt sein
                    if ((String.IsNullOrEmpty(txtBeschreibung.Text)) || (String.IsNullOrEmpty(txtSituation.Text))
                    || (String.IsNullOrEmpty(txtVeraenderung.Text)) || (String.IsNullOrEmpty(txtVorteil.Text)))
                    {
                        lblError.Text = "Bitte füllen Sie alle Felder aus!";
                        return false;
                    }
                }

                mObjKVP.Funktion = txtFunktion.Text;
                mObjKVP.Kurzbeschreibung = txtBeschreibung.Text;
                mObjKVP.SituationText = txtSituation.Text;
                mObjKVP.VeraenderungText = txtVeraenderung.Text;
                mObjKVP.VorteilText = txtVorteil.Text;

                mObjKVP.SaveKVP(Session["AppID"].ToString(), Session.SessionID, this, nurParken);

                if (nurParken)
                {
                    if (mObjKVP.HasError)
                    {
                        lblError.Text = "Fehler beim KVP-Parken: " + mObjKVP.Message;
                    }
                    else
                    {
                        lblError.Text = "Ihr KVP wurde erfolgreich geparkt!";
                        btnVerwerfen.Visible = true;
                    }
                }
                else
                {
                    if (mObjKVP.HasError)
                    {
                        lblError.Text = "Fehler beim KVP-Senden: " + mObjKVP.Message;
                    }
                    else
                    {
                        ClearKVPFields();
                        lblError.Text = "Ihr KVP wurde erfolgreich versendet!";
                    }
                }

                Session["mObjKVP"] = mObjKVP;

                return (!mObjKVP.HasError);
            }
            catch (Exception ex)
            {
                if (nurParken)
                {
                    lblError.Text = "Fehler beim KVP-Parken: " + ex.Message;
                }
                else
                {
                    lblError.Text = "Fehler beim KVP-Senden: " + ex.Message;
                }       
                return false;
            }
        }

        private void DeleteKVP()
        {
            try
            {
                mObjKVP.DeleteKVP(Session["AppID"].ToString(), Session.SessionID, this);

                if (mObjKVP.HasError)
                {
                    lblError.Text = "Fehler beim KVP-Löschen: " + mObjKVP.Message;
                }
                else
                {
                    ClearKVPFields();
                    lblError.Text = "KVP verworfen";
                }

                Session["mObjKVP"] = mObjKVP;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim KVP-Löschen: " + ex.Message;
            }
        }

        private void ClearKVPFields()
        {
            txtBeschreibung.Text = "";
            txtSituation.Text = "";
            txtVeraenderung.Text = "";
            txtVorteil.Text = "";
        }

        protected void btnPanelConfirmDeleteOK_Click(object sender, EventArgs e)
        {
            mpeConfirmDelete.Hide();
            DeleteKVP();
        }

        protected void btnPanelConfirmDeleteCancel_Click(object sender, EventArgs e)
        {
            mpeConfirmDelete.Hide();
        }

        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Session["mObjKVP"] = null;
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

    }
}
