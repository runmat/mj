using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using CKG.Base.Kernel.Common;


namespace AppMBB.elements
{
    public sealed class ExcelDataEventArgs : EventArgs
    {
        public DataTable ExcelData { get; set; }
    }

    public partial class ExcelDownload : System.Web.UI.UserControl
    {
        public event EventHandler<ExcelDataEventArgs> CreatingExcelData;

        public string UserName { get; set; }
        public string RelatedGridViewId { get; set; }

        protected void OnCreateExcel(object sender, EventArgs e)
        {
            var eh = this.CreatingExcelData;

            if (eh == null)
            {
                throw new InvalidOperationException("CreatingExcelData Ereignis muss behandelt werden");
            }

            var args = new ExcelDataEventArgs();

            eh(this, args);

            if (args.ExcelData == null)
            {
                throw new InvalidOperationException("CreatingExcelData Ereignishändler muss ExcelData setzen");
            }

            var tblTemp = args.ExcelData.Copy();
            var AppURL = this.Request.Url.LocalPath.Replace("/Services", "..");
            var tblTranslations = (DataTable)this.Session[AppURL];

            var gv = this.NamingContainer.FindControl(this.RelatedGridViewId) as GridView;

            if (gv == null)
            {
                throw new InvalidOperationException("RelatedGridViewId muss eine gültige GridView identifizieren");
            }

            foreach (DataControlField col in gv.Columns)
            {
                for (var i = tblTemp.Columns.Count - 1; i >= 0; i -= 1)
                {
                    var bVisibility = 0;
                    var col2 = tblTemp.Columns[i];

                    if (col2.ColumnName.ToUpper() == col.SortExpression.ToUpper())
                    {
                        var translatedName = Common.TranslateColLbtn(gv, tblTranslations, col.HeaderText, ref bVisibility);

                        if (bVisibility == 0)
                        {
                            tblTemp.Columns.Remove(col2);
                        }
                        else if (translatedName.Length > 0)
                        {
                            col2.ColumnName = translatedName;
                        }
                    }
                }

                tblTemp.AcceptChanges();
            }

            var excelFactory = new CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory();
            var filename = String.Format("{0:yyyyMMdd_HHmmss}_{1}", System.DateTime.Now, this.UserName);
            excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, this.Page, false, null, 0, 0);
        }
    }
}