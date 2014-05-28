using System;
using System.Web.UI;
using CKG.Components.Zulassung.DAL;
using System.Web.UI.WebControls;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep2Part1 : UserControl, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep2Part1";

        private bool IsInitialised
        {
            get { return ((bool?) ViewState["IsInitialised"]).GetValueOrDefault(); }
            set { ViewState["IsInitialised"] = value; }
        }

        // Accessor, um im folgenden Step darauf zugreifen zu können
        internal DropDownList DrpLand
        {
            get { return drpLand; }
        }

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
            var address = new AddressData
                              {
                                  Name1 = txtName.Text,
                                  Name2 = txtName2.Text,
                                  Street = txtStrasse.Text,
                                  ZipCode = txtPlz.Text,
                                  City = txtOrt.Text,
                                  Country = drpLand.SelectedValue
                              };
            page.DAL.HalterAddress = address;
            page.DAL.KundenDebitorNummer = hKundenDebitorNummer.Value;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            lblTipHead.InnerText = "Tipp!";
            lblTipMsg.InnerText = "Geben Sie Filterwerte an um die Suche zu" + Environment.NewLine +
                                  "beschleunigen (Bsp. a*) ";


            page = (IWizardPage) Page;
            page.DAL.PropertyChanged += DAL_PropertyChanged;
            FillCountryDropDownList(page.DAL);
        }

        private void FillAddressForm()
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

            if (!string.IsNullOrEmpty(page.DAL.KundenDebitorNummer))
                hKundenDebitorNummer.Value = page.DAL.KundenDebitorNummer;
        }

        private void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("HalterAddress", StringComparison.OrdinalIgnoreCase) &&
                page.DAL.HalterAddress != null)
            {
                FillAddressForm();
            }
        }

        private void AddressSearch1_AddressSelected(object sender, EventArgs e)
        {
            // hide overlay
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "HideSearchOverlay",
                                                ModalOverlay1.HideOverlayClientScript, true);
            FillAddress();


        }

        private void FillAddress()
        {
            page.DAL.HalterAddress = AddressSearch1.Address;
            page.DAL.KundenDebitorNummer = AddressSearch1.KundenDebitorNummer;
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

            if (!IsInitialised)
            {
                FillAddressForm();

                // Land aus Coc-Daten vorbelegen, wenn vorhanden
                if ((!String.IsNullOrEmpty(page.DAL.SelectedCountry)) && (page.DAL.SelectedCountry != drpLand.SelectedValue))
                {
                    drpLand.SelectedValue = page.DAL.SelectedCountry;
                }

                IsInitialised = true;
            }

            var step2 = NamingContainer.Parent as IStep2;

            if (step2 != null)
            {
                step2.Postcode = txtPlz.Text.Trim();
                step2.Town = txtOrt.Text.Trim();
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            DoAdressSearch();
        }

        private void DoAdressSearch()
        {

            int count = 0;
            int maxCount = 200;
            if (
                !AddressSearch1.Search(ZulassungDal.HalterKennung, txtName.Text, txtPlz.Text, txtOrt.Text, out count,
                                       maxCount))
            {

                if (count > maxCount)
                {
                    lblTipHead.InnerText = "Achtung!";
                    lblTipHead.Style.Add("color", "Red");
                    lblTipMsg.InnerText = "Es wurden mehr als " + maxCount +
                                          " Datensätze gefunden! Bitte grenzen Sie die Suche ein.";
                    lblTipMsg.Style.Add("color", "Red");
                }
                else
                {
                    lblTipHead.Style.Remove("color");
                    lblTipMsg.Style.Remove("color");
                    lblTipHead.InnerText = "Tipp!";
                    lblTipMsg.InnerText = "Geben Sie Filterwerte an um die Suche zu" + Environment.NewLine +
                                          "beschleunigen (Bsp. a*) ";

                    // show overlay
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "SearchOverlay",
                                                        ModalOverlay1.ShowOverlayClientScript, true);
                }
            }
            else
            {
                FillAddress();
                ColorAnimationExtender1.StartAnimation();
            }
        }
    }
}