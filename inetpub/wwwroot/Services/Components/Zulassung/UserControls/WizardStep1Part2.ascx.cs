using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Threading;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep1Part2 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep1Part2";

        public void Validate()
        {
            // ZB2-Nummer nur Pflicht, wenn Textbox für Kunden sichtbar, sonst nicht validieren
            valNummerZB2.Visible = txt_NummerZB2Anlage.Visible;

            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            FillCountryDropDownList(page.DAL);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void buttonAdd_Click(object sender, EventArgs e)
        {
            var searchCriterias = new VehicleSearchCriterias();
            searchCriterias.Contract = txtVertragsnummer.Text;
            searchCriterias.ChassisNumber = txtFahrgestellnummer.Text;
            searchCriterias.ZB2No = txt_NummerZB2Anlage.Text;

            this.Validate();

            if (Page.IsValid)
            {
                page.DAL.AddVehicle(searchCriterias);
            }
        }

        private void FillCountryDropDownList(ZulassungDal dal)
        {
            var countries = dal.Countries;
            if (countries != null)
            {
                drpLand.DataSource = countries;
                drpLand.DataTextField = "FullDesc";
                drpLand.DataValueField = "Land1";
                drpLand.SelectedValue = "DE";
                drpLand.DataBind();
            }
        }
    }
}