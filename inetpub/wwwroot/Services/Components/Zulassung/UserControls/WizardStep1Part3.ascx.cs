using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep1Part3 : System.Web.UI.UserControl, IPostBackEventHandler, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep1Part3";

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            fileUpload1.Attributes.Add("onchange", string.Format("{0}; $('#{1}').attr('disabled', 'disabled'); $('#{2}').attr('style', 'display:inline');", Page.ClientScript.GetPostBackEventReference(this, "upload"), fileUpload1.ClientID, labelWait.ClientID));
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("upload", StringComparison.OrdinalIgnoreCase))
            {
                if (fileUpload1.PostedFile != null && !string.IsNullOrEmpty(fileUpload1.PostedFile.FileName))
                {
                    if (!Path.GetExtension(fileUpload1.PostedFile.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        labelError.Text = "Es können nur Dateien im .XLS-Format verarbeitet werden.";
                        labelError.Visible = true;
                        return;
                    }
                }
                else
                {
                    labelError.Text = "Es wurde keine Datei ausgewählt.";
                    labelError.Visible = true;
                    return;
                }

                labelError.Visible = false;
                Session["UploadedFile"] = fileUpload1.PostedFile;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "SearchScript", string.Format("$('#searchPanel').attr('style', 'display:inline'); $('#uploadPanel').attr('style', 'display:none'); $('#filePath').html('<strong>Datei:</strong> {0}');", fileUpload1.PostedFile.FileName.Replace(@"\", @"\\")), true);
            }
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            var searchCriterias = ReadUploadedFile(Session["UploadedFile"] as HttpPostedFile);
            Session["UploadedFile"] = null;
            if (searchCriterias != null)
            {
                page.DAL.FindVehicles(searchCriterias.ToArray());
            }
        }

        private IEnumerable<VehicleSearchCriterias> ReadUploadedFile(HttpPostedFile file)
        {
            if (file == null)
            {
                return null;
            }

            try
            {
                var workingDir = ConfigurationManager.AppSettings["ExcelPath"];
                var localFileName = string.Format("{0}_{1:yyyyMMddhhmmss}.xls", page.User.UserName, DateTime.Now);
                var localFilePath = Path.Combine(workingDir, localFileName);
                file.SaveAs(localFilePath);

                if (!File.Exists(localFilePath))
                {
                    labelError.Text = "Fehler beim Zwischenspeichern der einzulesenden Datei.";
                    labelError.Visible = true;
                    return null;
                }

                var searchCriterias = new List<VehicleSearchCriterias>();

                var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"{0}\";Extended Properties=\"Excel 8.0;HDR=YES;\"", localFilePath);

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    DataTable tableNames = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

                    if (tableNames.Rows.Count > 0)
                    {
                        var tableName = tableNames.Rows[0]["TABLE_NAME"].ToString();

                        using (var command = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tableName), connection))
                        {
                            using (var adapter = new OleDbDataAdapter())
                            {
                                adapter.SelectCommand = command;
                                using (var dataSet = new DataSet())
                                {
                                    adapter.Fill(dataSet);
                                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Columns.Count > 0)
                                    {
                                        foreach (DataRow row in dataSet.Tables[0].Rows)
                                        {
                                            var searchCriteria = new VehicleSearchCriterias();
                                            var chassisNo = row[0].ToString().Trim();
                                            if (!string.IsNullOrEmpty(chassisNo))
                                            {
                                                searchCriteria.ChassisNumber = chassisNo;
                                            }

                                            if (dataSet.Tables[0].Columns.Count > 2)
                                            {
                                                var contract = row[1].ToString().Trim();
                                                if (!string.IsNullOrEmpty(contract))
                                                {
                                                    searchCriteria.Contract = contract;
                                                }

                                                var zb2No = row[2].ToString().Trim();
                                                if (!string.IsNullOrEmpty(zb2No))
                                                {
                                                    searchCriteria.ZB2No = zb2No;
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(searchCriteria.ToString()) && !searchCriterias.Contains(searchCriteria))
                                            {
                                                searchCriterias.Add(searchCriteria);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return searchCriterias;
            }
            catch
            {
                labelError.Text = "Beim Einlesen der Datei ist ein Fehler aufgetreten.";
                labelError.Visible = true;
                return null;
            }
        }
    }
}