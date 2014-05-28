using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using AppRemarketing.lib;
using CKG.Base.Kernel.Security;
using System.Configuration;

namespace AppRemarketing.forms
{
    public partial class Change01 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;

        private HaendlerSperrenPublic objSuche;

        private String FilterName = "";
        private String FilterPlz = "";
        private String FilterOrt = "";
        private String FilterNummer = "";



        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            
            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (!IsPostBack)
            {

                LoadHaendler();
            }
            else
            {

                if (Session["objNewHaendlerSuche"] != null)
                {
                    objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                }

            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {


             Searchoverddl();
            //if ((lbHaendler.SelectedIndex > -1 ))
            //{
                
            //}
            //else
            //{
            //    Search();
            //}
            
        }

        private void searchHaendlerInitial()
        {
            String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
            objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);


            objSuche.Haendlernr = txtNummerDetail.Text;
            objSuche.Kennung = "SPERR";
            objSuche.GetHaendler((string)Session["AppID"], (string)Session.SessionID, this);

            if (objSuche.Status != 0)
            {
                lblError.Visible = true;
                lblError.Text = objSuche.Message;
            }
            else
            {
                Session["objNewHaendlerSuche"] = objSuche;

            }        
        }


        private void Searchoverddl()
         {
             if (Session["objNewHaendlerSuche"] == null)
             {
                 String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                 objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                 Session["objNewHaendlerSuche"] = objSuche;
             }
                 objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                 objSuche.Debitor = lbHaendler.SelectedValue;
                 objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                 objSuche.IstGesperrt = false;


                 objSuche.Kennung = "SPERR";
                 objSuche.GetHaendler((string)Session["AppID"], (string)Session.SessionID, this);
                 


                 if (objSuche.HaendlerGesperrt !=null)
                 {
                     objSuche.HaendlerGesperrt.RowFilter = "Debitor = " + objSuche.Debitor;

                     if (objSuche.HaendlerGesperrt.Count > 0)
                     {
                         objSuche.IstGesperrt = true;
                     }


                     objSuche.HaendlerGesperrt.RowFilter = "";

                 }


                 

                 Session["objNewHaendlerSuche"] = objSuche;
                 Session["obj_SucheModus"] = "DropDown";
                 Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
         }

        private void LoadHaendler()
        {

            String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
            objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
            
            objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);

