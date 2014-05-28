    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CKG.Base.Business;
    using CKG.Base.Kernel;
    using CKG.Base.Kernel.Common;
    using System.Data.OleDb;
    using System.IO; 
    using System.Configuration;
    using System.Data;
    using System.Text.RegularExpressions;

    namespace appCheckServices.forms
    {
         public partial class Report01 : System.Web.UI.Page 
        {
             private CKG.Base.Kernel.Security.User m_User;
            DataTable table; 
            string fileSourcePath; 
            
            protected void Page_Load(object sender, System.EventArgs e) 
            { 
                
                m_User = Common.GetUser(this);
                    
                Common.FormAuth(this, m_User);
                
                 Common.GetAppIDFromQueryString(this);

                 lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"]; 
                
                Literal1.Text = "";

                
                try { 
                    if ((string)Request.QueryString["check"] == "1") { 
                        // Kunde 
                        fileSourcePath = ConfigurationManager.AppSettings["UpDownKundeXL"]; 
                        fileSourcePath += m_User.Organization.OrganizationReference + "\\export\\" + m_User.Reference + "\\"; 
                    } 
                    else { 
                        fileSourcePath = ConfigurationManager.AppSettings["DownloadPathZulXL"]; 
                    } 
                    
                    
                    if (!IsPostBack) { 
                        //DoSubmit(); 
                    } 
                } 
                catch { 
                    lblNoData.Visible = true; 
                    lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."; 
                } 
            }
            private void Fillgrid(Int32 intPageIndex, DataTable Table, String strSort) 
            { 
                
                if (Table.Rows.Count == 0) { 
                    DataGrid1.Visible = false; 
                    lblNoData.Visible = true; 
                    
                    lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."; 
                } 
                else { 
                    DataGrid1.Visible = true; 
                    lblNoData.Visible = false; 
                    
                    DataView tmpDataView = new DataView(); 
                    tmpDataView = Table.DefaultView; 
                    
                    Int32 intTempPageIndex = intPageIndex;
                    String strTempSort = "";
                    String strDirection = null; 
                    
                    if (strSort.Trim(' ').Length > 0) { 
                        intTempPageIndex = 0; 
                        strTempSort = strSort.Trim(' ');
                        if ((this.ViewState["Sort"] == null) || ((String)this.ViewState["Sort"] == strTempSort))
                        { 
                            if (this.ViewState["Direction"] == null) { 
                                strDirection = "desc"; 
                            } 
                            else { 
                                strDirection = (String)this.ViewState["Direction"]; 
                            } 
                        } 
                        else { 
                            strDirection = "desc"; 
                        } 
                        
                        if (strDirection == "asc") { 
                            strDirection = "desc"; 
                        } 
                        else { 
                            strDirection = "asc"; 
                        } 
                        
                        this.ViewState["Sort"] = strTempSort; 
                        this.ViewState["Direction"] = strDirection; 
                    } 
                    DataGrid1.DataSource = tmpDataView; 
                    DataGrid1.DataBind(); 
                    
                    //DataGridItem item = default(DataGridItem); 
                    TableCell cell = default(TableCell); 
                    ImageButton linBut = default(ImageButton); 
                    Control control = default(Control); 
                    string sPattern = null; 
                    
                    foreach (DataGridItem item in DataGrid1.Items) { 
                        sPattern = item.Cells[3].Text; 
                        cell = item.Cells[0]; 
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
                    Result.Visible = true;
                    DivPlaceholder.Visible = false;
                    lblNoData.Visible = true; 
                    lblNoData.Text = "Folgende Dokumente stehen zum Download bereit."; 
                } 
            }

            protected void DataGrid1_ItemCommand(Object sender, DataGridCommandEventArgs e) 
            { 
                if (e.CommandName == "open") { 
                    string sPfad = null; 
                    string fname = null; 
                    try { 
                        if (e.Item.Cells[3].Text == "pdf") { 
                            sPfad = e.Item.Cells[2].Text; 
                            Session["App_Filepath"] = sPfad; 
                            Session["App_ContentType"] = "Application/pdf"; 
                            
                            Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine; 
                            Literal1.Text += " <!-- //" + Environment.NewLine; 
                            Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + (String)Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine; 
                            Literal1.Text += " //-->" + Environment.NewLine; 
                                
                            Literal1.Text += " </script>" + Environment.NewLine; 
                        } 
                        else if (e.Item.Cells[3].Text == "xls") {
                            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory ExcelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory(); 
                            sPfad = e.Item.Cells[2].Text;
                            fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);
                            //upFile.PostedFile.FileName.Substring(upFile.PostedFile.FileName.LastIndexOf(@"\") + 1);

                            ExcelFactory.ReturnExcelTab(sPfad, fname, this.Page); 
                        } 
                        else if (e.Item.Cells[3].Text == "csv") {
                            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory ExcelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory(); 
                            sPfad = e.Item.Cells[2].Text;
                            fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1);
                            
                            ExcelFactory.CSVGetExcelTab(sPfad, fname, this.Page); 
                        } 
                        else if (e.Item.Cells[3].Text == "doc") { 
                            System.Data.DataTable dt = new System.Data.DataTable();
                            System.Collections.Hashtable imageHashTable = new System.Collections.Hashtable();
                            CKG.Base.Kernel.DocumentGeneration.WordDocumentFactory WordFactory = new CKG.Base.Kernel.DocumentGeneration.WordDocumentFactory(dt, imageHashTable); 
                            sPfad = e.Item.Cells[2].Text;
                            fname = sPfad.Substring(sPfad.LastIndexOf(@"\") + 1); 
                            WordFactory.Returndoc(sPfad, fname, this.Page); 
                        } 
                        else if (e.Item.Cells[3].Text == "jpg" | e.Item.Cells[3].Text == "gif") { 
                            sPfad = e.Item.Cells[2].Text; 
                            Session["App_Filepath"] = sPfad; 
                            if (e.Item.Cells[3].Text == "jpg") { 
                                Session["App_ContentType"] = "image/JPEG"; 
                            } 
                            else { 
                                Session["App_ContentType"] = "image/GIF"; 
                            } 
                            
                            
                            Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine; 
                            Literal1.Text += " <!-- //" + Environment.NewLine; 
                            Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + (String)Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine; 
                            Literal1.Text += " //-->" + Environment.NewLine; 
                            Literal1.Text += " </script>" + Environment.NewLine; 
                        } 
                        else if (e.Item.Cells[3].Text == "zip") { 
                            sPfad = e.Item.Cells[2].Text; 
                            Session["App_Filepath"] = sPfad; 
                            Session["App_ContentType"] = "application/x-zip-compressed"; 
                            
                            Literal1.Text = " <script language=\"Javascript\">" + Environment.NewLine; 
                            Literal1.Text += " <!-- //" + Environment.NewLine; 
                            Literal1.Text += " window.open(\"Report01_2.aspx?AppID=" + Session["AppID"] + "\", \"_blank\", \"left=0,top=0,resizable=YES,scrollbars=YES\");" + Environment.NewLine; 
                            Literal1.Text += " //-->" + Environment.NewLine; 
                            Literal1.Text += " </script>" + Environment.NewLine; 
                        } 
                    } 
                    catch (Exception ex) { 
                        lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message; 
                    } 
                } 
            } 

            private void DoSubmit() 
            { 
                DataRow row = default(DataRow); 
                DataColumn column = default(DataColumn); 
                string[] files = null; 
                string[] files2 = null; 
                string[] files3 = null; 
                string[] files4 = null; 
                string[] files5 = null; 
                string[] files6 = null; 
                string[] files7 = null; 
                System.IO.FileInfo info = default(System.IO.FileInfo); 
                int i = 0; 
                string fname = null; 
                string fname1 = null; 
                
                try { 
                    table = new DataTable(); 
                    column = new DataColumn("Serverpfad", System.Type.GetType("System.String")); 
                    table.Columns.Add(column); 
                    column = new DataColumn("Filename", System.Type.GetType("System.String")); 
                    table.Columns.Add(column); 
                    column = new DataColumn("Filedate", System.Type.GetType("System.DateTime")); 
                    table.Columns.Add(column); 
                    column = new DataColumn("Pfad", System.Type.GetType("System.String")); 
                    table.Columns.Add(column); 
                    column = new DataColumn("Pattern", System.Type.GetType("System.String")); 
                    table.Columns.Add(column); 
                    
                    
                    files = System.IO.Directory.GetFiles(fileSourcePath, "*.pdf"); 
                    files2 = System.IO.Directory.GetFiles(fileSourcePath, "*.xls"); 
                    files3 = System.IO.Directory.GetFiles(fileSourcePath, "*.doc"); 
                    files4 = System.IO.Directory.GetFiles(fileSourcePath, "*.jpg"); 
                    files5 = System.IO.Directory.GetFiles(fileSourcePath, "*.gif"); 
                    files6 = System.IO.Directory.GetFiles(fileSourcePath, "*.csv"); 
                    files7 = System.IO.Directory.GetFiles(fileSourcePath, "*.zip"); 
                    
                    
                    for (i = 0; i <= files.Length - 1; i++) { 
                        info = new System.IO.FileInfo((String)files.GetValue(i)); 
                        fname = (String)files.GetValue(i);
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1); ; 
                        row = table.NewRow(); 
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1; 
                        fname1 = fname1.Substring(0,fname1.LastIndexOf(".")) ;
                        row["Filename"] = fname1; 
                        row["Filedate"] = info.CreationTime; 
                        row["Pattern"] = "pdf"; 
                        table.Rows.Add(row); 
                    } 
                    
                    for (i = 0; i <= files2.Length - 1; i++) { 
                        info = new System.IO.FileInfo((String)files2.GetValue(i)); 
                        fname = (String)files2.GetValue(i); 
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1);
                        row = table.NewRow(); 
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                        fname1 = fname1.Substring(0, fname1.LastIndexOf(".")); 

                        row["Filename"] = fname1; 
                        row["Filedate"] = info.CreationTime; 
                        row["Pattern"] = "xls"; 
                            
                        table.Rows.Add(row); 
                    } 
                    for (i = 0; i <= files3.Length - 1; i++) { 
                        info = new System.IO.FileInfo((String)files3.GetValue(i)); 
                        fname = (String)files3.GetValue(i);
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1); 
                        row = table.NewRow(); 
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                        fname1 = fname1.Substring(0, fname1.LastIndexOf("."));  
                        row["Filename"] = fname1; 
                        row["Filedate"] = info.CreationTime; 
                        row["Pattern"] = "doc"; 
                        table.Rows.Add(row); 
                    } 
                    for (i = 0; i <= files4.Length - 1; i++) {
                        info = new System.IO.FileInfo((String)files4.GetValue(i));
                        fname = (String)files4.GetValue(i);
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1);
                        row = table.NewRow();
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                        fname1 = fname1.Substring(0, fname1.LastIndexOf("."));
                        row["Filename"] = fname1;
                        row["Filedate"] = info.CreationTime; 
                        row["Pattern"] = "jpg"; 
                        table.Rows.Add(row); 
                    } 
                    for (i = 0; i <= files5.Length - 1; i++) {
                        info = new System.IO.FileInfo((String)files5.GetValue(i));
                        fname = (String)files5.GetValue(i);
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1);
                        row = table.NewRow();
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                        fname1 = fname1.Substring(0, fname1.LastIndexOf("."));
                        row["Filename"] = fname1;
                        row["Filedate"] = info.CreationTime; 
                        row["Pattern"] = "gif"; 
                        table.Rows.Add(row); 
                    } 
                    for (i = 0; i <= files6.Length - 1; i++) {
                        info = new System.IO.FileInfo((String)files6.GetValue(i));
                        fname = (String)files6.GetValue(i);
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1);
                        row = table.NewRow();
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                        fname1 = fname1.Substring(0, fname1.LastIndexOf("."));
                        row["Filename"] = fname1;
                        row["Filedate"] = info.CreationTime; 
                        row["Pattern"] = "csv"; 
                        table.Rows.Add(row); 
                    } 
                    
                    for (i = 0; i <= files7.Length - 1; i++) {
                        info = new System.IO.FileInfo((String)files7.GetValue(i));
                        fname = (String)files7.GetValue(i);
                        fname1 = fname.Substring(fname.LastIndexOf(@"\") + 1);
                        row = table.NewRow();
                        row["Serverpfad"] = fileSourcePath + "\\" + fname1;
                        fname1 = fname1.Substring(0, fname1.LastIndexOf("."));
                        row["Filename"] = fname1;
                        row["Filedate"] = info.CreationTime;  
                        row["Pattern"] = "zip"; 
                        table.Rows.Add(row); 
                    } 
                    Fillgrid(0, table,""); 
                } 
                catch { 
                    lblNoData.Visible = true; 
                    lblNoData.Text = "Keine Dokumente zur Anzeige gefunden."; 
                } 
            }

            protected void btnConfirm_Click(object sender, EventArgs e)
            {
                DoSubmit();
            }

            protected void lbBack_Click(object sender, EventArgs e)
            {
                Response.Redirect("../../../Start/Selection.aspx?AppID=" + Session["AppID"], false);

            }

            protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
            {

            }



        } 

    }
