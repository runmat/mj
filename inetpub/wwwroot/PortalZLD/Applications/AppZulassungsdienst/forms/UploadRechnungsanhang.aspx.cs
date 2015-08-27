using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektionseite Nacherfassung.  
    /// </summary>
    public partial class UploadRechnungsanhang : System.Web.UI.Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objNacherf = new NacherfZLD(m_User.Reference) { SelUploadRechnungsanhaenge = true };

                Session["Sort"] = null;
                Session["Direction"] = null;
                Session["SucheValue"] = null;
                Session["Rowfilter"] = null;
                FillForm();
            }
            else
            {
                objNacherf = (NacherfZLD)Session["objNacherf"];
            }

            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Sammeln der Selektionsdaten und an Sap übergeben. 
        /// Dann werden die Datensätze für die Anzeige selektiert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (String.IsNullOrEmpty(txtZulDate.Text))
            {
                lblError.Text = "Bitte geben Sie ein Zulassungsdatum an!";
                return;
            }

            objNacherf.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);

            if (!String.IsNullOrEmpty(txtZulDateBis.Text))
            {
                objNacherf.SelDatumBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);

                if (String.IsNullOrEmpty(objNacherf.SelDatum) || String.IsNullOrEmpty(objNacherf.SelDatumBis))
                {
                    lblError.Text = "Bitte geben Sie einen gültigen Zeitraum für das Zulassungsdatum an!";
                    return;
                }

                DateTime vonDatum = DateTime.Parse(objNacherf.SelDatum);
                DateTime bisDatum = DateTime.Parse(objNacherf.SelDatumBis);
                if (vonDatum > bisDatum)
                {
                    lblError.Text = "Das Bis-Datum muss größer sein als das Von-Datum!";
                    return;
                }
                if ((bisDatum - vonDatum).TotalDays > 92)
                {
                    lblError.Text = "Zeitraum max. 92 Tage möglich!";
                    return;
                }
            }

            if (rbON.Checked)
            {
                objNacherf.SelVorgang = "ON";
                objNacherf.SelStatus = "ON,OA";
            }
            if (rbAH.Checked)
            {
                objNacherf.SelVorgang = "A";
                objNacherf.SelStatus = "AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge
            }
            if (rbAH_NZ.Checked)
            {
                objNacherf.SelVorgang = "ANZ";
                objNacherf.SelStatus = "NZ,AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Autohausvorgänge und normal Nacherfassung
            }
            if (rbAH_ON_NZ.Checked)
            {
                objNacherf.SelVorgang = "AONNZ";
                objNacherf.SelStatus = "NZ,ON,OA,AN,AA,AB,AG,AS,AU,AF,AK,AZ"; // alle Online- & Autohausvorgänge und normal Nacherfassung
            }

            if (String.IsNullOrEmpty(txtStVavon.Text))
            {
                lblError.Text = "Bitte geben Sie ein Amt an!";
                return;
            }

            objNacherf.SelKreis = txtStVavon.Text.ToUpper();

            if (!String.IsNullOrEmpty(txtStVaBis.Text))
            {
                objNacherf.SelKreisBis = txtStVaBis.Text.ToUpper();

                if (String.Compare(objNacherf.SelKreis, objNacherf.SelKreisBis) > 0)
                {
                    lblError.Text = "Das Amt-von muss alphabetisch vor dem Amt-bis liegen!";
                    return;
                }
            }

            if (String.IsNullOrEmpty(txtDatenAbZeile.Text))
            {
                lblError.Text = "Bitte geben Sie an, in welcher Zeile der Excel-Tabelle die Nutzdaten beginnen!";
                return;
            }
            if (txtDatenAbZeile.Text.ToInt(0) <= 0)
            {
                lblError.Text = "Die Nummer der Zeile, die den Beginn der Nutzdaten markiert, muss > 0 sein!";
                return;
            }

            if (String.IsNullOrEmpty(txtSpalteKennzeichen.Text))
            {
                lblError.Text = "Bitte geben Sie an, in welcher Spalte der Excel-Tabelle das Kennzeichen steht!";
                return;
            }

            if (String.IsNullOrEmpty(txtSpalteGebuehren.Text))
            {
                lblError.Text = "Bitte geben Sie an, in welcher Spalte der Excel-Tabelle die Gebühren stehen!";
                return;
            }

            if (String.IsNullOrEmpty(txtSpalteZulassungsdatum.Text))
            {
                lblError.Text = "Bitte geben Sie an, in welcher Spalte der Excel-Tabelle das Zulassungsdatum steht!";
                return;
            }

            var uploadTemplate = new RechnungsanhangTemplates
                {
                    DatenAbZeile = txtDatenAbZeile.Text.ToInt(0),
                    SpalteKennzeichen = txtSpalteKennzeichen.Text.ToUpper(),
                    SpalteGebuehren = txtSpalteGebuehren.Text.ToUpper(),
                    SpalteZulassungsdatum = txtSpalteZulassungsdatum.Text.ToUpper()
                };

            var uploadList = GetUploadData(upFile.PostedFile, uploadTemplate);

            if (uploadList == null)
                return;

            var doppelteKennzeichen = uploadList.GroupBy(u => u.Kennzeichen).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            uploadList.RemoveAll(u => doppelteKennzeichen.Contains(u.Kennzeichen));

            objNacherf.LoadVorgaengeFromSap(objCommon.MaterialStamm);

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            if (objNacherf.Vorgangsliste.Any())
            {
                var anzGefunden = objNacherf.InsertGebuehrenFromUploadData(objCommon.MaterialStamm, objCommon.StvaStamm, uploadList);

                if (objNacherf.ErrorOccured)
                {
                    lblError.Text = objNacherf.Message;
                    return;
                }

                if (anzGefunden == 0)
                {
                    lblError.Text = "Zu den hochgeladenen Daten wurden keine Vorgänge gefunden!";
                    return;
                }

                Session["objNacherf"] = objNacherf;
                Response.Redirect("ChangeZLDNach.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                lblError.Text = "Keine Daten gefunden!" + objNacherf.Message;
            }
        }

        /// <summary>
        /// Zurück zur Startseite.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        protected void rblTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectTemplate(rblTemplate.SelectedValue);
        }

        #endregion

        #region Methods

        private void FillForm()
        {
            if (objNacherf.RechnungUploadTemplates == null)
            {
                objNacherf.LoadRechnungsanhangTemplatesFromSql();
                Session["objNacherf"] = objNacherf;
            }

            rblTemplate.DataSource = objNacherf.RechnungUploadTemplates;
            rblTemplate.DataValueField = "ID";
            rblTemplate.DataTextField = "Bezeichnung";
            rblTemplate.DataBind();

            if (objNacherf.RechnungUploadTemplates.AnyAndNotNull())
            {
                rblTemplate.SelectedValue = objNacherf.RechnungUploadTemplates.First().ID.ToString();
                SelectTemplate(rblTemplate.SelectedValue);
            }  
        }

        private void SelectTemplate(string rblValue)
        {
            var templateId = rblValue.ToInt(0);

            var tmplt = objNacherf.RechnungUploadTemplates.FirstOrDefault(t => t.ID == templateId);
            if (tmplt != null)
            {
                txtDatenAbZeile.Text = tmplt.DatenAbZeile.ToString();
                txtSpalteKennzeichen.Text = tmplt.SpalteKennzeichen;
                txtSpalteGebuehren.Text = tmplt.SpalteGebuehren;
                txtSpalteZulassungsdatum.Text = tmplt.SpalteZulassungsdatum;
            }
        }

        private List<RechnungsanhangDaten> GetUploadData(System.Web.HttpPostedFile uFile, RechnungsanhangTemplates uplDefinition)
        {
            var list = new List<RechnungsanhangDaten>();

            if (uFile == null || String.IsNullOrEmpty(uFile.FileName))
            {
                lblError.Text = "Bitte wählen Sie eine Excel-Datei für den Upload aus!";
                return null;
            }

            string dateiEndung;
            if (upFile.PostedFile.FileName.ToLower().EndsWith(".xls"))
            {
                dateiEndung = "xls";
            }
            else if (upFile.PostedFile.FileName.ToLower().EndsWith(".xlsx"))
            {
                dateiEndung = "xlsx";
            }
            else
            {
                lblError.Text = "Die Upload-Datei muss im Excel-Format vorliegen!";
                return null;
            }

            var filename = m_User.UserName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + dateiEndung;
            var filepath = ConfigurationManager.AppSettings["ExcelPath"] + filename;

            upFile.PostedFile.SaveAs(filepath);

            if (!File.Exists(filepath))
            {
                lblError.Text = "Fehler beim Speichern der Upload-Datei";
                return null;
            }

            var excelConnString = "";
            if (dateiEndung == "xls")
                excelConnString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=No\"";
            else if (dateiEndung == "xlsx")
                excelConnString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=No\"";

            using (var objConn = new OleDbConnection(excelConnString))
            {
                objConn.Open();

                object[] tmpObj = {
                                null,
                                null,
                                null,
                                "Table"
                              };

                var schemaTable = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, tmpObj);
                if (schemaTable != null)
                {
                    DataSet objDataset1 = new DataSet();

                    foreach (DataRow sheet in schemaTable.Rows)
                    {
                        var tableName = sheet["Table_Name"].ToString();
                        if (!tableName.EndsWith("_"))
                        {
                            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [" + tableName + "]", objConn);
                            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter(objCmdSelect);
                            objAdapter1.Fill(objDataset1, tableName);
                        }
                    }

                    var tblTemp = objDataset1.Tables[0];

                    for (var i = 0; i < (uplDefinition.DatenAbZeile - 1); i++)
                    {
                        tblTemp.Rows[i].Delete();
                    }

                    tblTemp.AcceptChanges();

                    var colIndexKennzeichen = ZLDCommon.GetTableColumnIndexFromExcelColumnName(uplDefinition.SpalteKennzeichen);
                    var colIndexGebuehren = ZLDCommon.GetTableColumnIndexFromExcelColumnName(uplDefinition.SpalteGebuehren);
                    var colIndexZulassungsdatum = ZLDCommon.GetTableColumnIndexFromExcelColumnName(uplDefinition.SpalteZulassungsdatum);

                    for (var i = 0; i < tblTemp.Rows.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(tblTemp.Rows[i][colIndexKennzeichen].ToString()))
                        {
                            list.Add(new RechnungsanhangDaten
                                {
                                    Kennzeichen = tblTemp.Rows[i][colIndexKennzeichen].ToString().Replace(" ", ""),
                                    Gebuehren = tblTemp.Rows[i][colIndexGebuehren].ToString().Replace(" ", "").Replace('.', ',').Replace("€", ""),
                                    Zulassungsdatum = tblTemp.Rows[i][colIndexZulassungsdatum].ToString().Replace(" ", "").Replace("00:00:00", "")
                                });
                        }
                    }
                }

                objConn.Close();
            }

            return list;
        }

        #endregion
    }
}
