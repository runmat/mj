using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;
using CKG.Components.Controls;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep2Part4 : System.Web.UI.UserControl, IWizardStepPart
    {
        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep2Part4";

        public void Validate()
        {
            // eVb-Nr. nur Pflicht, wenn Land "DE", sonst nicht validieren
            foreach (var contr in this.Parent.Parent.Parent.Controls)
            {
                if (contr is CollapsibleWizardControl)
                {
                    var wiz = (CollapsibleWizardControl)contr;
                    foreach (CollapsibleWizardStep step in wiz.Steps)
                    {
                        if (step.Title == "Halter")
                        {
                            var halterControl = (WizardStep2Part1)step.Content;
                            if (halterControl != null)
                            {
                                string land = halterControl.DrpLand.SelectedValue;
                                valEvbNo.Visible = (land.ToUpper() == "DE");
                                valEvbNo2.Visible = (land.ToUpper() == "DE");
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {
            var address = new AddressData();
            address.Name1 = txtVersicherungsgesellschaft.Text;
            page.DAL.VersichererAddress = address;

            page.DAL.EvbNo = txtEvbNo.Text;

            DateTime value;
            if (DateTime.TryParse(txtGueltigVon.Text, out value))
            {
                page.DAL.VersicherungFrom = value;
            }
            else
            {
                page.DAL.VersicherungFrom = null;
            }

            if (DateTime.TryParse(txtGueltigBis.Text, out value))
            {
                page.DAL.VersicherungUntil = value;
            }
            else
            {
                page.DAL.VersicherungUntil = null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            page.DAL.PropertyChanged += DAL_PropertyChanged;
            txtGueltigVon.Attributes.Add("readonly", "true");
            txtGueltigBis.Attributes.Add("readonly", "true");
        }

        void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("VersichererAddress", StringComparison.OrdinalIgnoreCase) && page.DAL.VersichererAddress != null)
            {
                txtVersicherungsgesellschaft.Text = page.DAL.VersichererAddress.Name1;
            }
            else if (e.PropertyName.Equals("EvbNo", StringComparison.OrdinalIgnoreCase) && page.DAL.EvbNo != null)
            {
                txtEvbNo.Text = page.DAL.EvbNo;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AddressSearch1 != null)
            {
                AddressSearch1.AddressSelected += AddressSearch1_AddressSelected;
            }
            if (AddressSearch2 != null)
            {
                AddressSearch2.AddressSelected += AddressSearch2_AddressSelected;
            }

            if (!String.IsNullOrEmpty(page.DAL.EvbNo))
            {
                if (String.IsNullOrEmpty(txtEvbNo.Text)==true)
                {
                    txtEvbNo.Text = page.DAL.EvbNo;
                }
               
            }
        }

        void AddressSearch2_AddressSelected(object sender, EventArgs e)
        {
            // hide overlay
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "HideSearchOverlay2", ModalOverlay2.HideOverlayClientScript, true);
            FillEvbNo();
        }

        protected void EvbButton_Click(object sender, EventArgs e)
        {

            int count;
            int maxCount = 200;

            if (!AddressSearch2.Search(ZulassungDal.EvbKennung, string.Empty, string.Empty, string.Empty, out count, maxCount))
            {
                    // show overlay
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "SearchOverlay2", ModalOverlay2.ShowOverlayClientScript, true);
            }
            else
            {
                FillEvbNo();
                ColorAnimationExtender2.StartAnimation();
            }
        }

        private void FillEvbNo()
        {
            page.DAL.EvbNo = AddressSearch2.Address.Description;
        }

        protected void VersichererButton_Click(object sender, EventArgs e)
        {
            int count = 0;
            int maxCount = 200;
            if (!AddressSearch1.Search(ZulassungDal.VersichererKennung, txtVersicherungsgesellschaft.Text, string.Empty, string.Empty, out count, maxCount))
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

        void AddressSearch1_AddressSelected(object sender, EventArgs e)
        {
            // hide overlay
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "HideSearchOverlay", ModalOverlay1.HideOverlayClientScript, true);
            FillAddress();
        }

        private void FillAddress()
        {
            page.DAL.VersichererAddress = AddressSearch1.Address;
        }
    }
}