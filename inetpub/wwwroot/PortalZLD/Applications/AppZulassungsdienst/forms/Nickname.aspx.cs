using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Setzen eines "Nickname" für einen Kunden
    /// </summary>
    public partial class Nickname : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private ZLDCommon objCommon;

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }

            fillDropdown();
        }

        private void fillDropdown()
        {
            DataView tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();
            txtKundeSearch.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKundeSearch.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            ddlKunnr.Attributes.Add("onchange", "SetTexttValue(" + ddlKunnr.ClientID + "," + txtKundeSearch.ClientID + ")");
        }

        /// <summary>
        /// Nach einem bereits angelegten "nickname" des ausgewählten Kunden suchen(Z_ZLD_GET_NICKNAME).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtKundeSearch.Text.Length > 0)
            {
                objCommon.GetKundeNickname(Session["AppID"].ToString(), Session.SessionID, this, txtKundeSearch.Text );
                if(objCommon.Message.Length>0)
                {   lblError.Text = objCommon.Message;}
                else
                {
                    txtKundeNr.Text= txtKundeSearch.Text;
                    txtKundeName.Text=objCommon.Kundename;
                    txtNickname.Text = objCommon.Nickname;
                    lbtnDelete.Visible = true;
                    lbAbsenden.Visible = true;
                }
            }
            else
            { 
                lblError.Text ="Bitte geben Sie eine Kunndennummer ein!";
            }
        }

        /// <summary>
        /// "Nickname" in SAP speichern(Z_ZLD_SET_NICKNAME).
        /// </summary> 
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbAbsenden_Click(object sender, EventArgs e)
        {  

            if (txtNickname.Text.Length > 0)
            {
                objCommon.Nickname= txtNickname.Text;
                objCommon.SetKundeNickname(Session["AppID"].ToString(), Session.SessionID, this, txtKundeSearch.Text, "");
                if (objCommon.Message.Length > 0)
                { lblError.Text = objCommon.Message; }
                else
                {
                    txtKundeSearch.Text="";
                    ddlKunnr.SelectedValue = "0";
                    txtKundeNr.Text = "";
                    txtKundeName.Text = "";
                    txtNickname.Text = "";
                    lbtnDelete.Visible = false;
                    lbAbsenden.Visible = false;
                }
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
            objCommon.Nickname ="";
            objCommon.SetKundeNickname(Session["AppID"].ToString(), Session.SessionID, this, txtKundeSearch.Text, "X");
                if (objCommon.Message.Length > 0)
                { lblError.Text = objCommon.Message; }
                else
                {
                    txtKundeSearch.Text="";
                    txtKundeNr.Text = "";
                    txtKundeName.Text = "";
                    txtNickname.Text = "";
                    lbtnDelete.Visible = false;
                    lbAbsenden.Visible = false;
                }
            }

        }
}
