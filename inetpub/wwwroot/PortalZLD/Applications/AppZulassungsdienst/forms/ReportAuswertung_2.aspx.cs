using System;
using System.Data;
using System.IO;
using System.Linq;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using Telerik.Web.UI;
using AppZulassungsdienst.lib;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Listenansicht Auswertung.
    /// </summary>
    public partial class ReportAuswertung_2 : System.Web.UI.Page
    {
        private User m_User;
        private App m_App;
        private Listen objListe;

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";

            if (Session["objListe"] != null)
            {
                objListe = (Listen)Session["objListe"];

                if (!IsPostBack)
                {
                    Fillgrid();
                }
            }
            else
            {
                SearchMode();
                lblError.Text = "Keine Daten übergeben!";
            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Fillgrid()
        {
            if (objListe.Auswertung.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Daten zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        private void SearchMode(bool search = true)
        {
            Result.Visible = !search;
        }

        /// <summary>
        /// Bei Änderung der Auswahl FillGrid() aufrufen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void rbAuswahl_SelectedIndexChanged(object sender, EventArgs e)
        {
            rgGrid1.Rebind();
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (objListe.Auswertung != null)
            {
                rgGrid1.DataSource = objListe.Auswertung.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }

            // Gruppierungen/Sortierungen setzen
            rgGrid1.MasterTableView.GroupByExpressions.Clear();

            switch (rbAuswahl.SelectedValue)
            {
                case "1":
                    // nach Kennzeichen
                    rgGrid1.MasterTableView.GroupByExpressions.Add("ZZKENN Kennzeichen GROUP BY ZZKENN");
                    break;
                case "2":
                    // nach Datum/Amt
                    rgGrid1.MasterTableView.GroupByExpressions.Add("ZZZLDAT Zulassungsdatum GROUP BY ZZZLDAT");
                    rgGrid1.MasterTableView.GroupByExpressions.Add("KREISKZ Amt GROUP BY KREISKZ");
                    break;
                case "3":
                    // nach Datum/Amt/EC
                    rgGrid1.MasterTableView.GroupByExpressions.Add("ZZZLDAT Zulassungsdatum GROUP BY ZZZLDAT");
                    rgGrid1.MasterTableView.GroupByExpressions.Add("KREISKZ Amt GROUP BY KREISKZ");
                    rgGrid1.MasterTableView.GroupByExpressions.Add("EC_JN EC GROUP BY EC_JN");
                    break;
                case "4":
                    // nach Kundennummer
                    rgGrid1.MasterTableView.GroupByExpressions.Add("KUNNR Kundennummer GROUP BY KUNNR");
                    break;
                case "5":
                    // nach Dienstleistung
                    rgGrid1.MasterTableView.GroupByExpressions.Add("MAKTX Dienstleistung GROUP BY MAKTX");
                    break;
            }
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportAuswertung.aspx?AppID=" + Session["AppID"].ToString() + "&BackFromList=X");
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            String FilePath;
            DataRow[] RowTemp;

            switch (e.CommandName)
            {
                case "PrintBarquittung":
                    RowTemp = objListe.Auswertung.Select("ZULBELN= " + e.CommandArgument);

                    if (RowTemp.Length > 0)
                    {
                        String Barqnr = RowTemp[0]["BARQ_NR"].ToString();
                        String EasyID = RowTemp[0]["OBJECT_ID"].ToString();

                        if (!String.IsNullOrEmpty(EasyID))
                        {
                            objListe.GetBarqFromEasy(Session["AppID"].ToString(), Session.SessionID, this, Barqnr,
                                                     EasyID);
                            if (objListe.Status != 0)
                            {
                                lblError.Text = objListe.Message;
                                return;
                            }
                        }
                        else
                        {
                            objListe.Filename = Barqnr + ".pdf";
                        }
                        if (m_User.IsTestUser)
                        {
                            FilePath = "\\\\192.168.10.96\\test\\portal\\barquittung\\" + objListe.Filename;
                        }
                        else
                        {
                            FilePath = "\\\\192.168.10.96\\prod\\portal\\barquittung\\" + objListe.Filename;
                        }
                        Session["App_ContentType"] = "Application/pdf";
                        Session["App_Filepath"] = FilePath;
                        if (EasyID.Length > 0)
                        {
                            Session["App_FileDelete"] = "X";
                        }
                        ResponseHelper.Redirect("Printpdf.aspx", "_blank",
                                                "left=0,top=0,resizable=YES,scrollbars=YES");
                    }
                    break;

                case "PrintSofortabrechnung":
                    RowTemp = objListe.Auswertung.Select("ZULBELN= " + e.CommandArgument);

                    if (RowTemp.Length > 0)
                    {
                        objListe.Filename = RowTemp[0]["SA_PFAD"].ToString().TrimStart('/').Replace('/', '\\');

                        if (m_User.IsTestUser)
                        {
                            FilePath = "\\\\192.168.10.96\\test\\portal\\sofortabrechnung\\" + objListe.Filename;
                        }
                        else
                        {
                            FilePath = "\\\\192.168.10.96\\prod\\portal\\sofortabrechnung\\" + objListe.Filename;
                        }

                        if (File.Exists(FilePath))
                        {
                            Session["App_ContentType"] = "Application/pdf";
                            Session["App_Filepath"] = FilePath;

                            ResponseHelper.Redirect("Printpdf.aspx", "_blank",
                                                    "left=0,top=0,resizable=YES,scrollbars=YES");
                        }
                    }
                    break;
            }
        }

        protected void rgGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridGroupFooterItem)
            {
                GridGroupFooterItem groupFooter = (GridGroupFooterItem)e.Item;
                GridGroupHeaderItem groupHeader = rgGrid1.MasterTableView.GetItems(GridItemType.GroupHeader)
                    .First(i => i.GroupIndex == groupFooter.GroupIndex) as GridGroupHeaderItem;

                // Default-Gruppenfooter-Farbe (falls von den u.g. Bedingungen keine zutreffen)
                groupFooter.BackColor = System.Drawing.Color.FromArgb(250, 255, 191);

                string gruppenBez = groupHeader.DataCell.Text;

                if ((!String.IsNullOrEmpty(gruppenBez)) && (gruppenBez.Contains(":")))
                {
                    string gruppenName = gruppenBez.Substring(0, gruppenBez.IndexOf(':'));

                    // wenn die Gruppierungen (s.o.) geändert werden, müssen ggf. auch die Farbdefinitionen angepasst werden
                    switch (gruppenName)
                    {
                        case "Kennzeichen":
                        case "Zulassungsdatum":
                        case "Kundennummer":
                        case "Dienstleistung":
                            groupFooter.BackColor = System.Drawing.Color.FromArgb(250, 255, 191);
                            break;
                        case "Amt":
                            groupFooter.BackColor = System.Drawing.Color.FromArgb(210, 215, 191);
                            break;
                        case "EC":
                            groupFooter.BackColor = System.Drawing.Color.FromArgb(250, 255, 161);
                            break;
                    }
                }
            }
        }

        protected void rgGrid1_DataBound(object sender, EventArgs e)
        {
            // Zeilen mit gleicher ID gleich färben
            if (rgGrid1.Items.Count > 0)
            {
                String myId = rgGrid1.Items[0]["ZULBELN"].Text;
                String Css = "ItemStyle";

                foreach (GridDataItem row in rgGrid1.Items)
                {
                    if ((row.ItemType == GridItemType.Item) || (row.ItemType == GridItemType.AlternatingItem))
                    {
                        if (row["ZULBELN"].Text == myId)
                        {
                            row.CssClass = Css;
                        }
                        else
                        {
                            if (Css == "ItemStyle")
                            {
                                Css = "GridTableAlternate2";
                            }
                            else
                            {
                                Css = "ItemStyle";
                            }
                            row.CssClass = Css;
                            myId = row["ZULBELN"].Text;
                        }
                    }
                }
            }
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
            DataTable tblTemp = objListe.Auswertung.Copy();

            // Spalten entfernen, die nicht exportiert werden sollen/können
            tblTemp.Columns.Remove("BARQ_NR");
            tblTemp.Columns.Remove("SA_PFAD");

            // Spalten analog zur Anzeige ausblenden/umbenennen
            for (int i = tblTemp.Columns.Count - 1; i >= 0; i--)
            {
                bool found = false;

                foreach (GridColumn cCol in rgGrid1.Columns)
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

                if (!found)
                {
                    tblTemp.Columns.Remove(tblTemp.Columns[i]);
                }
            }

            tblTemp.AcceptChanges();

            // Sortierung setzen
            DataView dvExport = tblTemp.DefaultView;

            switch (rbAuswahl.SelectedValue)
            {
                case "1":
                    // nach Kennzeichen
                    dvExport.Sort = "Kennzeichen, ID";
                    break;
                case "2":
                    // nach Datum/Amt
                    dvExport.Sort = "Zulassungsdatum, StVA, ID";
                    break;
                case "3":
                    // nach Datum/Amt/EC
                    dvExport.Sort = "Zulassungsdatum, StVA, EC, ID";
                    break;
                case "4":
                    // nach Kundennummer
                    dvExport.Sort = "Kundennr, ID";
                    break;
                case "5":
                    // nach Dienstleistung
                    dvExport.Sort = "Dienstleistung, ID";
                    break;
                default:
                    // Übersicht
                    // keine Gruppierung
                    dvExport.Sort = "ID";
                    break;
            }

            CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            string filename = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now) + m_User.UserName;
            excelFactory.CreateDocumentAndSendAsResponse(filename, dvExport.ToTable(), this.Page, false, @"Applications\AppZulassungsdienst\Documents\Vorlage_Auswertung.xls", 0, 0);
        }

    }
}
