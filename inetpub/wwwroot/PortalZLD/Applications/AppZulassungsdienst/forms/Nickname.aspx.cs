using System;
using System.Linq;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Setzen eines "Nickname" für einen Kunden
    /// </summary>
    public partial class Nickname : System.Web.UI.Page
    {
        private User m_User;
        private ZLDCommon objCommon;
        private clsNickname objNickname;

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
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

            if ((Session["objNickname"] != null))
            {
                objNickname = (clsNickname)Session["objNickname"];
            }
            else
            {
                objNickname = new clsNickname(m_User.Reference);
                Session["objNickname"] = objNickname;
            }

            fillDropdown();
        }

        /// <summary>
        /// Nach einem bereits angelegten "nickname" des ausgewählten Kunden suchen(Z_ZLD_GET_NICKNAME).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (!String.IsNullOrEmpty(txtKundeSearch.Text))
            {
                objNickname.GetKundeNickname(txtKundeSearch.Text);

                if (objNickname.ErrorOccured)
                {
                    lblError.Text = objNickname.Message;
                }
                else
                {
                    txtKundeNr.Text = txtKundeSearch.Text;
                    txtKundeName.Text = objNickname.KundenName;
                    txtNickname.Text = objNickname.KundenNickname;
                    lbtnDelete.Visible = true;
                    lbAbsenden.Visible = true;
                }

                Session["objNickname"] = objNickname;
            }
            else
            {
                lblError.Text = "Bitte geben Sie eine Kunndennummer ein!";
            }
        }

        /// <summary>
        /// "Nickname" in SAP speichern(Z_ZLD_SET_NICKNAME).
        /// </summary> 
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbAbsenden_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNickname.Text))
            {
                objNickname.KundenNickname = txtNickname.Text;

                objNickname.SetKundeNickname(txtKundeSearch.Text, false);

                if (objNickname.ErrorOccured)
                {
                    lblError.Text = objNickname.Message;
                }
                else
                {
                    txtKundeSearch.Text = "";
                    ddlKunnr.SelectedValue = "0";
                    txtKundeNr.Text = "";
                    txtKundeName.Text = "";
                    txtNickname.Text = "";
                    lbtnDelete.Visible = false;
                    lbAbsenden.Visible = false;
                }

                Session["objNickname"] = objNickname;
            }
            else
            {
                lblError.Text = "Bitte geben Sie einen Suchbegriff ein!";
            }
        }

        /// <summary>
        /// "Nickname" in SAP löschen(Z_ZLD_SET_NICKNAME).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            objNickname.KundenNickname = "";

            objNickname.SetKundeNickname(txtKundeSearch.Text, true);

            if (objNickname.ErrorOccured)
            {
                lblError.Text = objNickname.Message;
            }
            else
            {
                txtKundeSearch.Text = "";
                txtKundeNr.Text = "";
                txtKundeName.Text = "";
                txtNickname.Text = "";
                lbtnDelete.Visible = false;
                lbAbsenden.Visible = false;
            }

            Session["objNickname"] = objNickname;
        }

        #endregion

        #region Methods

        private void fillDropdown()
        {
            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv);
            ddlKunnr.DataValueField = "KundenNr";
            ddlKunnr.DataTextField = "Name";
            ddlKunnr.DataBind();

            txtKundeSearch.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKundeSearch.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            ddlKunnr.Attributes.Add("onchange", "SetTexttValue(" + ddlKunnr.ClientID + "," + txtKundeSearch.ClientID + ")");
        }

        #endregion
    }
}
