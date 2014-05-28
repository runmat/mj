using System;
using System.Web.UI;
using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep4Part1 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep4Part1";

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;

            page.DAL.PropertyChanged += DAL_PropertyChanged;
            FillCountryDropDownList(page.DAL);

            if (page.DAL.VersandAddress != null)
            {
                this.rblAdresse.SelectedValue = "Andere";
                this.FillAddressForm(page.DAL.VersandAddress);
                this.EnableInput(true);
            }

            if (this.rblAdresse.SelectedValue.Equals("Halter", StringComparison.Ordinal) && page.DAL.HalterAddress != null)
            {
                this.FillAddressForm(page.DAL.HalterAddress);
                this.EnableInput(false);
            }
            else if (this.rblAdresse.SelectedValue.Equals("Auftraggeber", StringComparison.Ordinal) && page.DAL.AuftraggeberAddress != null)
            {
                this.FillAddressForm(page.DAL.AuftraggeberAddress);
                this.EnableInput(false);
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
                    this.FillAddressForm(address);
                }
            }
        }

        public void Save()
        {
            if (this.rblAdresse.SelectedValue.Equals("Andere", StringComparison.Ordinal))
            {
                var address = new AddressData();
                address.Name1 = txtName.Text;
                address.Name2 = txtName2.Text;
                address.Street = txtStrasse.Text;
                address.ZipCode = txtPlz.Text;
                address.City = txtOrt.Text;
                address.Country = drpLand.SelectedValue;
                page.DAL.VersandAddress = address;
            }
            else
            {
                if (page.DAL.VersandAddress != null)
                {
                    page.DAL.VersandAddress = null;
                }

                page.DAL.VersandAdressTyp = this.rblAdresse.SelectedValue.Equals("Halter", StringComparison.Ordinal) ? VersandAdressTyp.Halter : VersandAdressTyp.Auftraggeber;
            }
        }

        private void FillAddressForm(AddressData address)
        {
            txtName.Text = address.Name1;
            txtName2.Text = address.Name2;
            txtStrasse.Text = address.Street;
            txtPlz.Text = address.ZipCode;
            txtOrt.Text = address.City;
            drpLand.SelectedValue = address.Country;
        }

        void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AddressData address = null;

            if (this.rblAdresse.SelectedValue.Equals("Halter", StringComparison.Ordinal))
            {
                if (e.PropertyName.Equals("HalterAddress", StringComparison.OrdinalIgnoreCase))
                {
                    address = page.DAL.HalterAddress;
                }
            }
            else if (this.rblAdresse.SelectedValue.Equals("Auftraggeber", StringComparison.Ordinal))
            {
                if (e.PropertyName.Equals("AuftraggeberAddress", StringComparison.OrdinalIgnoreCase))
                {
                    address = page.DAL.AuftraggeberAddress;
                }
            }
            else if (e.PropertyName.Equals("VersandAddress", StringComparison.OrdinalIgnoreCase))
            {
                address = page.DAL.VersandAddress;
            }

            if (address != null)
            {
                this.FillAddressForm(address);
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

        private void FillAddress()
        {
            page.DAL.VersandAddress = AddressSearch1.Address;
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
            if (!AddressSearch1.Search(ZulassungDal.VersandKennung, txtName.Text, txtPlz.Text, txtOrt.Text, out count, maxCount))
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

        private void EnableInput(bool enabled)
        {
            txtName.Enabled = enabled;
            txtName2.Enabled = enabled;
            txtOrt.Enabled = enabled;
            txtPlz.Enabled = enabled;
            txtStrasse.Enabled = enabled;
            drpLand.Enabled = enabled;
            btnSearch.Enabled = enabled;
         }

        protected void VersandAnChanged(object sender, EventArgs e)
        {
            bool enableInput = false;

            txtName.Text = string.Empty;
            txtName2.Text = string.Empty;
            txtStrasse.Text = string.Empty;
            txtPlz.Text = string.Empty;
            txtOrt.Text = string.Empty;
            drpLand.SelectedValue = "DE";

            AddressData address;

            if (this.rblAdresse.SelectedValue.Equals("Andere", StringComparison.Ordinal))
            {
                address = page.DAL.VersandAddress;
                enableInput = true;
            }
            else
            {
                page.DAL.VersandAddress = null;

                if (this.rblAdresse.SelectedValue.Equals("Halter", StringComparison.Ordinal))
                {
                    address = page.DAL.HalterAddress;
                    page.DAL.VersandAdressTyp = VersandAdressTyp.Halter;
                }
                else
                {
                    address = page.DAL.AuftraggeberAddress;
                    page.DAL.VersandAdressTyp = VersandAdressTyp.Auftraggeber;
                }
            }

            if (address != null)
            {
                this.FillAddressForm(address);
            }

            this.EnableInput(enableInput);
        }
    }
}