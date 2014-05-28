using System;
using System.Web.UI;
using CKG.Components.Zulassung.DAL;
using System.Data;
using System.Web.UI.WebControls;


namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep4Part3 : System.Web.UI.UserControl, IWizardStepPart
    {

        #region declarations

        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep4Part3";
        private bool showMe = false; //Wenn True dann wird dieses Control angezeigt

        #endregion

        #region PageEvents

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            FillPartnerRE();
            page.DAL.PropertyChanged += DAL_PropertyChanged;
            ddlPartnerRE.Enabled = true;
            
        }

        protected override void OnPreRender(EventArgs e)
        {

            Controls.CollapsibleWizardControl myWizard = (Controls.CollapsibleWizardControl)this.Parent.Parent;
            if (!showMe)
            {
                myWizard.Steps[2].Enabled = false;
            }
            else
            {
                myWizard.Steps[2].Enabled = true;
            }

        }

        #endregion

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {

        }

        protected void ddlPartnerRE_SelectedIndexChanged(object sender, EventArgs e)
        {


            DataView dv = this.page.DAL.Partner.DefaultView;

            if (ddlPartnerRE.SelectedItem.Value.Equals("0"))
            {
                this.txtArAnsprechpartner.Text = "";
                this.txtArFirma.Text = "";
                this.txtArOrt.Text = "";
                this.txtArPLZ.Text = "";
                this.txtArStrasse.Text = "";
                this.txtArTelefon.Text = "";
                page.DAL.Empfänger = "";

            }
            else
            {

                int rowIndex = 0;

                foreach (DataRow row in dv.Table.Rows)
                {

                    if (row["PARVW"].Equals("RE") && row["KUNNR"].Equals(ddlPartnerRE.SelectedItem.Value))
                    {

                        page.DAL.Empfänger = ddlPartnerRE.SelectedItem.Value.TrimStart('0') ;

                        this.txtArAnsprechpartner.Text = row["NAME2"].ToString();
                        this.txtArFirma.Text = row["NAME1"].ToString();
                        this.txtArOrt.Text = row["CITY1"].ToString();
                        this.txtArPLZ.Text = row["POST_CODE1"].ToString();
                        this.txtArStrasse.Text = row["STREET"].ToString() + " " + row["HOUSE_NUM1"].ToString();
                        this.txtArTelefon.Text = row["TEL_NUMBER"].ToString();

                        break;
                    }
                    rowIndex++;
                }

            }

        }

        void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }


        private void FillPartnerRE()
        {

            string RowFilterValue = "RE";
            DataView dv = this.page.DAL.Partner.DefaultView;
            int e = 1;
            int currentRowIndex = -1;
            int selIndex = -1;
            dv.RowFilter = "PARVW = '" + RowFilterValue + "'";


            //Kein Empfänger in SAP gefunden -> Abbrechen (Actuelle AG wird als Regulierer genommen)
            if (dv.Count < 1)
            {
                page.DAL.Empfänger = page.DAL.KNDNummer;
                return;
            }

            //Nur ein Empfänger gefunden -> Diesen setzen. (Auswahlcontroll bleibt unsichtbar)
            if (dv.Count == 1)
            {
                foreach (DataRow row in dv.Table.Rows)
                {
                    if (row["PARVW"].Equals(RowFilterValue))
                    {
                        page.DAL.Empfänger = row["KUNNR"].ToString().TrimStart('0');
                        return;
                    }
                }
                return;
            }

            //Mehrere Empfänger in SAP gefunden -> Auswahlcontrol füllen und anzeigen 

            showMe = true;

            if (Session["SELRGINDEX"] != null)
            {
                selIndex = (int)Session["SELRGINDEX"];
            }

            ddlPartnerRE.Items.Add(new ListItem("Bitte auswählen", "0"));

            foreach (DataRow row in dv.Table.Rows)
            {
                currentRowIndex++;

                if (!row["PARVW"].Equals(RowFilterValue))
                {
                    continue;
                }

                ddlPartnerRE.Items.Add(new ListItem(row["NAME1"].ToString() + ", " + row["CITY1"].ToString(), row["KUNNR"].ToString()));

                if ((selIndex < 0 && row["DEFPA"].Equals("X")) || (selIndex >= 0 && currentRowIndex == selIndex))
                {
                    ddlPartnerRE.Items[e].Selected = true;
                    page.DAL.Empfänger = row["KUNNR"].ToString().TrimStart('0');
                    this.txtArAnsprechpartner.Text = row["NAME2"].ToString();
                    this.txtArFirma.Text = row["NAME1"].ToString();
                    this.txtArOrt.Text = row["CITY1"].ToString();
                    this.txtArPLZ.Text = row["POST_CODE1"].ToString();
                    this.txtArStrasse.Text = row["STREET"].ToString() + " " + row["HOUSE_NUM1"].ToString();
                    this.txtArTelefon.Text = row["TEL_NUMBER"].ToString();

                }

                e++;
            }

        }//end fillPartner



    }
}