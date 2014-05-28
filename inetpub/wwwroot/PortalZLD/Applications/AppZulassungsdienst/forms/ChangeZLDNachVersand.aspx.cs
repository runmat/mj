﻿using System;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;


namespace AppZulassungsdienst.forms
{
    public partial class ChangeZLDNachVersand : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        protected void Page_Load(object sender, EventArgs e)
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

            if (IsPostBack != true)
            {
                objNacherf = new NacherfZLD(ref m_User, m_App, "VZ");
                objNacherf.VKBUR = m_User.Reference.Substring(4, 4);
                objNacherf.VKORG = m_User.Reference.Substring(0, 4);
                fillForm();
            }
            else
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
                objNacherf.SelID = txtID.Text;
                objNacherf.SelKunde = txtKunnr.Text;
                objNacherf.SelKreis = txtStVa.Text;
                objNacherf.SelMatnr = "";
                objNacherf.SelLief = "";
                objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objNacherf.Vorgang = "VZ";
                Session["objNacherf"] = objNacherf;
            }
        }

        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            txtStVa.Text = ddlStVa.SelectedValue;
        }

        protected void ddlKunnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKunnr.Text = ddlKunnr.SelectedValue;
        }

        private void fillForm()
        {
            Session["objNacherf"] = objNacherf;
            if (objNacherf.Status > 0)
            {
                lblError.Text = objNacherf.Message;
                return;
            }
            else
            {
                DataView tmpDView = new DataView();
                tmpDView = objCommon.tblKundenStamm.DefaultView;
                tmpDView.Sort = "NAME1";
                ddlKunnr.DataSource = tmpDView;
                ddlKunnr.DataValueField = "KUNNR";
                ddlKunnr.DataTextField = "NAME1";
                ddlKunnr.DataBind();
                ddlKunnr.SelectedValue = "0";
                txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
                txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");

                lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
                lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
                lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");

                if (objNacherf.Status == 0)
                {
                    tmpDView = new DataView();
                    tmpDView = objCommon.tblStvaStamm.DefaultView;
                    tmpDView.Sort = "KREISTEXT";
                    ddlStVa.DataSource = tmpDView;
                    ddlStVa.DataValueField = "KREISKZ";
                    ddlStVa.DataTextField = "KREISTEXT";
                    ddlStVa.DataBind();
                    ddlStVa.SelectedValue = "0";
                    Session["objNacherf"] = objNacherf;
                }
                else
                {
                    lblError.Text = objNacherf.Message;
                    return;
                }

            }
        }

        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return;
            }

            objNacherf = (NacherfZLD)Session["objNacherf"];

            objNacherf.SelStatus = "VZ,AV";
            objNacherf.SelFlieger = chkFlieger.Checked;
            objNacherf.DZVKBUR = "X";
            objNacherf.getSAPDatenNacherf(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm, objCommon.tblMaterialStamm);
            
            //objNacherf.LoadDB_ZLDRecordsetNach("X");
            Session["objNacherf"] = objNacherf;
            if (objNacherf.Status == 0)
            {
                objNacherf.LadeNacherfassungDB_ZLDNew("VZ");
                if (objNacherf.Status == 0)
                {
                    if (objNacherf.tblEingabeListe.Rows.Count > 0)
                    {
                        Session["objNacherf"] = objNacherf;
                        Response.Redirect("ChangeZLDNachVersandListe.aspx?AppID=" + Session["AppID"].ToString());
                    }
                    else
                    {
                        lblError.Text += "Keine Daten gefunden!" + objNacherf.Message;
                    }
                }
                else
                {
                    lblError.Text += objNacherf.Message;
                }
               
            }
            else
            {
                lblError.Text = objNacherf.Message;
            }
        }

        protected void txtStVa_TextChanged(object sender, EventArgs e)
        {
            objNacherf = (NacherfZLD)Session["objNacherf"];
            objNacherf.SelKreis = txtStVa.Text.ToUpper();
            ddlStVa.SelectedValue = txtStVa.Text.ToUpper();
            txtStVa.Text=txtStVa.Text.ToUpper();

            ddlStVa.Focus();
        }
    }
}
