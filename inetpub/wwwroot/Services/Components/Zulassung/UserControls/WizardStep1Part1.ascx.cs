using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;
using System.Threading;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep1Part1 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep1Part1";

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            FillCountryDropDownList(page.DAL);
        }

        public void Save()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnCountryChanged(object sender, EventArgs e)
        {
            this.page.DAL.SelectedCountry = this.drpLand.SelectedValue;
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            var searchCriterias = new VehicleSearchCriterias();
            searchCriterias.Contract = txtVertragsnummer.Text;
            searchCriterias.ChassisNumber = txtFahrgestellnummer.Text;
            searchCriterias.ZB2No = txt_NummerZB2Suche.Text;
            this.Validate();

            if (Page.IsValid)
            {
                page.DAL.FindVehicles(searchCriterias);
                if ((!String.IsNullOrEmpty(page.DAL.SelectedCountry)) && (page.DAL.SelectedCountry != drpLand.SelectedValue))
                {
                    drpLand.SelectedValue = page.DAL.SelectedCountry;
                }
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