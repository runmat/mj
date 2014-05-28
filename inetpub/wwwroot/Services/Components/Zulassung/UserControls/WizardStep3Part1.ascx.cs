using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep3Part1 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;
        private const string script = @"{0}
                                        $(document).ready(function(){{
	                                    $('.multiselect li').mouseenter(function() {{
		                                    for(var i=0; i<serviceData.length; i++) {{
			                                    if($(this).attr('id') == serviceData[i][0]) 
			                                    {{
				                                    $('#infopanel #header').html($(this).html());
				                                    $('#infopanel #description').html(serviceData[i][2]);
				                                    $('#infopanel #price').html(serviceData[i][1]);
				                                    return;
			                                    }}
		                                    }}
		                                    $('#infopanel #header').html('{1}');
		                                    $('#infopanel #description').html('{2}');
		                                    $('#infopanel #price').html('');
	                                    }});
                                    }});";
        protected const string ValidationGroup = "ZulassungStep3Part1";

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("DIENSTL_NR", typeof(string));
            dataTable.Columns.Add("DIENSTL_TEXT", typeof(string));
            dataTable.Columns.Add("MATNR", typeof(string));
            foreach (var id in MultiselectBox1.SelectedValues)
            {
                var row = dataTable.NewRow();
                //var id = listItem.Value;

                var dataRow = page.DAL.Services.Select(string.Format("ASNUM = '{0}'", id)).FirstOrDefault();

                if (dataRow != null)
                {
                    row["DIENSTL_NR"] = id;
                    row["DIENSTL_TEXT"] = dataRow["ASKTX"];
                    row["MATNR"] = dataRow["EAN11"];

                    dataTable.Rows.Add(row);
                }
            }
            page.DAL.SelectedServices = dataTable;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Register client scripts
            var dataArrays = new List<string>();
            foreach (DataRowView row in page.DAL.Services.DefaultView)
            {
                if ((row["TBTWR"] != null && Convert.ToInt32(row["TBTWR"]) > 0) || !string.IsNullOrEmpty(row["Description"] as string))
                {
                    var price = Convert.ToInt32(row["TBTWR"]);
                    dataArrays.Add(string.Format("new Array('{0}','{1}','{2}')", row["ASNUM"], ((price == 0) ? "gratis" : price.ToString("C")), row["Description"]));
                }
            }
            var array = string.Format("var serviceData = new Array({0});", string.Join(",", dataArrays.ToArray()));
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "infoscript", string.Format(script, array, "Tipp!", "Nutzen Sie Drag & Drop um Dienstleistungen der Auswahl hinzuzufügen oder zu entfernen."), true);

            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var transportType = 1;
            var dataSource = page.DAL.Services;
            dataSource.DefaultView.RowFilter = string.Format("EXTGROUP='{0}' AND ASNUM <> '' AND KTEXT1_H2 = ''", transportType);


            //MultiselectBox1.SelectedValues = new[] { "000000000003000005", "000000000003000018" };

            MultiselectBox1.DataSource = dataSource.DefaultView;
            MultiselectBox1.DataTextField = "ASKTX";
            MultiselectBox1.DataValueField = "ASNUM";
            MultiselectBox1.DataBind();

            if (Session[_Default.InitMultiSessionKey] == null)
            {
                Session[_Default.InitMultiSessionKey] = true;

                string sVWAG = string.Empty;
                string sASNUM = string.Empty; 

                foreach (ListItem li in MultiselectBox1.Items)
                {
                    foreach (DataRow row in dataSource.Rows)
                    {

                        sVWAG = row["VW_AG"].ToString().Trim();
                        sASNUM = row["ASNUM"].ToString();

                        if (!string.IsNullOrEmpty(sVWAG) && sASNUM.Equals(li.Value))
                        {
                            li.Selected = true;
                        }
                    }

                    //var oldSelectedValues = MultiselectBox1.SelectedValues.ToList();
                    //var preselect = page.DAL.Services.Select("VW_AG='X'").Select(r => (string)r["ASNUM"]);
                    //MultiselectBox1.SelectedValues = preselect.Union(oldSelectedValues).ToList();
                }
            }
        }
    }
}