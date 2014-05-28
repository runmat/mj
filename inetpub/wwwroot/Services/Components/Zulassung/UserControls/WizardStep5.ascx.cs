using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Components.Zulassung.DAL;
using System.IO;
using System.Data;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep5 : System.Web.UI.UserControl, IWizardStep
    {
        protected const string ValidationGroup = "ZulassungStep5";
        IWizardPage page;
        bool dataLoaded = false;


        public void Validate()
        {
            Page.Validate(ValidationGroup);
        }

        public void ResetNavigation()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            GridNavigation1.setGridElment(ref GridView1);
            GridNavigation1.PagerChanged += PageIndexChanged;
            GridNavigation1.PageSizeChanged += PageSizeChanged;
        }

        protected string ResolveDownloadUrl(string fileName)
        {
            string filepath = "~\\Components\\Zulassung\\Dokumente\\TempDoc\\" + page.User.KUNNR;
            filepath = Path.Combine(filepath, fileName);
            return Page.ResolveClientUrl(filepath);
        }

        private void PageIndexChanged(int pageIndex)
        {
            FillGrid(pageIndex);
        }

        private void PageSizeChanged()
        {
            FillGrid(GridView1.PageIndex);
        }

        protected override void OnPreRender(EventArgs e)
        {

            var dal = page.DAL;
            lblReg.Text = "";
            lblEmpf.Text = "";
            if (string.IsNullOrEmpty(dal.Regulierer) )
            {
                dal.Regulierer = dal.KNDNummer; 
            }

            if (string.IsNullOrEmpty(dal.Empfänger))
            {
                dal.Empfänger = dal.KNDNummer; 
            }


            lblErst.Text = dal.Ersteller;
            string tmp= string.Empty;
            foreach (DataRow r in  dal.Partner.Rows)
            {
                tmp = dal.Regulierer.PadLeft(10, '0');

                if (r["KUNNR"].ToString().Equals(tmp) && r["PARVW"].ToString().Equals("RG"))
                {
                     lblReg.Text = r["NAME1"] + " (" + dal.Regulierer + ")";
                    continue;
                }
                tmp = dal.Empfänger.PadLeft(10, '0');
                if (r["KUNNR"].ToString().Equals(tmp) && r["PARVW"].ToString().Equals("RE"))
                {
                    lblEmpf.Text = r["NAME1"] + " (" + dal.Empfänger + ")";
                    continue;
                }
            }
            //Wenn Name nicht gefunden dann nur die Nummer ausgeben

            if (string.IsNullOrEmpty(lblReg.Text)) lblReg.Text = dal.Regulierer;
            if (string.IsNullOrEmpty(lblEmpf.Text)) lblEmpf.Text = dal.Empfänger;
            

            var halter = dal.HalterAddress;
            if (halter != null)
            {
                HalterName.Text = halter.Name1;
                HalterName2.Text = halter.Name2;
                HalterOrt.Text = halter.City;
                HalterPlz.Text = halter.ZipCode;
                HalterStrasse.Text = halter.Street;
                HalterLand.Text = halter.Country;
            }

            var versicherung = dal.VersichererAddress;
            if (versicherung != null)
            {
                VersicherungName.Text = versicherung.Name1;
            }
            VersicherungEvbNr.Text = dal.EvbNo ?? string.Empty;
            VersicherungVon.Text = dal.VersicherungFrom.HasValue ? dal.VersicherungFrom.Value.ToShortDateString() : String.Empty; ;
            VersicherungBis.Text = dal.VersicherungUntil.HasValue ? dal.VersicherungUntil.Value.ToShortDateString() : String.Empty; ;

            var versicherter = dal.VersicherungsnehmerAddress ?? dal.HalterAddress;
            if (versicherter != null)
            {
                VersicherterName.Text = versicherter.Name1;
                VersicherterName2.Text = versicherter.Name2;
                VersicherterOrt.Text = versicherter.City;
                VersicherterPlz.Text = versicherter.ZipCode;
                VersicherterStrasse.Text = versicherter.Street;
                VersicherterLand.Text = versicherter.Country;
            }

            var versand = dal.VersandAddress ?? (dal.VersandAdressTyp == DAL.VersandAdressTyp.Halter ? dal.HalterAddress : dal.AuftraggeberAddress);
            if (versand != null)
            {
                VersandName.Text = versand.Name1;
                VersandName2.Text = versand.Name2;
                VersandOrt.Text = versand.City;
                VersandPlz.Text = versand.ZipCode;
                VersandStrasse.Text = versand.Street;
                VersandLand.Text = versand.Country;
            }

            var documents = dal.Protokollarten;
            if (documents != null)
            {
                documents.DefaultView.RowFilter = "Filename <> ''";
                Documents.DataSource = documents;
                Documents.DataBind();
                documents.DefaultView.RowFilter = string.Empty;
            }

            Services.DataSource = dal.SelectedServices;
            Services.DataBind();

            Zulassungsart.Text = dal.ZulassungsTyp;
            Zulassungsdatum.Text = dal.ZulassungsDate.ToShortDateString();
            Zulassungskreis.Text = dal.ZulassungsKreis;

            if (!dataLoaded)
            {
                FillGrid(GridView1.PageIndex);
            }

            var dokumente = dal.FindVorhandeneDokumente();

            if (dokumente != null)
            {
                this.DokumenteKopf.Visible = true;
                this.DokumenteDetail.Visible = true;

                this.cbxVollmancht.Checked = dokumente.Vollmacht;
                this.cbxRegister.Checked = dokumente.Register;
                this.cbxPerso.Checked = dokumente.Perso;
                this.cbxGewerbe.Checked = dokumente.Gewerbe;
                this.cbxEinzug.Checked = dokumente.Einzug;
                this.cbxKarte.Checked = dokumente.Karte;
                this.cbxVollst.Checked = dokumente.Vollst;
            }

            base.OnPreRender(e);
        }

        private void FillGrid(int pageIndex)
        {
            var dataView = page.DAL.SelectedVehicles;
            if (dataView != null)
            {
                GridView1.PageIndex = pageIndex;
                GridView1.DataSource = dataView;
                GridView1.Visible = true;
                GridNavigation1.Visible = (dataView.Count > GridView1.PageSize);
                GridView1.DataBind();
                dataLoaded = true;
                return;
            }
        }

        public event EventHandler<EventArgs> Completed;

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
                
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            OnNavigateBack(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> NavigateBack;

        void OnNavigateBack(object sender, EventArgs e)
        {
            if (NavigateBack != null)
            {
                NavigateBack(this, EventArgs.Empty);
            }
        }
    }
}