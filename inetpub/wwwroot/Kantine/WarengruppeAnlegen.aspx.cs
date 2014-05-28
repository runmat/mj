using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace Kantine.Warengruppe
{
	public partial class WarengruppeAnlegen : KantinePage
	{
        //DB_Kantine DB;// = new DB_Kantine(ConfigurationManager.ConnectionStrings["CurrentCon"].ConnectionString);
        //KantinenBenutzer KB;
        //Kantine Kan;

		DataTable dt;
		int WarengruppenID;

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    Kan = (Kantine)this.Master;
        //    KB = Kan.User;
        //    DB = Kan.Database;
        //}

		protected void Page_Load(object sender, EventArgs e)
		{
            //if (Session["Benutzer"] != null)
            //{
                //KantinenBenutzer KB = (KantinenBenutzer)Session["Benutzer"];
                if (KB != null)
                {
                    if (!(KB.IsAdmin))
                    {
                        Response.Redirect("Default.aspx");
                    }
                    else { }
                }
            //}

			if (Session["MaterialgroupToEdit"] != null)
			{
				WarengruppenID = Convert.ToInt32(Session["MaterialgroupToEdit"]);
				lblÜberschrift.Text = "Warengruppe bearbeiten";
				dt = DB.GetWarengruppeAllByID(WarengruppenID);
				
				if (!Page.IsPostBack)
				{
					FillDynamicData();
				}
			}
		}

		private void FillDynamicData()
		{
			if (dt.Rows.Count > 0)
			{
				txtWarengruppenBezeichnung.Text = dt.Rows[0]["BezeichnungWarengruppe"].ToString();
			}
		}

		protected void btnSpeichern_Click(object sender, EventArgs e)
		{				
			if (Session["MaterialgroupToEdit"] != null) //Wenn null neue Warengruppe
			{
				if ((bool)Session["WarBezChg"])
				{
					DB.UpdateUniversal("Warengruppen", "WarengruppeID", WarengruppenID, "BezeichnungWarengruppe", txtWarengruppenBezeichnung.Text);
				}
			}
			else
			{
				if (txtWarengruppenBezeichnung.Text != "")
				{
					DB.AddWarengruppe(txtWarengruppenBezeichnung.Text);
				}
			}

			Session["MaterialgroupToEdit"] = null;
			Session["WarBezChg"] = null;

			Response.Redirect("./Warengruppe.aspx");
		}

		protected void btnBack_Click(object sender, EventArgs e)
		{
			Session["MaterialgroupToEdit"] = null;
			Session["WarBezChg"] = null;

			Response.Redirect("./Warengruppe.aspx");
		}

		protected void txtWarengruppenBezeichnung_TextChanged(object sender, EventArgs e)
		{
			Session["WarBezChg"] = true;
		}
	}
}
