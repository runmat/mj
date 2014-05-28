using System;
using System.Web.UI;
using CKG.Components.Zulassung.DAL;
using System.Data;
using System.Web.UI.WebControls;


namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep4Part2 : System.Web.UI.UserControl, IWizardStepPart
    {
        #region declarations

        private IWizardPage page;
        protected const string ValidationGroup = "ZulassungStep4Part2";
        private bool showMe = false; //Wenn True dann wird dieses Control angezeigt

        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;


        #endregion

        #region PageEvents

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            FillPartnerRG();
            page.DAL.PropertyChanged += DAL_PropertyChanged;
            ddlPartnerRG.Enabled = true;

        }

        protected override void OnPreRender(EventArgs e)
        {

            Controls.CollapsibleWizardControl myWizard = (Controls.CollapsibleWizardControl)this.Parent.Parent;
            if (!showMe)
            {
                myWizard.Steps[1].Enabled = false;  
            }
            else
            {
                myWizard.Steps[1].Enabled = true;  
            }
    
        }

        #endregion

        #region methods

        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void Save()
        {



        }

        protected void ddlPartnerRG_SelectedIndexChanged(object sender, EventArgs e)
         {


             DataView dv = this.page.DAL.Partner.DefaultView;

            if (ddlPartnerRG.SelectedItem.Value.Equals("0"))
            {
                this.txtRzAnsprechpartner.Text = "";
                this.txtRzFirma.Text = "";
                this.txtRzOrt.Text = "";
                this.txtRzPLZ.Text = "";
                this.txtRzStrasse.Text = "";
                this.txtRzTelefon.Text = "";
                page.DAL.Regulierer = "";

            }else
            {

                int rowIndex = 0;

                foreach (DataRow row in dv.Table.Rows)
	            {
                  
                    if (row["PARVW"].Equals("RG") && row["KUNNR"].Equals(ddlPartnerRG.SelectedItem.Value))
	                {


                        page.DAL.Regulierer = ddlPartnerRG.SelectedItem.Value.TrimStart('0');

                        this.txtRzAnsprechpartner.Text = row["NAME2"].ToString();
                        this.txtRzFirma.Text = row["NAME1"].ToString();
                        this.txtRzOrt.Text = row["CITY1"].ToString();
                        this.txtRzPLZ.Text = row["POST_CODE1"].ToString();
                        this.txtRzStrasse.Text = row["STREET"].ToString() + " " + row["HOUSE_NUM1"].ToString();
                        this.txtRzTelefon.Text = row["TEL_NUMBER"].ToString();
                        
                        break;
	                }
                    rowIndex++;
                 }

            }
         
            }

        void DAL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }


        private void FillPartnerRG()
        {

            string RowFilterValue = "RG";
            DataView dv  =this.page.DAL.Partner.DefaultView;
            int e = 1;
            int currentRowIndex = -1;
            int selIndex= -1;

            dv.RowFilter = "PARVW = '" + RowFilterValue + "'";
            //Kein Regulierer in SAP gefunden -> Abbrechen (Actuelle AG wird als Regulierer genommen)
            if (dv.Count < 1)
            {
                page.DAL.Regulierer = page.DAL.KNDNummer;
                return;
            }

            //Nur ein Regulierer gefunden -> Diesen setzen. (Auswahlcontroll bleibt unsichtbar)
            if (dv.Count == 1)
            {
                 foreach (DataRow row in dv.Table.Rows)
	             {
                     if (row["PARVW"].Equals(RowFilterValue))
                     {
                         page.DAL.Regulierer = row["KUNNR"].ToString().TrimStart('0') ;
                         return;
                     }
                }
                return;
            }


            //Mehrere Regulierer in SAP gefunden -> Auswahlcontrol füllen und anzeigen 

            showMe = true;

            if (Session["SELRGINDEX"] != null)
            {
                selIndex = (int)Session["SELRGINDEX"];
            } 

            ddlPartnerRG.Items.Add(new ListItem("Bitte auswählen", "0"));
            
            foreach (DataRow row in dv.Table.Rows)
	        {
                currentRowIndex++;

                if (!row["PARVW"].Equals(RowFilterValue))
                {
                    continue;
                }

                ddlPartnerRG.Items.Add(new ListItem(row["NAME1"].ToString() + ", " + row["CITY1"].ToString() ,row["KUNNR"].ToString()));

                if ((selIndex < 0 && row["DEFPA"].Equals("X")) || (selIndex >=0 && currentRowIndex== selIndex))
	            {
		            ddlPartnerRG.Items[e].Selected = true;
                    page.DAL.Regulierer = row["KUNNR"].ToString().TrimStart('0');
                    this.txtRzAnsprechpartner.Text = row["NAME2"].ToString();
                    this.txtRzFirma.Text = row["NAME1"].ToString();
                    this.txtRzOrt.Text = row["CITY1"].ToString();
                    this.txtRzPLZ.Text = row["POST_CODE1"].ToString();
                    this.txtRzStrasse.Text = row["STREET"].ToString() + " " + row["HOUSE_NUM1"].ToString();
                    this.txtRzTelefon.Text = row["TEL_NUMBER"].ToString();

	            }

                e++;
	        }
      
           }//end fillPartner

#endregion
    }
}