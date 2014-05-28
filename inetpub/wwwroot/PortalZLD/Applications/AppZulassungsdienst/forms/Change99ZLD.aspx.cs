using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Pflege der Dokumentenanforderung der Zulassungsstellen.
    /// Selektion, Ausgabe und Pflege.
    /// </summary>
    public partial class Change99ZLD : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private Report99 ChangeAnforderungen;

        /// <summary>
        /// Page_Load-Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); //erzeugt ein App_objekt 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Request.QueryString["AppID"].Length > 0)
            {
                Session["AppID"] = Request.QueryString["AppID"];
            }
            txtKennzeichen.Attributes.Add("onkeyup", "FilterKennz(this,event)");
        }
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblMessage.Text = "";
            ClearForm();
            txtKennzeichen.Text = txtKennzeichen.Text.Replace(" ", "");
            if (txtKennzeichen.Text != "")
            {
                DoSubmit();
            }
            else
            {
                lblError.Text = "Bitte ein Ortskennzeichen eingeben.";
            }
        }


        private void ClearForm()
        {
            foreach (HtmlTableRow TableRow in tblData.Rows)
            {
                foreach (HtmlTableCell TableCell in TableRow.Cells)
                {
                    foreach (Control item in TableCell.Controls)
                    {
                        if (item.GetType() == typeof(TextBox))
                        {
                            TextBox txt = (TextBox)item;
                            txt.Text = "";
                        }
                    }         
                }        
            }

            cmdSave.Visible = false;


        }

        /// <summary>
        /// Vorhandene Daten aus SAP selektieren.
        /// </summary>
        private void DoSubmit()
        {

            lblError.Text = "";
            ChangeAnforderungen = new Report99(ref m_User, m_App, "");
            ChangeAnforderungen.PKennzeichen = txtKennzeichen.Text;


            ChangeAnforderungen.Fill(Session["AppID"].ToString(), Session.SessionID, this);

            Session["ChangeAnforderungen"] = ChangeAnforderungen;


            if (ChangeAnforderungen.Status != 0)
            {
                lblError.Text = "Fehler: " + ChangeAnforderungen.Message;
            }
            else
            {
                if (ChangeAnforderungen.Result.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                    txtKennzeichen.Text = "";
                }
                else
                {
                    lblKennz.Text = txtKennzeichen.Text.Trim();
                    FillForm();
                    lblError.Visible = false;
                    //lblEingabe.Visible = false;
                }
            }

        }
        /// <summary>
        /// Controls mit selektierten Daten füllen. 
        /// </summary>
        private void FillForm()
        {
            ChangeAnforderungen = (Report99)(Session["ChangeAnforderungen"]);

            DataRow[] SelectRow;
            DataRow resultRow;

            if (ChangeAnforderungen.Result.Rows.Count == 1)
            {
                resultRow = ChangeAnforderungen.Result.Rows[0];
            }
            else
            {
                SelectRow = ChangeAnforderungen.Result.Select("Zkba2='00'");
                if (SelectRow.Length > 0)
                {
                    resultRow = SelectRow[0];
                }
                else
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                    return;
                }
            }


            Result.Visible = true;
            cmdSave.Visible = true;
            //Privat Zulassung
            txtPZUL_BRIEF.Text = resultRow["PZUL_BRIEF"].ToString();
            txtPZUL_SCHEIN.Text = resultRow["PZUL_SCHEIN"].ToString();
            txtPZUL_COC.Text = resultRow["PZUL_COC"].ToString();
            txtPZUL_DECK.Text = resultRow["PZUL_DECK"].ToString();
            txtPZUL_VOLLM.Text = resultRow["PZUL_VOLLM"].ToString();
            txtPZUL_AUSW.Text = resultRow["PZUL_AUSW"].ToString();
            txtPZUL_GEWERB.Text = resultRow["PZUL_GEWERB"].ToString();
            txtPZUL_HANDEL.Text = resultRow["PZUL_HANDEL"].ToString();
            txtPZUL_LAST.Text = resultRow["PZUL_LAST"].ToString();
            txtPZUL_BEM.Text = resultRow["PZUL_BEM"].ToString();

            //Privat Umschreibung

            txtPUMSCHR_SCHEIN.Text = resultRow["PUMSCHR_SCHEIN"].ToString();
            txtPUMSCHR_BRIEF.Text = resultRow["PUMSCHR_BRIEF"].ToString();
            txtPUMSCHR_COC.Text = resultRow["PUMSCHR_COC"].ToString();
            txtPUMSCHR_DECK.Text = resultRow["PUMSCHR_DECK"].ToString();
            txtPUMSCHR_VOLLM.Text = resultRow["PUMSCHR_VOLLM"].ToString();
            txtPUMSCHR_AUSW.Text = resultRow["PUMSCHR_AUSW"].ToString();
            txtPUMSCHR_GEWERB.Text = resultRow["PUMSCHR_GEWERB"].ToString();
            txtPUMSCHR_HANDEL.Text = resultRow["PUMSCHR_HANDEL"].ToString();
            txtPUMSCHR_LAST.Text = resultRow["PUMSCHR_LAST"].ToString();
            txtPUMSCHR_BEM.Text = resultRow["PUMSCHR_BEM"].ToString();

            //Privat Umkennzeichnung
            txtPUMK_BRIEF.Text = resultRow["PUMK_BRIEF"].ToString();
            txtPUMK_SCHEIN.Text = resultRow["PUMK_SCHEIN"].ToString();
            txtPUMK_COC.Text = resultRow["PUMK_COC"].ToString();
            txtPUMK_DECK.Text = resultRow["PUMK_DECK"].ToString();
            txtPUMK_VOLLM.Text = resultRow["PUMK_VOLLM"].ToString();
            txtPUMK_AUSW.Text = resultRow["PUMK_AUSW"].ToString();
            txtPUMK_GEWERB.Text = resultRow["PUMK_GEWERB"].ToString();
            txtPUMK_HANDEL.Text = resultRow["PUMK_HANDEL"].ToString();
            txtPUMK_LAST.Text = resultRow["PUMK_LAST"].ToString();
            txtPUMK_BEM.Text = resultRow["PUMK_BEM"].ToString();

            //Privat Ersatzfahrzeugschein
            txtPERS_BRIEF.Text = resultRow["PERS_BRIEF"].ToString();
            txtPERS_SCHEIN.Text = resultRow["PERS_SCHEIN"].ToString();
            txtPERS_COC.Text = resultRow["PERS_COC"].ToString();
            txtPERS_DECK.Text = resultRow["PERS_DECK"].ToString();
            txtPERS_VOLLM.Text = resultRow["PERS_VOLLM"].ToString();
            txtPERS_AUSW.Text = resultRow["PERS_AUSW"].ToString();
            txtPERS_GEWERB.Text = resultRow["PERS_GEWERB"].ToString();
            txtPERS_HANDEL.Text = resultRow["PERS_HANDEL"].ToString();
            txtPERS_LAST.Text = resultRow["PERS_LAST"].ToString();
            txtPERS_BEM.Text = resultRow["PERS_BEM"].ToString();

            //Unternehmen Zulassung
            txtUZUL_BRIEF.Text = resultRow["UZUL_BRIEF"].ToString();
            txtUZUL_SCHEIN.Text = resultRow["UZUL_SCHEIN"].ToString();
            txtUZUL_COC.Text = resultRow["UZUL_COC"].ToString();
            txtUZUL_DECK.Text = resultRow["UZUL_DECK"].ToString();
            txtUZUL_VOLLM.Text = resultRow["UZUL_VOLLM"].ToString();
            txtUZUL_AUSW.Text = resultRow["UZUL_AUSW"].ToString();
            txtUZUL_GEWERB.Text = resultRow["UZUL_GEWERB"].ToString();
            txtUZUL_HANDEL.Text = resultRow["UZUL_HANDEL"].ToString();
            txtUZUL_LAST.Text = resultRow["UZUL_LAST"].ToString();
            txtUZUL_BEM.Text = resultRow["UZUL_BEM"].ToString();

            //Unternehmen Umschreibung
            txtUUMSCHR_BRIEF.Text = resultRow["UUMSCHR_BRIEF"].ToString();
            txtUUMSCHR_SCHEIN.Text = resultRow["UUMSCHR_SCHEIN"].ToString();
            txtUUMSCHR_COC.Text = resultRow["UUMSCHR_COC"].ToString();
            txtUUMSCHR_DECK.Text = resultRow["UUMSCHR_DECK"].ToString();
            txtUUMSCHR_VOLLM.Text = resultRow["UUMSCHR_VOLLM"].ToString();
            txtUUMSCHR_AUSW.Text = resultRow["UUMSCHR_AUSW"].ToString();
            txtUUMSCHR_GEWERB.Text = resultRow["UUMSCHR_GEWERB"].ToString();
            txtUUMSCHR_HANDEL.Text = resultRow["UUMSCHR_HANDEL"].ToString();
            txtUUMSCHR_LAST.Text = resultRow["UUMSCHR_LAST"].ToString();
            txtUUMSCHR_BEM.Text = resultRow["UUMSCHR_BEM"].ToString();
            //Unternehmen Umkennzeichnung
            txtUUMK_BRIEF.Text = resultRow["UUMK_BRIEF"].ToString();
            txtUUMK_SCHEIN.Text = resultRow["UUMK_SCHEIN"].ToString();
            txtUUMK_COC.Text = resultRow["UUMK_COC"].ToString();
            txtUUMK_DECK.Text = resultRow["UUMK_DECK"].ToString();
            txtUUMK_VOLLM.Text = resultRow["UUMK_VOLLM"].ToString();
            txtUUMK_AUSW.Text = resultRow["UUMK_AUSW"].ToString();
            txtUUMK_GEWERB.Text = resultRow["UUMK_GEWERB"].ToString();
            txtUUMK_HANDEL.Text = resultRow["UUMK_HANDEL"].ToString();
            txtUUMK_LAST.Text = resultRow["UUMK_LAST"].ToString();
            txtUUMK_BEM.Text = resultRow["UUMK_BEM"].ToString();

            //Unternehmen Ersatzfahrzeugschein
            txtUERS_BRIEF.Text = resultRow["UERS_BRIEF"].ToString();
            txtUERS_SCHEIN.Text = resultRow["UERS_SCHEIN"].ToString();
            txtUERS_COC.Text = resultRow["UERS_COC"].ToString();
            txtUERS_DECK.Text = resultRow["UERS_DECK"].ToString();
            txtUERS_VOLLM.Text = resultRow["UERS_VOLLM"].ToString();
            txtUERS_AUSW.Text = resultRow["UERS_AUSW"].ToString();
            txtUERS_GEWERB.Text = resultRow["UERS_GEWERB"].ToString();
            txtUERS_HANDEL.Text = resultRow["UERS_HANDEL"].ToString();
            txtUERS_LAST.Text = resultRow["UERS_LAST"].ToString();
            txtUERS_BEM.Text = resultRow["UERS_BEM"].ToString();


            txtAmt.Text = resultRow["STVALN"].ToString();
            txtWunsch.Text = resultRow["URL"].ToString();
            txtFormular.Text = resultRow["STVALNFORM"].ToString();
            txtGeb.Text = resultRow["STVALNGEB"].ToString();

        }

        /// <summary>
        /// Daten sammeln und in SAP speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            ChangeAnforderungen = (Report99)(Session["ChangeAnforderungen"]);


            lblMessage.Text = "";
            lblError.Text = "";


            if (lblKennz.Text.Trim().Length > 0)
            {
                DataTable tblData = new DataTable(); ;
                CreateTable(ref tblData);

                DataRow resultRow = tblData.NewRow();

                DataRow[] SelectRow;
                DataRow OldRow;

                if (ChangeAnforderungen.Result.Rows.Count == 1)
                {
                    OldRow = ChangeAnforderungen.Result.Rows[0];
                }
                else
                {
                    SelectRow = ChangeAnforderungen.Result.Select("Zkba2='00'");
                    OldRow = SelectRow[0];
                }
                resultRow["MANDT"] = OldRow["MANDT"];
                resultRow["ZKBA1"] = OldRow["ZKBA1"];
                resultRow["ZKBA2"] = OldRow["ZKBA2"];
                resultRow["AENAM"] = m_User.UserName.PadLeft(12);
                resultRow["AEDAT"] = System.DateTime.Now.ToShortDateString();

                //Privat Zulassung

                resultRow["PZUL_BRIEF"] = txtPZUL_BRIEF.Text;
                resultRow["PZUL_SCHEIN"] = txtPZUL_SCHEIN.Text;
                resultRow["PZUL_COC"] = txtPZUL_COC.Text;
                resultRow["PZUL_DECK"] = txtPZUL_DECK.Text;
                resultRow["PZUL_VOLLM"] = txtPZUL_VOLLM.Text;
                resultRow["PZUL_AUSW"] = txtPZUL_AUSW.Text;
                resultRow["PZUL_GEWERB"] = txtPZUL_GEWERB.Text;
                resultRow["PZUL_HANDEL"] = txtPZUL_HANDEL.Text;
                resultRow["PZUL_LAST"] = txtPZUL_LAST.Text;
                resultRow["PZUL_BEM"] = txtPZUL_BEM.Text;

                //Privat Umschreibung

                resultRow["PUMSCHR_SCHEIN"] = txtPUMSCHR_SCHEIN.Text;
                resultRow["PUMSCHR_BRIEF"] = txtPUMSCHR_BRIEF.Text;
                resultRow["PUMSCHR_COC"] = txtPUMSCHR_COC.Text;
                resultRow["PUMSCHR_DECK"] = txtPUMSCHR_DECK.Text;
                resultRow["PUMSCHR_VOLLM"] = txtPUMSCHR_VOLLM.Text;
                resultRow["PUMSCHR_AUSW"] = txtPUMSCHR_AUSW.Text;
                resultRow["PUMSCHR_GEWERB"] = txtPUMSCHR_GEWERB.Text;
                resultRow["PUMSCHR_HANDEL"] = txtPUMSCHR_HANDEL.Text;
                resultRow["PUMSCHR_LAST"] = txtPUMSCHR_LAST.Text;
                resultRow["PUMSCHR_BEM"] = txtPUMSCHR_BEM.Text;

                //Privat Umkennzeichnung
                resultRow["PUMK_BRIEF"] = txtPUMK_BRIEF.Text;
                resultRow["PUMK_SCHEIN"] = txtPUMK_SCHEIN.Text;
                resultRow["PUMK_COC"] = txtPUMK_COC.Text;
                resultRow["PUMK_DECK"] = txtPUMK_DECK.Text;
                resultRow["PUMK_VOLLM"] = txtPUMK_VOLLM.Text;
                resultRow["PUMK_AUSW"] = txtPUMK_AUSW.Text;
                resultRow["PUMK_GEWERB"] = txtPUMK_GEWERB.Text;
                resultRow["PUMK_HANDEL"] = txtPUMK_HANDEL.Text;
                resultRow["PUMK_LAST"] = txtPUMK_LAST.Text;
                resultRow["PUMK_BEM"] = txtPUMK_BEM.Text;

                //Privat Ersatzfahrzeugschein
                resultRow["PERS_BRIEF"] = txtPERS_BRIEF.Text;
                resultRow["PERS_SCHEIN"] = txtPERS_SCHEIN.Text;
                resultRow["PERS_COC"] = txtPERS_COC.Text;
                resultRow["PERS_DECK"] = txtPERS_DECK.Text;
                resultRow["PERS_VOLLM"] = txtPERS_VOLLM.Text;
                resultRow["PERS_AUSW"] = txtPERS_AUSW.Text;
                resultRow["PERS_GEWERB"] = txtPERS_GEWERB.Text;
                resultRow["PERS_HANDEL"] = txtPERS_HANDEL.Text;
                resultRow["PERS_LAST"] = txtPERS_LAST.Text;
                resultRow["PERS_BEM"] = txtPERS_BEM.Text;

                //Unternehmen Zulassung
                resultRow["UZUL_BRIEF"] = txtUZUL_BRIEF.Text;
                resultRow["UZUL_SCHEIN"] = txtUZUL_SCHEIN.Text;
                resultRow["UZUL_COC"] = txtUZUL_COC.Text;
                resultRow["UZUL_DECK"] = txtUZUL_DECK.Text;
                resultRow["UZUL_VOLLM"] = txtUZUL_VOLLM.Text;
                resultRow["UZUL_AUSW"] = txtUZUL_AUSW.Text;
                resultRow["UZUL_GEWERB"] = txtUZUL_GEWERB.Text;
                resultRow["UZUL_HANDEL"] = txtUZUL_HANDEL.Text;
                resultRow["UZUL_LAST"] = txtUZUL_LAST.Text;
                resultRow["UZUL_BEM"] = txtUZUL_BEM.Text;

                //Unternehmen Umschreibung
                resultRow["UUMSCHR_BRIEF"] = txtUUMSCHR_BRIEF.Text;
                resultRow["UUMSCHR_SCHEIN"] = txtUUMSCHR_SCHEIN.Text;
                resultRow["UUMSCHR_COC"] = txtUUMSCHR_COC.Text;
                resultRow["UUMSCHR_DECK"] = txtUUMSCHR_DECK.Text;
                resultRow["UUMSCHR_VOLLM"] = txtUUMSCHR_VOLLM.Text;
                resultRow["UUMSCHR_AUSW"] = txtUUMSCHR_AUSW.Text;
                resultRow["UUMSCHR_GEWERB"] = txtUUMSCHR_GEWERB.Text;
                resultRow["UUMSCHR_HANDEL"] = txtUUMSCHR_HANDEL.Text;
                resultRow["UUMSCHR_LAST"] = txtUUMSCHR_LAST.Text;
                resultRow["UUMSCHR_BEM"] = txtUUMSCHR_BEM.Text;

                //Unternehmen Umkennzeichnung
                resultRow["UUMK_BRIEF"] = txtUUMK_BRIEF.Text;
                resultRow["UUMK_SCHEIN"] = txtUUMK_SCHEIN.Text;
                resultRow["UUMK_COC"] = txtUUMK_COC.Text;
                resultRow["UUMK_DECK"] = txtUUMK_DECK.Text;
                resultRow["UUMK_VOLLM"] = txtUUMK_VOLLM.Text;
                resultRow["UUMK_AUSW"] = txtUUMK_AUSW.Text;
                resultRow["UUMK_GEWERB"] = txtUUMK_GEWERB.Text;
                resultRow["UUMK_HANDEL"] = txtUUMK_HANDEL.Text;
                resultRow["UUMK_LAST"] = txtUUMK_LAST.Text;
                resultRow["UUMK_BEM"] = txtUUMK_BEM.Text;

                //Unternehmen Ersatzfahrzeugschein
                resultRow["UERS_BRIEF"] = txtUERS_BRIEF.Text;
                resultRow["UERS_SCHEIN"] = txtUERS_SCHEIN.Text;
                resultRow["UERS_COC"] = txtUERS_COC.Text;
                resultRow["UERS_DECK"] = txtUERS_DECK.Text;
                resultRow["UERS_VOLLM"] = txtUERS_VOLLM.Text;
                resultRow["UERS_AUSW"] = txtUERS_AUSW.Text;
                resultRow["UERS_GEWERB"] = txtUERS_GEWERB.Text;
                resultRow["UERS_HANDEL"] = txtUERS_HANDEL.Text;
                resultRow["UERS_LAST"] = txtUERS_LAST.Text;
                resultRow["UERS_BEM"] = txtUERS_BEM.Text;

                resultRow["STVALN"] = txtAmt.Text;
                resultRow["URL"] = txtWunsch.Text;
                resultRow["STVALNFORM"] = txtFormular.Text;
                resultRow["STVALNGEB"] = txtGeb.Text;

                tblData.Rows.Add(resultRow);

                ChangeAnforderungen.Change(Session["AppID"].ToString(), Session.SessionID, this, tblData);

                if (ChangeAnforderungen.Status != 0)
                {
                    lblError.Text = "Fehler: " + ChangeAnforderungen.Message;
                }
                else
                {
                    lblMessage.Text = "Daten erfolgreich gespeichert!";
                    ClearForm();
                    Result.Visible =false;
                    cmdSave.Visible = false;
                    lblKennz.Text = "";
                }
            }
            else 
            {
                lblError.Text = "Fehler: " + ChangeAnforderungen.Message;
            }
        }

        /// <summary>
        /// Weiterleitung zum Verkehrsamtes des selektierten Kreises.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnAmt_Click(object sender, ImageClickEventArgs e)
        {
            String sUrl=txtAmt.Text.Trim();
            ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");
        }

        /// <summary>
        /// Weiterleitung zur Wunschkennzeichenseite des Verkehrsamtes des selektierten Kreises.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnWunsch_Click(object sender, ImageClickEventArgs e)
        {
            String sUrl = txtWunsch.Text.Trim();
            ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");
        }
        /// <summary>
        /// Weiterleitung zur Formulardownloadseite des Verkehrsamtes des selektierten Kreises.         
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnFormular_Click(object sender, ImageClickEventArgs e)
        {
            String sUrl = txtFormular.Text.Trim();
            ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");
        }
        /// <summary>
        /// Weiterleitung zur Gebührenseite des Verkehrsamtes des selektierten Kreises.         
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void ibtnGeb_Click(object sender, ImageClickEventArgs e)
        {
            String sUrl = txtGeb.Text.Trim();
            ResponseHelper.Redirect(sUrl, "_blank", "left=0,top=0,resizable=YES,scrollbars=YES,menubar=YES,resizable=yes,scrollbars=YES,status=YES,toolbar=YES");

        }
        /// <summary>
        /// Tabelle für die Speicherung erstellen.
        /// </summary>
        /// <param name="tblData">Tabelle Dokumente</param>
        private void CreateTable(ref DataTable tblData)
        {
            
            tblData.Columns.Add("MANDT", typeof(String));
            tblData.Columns.Add("ZKBA1", typeof(String));
            tblData.Columns.Add("ZKBA2", typeof(String));
            tblData.Columns.Add("AEDAT", typeof(String));
            tblData.Columns.Add("AENAM", typeof(String));
            tblData.Columns.Add("PZUL_BRIEF", typeof(String));
            tblData.Columns.Add("PUMSCHR_BRIEF", typeof(String));
            tblData.Columns.Add("PUMK_BRIEF", typeof(String));
            tblData.Columns.Add("PERS_BRIEF", typeof(String));
            tblData.Columns.Add("UZUL_BRIEF", typeof(String));
            tblData.Columns.Add("UUMSCHR_BRIEF", typeof(String));
            tblData.Columns.Add("UUMK_BRIEF", typeof(String));
            tblData.Columns.Add("UERS_BRIEF", typeof(String));
            tblData.Columns.Add("PZUL_SCHEIN", typeof(String));
            tblData.Columns.Add("PUMSCHR_SCHEIN", typeof(String));
            tblData.Columns.Add("PUMK_SCHEIN", typeof(String));
            tblData.Columns.Add("PERS_SCHEIN", typeof(String));
            tblData.Columns.Add("UZUL_SCHEIN", typeof(String));
            tblData.Columns.Add("UUMSCHR_SCHEIN", typeof(String));
            tblData.Columns.Add("UUMK_SCHEIN", typeof(String));
            tblData.Columns.Add("UERS_SCHEIN", typeof(String));
            tblData.Columns.Add("PZUL_COC", typeof(String));
            tblData.Columns.Add("PUMSCHR_COC", typeof(String));
            tblData.Columns.Add("PUMK_COC", typeof(String));
            tblData.Columns.Add("PERS_COC", typeof(String));
            tblData.Columns.Add("UZUL_COC", typeof(String));
            tblData.Columns.Add("UUMSCHR_COC", typeof(String));
            tblData.Columns.Add("UUMK_COC", typeof(String));
            tblData.Columns.Add("UERS_COC", typeof(String));
            tblData.Columns.Add("PZUL_DECK", typeof(String));
            tblData.Columns.Add("PUMSCHR_DECK", typeof(String));
            tblData.Columns.Add("PUMK_DECK", typeof(String));
            tblData.Columns.Add("PERS_DECK", typeof(String));
            tblData.Columns.Add("UZUL_DECK", typeof(String));
            tblData.Columns.Add("UUMSCHR_DECK", typeof(String));
            tblData.Columns.Add("UUMK_DECK", typeof(String));
            tblData.Columns.Add("UERS_DECK", typeof(String));
            tblData.Columns.Add("PZUL_VOLLM", typeof(String));
            tblData.Columns.Add("PUMSCHR_VOLLM", typeof(String));
            tblData.Columns.Add("PUMK_VOLLM", typeof(String));
            tblData.Columns.Add("PERS_VOLLM", typeof(String));
            tblData.Columns.Add("UZUL_VOLLM", typeof(String));
            tblData.Columns.Add("UUMSCHR_VOLLM", typeof(String));
            tblData.Columns.Add("UUMK_VOLLM", typeof(String));
            tblData.Columns.Add("UERS_VOLLM", typeof(String));
            tblData.Columns.Add("PZUL_AUSW", typeof(String));
            tblData.Columns.Add("PUMSCHR_AUSW", typeof(String));
            tblData.Columns.Add("PUMK_AUSW", typeof(String));
            tblData.Columns.Add("PERS_AUSW", typeof(String));
            tblData.Columns.Add("UZUL_AUSW", typeof(String));
            tblData.Columns.Add("UUMSCHR_AUSW", typeof(String));
            tblData.Columns.Add("UUMK_AUSW", typeof(String));
            tblData.Columns.Add("UERS_AUSW", typeof(String));
            tblData.Columns.Add("PZUL_GEWERB", typeof(String));
            tblData.Columns.Add("PUMSCHR_GEWERB", typeof(String));
            tblData.Columns.Add("PUMK_GEWERB", typeof(String));
            tblData.Columns.Add("PERS_GEWERB", typeof(String));
            tblData.Columns.Add("UZUL_GEWERB", typeof(String));
            tblData.Columns.Add("UUMSCHR_GEWERB", typeof(String));
            tblData.Columns.Add("UUMK_GEWERB", typeof(String));
            tblData.Columns.Add("UERS_GEWERB", typeof(String));
            tblData.Columns.Add("PZUL_HANDEL", typeof(String));
            tblData.Columns.Add("PUMSCHR_HANDEL", typeof(String));
            tblData.Columns.Add("PUMK_HANDEL", typeof(String));
            tblData.Columns.Add("PERS_HANDEL", typeof(String));
            tblData.Columns.Add("UZUL_HANDEL", typeof(String));
            tblData.Columns.Add("UUMSCHR_HANDEL", typeof(String));
            tblData.Columns.Add("UUMK_HANDEL", typeof(String));
            tblData.Columns.Add("UERS_HANDEL", typeof(String));
            tblData.Columns.Add("PZUL_LAST", typeof(String));
            tblData.Columns.Add("PUMSCHR_LAST", typeof(String));
            tblData.Columns.Add("PUMK_LAST", typeof(String));
            tblData.Columns.Add("PERS_LAST", typeof(String));
            tblData.Columns.Add("UZUL_LAST", typeof(String));
            tblData.Columns.Add("UUMSCHR_LAST", typeof(String));
            tblData.Columns.Add("UUMK_LAST", typeof(String));
            tblData.Columns.Add("UERS_LAST", typeof(String));
            tblData.Columns.Add("PZUL_BEM", typeof(String));
            tblData.Columns.Add("PUMSCHR_BEM", typeof(String));
            tblData.Columns.Add("PUMK_BEM", typeof(String));
            tblData.Columns.Add("PERS_BEM", typeof(String));
            tblData.Columns.Add("UZUL_BEM", typeof(String));
            tblData.Columns.Add("UUMSCHR_BEM", typeof(String));
            tblData.Columns.Add("UUMK_BEM", typeof(String));
            tblData.Columns.Add("UERS_BEM", typeof(String));
            tblData.Columns.Add("ZKFZKZ", typeof(String));
            tblData.Columns.Add("STVALN", typeof(String));
            tblData.Columns.Add("STVALNFORM", typeof(String));
            tblData.Columns.Add("STVALNGEB", typeof(String));
            tblData.Columns.Add("URL", typeof(String));
            
        }

    }
}
