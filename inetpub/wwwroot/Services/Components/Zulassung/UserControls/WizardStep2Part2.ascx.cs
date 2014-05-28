using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Components.Controls;

namespace CKG.Components.Zulassung.UserControls
{
    public partial class WizardStep2Part2 : System.Web.UI.UserControl, IWizardStepPart
    {
        protected const string ValidationGroup = "ZulassungStep2Part2";
        private IWizardPage page;

        protected override void OnPreRender(EventArgs e)
        {
            Common.SetEndASPXAccess(Page);

            //Nur füllen wenn noch kein Datum angegeben wurde
            if (string.IsNullOrEmpty( txtZulassungsdatum.Text.Trim()))
            {      
                if (page.DAL.ZulassungsDate > DateTime.MinValue)
                {
                    // Zulassungsdatum aus Coc-Daten, wenn vorhanden
                    txtZulassungsdatum.Text = page.DAL.ZulassungsDate.ToShortDateString();
                }
                else
                {
                    // Kundenspezifische Vorbelegung des Zulassungsdatums
                    // Hierfür werden die zu addierenden Tage (INTEGER) in der FELDÜBERSETZUNG label 'lbl_ZulDatAddDays' 
                    // als Übersetzungstext gesetzt

                    DateTime currentDat = DateTime.Now;
                    int addDays = 0;

                    if (int.TryParse(lbl_ZulDatAddDays.Text.Trim(), out addDays))
                    {
                        for (int i = 1; i <= addDays; i++)
                        {
                            if ((int)currentDat.AddDays(i).DayOfWeek == 6) // if Sonnabend
                            {
                                addDays++;
                            }

                            if ((int)currentDat.AddDays(i).DayOfWeek == 0) // if Sonntag
                            {
                                addDays++;
                            }
                        }

                        txtZulassungsdatum.Text = currentDat.AddDays(addDays).ToShortDateString();
                    }
                }
            }
        }

        public void Validate()
        {
            // Zulassungskreis nur Pflicht, wenn Land "DE", sonst nicht validieren
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
                                valZulassungskreis.Visible = (land.ToUpper() == "DE");
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
            if (drpZulassungsart.SelectedIndex > 0)
            {
                page.DAL.ZulassungsTyp = drpZulassungsart.SelectedItem.Text;
            }
            page.DAL.ZulassungsKreis = txtZulassungskreis.Text;
            page.DAL.ZulassungsDate = DateTime.Parse(txtZulassungsdatum.Text);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (IWizardPage)Page;
            txtZulassungsdatum.Attributes.Add("readonly", "true");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (drpZulassungsart.Items.Count == 0)
            {
                var dataSource = page.DAL.Services;
                dataSource.DefaultView.RowFilter = "ASNUM = '' AND KTEXT1_H2 = ''";
                dataSource.DefaultView.Sort = "EXTGROUP ASC";

                drpZulassungsart.DataSource = dataSource.DefaultView;
                drpZulassungsart.DataValueField = "EXTGROUP";
                drpZulassungsart.DataTextField = "KTEXT1_H1";
                drpZulassungsart.DataBind();
            }
            // Ersten Eintrag vorselektieren, falls noch nichts ausgewählt wurde
            if ((drpZulassungsart.Items.Count > 0) && (drpZulassungsart.SelectedIndex <= 0))
            {
                drpZulassungsart.SelectedValue = page.DAL.DefaultZulassungsart;
            }

            if (String.IsNullOrEmpty(this.txtZulassungskreis.Text))
            {
                this.Button1_Click(null, EventArgs.Empty);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var step2 = this.NamingContainer.Parent as IStep2;
            string postcode, town;

            if (step2 == null)
            {
                postcode = String.Empty;
                town = String.Empty;
            }
            else
            {
                postcode = step2.Postcode;
                town = step2.Town;
            }

            if (!String.IsNullOrEmpty(postcode) || !String.IsNullOrEmpty(town))
            {
                page.DAL.UpdateStva(postcode, town);
                txtZulassungskreis.Text = page.DAL.Stva;
                this.UpdatePanel1.Update();
            }
        }
    }
}