            if (objSuche.Status != 0)
            {
                lblError.Visible = true;
                lblError.Text = objSuche.Message;
                return;
            }
            lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
            //fillDropDown();
            Session["objNewHaendlerSuche"] = objSuche;

        }

        private void Search2()
        {



            try
            {
                if (objSuche.AnzahlHaendler == 0)
                {
                    lblMessage.Text = "keine Ergebnisse gefunden";

                    lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                    lbHaendler.Items.Clear();
                    tr_HaendlerAuswahl.Visible = false;
                }
                else if (objSuche.AnzahlHaendler > 0)
                {

                    if (objSuche.Haendler.Table.Rows.Count > 0)
                    {

                        objSuche.Haendler.RowFilter = "REFERENZ='" + objSuche.Haendlernr + "'";

                        if (objSuche.Haendler.Count == 0)
                        {

                            objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                            if (objSuche.Status != 0)
                            {
                                lblError.Visible = true;
                                lblError.Text = objSuche.Message;

                                if (objSuche.Status == 101)
                                {

                                    lblMessage.Text = "Keine Ergebnisse gefunden! Bitte wählen Sie einen Eintrag aus der Liste!";
                                    lblError.Text = "";
                                    objSuche.Haendlernr = "";
                                    objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                                    if (objSuche.Status != 0)
                                    {
                                        lblError.Visible = true;
                                        lblError.Text = objSuche.Message;
                                        return;

                                    }
                                    lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                                    fillDropDown();
                                    Session["objNewHaendlerSuche"] = objSuche;
                                }
                            }
                            else if (objSuche.Haendler.Count == 1)
                            {
                                Session["objNewHaendlerSuche"] = objSuche;
                                lblHalter.Text = objSuche.Haendlernr;
                                DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Haendlernr + "'");
                                objSuche.Debitor = dRow[0]["Debitor"].ToString();
                                objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                                if (objSuche.Status != 0)
                                {
                                    lblError.Visible = true;
                                    lblError.Text = objSuche.Message;
                                    return;
                                }
                                else
                                {
                                    Session["obj_SucheModus"] = "normal";
                                    Session["objNewHaendlerSuche"] = objSuche;
                                    Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
                                }

                            }
                            else if (objSuche.Haendler.Count == 0)
                            {
                                lblMessage.Text = "Keine Ergebnisse gefunden! Bitte wählen Sie einen Eintrag aus der Liste! ";
                                lblError.Text = "";
                                objSuche.Haendlernr = "";
                                objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                                lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                                fillDropDown();



                            }
                            else if (objSuche.Haendler.Count > 0)
                            {
                                //lblMessage.Text = "Keine Ergebnisse gefunden!Bitte wählen Sie einen Eintrag aus der Liste!";
                                if (objSuche.Status != 0)
                                {
                                    lblError.Visible = true;
                                    lblError.Text = objSuche.Message;
                                    return;

                                }
                                lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                                lblError.Text = "";
                                //fillDropDown();
                                tr_HaendlerAuswahl.Visible = true;
                                Session["objNewHaendlerSuche"] = objSuche;

                            }

                        }
                        else if (objSuche.Haendler.Count == 1)
                        {
                            objSuche.IstGesperrt = true;
                            DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Haendlernr + "'");
                            objSuche.Debitor = dRow[0]["Debitor"].ToString();
                            objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                            if (objSuche.Status != 0)
                            {
                                lblError.Visible = true;
                                lblError.Text = objSuche.Message;
                            }
                            else
                            {
                                Session["obj_SucheModus"] = "normal";
                                Session["objNewHaendlerSuche"] = objSuche;
                                Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
                                return;
                            }
                        }
                    }


                }
                else if (objSuche.Haendler.Count == 1)
                {
                    objSuche.IstGesperrt = true;
                    DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Haendlernr + "'");
                    objSuche.Debitor = dRow[0]["Debitor"].ToString();
                    objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                    if (objSuche.Status != 0)
                    {
                        lblError.Visible = true;
                        lblError.Text = objSuche.Message;
                    }
                    else
                    {
                        Session["objNewHaendlerSuche"] = objSuche;
                        Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Es ist ein Fehler aufgetreten: " + ex.Message;
            }
        }

        private void Search()
        {

            
            
            if (Session["objNewHaendlerSuche"] == null)
            {
                String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                objSuche = new HaendlerSperrenPublic(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                Session["objNewHaendlerSuche"] = objSuche;
                searchHaendlerInitial();
            }
            else
            {
                objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
                searchHaendlerInitial();
            }

            try
            {
                if (objSuche.AnzahlHaendler == 0)
                {
                    lblMessage.Text = "keine Ergebnisse gefunden";

                    lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                    lbHaendler.Items.Clear();
                    tr_HaendlerAuswahl.Visible = false;
                }
                else if (objSuche.AnzahlHaendler > 0)
                {

                    if (objSuche.Haendler.Table.Rows.Count > 0)
                    {

                        objSuche.Haendler.RowFilter = "REFERENZ='" + objSuche.Haendlernr + "'";

                        if (objSuche.Haendler.Count == 0)
                        {

                            objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                            if (objSuche.Status != 0)
                            {
                                lblError.Visible = true;
                                lblError.Text = objSuche.Message;

                                if (objSuche.Status == 101)
                                {

                                    lblMessage.Text = "Keine Ergebnisse gefunden! Bitte wählen Sie einen Eintrag aus der Liste!";
                                    lblError.Text = "";
                                    objSuche.Haendlernr = "";
                                    objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                                    if (objSuche.Status != 0)
                                    {
                                        lblError.Visible = true;
                                        lblError.Text = objSuche.Message;
                                        return;

                                    }
                                    lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                                    fillDropDown();
                                    Session["objNewHaendlerSuche"] = objSuche;
                                }
                            }
                            else if (objSuche.Haendler.Count == 1)
                                {
                                    Session["objNewHaendlerSuche"] = objSuche;
                                    lblHalter.Text = objSuche.Haendlernr;
                                    DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Haendlernr + "'");
                                    objSuche.Debitor = dRow[0]["Debitor"].ToString();
                                    objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                                    if (objSuche.Status != 0)
                                    {
                                        lblError.Visible = true;
                                        lblError.Text = objSuche.Message;
                                        return;
                                    }
                                    else
                                    {
                                        Session["obj_SucheModus"] = "normal";
                                        Session["objNewHaendlerSuche"] = objSuche;
                                        Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
                                    }

                                }
                                else if (objSuche.Haendler.Count == 0)
                                {
                                    lblMessage.Text = "Keine Ergebnisse gefunden! Bitte wählen Sie einen Eintrag aus der Liste! ";
                                    lblError.Text = "";
                                    objSuche.Haendlernr = "";
                                    objSuche.GetHaendlerUngesperrt((string)Session["AppID"], (string)Session.SessionID, this);
                                    lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                                    fillDropDown();

                                    

                                }
                                else if (objSuche.Haendler.Count > 0)
                                {
                                    //lblMessage.Text = "Keine Ergebnisse gefunden!Bitte wählen Sie einen Eintrag aus der Liste!";
                                    if (objSuche.Status != 0)
                                    {
                                        lblError.Visible = true;
                                        lblError.Text = objSuche.Message;
                                        return;

                                    }
                                    lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
                                    lblError.Text = "";
                                    //fillDropDown();
                                    tr_HaendlerAuswahl.Visible = true;
                                    Session["objNewHaendlerSuche"] = objSuche;
                                    
                                }

                            }
                            else if (objSuche.Haendler.Count == 1)
                            {
                                objSuche.IstGesperrt = true;
                                DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Haendlernr + "'");
                                objSuche.Debitor = dRow[0]["Debitor"].ToString();
                                objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                                if (objSuche.Status != 0)
                                {
                                    lblError.Visible = true;
                                    lblError.Text = objSuche.Message;
                                }
                                else
                                {
                                    Session["obj_SucheModus"] = "normal";
                                    Session["objNewHaendlerSuche"] = objSuche;
                                    Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
                                    return;
                                }
                            }
                        }


                    }
                    else if (objSuche.Haendler.Count == 1)
                    {
                        objSuche.IstGesperrt = true;
                        DataRow[] dRow = objSuche.Haendler.Table.Select("REFERENZ='" + objSuche.Haendlernr + "'");
                                objSuche.Debitor = dRow[0]["Debitor"].ToString();
                        objSuche.GetVersAnf((string)Session["AppID"], (string)Session.SessionID, this);
                        if (objSuche.Status != 0)
                        {
                            lblError.Visible = true;
                            lblError.Text = objSuche.Message;
                        }
                        else
                        {
                            Session["objNewHaendlerSuche"] = objSuche;
                            Response.Redirect("Change01_2.aspx?AppID=" + (string)Session["AppID"]);
                            return;
                        }
                    }
                }
             catch (Exception ex)
            {
                lblError.Text = "Es ist ein Fehler aufgetreten: " + ex.Message;
            }
        }

        private void fillDropDown ()
        {
            tr_HaendlerAuswahl.Visible = true;
            objSuche.Haendler.Sort = "Name1 asc";
            lbHaendler.DataSource = objSuche.Haendler;
            lbHaendler.DataTextField = "Adresse";
            lbHaendler.DataValueField = "Debitor";
            lbHaendler.DataBind();
            
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lbHaendler_SelectedIndexChanged(object sender, EventArgs e)
        {
            objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];
            DataRow[] selRow = objSuche.Haendler.Table.Select("Debitor = '" + lbHaendler.SelectedValue + "'");
            String strHalter = selRow[0]["Referenz"].ToString();
            lblHaendlerName1.Text = selRow[0]["NAME1"].ToString();
            lblHaendlerName2.Text = selRow[0]["NAME2"].ToString();
            lblHaendlerStrasse.Text = selRow[0]["STRAS"].ToString();
            lblHaendlerPLZ.Text = selRow[0]["PSTLZ"].ToString();
            lblHaendlerOrt.Text = selRow[0]["ORT01"].ToString();
            lblHalter.Text=strHalter.TrimStart('0');

         }

        protected void lbSelektionZurueckSetzen_Click(object sender, EventArgs e)
        {
            Session["objNewHaendlerSuche"] = null;
            lbHaendler.Items.Clear();
            lblHaendlerName1.Text = "";
            lblHaendlerName2.Text = "";
            lblHaendlerStrasse.Text = "";
            lblHaendlerPLZ.Text = "";
            lblHaendlerOrt.Text ="";
            lblHalter.Text = "";
            tr_HaendlerAuswahl.Visible = false;
            trHaendlernummer.Visible = true;
            txtNummerDetail.Text = "";
            lblMessage.Text = "";
            lblError.Text = "";
        }



        private void Search(String Filter)
        {

            if (Session["objNewHaendlerSuche"] != null)
            {
                objSuche = (HaendlerSperrenPublic)Session["objNewHaendlerSuche"];

                Filter = "";
                if (FilterNummer.Length > 0)
                {
                    Filter = FilterNummer;
                    if (FilterName.Length > 0)
                    { Filter += " AND " + FilterName; }
                    if (FilterPlz.Length > 0)
                    { Filter += " AND " + FilterPlz; }
                    if (FilterOrt.Length > 0)
                    { Filter += " AND " + FilterOrt; }
                }
                else if (FilterName.Length > 0)
                {
                    Filter = FilterName;
                    if (FilterPlz.Length > 0)
                    {
                        Filter += " AND " + FilterPlz;
                        if (FilterOrt.Length > 0)
                        {
                            Filter += " AND " + FilterOrt;
                        }
                    }
                    else if (FilterOrt.Length > 0)
                    {
                        Filter += " AND " + FilterOrt;
                    }
                }
                else if (FilterPlz.Length > 0)
                {
                    Filter = FilterPlz;
                    if (FilterOrt.Length > 0)
                    {
                        Filter += " AND " + FilterOrt;
                    }
                }
                else if (FilterOrt.Length > 0)
                {
                    Filter = FilterOrt;
                }

                objSuche.Haendler.RowFilter = Filter;
                if (objSuche.Haendler.Count == 1)
                {
                    Session["obj_SucheModus"] = "normal";
                    Session["objNewHaendlerSuche"] = objSuche;
                    fillDropDown();
                    lbHaendler.SelectedValue = objSuche.Haendler[0]["Debitor"].ToString();
                    String strHalter = objSuche.Haendler[0]["Referenz"].ToString();
                    lblHaendlerName1.Text = objSuche.Haendler[0]["NAME1"].ToString();
                    lblHaendlerName2.Text = objSuche.Haendler[0]["NAME2"].ToString();
                    lblHaendlerStrasse.Text = objSuche.Haendler[0]["STRAS"].ToString();
                    lblHaendlerPLZ.Text = objSuche.Haendler[0]["PSTLZ"].ToString();
                    lblHaendlerOrt.Text = objSuche.Haendler[0]["ORT01"].ToString();
                    lblHalter.Text = strHalter.TrimStart('0');
                    lblErgebnissAnzahl.Text = "1";
                    cmdSearch.Visible = true;
                }
                else if (objSuche.Haendler.Count == 0)
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    lblError.Visible = true;
                    lblError.Text = "Es wurden keine Ergebnisse gefunden, bitte überprüfen Sie Ihre Eingaben!";
                    lblErgebnissAnzahl.Text = "0";

                }
                else if (objSuche.Haendler.Count <= 30)
                {
                    Session["obj_SucheModus"] = "normal";
                    Session["objNewHaendlerSuche"] = objSuche;
                    fillDropDown();
                    lblError.Visible = true;
                    lblError.Text = "Es wurden mehrere Ergebnisse gefunden, bitte wählen Sie!";
                    lblErgebnissAnzahl.Text = objSuche.Haendler.Count.ToString();
                }
                else
                {
                    Session["obj_SucheModus"] = "DropDown";
                    Session["objNewHaendlerSuche"] = objSuche;
                    lblError.Visible = true;
                    lblError.Text = "Es wurden mehr als 30 Ergebnisse gefunden, bitte versuchen Sie die Ergebnisse weiter einzuschränken!";
                    lblErgebnissAnzahl.Text = objSuche.Haendler.Count.ToString();
                }
            }
            else
            {
                Session["obj_SucheModus"] = "normal";
                trName1.Visible = false;
                trPLz.Visible = false;
                trOrt.Visible = false;
                //trHaendlerAuswahl.Visible = false;
                //trSelectionButton.Visible = false;
                //trNummerDetail.Visible = false;
            }
        }



        protected void txtNummerDetail_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            if (txtNummerDetail.Text.Length > 0)
            {
                strFilter = txtNummerDetail.Text.Replace("*", "%");
                FilterNummer = "Referenz LIKE '" + strFilter + "'";
                if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
                if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
                if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
                Search(""); 
            }
        }

        protected void txtName1_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            if (txtName1.Text.Length > 0)
            {
                strFilter = txtName1.Text.Replace("*", "%");
                FilterName = "Adresse LIKE '" + strFilter + "'";
                if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
                if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
                if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
                Search(""); 
            }
        }

        protected void txtPLZ_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            strFilter = txtPLZ.Text.Replace("*", "%");
            if (txtPLZ.Text.Length > 0)
            {
                FilterPlz = "PSTLZ LIKE '" + strFilter + "'";
                if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
                if (txtOrt.Text.Length > 0) { FilterOrt = "ORT01 LIKE '" + txtOrt.Text.Replace("*", "%") + "'"; }
                if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
                Search("");  
            }
        }

        protected void txtOrt_TextChanged(object sender, EventArgs e)
        {
            String strFilter;
            if (txtOrt.Text.Length > 0)
            {
                strFilter = txtOrt.Text.Replace("*", "%");
                FilterOrt = "ORT01 LIKE '" + strFilter + "'";
                if (txtName1.Text.Length > 0) { FilterName = "Adresse LIKE '" + txtName1.Text.Replace("*", "%") + "'"; }
                if (txtPLZ.Text.Length > 0) { FilterPlz = "PSTLZ LIKE '" + txtPLZ.Text.Replace("*", "%") + "'"; }
                if (txtNummerDetail.Text.Length > 0) { FilterNummer = "Referenz LIKE '" + txtNummerDetail.Text.Replace("*", "%") + "'"; }
                Search(""); 
            }
        }

        protected void lbSelektionZurueckSetzen_Click1(object sender, EventArgs e)
        {
            lbHaendler.Items.Clear();
            lblHaendlerName1.Text = "";
            lblHaendlerName2.Text = "";
            lblHaendlerStrasse.Text = "";
            lblHaendlerPLZ.Text = "";
            lblHaendlerOrt.Text = "";
            lblHalter.Text = "";
            txtNummerDetail.Text = "";
            txtPLZ.Text = "";
            txtOrt.Text = "";
            txtName1.Text = "";
            lblMessage.Text = "";
            lblError.Text = "";
            lblErgebnissAnzahl.Text = objSuche.AnzahlHaendler.ToString();
            objSuche.Kennung = "";
            Session["objNewHaendlerSuche"] = objSuche;
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }


    }
}
