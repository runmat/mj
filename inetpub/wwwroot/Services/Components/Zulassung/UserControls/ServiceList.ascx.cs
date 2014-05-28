using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class ServiceList : System.Web.UI.UserControl
    {
        private IWizardPage page;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
        }

        protected DataTable Services
        {
            get
            {
                return page.DAL.Services;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var transportType = 1;
            var dataSource = Services;
            dataSource.DefaultView.RowFilter = string.Format("EXTGROUP='{0}' AND ASNUM <> '' AND KTEXT1_H2 = ''", transportType);

            grvDL.Columns[2].Visible = false;
            grvDL.Columns[3].Visible = false;
            foreach (DataRow row in dataSource.Rows)
            {
                if (!grvDL.Columns[2].Visible && !string.IsNullOrEmpty(row["TBTWR"] as string) && Convert.ToDecimal(row["TBTWR"]) > 0)
                {
                    grvDL.Columns[2].Visible = true;
                }

                if (!grvDL.Columns[3].Visible && !string.IsNullOrEmpty(row["Description"] as string))
                {
                    grvDL.Columns[3].Visible = true;
                }
            }

            grvDL.DataSource = dataSource;
            grvDL.DataBind();
        }
    }
}