using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Configuration;
using AutohausPortal.lib;
using CKG.Base.Kernel.Security;
using CKG.Base.Kernel.Common;
using Telerik.Web.UI.GridExcelBuilder;

namespace AutohausPortal.forms
{
    /// <summary>
    /// Listenansicht Zulassungsstatistik. 
    /// Benutzte Klassen ZLD_Suche und ZLDCommon.
    /// </summary>
    public partial class Zulassungsstatistik_02 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private ZLDCommon objCommon;
        private ZLD_Suche objZLDSuche;

        /// <summary>
        /// Wird ein Asynchroner Postback ausgeführt müssen die Controls der Seite graphisch neu initiert werden.
        /// JSript-Funktion: kroschkeportal.js - initiate4() 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            Boolean IsInAsync = ScriptManager.GetCurrent(this).IsInAsyncPostBack;
            if (IsInAsync)
            {
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "initiate4",
                  "initiate4();", true);
            }
        }

        /// <summary>
        /// Überprüfung ob dem User diese Applikation zugeordnet ist. 
        /// Laden der Stammdaten wenn noch nicht in der Session(objCommon) vorhanden. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
         protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                if (!objCommon.Init(Session["AppID"].ToString(), Session.SessionID.ToString(), this))
                {
                    lblError.Visible = true;
                    lblError.Text = objCommon.Message;
                    return;
                }
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

            }

        }

        /// <summary>
        /// Binden der Tabelle KundenDaten an das RadGrind-Control als MasterTable.
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">GridNeedDataSourceEventArgs</param>
        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!e.IsFromDetailTable)
            {
                objZLDSuche = (ZLD_Suche)Session["objZLDSuche"];
                RadGrid1.DataSource = objZLDSuche.KundenDaten;      
            }
        }

        /// <summary>
        ///  Binden der Tabelle Auftragsdaten an das RadGrind-Control als Detailtabelle.
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">GridDetailTableDataBindEventArgs</param>
        protected void RadGrid1_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "Orders":
                    {
                        objZLDSuche = (ZLD_Suche)Session["objZLDSuche"];
                        string CustomerID = dataItem.GetDataKeyValue("KUNNR").ToString();
                        DataView dv = new DataView();
                        dv = objZLDSuche.Auftragsdaten.DefaultView;
                        dv.RowFilter = "KUNNR ='" + CustomerID + "'";
                        e.DetailTableView.DataSource = dv;
                        
                        break;
                    }
            }
           
        }

        /// <summary>
        /// Setzen der Überschriften im Radgrid für die Referenzspalten je nach Kunde.
        /// </summary>
        private void setHeaderText() 
        {
            foreach (GridDataItem dataItem in RadGrid1.MasterTableView.Items)
            {
                if (dataItem.Expanded == true) 
                {
                    string CustomerID = dataItem.GetDataKeyValue("KUNNR").ToString();
                    String Ref1 = "", Ref2 = "", Ref3 = "", Ref4 = "";
                    DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + CustomerID + "'");
                    if (drow.Length > 0)
                    {
                        Ref1 = drow[0]["REF_NAME_01"].ToString();
                        Ref2 = drow[0]["REF_NAME_02"].ToString();
                        Ref3 = drow[0]["REF_NAME_03"].ToString();
                        Ref4 = drow[0]["REF_NAME_04"].ToString();
                    }

                    if (Ref1 != "" )dataItem.ChildItem.NestedTableViews[0].GetColumn("ZZREFNR1").HeaderText = Ref1 ;
                    if (Ref2 != "") dataItem.ChildItem.NestedTableViews[0].GetColumn("ZZREFNR2").HeaderText = Ref2;
                    if (Ref3 != "") dataItem.ChildItem.NestedTableViews[0].GetColumn("ZZREFNR3").HeaderText = Ref3;
                    if (Ref4 != "") dataItem.ChildItem.NestedTableViews[0].GetColumn("ZZREFNR4").HeaderText = Ref4; 
                    dataItem.ChildItem.NestedTableViews[0].Rebind();
                }
            
            }
        
        }
        
        /// <summary>
        /// Den ersten Tabelleneintrag aufklappen (Master- und Detailtabelle). Headertexte setzen(Refenzfelder).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            if ((!Page.IsPostBack) && (RadGrid1.MasterTableView.Items.Count > 0))
            {
                RadGrid1.MasterTableView.Items[0].Expanded = true;
                RadGrid1.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            }
            setHeaderText();
        }

        /// <summary>
        /// Formatieren der Zeile der Gesamtbeträge in der Detailtabelle. Formatieren der PageSize-Combo.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridItemEventArgs</param>
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name =="Orders")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;          
                if (dataItem["MAKTX"].Text.ToString() == "Gesamt: ") 
                {
                    dataItem["MAKTX"].ForeColor = System.Drawing.Color.Black;
                    dataItem["PREIS_DL"].ForeColor = System.Drawing.Color.Black;
                    dataItem["PREIS_GB"].ForeColor = System.Drawing.Color.Black;
                    dataItem["PREIS_KZ"].ForeColor = System.Drawing.Color.Black;
                    dataItem["PREIS_ST"].ForeColor = System.Drawing.Color.Black;
                }
            }
            if (e.Item is GridPagerItem && e.Item.OwnerTableView.Name == "Orders")
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                RadComboBox PageSizeComboBox = (RadComboBox)pager.FindControl("PageSizeComboBox");
                PageSizeComboBox.Width = Unit.Pixel(55); 
            } 
        }

        /// <summary>
        /// Exportieren der Einträge im PDF-Format.
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">GridCommandEventArgs</param>
        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Contains("Export"))
            {
                    RadGrid1.MasterTableView.HierarchyDefaultExpanded = true; // for the first level
                    RadGrid1.MasterTableView.DetailTables[0].HierarchyDefaultExpanded = true; // for the second level
                    RadGrid1.MasterTableView.Rebind();
            }

            Control checkCtrl= (Control)e.CommandSource;
            if (checkCtrl.GetType() == typeof( ImageButton ))
            {
                ImageButton lnkPrint = (ImageButton)checkCtrl;

                if (lnkPrint!=null && e.Item.OwnerTableView.Name == "Orders")
                {
                    Session["App_ContentType"] = "Application/pdf";
                    if (m_User.IsTestUser)
                    {
                        Session["App_Filepath"] = "\\\\192.168.10.96\\test\\portal\\zld\\ah_auftrag\\" + lnkPrint.CommandArgument.TrimStart('/').Replace('/', '\\');
                    }
                    else
                    {
                        Session["App_Filepath"] = "\\\\192.168.10.96\\prod\\portal\\zld\\ah_auftrag\\" + lnkPrint.CommandArgument.TrimStart('/').Replace('/', '\\');
                    }
                   
                    GetPdf();
                }
            }
        }

        private void GetPdf()
        {
            if (Session["App_Filepath"] != null)
            {
                String sPfad = Session["App_Filepath"].ToString();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = Session["App_ContentType"].ToString();
                Session["App_Filepath"] = null;
                Session["App_ContentType"] = null;

                String fname = sPfad.Substring(sPfad.LastIndexOf("\\") + 1);

                // Datei direkt an Client senden, nicht im Browser anzeigen (führte teilweise zu Problemen)
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fname);
                Response.TransmitFile(sPfad);
                Response.End();
            }
        }

        /// <summary>
        /// Exportieren der Daten im Excel-Format.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdExport_Click(object sender, EventArgs e)
        {
            PerformExcelExport();
        }

        /// <summary>
        /// Exportieren der Daten im Excel-Format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCreateExcel_Click(object sender, EventArgs e)
        {
            PerformExcelExport();
        }

        /// <summary>
        /// Nutzung einer separaten Excel-Export-Funktion, um eine vorformatierte Excel-Datei als Vorlage nutzen zu können
        /// </summary>
        private void PerformExcelExport()
        {
            bool found;

            DataTable tblTemp = ((ZLD_Suche) Session["objZLDSuche"]).Auftragsdaten.Copy();

            // Summenzeilen entfernen
            DataRow[] sumRows = tblTemp.Select("ZULBELN IS NULL OR ZULBELN = ''");
            foreach (DataRow sumRow in sumRows)
            {
                tblTemp.Rows.Remove(sumRow);
            }

            // Unerwünschte Spalten entfernen
            tblTemp.Columns.Remove("KUNDEN_REF");
            tblTemp.Columns.Remove("KUNDEN_NOTIZ");

            // Kundendaten-Spalten ergänzen und füllen
            tblTemp.Columns.Add("Kundenname", typeof(string));
            foreach (DataRow kdRow in ((ZLD_Suche)Session["objZLDSuche"]).KundenDaten.Rows)
            {
                DataRow[] kRows = tblTemp.Select("KUNNR = '" + kdRow["KUNNR"] + "'");

                foreach (DataRow kRow in kRows)
                {
                    kRow["Kundenname"] = kdRow["NAME1"];
                }
            }

            // Spalten analog zur Anzeige ausblenden/umbenennen
            for (int i = tblTemp.Columns.Count - 1; i >= 0; i--)
            {
                found = false;

                foreach (GridTableView cGrid in RadGrid1.MasterTableView.DetailTables)
                {
                    if (cGrid.Name == "Orders")
                    {
                        foreach (GridColumn cCol in cGrid.Columns)
                        {
                            if ((!String.IsNullOrEmpty(cCol.SortExpression)) && (cCol.SortExpression.ToUpper() == tblTemp.Columns[i].ColumnName.ToUpper()))
                            {
                                found = true;

                                if (!cCol.Visible)
                                {
                                    tblTemp.Columns.Remove(tblTemp.Columns[i]);
                                }
                                else
                                {
                                    tblTemp.Columns[i].ColumnName = cCol.HeaderText;
                                }

                                break;
                            }
                        }
                    }
                }

                if ((!found) && (tblTemp.Columns[i].ColumnName != "Kundenname"))
                {
                    tblTemp.Columns.Remove(tblTemp.Columns[i]);
                }
            }

            tblTemp.AcceptChanges();

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, @"Documents\Vorlage_Auftragsstatistik.xls", 0, 0);
        }
    }
}