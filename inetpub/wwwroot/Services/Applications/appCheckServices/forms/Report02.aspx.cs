using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using appCheckServices.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;
using System.Configuration;

namespace appCheckServices.forms
{
    public partial class Report02 : System.Web.UI.Page
    {

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        DataTable table;
        String fileSourcePath;
        String fileSavePath;
        clsInfo m_Info;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);

            Common.FormAuth(this, m_User);

            m_App = new App(m_User); 

            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            Literal1.Text = "";
            try
            {
                        fileSourcePath = ConfigurationManager.AppSettings["UpDownKundeXL"];
                        fileSourcePath += m_User.Organization.OrganizationReference + "\\export\\" + m_User.Reference + "\\";
            }
            catch (Exception ex)
            {
                lblNoData.Visible = true;
                lblNoData.Text = ex.Message;
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        void DoSubmit()
        {

            DataRow row;
            DataColumn column;
            Int32 i;
            String fname1;

            try
            {
                table = new DataTable();
                column = new DataColumn("Serverpfad", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Filename", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Belegnummer", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("AnzPos", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("geprueft", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("nicht_geprueft", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Rueckmeldung", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Beauftragungsdatum", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Erstellungsdatum", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Downloaddatum", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("Downloaduser", System.Type.GetType("System.String"));
                table.Columns.Add(column);
                column = new DataColumn("pattern", System.Type.GetType("System.String"));
                table.Columns.Add(column);

                m_Info = new clsInfo( ref m_User,m_App, "");
                m_Info.DatumVon = txtDatumvon.Text;
                m_Info.DatumBis = txtDatumBis.Text;
                m_Info.FILL(Session["AppID"].ToString(),  Session.SessionID.ToString(), this);


                for (i = 0; i <= m_Info.tblInfo.Rows.Count - 1; i++)
                {

                    row = table.NewRow();
                    DataRow infoRow;
                    infoRow = m_Info.tblInfo.Rows[i];
                    fname1 = infoRow["DSN"].ToString();
                    row["Serverpfad"] = fileSourcePath + fname1;
                    row["Filename"] = fname1;
                    row["Belegnummer"] = infoRow["FBELN"].ToString();
                    row["AnzPos"] = infoRow["ANZPOS"].ToString();
                    row["nicht_geprueft"] = infoRow["PRUFNBEST"].ToString();
                    row["geprueft"] = infoRow["PRUFBEST"].ToString();
                    if (IsDate(infoRow["RU_IEDD"].ToString())) { row["Rueckmeldung"] = Convert.ToDateTime(infoRow["RU_IEDD"].ToString()).ToShortDateString(); }
                    if (IsDate(infoRow["BEAUFDAT"].ToString())) { row["Beauftragungsdatum"] = Convert.ToDateTime(infoRow["BEAUFDAT"].ToString()).ToShortDateString(); }
                    if (IsDate(infoRow["ERDAT"].ToString())) { row["Erstellungsdatum"] = Convert.ToDateTime(infoRow["ERDAT"].ToString()).ToShortDateString(); }
                    if (IsDate(infoRow["DOWNLDAT"].ToString())) { row["Downloaddatum"] = Convert.ToDateTime(infoRow["DOWNLDAT"].ToString()).ToShortDateString(); }
                    row["Downloaduser"] = infoRow["DOWNLUSR"].ToString();
                    row["pattern"] = "csv";
                    table.Rows.Add(row);
                    Session["App_Info"] = m_Info;
                    Fillgrid(0, table,"");
                }

            }
            catch (Exception ex)
            { 
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }


        void Fillgrid(Int32 intPageIndex, DataTable Table, String strSort)
        {
            if (Table.Rows.Count == 0)
            {
                Result.Visible = false;
                lblNoData.Visible = true;
                lblNoData.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                Result.Visible = true;
                lblNoData.Visible = false;  
                    
                DataView tmpDataView = new DataView();
                tmpDataView= Table.DefaultView;

                Int32 intTempPageIndex = intPageIndex;
                String strTempSort = "";
                String strDirection = null;

                if (strSort.Trim(' ').Length > 0)
                {
                    intTempPageIndex = 0;
                    strTempSort = strSort.Trim(' ');
                    if ((this.ViewState["Sort"] == null) || ((String)this.ViewState["Sort"] == strTempSort))
                    {
                        if (this.ViewState["Direction"] == null)
                        {
                            strDirection = "desc";
                        }
                        else
                        {
                            strDirection = (String)this.ViewState["Direction"];
                        }
                    }
                    else
                    {
                        strDirection = "desc";
                    }

                    if (strDirection == "asc")
                    {
                        strDirection = "desc";
                    }
                    else
                    {
                        strDirection = "asc";
                    }

                    this.ViewState["Sort"] = strTempSort;
                    this.ViewState["Direction"] = strDirection;
                } 

                DataGrid1.DataSource = tmpDataView;
                DataGrid1.DataBind();

                    TableCell cell = default(TableCell); 
                    ImageButton linBut = default(ImageButton); 
                    Control control = default(Control); 
                    string sPattern = null; 
                    
                    foreach (DataGridItem item in DataGrid1.Items) {
                        sPattern = item.Cells[11].Text; 
                        cell = item.Cells[1]; 
                        if (sPattern == "xls" | sPattern == "csv") { 
                            foreach (Control ctrl in cell.Controls) { 
                                if (ctrl is ImageButton) {
                                    linBut = (ImageButton)ctrl; 
                                    if (linBut.ID == "lbtExcel") { 
                                        linBut.Visible = true; 
                                    } 
                                } 
                            } 
                        } 
                        else if (sPattern == "doc") { 
                            foreach (Control ctrl in cell.Controls) {
                                if (ctrl is ImageButton)
                                {
                                    linBut = (ImageButton)ctrl; 
                                    if (linBut.ID == "lbtWord") { 
                                        linBut.Visible = true; 
                                    } 
                                } 
                                
                            } 
                        } 
                        else if ((sPattern.ToLower() == "jpg") | (sPattern == "jepg")) { 
                            foreach (Control ctrl in cell.Controls) {
                                if (ctrl is ImageButton)
                                {
                                    linBut = (ImageButton)ctrl; 
                                    if (linBut.ID == "lbtJepg") { 
                                        linBut.Visible = true; 
                                    } 
                                } 
                            } 
                        } 
                        else if (sPattern.ToLower() == "pdf") { 
                            foreach (Control ctrl in cell.Controls) { 
                                if (ctrl is ImageButton) { 
                                    linBut = (ImageButton)ctrl; 
                                    if (linBut.ID == "lbtPDF") { 
                                        linBut.Visible = true; 
                                    } 
                                } 
                            } 
                        } 
                        else if (sPattern.ToLower() == "gif") { 
                            foreach (Control ctrl in cell.Controls) { 
                                if (ctrl is ImageButton) { 
                                    linBut = (ImageButton)control; 
                                    if (linBut.ID == "lbtGif") { 
                                        linBut.Visible = true; 
                                    } 
                                } 
                            } 
                        } 
                        else if (sPattern.ToLower() == "zip") { 
                            foreach (Control ctrl in cell.Controls) {
                                if (ctrl is ImageButton)
                                {
                                    linBut = (ImageButton)ctrl; 
                                    if (linBut.ID == "lbtZip") { 
                                        linBut.Visible = true; 
                                    } 
                                } 
                            } 
                        } 
                    }
                    lblNoData.Visible = true; 
                    lblNoData.Text = "Folgende Dokumente stehen zum Download bereit."; 
            }
        
        }

        public bool IsDate(string inValue)
        {
            bool result;
            try
            {
                DateTime myDT = DateTime.Parse(inValue);
                result = true;
            }
            catch (FormatException e)
            {
                result = false;
            }
                return result;
            
        }


        protected void DataGrid1_ItemCommand(Object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "open")
            {
                String sPfad = null;
                String fname = null;
                String sBelegnr = null;
                String sDatum = null;

                try
                {
                    if (e.Item.Cells[11].Text == "pdf")
                    {
                        sPfad = e.Item.Cells[10].Text;
                        Session["App_Filepath"] = sPfad;
                        Session["App_ContentType"] = "Application/pdf";

                        Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine;
                        Literal1.Text += " <!-- //" + Environment.NewLine;
                        Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + (String)Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine;
                        Literal1.Text += " //-->" + Environment.NewLine;

                        Literal1.Text += " </script>" + Environment.NewLine;
                    }
                    else if (e.Item.Cells[11].Text == "xls")
                    {
                        CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory ExcelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
                        sPfad = e.Item.Cells[10].Text;
                        fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);
                        //upFile.PostedFile.FileName.Substring(upFile.PostedFile.FileName.LastIndexOf(@"\") + 1);

                        ExcelFactory.ReturnExcelTab(sPfad, fname, this.Page);
                    }
                    else if (e.Item.Cells[11].Text == "csv")
                    {
                        CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory ExcelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
                        sPfad = e.Item.Cells[10].Text;
                        fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);

                        ExcelFactory.CSVGetExcelTab(sPfad, fname, this.Page);
                            sPfad = e.Item.Cells[10].Text;
                            fname = sPfad.PadRight(sPfad.Length - sPfad.LastIndexOf("\\") - 1);
                        
                            if ( Session["App_Info"] != null){
                                m_Info = (clsInfo) Session["App_Info"];
                                sBelegnr = ((Label)e.Item.Cells[0].FindControl("lblBELGNR")).Text;
                                sDatum = ((Label)e.Item.Cells[7].FindControl("lblErdat")).Text;
                                m_Info.Change(Session["AppID"].ToString(), Session.SessionID.ToString(), this, sBelegnr, sDatum);
                                if (m_Info.Status != 0 )
                                {
                                    lblError.Text = m_Info.Message;
                                }                            
                            }


                    }
                    else if (e.Item.Cells[11].Text == "doc")
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        System.Collections.Hashtable imageHashTable = new System.Collections.Hashtable();
                        CKG.Base.Kernel.DocumentGeneration.WordDocumentFactory WordFactory = new CKG.Base.Kernel.DocumentGeneration.WordDocumentFactory(dt, imageHashTable);
                        sPfad = e.Item.Cells[10].Text;
                        fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);
                        WordFactory.Returndoc(sPfad, fname, this.Page);
                    }
                    else if (e.Item.Cells[11].Text == "jpg" | e.Item.Cells[3].Text == "gif")
                    {
                        sPfad = e.Item.Cells[10].Text;
                        Session["App_Filepath"] = sPfad;
                        if (e.Item.Cells[10].Text == "jpg")
                        {
                            Session["App_ContentType"] = "image/JPEG";
                        }
                        else
                        {
                            Session["App_ContentType"] = "image/GIF";
                        }


                        Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine;
                        Literal1.Text += " <!-- //" + Environment.NewLine;
                        Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + (String)Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine;
                        Literal1.Text += " //-->" + Environment.NewLine;
                        Literal1.Text += " </script>" + Environment.NewLine;
                    }
                    else if (e.Item.Cells[11].Text == "zip")
                    {
                        sPfad = e.Item.Cells[10].Text;
                        Session["App_Filepath"] = sPfad;
                        Session["App_ContentType"] = "application/x-zip-compressed";

                        Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine;
                        Literal1.Text += " <!-- //" + Environment.NewLine;
                        Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine;
                        Literal1.Text += " //-->" + Environment.NewLine;
                        Literal1.Text += " </script>" + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message;
                }
            }

        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../../Start/Selection.aspx?AppID=" + Session["AppID"], false);

        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }
    
    }
}
