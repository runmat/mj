using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep2Part5 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep2Part5";

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
            if (chk_Halter.Checked)
            {
                if (page.DAL.VersicherungsnehmerAddress != null)
                {
                    page.DAL.VersicherungsnehmerAddress = null;
                }
            }
            else
            {
                var address = new AddressData();
                address.Name1 = txtName.Text;
                address.Name2 = txtName2.Text;
                address.Street = txtStrasse.Text;
                address.ZipCode = txtPlz.Text;
                address.City = txtOrt.Text;
                address.Country = drpLand.SelectedValue;
                page.DAL.VersicherungsnehmerAddress = address;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            page.DAL.PropertyChanged += DAL_PropertyChanged;
            FillCountryDropDownList(page.DAL);
        }

        void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (chk_Halter.Checked)
            {
                if (e.PropertyName.Equals("HalterAddress", StringComparison.OrdinalIgnoreCase) && page.DAL.HalterAddress != null)
                {
                    var address = page.DAL.HalterAddress;

                    if (address != null)
                    {
                        txtName.Text = address.Name1;
                        txtName2.Text = address.Name2;
                        txtStrasse.Text = address.Street;
                        txtPlz.Text = address.ZipCode;
                        txtOrt.Text = address.City;
                        drpLand.SelectedValue = address.Country;
                    }
                }
            }
            else if (e.PropertyName.Equals("VersicherungsnehmerAddress", StringComparison.OrdinalIgnoreCase) && page.DAL.VersicherungsnehmerAddress != null)
            {
                var address = page.DAL.VersicherungsnehmerAddress;

                if (address != null)
                {
                    txtName.Text = address.Name1;
                    txtName2.Text = address.Name2;
                    txtStrasse.Text = address.Street;
                    txtPlz.Text = address.ZipCode;
                    txtOrt.Text = address.City;
                    drpLand.SelectedValue = address.Country;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AddressSearch1 != null)
            {
                AddressSearch1.AddressSelected += AddressSearch1_AddressSelected;
            }

            if (!Page.IsPostBack)
            {
                var address = page.DAL.HalterAddress;

                if (address != null)
                {
                    txtName.Text = address.Name1;
                    txtName2.Text = address.Name2;
                    txtStrasse.Text = address.Street;
                    txtPlz.Text = address.ZipCode;
                    txtOrt.Text = address.City;
                    drpLand.SelectedValue = address.Country;
                }
            }
        }

        private void FillAddress()
        {
            page.DAL.VersicherungsnehmerAddress = AddressSearch1.Address;
        }

        void AddressSearch1_AddressSelected(object sender, EventArgs e)
        {
            // hide overlay
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "HideSearchOverlay", ModalOverlay1.HideOverlayClientScript, true);
            FillAddress();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int count = 0;
             int maxCount = 200;
            if (!AddressSearch1.Search(ZulassungDal.HalterKennung, txtName.Text, txtPlz.Text, txtOrt.Text, out count,maxCount))
            {
                // show overlay
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "SearchOverlay", ModalOverlay1.ShowOverlayClientScript, true);
            }
            else
            {
                FillAddress();
                ColorAnimationExtender1.StartAnimation();
            }
        }

        protected void Halter_Check(object sender, EventArgs e)
        {
            var enableInput = !chk_Halter.Checked;

            txtName.Text = string.Empty;
            txtName2.Text = string.Empty;
            txtStrasse.Text = string.Empty;
            txtPlz.Text = string.Empty;
            txtOrt.Text = string.Empty;
            drpLand.SelectedValue = "DE";

            if (chk_Halter.Checked)
            {
                page.DAL.VersicherungsnehmerAddress = null;

                var address = page.DAL.HalterAddress;

                if (address != null)
                {
                    txtName.Text = address.Name1;
                    txtName2.Text = address.Name2;
                    txtStrasse.Text = address.Street;
                    txtPlz.Text = address.ZipCode;
                    txtOrt.Text = address.City;
                    drpLand.SelectedValue = address.Country;
                }
            }
            else
            {
                var address = page.DAL.VersicherungsnehmerAddress;

                if (address != null)
                {
                    txtName.Text = address.Name1;
                    txtName2.Text = address.Name2;
                    txtStrasse.Text = address.Street;
                    txtPlz.Text = address.ZipCode;
                    txtOrt.Text = address.City;
                    drpLand.SelectedValue = address.Country;
                }
            }

            txtName.Enabled = enableInput;
            txtName2.Enabled = enableInput;
            txtOrt.Enabled = enableInput;
            txtPlz.Enabled = enableInput;
            txtStrasse.Enabled = enableInput;
            drpLand.Enabled = enableInput;
            btnSearch.Enabled = enableInput;
        }
    }
}