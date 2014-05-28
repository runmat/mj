using System;
using System.ComponentModel;
using System.Data;

using CKG.Components.Zulassung.DAL;

namespace CKG.Components.Zulassung.UserControls
{
    public class AddressSearch : Controls.AddressSearch
    {
        private IWizardPage wizardPage;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public AddressData Address { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string KundenDebitorNummer { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.wizardPage = (IWizardPage)Page;
        }

        protected override DataTable DoSearch(string addressType, string name, string postcode, string town)
        {
            return this.wizardPage.DAL.SearchAddress(addressType, name, postcode, town);
        }

        protected override void ExtractData(DataRow addressRow)
        {
            this.Address = new AddressData()
            {
                Name1 = addressRow.Field<string>("NAME1"),
                Name2 = addressRow.Field<string>("NAME2"),
                Street = addressRow.Field<string>("STRAS"),
                ZipCode = addressRow.Field<string>("PSTLZ"),
                City = addressRow.Field<string>("ORT01"),
                Description = addressRow.Field<string>("POS_Text"),
            };

            this.KundenDebitorNummer = addressRow.Field<string>("SAPNR");
        }
    }
}