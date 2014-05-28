using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kantine
{
	public partial class Zahlenfeld : System.Web.UI.UserControl
	{
		public enum Feldmodi { Rechner, Zahlen };
		Feldmodi mod;

		public delegate void CommitEventHandler(object sender, EventArgs e);
		
		public event CommitEventHandler CommitEvent;

		public decimal Value
		{ get; set; }

		public Feldmodi Modus
		{
			set{mod = value;} 
		}
		
		protected void OnCommitEvent(EventArgs e)
		{
			CommitEvent(this, e);
		}

		public void ChangeVisibility()
		{
			changeState();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			switch (mod) 
			{ 
				case Feldmodi.Rechner:
					tblRechner.Visible = true;
					tblZahlen.Visible = false;
					break;
				case Feldmodi.Zahlen:
					tblRechner.Visible = false;
					tblZahlen.Visible = true;
					break;
				default:
					tblRechner.Visible = true;
					tblZahlen.Visible = false;
					break;
			}
		}

		protected void btnNumber_Click(object sender, EventArgs e)
		{
			Button Btn = (Button)sender;
			txtZahlenfeld.Text += Btn.Text;
		}
		
		protected void btnClear_Click(object sender, EventArgs e)
		{
			txtZahlenfeld.Text = "";
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			if (txtZahlenfeld.Text != null)
			{
				txtZahlenfeld.Text = txtZahlenfeld.Text.Remove(txtZahlenfeld.Text.Length - 1);
			}
		}

		protected void btnPlus_Click(object sender, EventArgs e)
		{
			if (txtZahlenfeld.Text.StartsWith("-"))
			{
				txtZahlenfeld.Text = txtZahlenfeld.Text.Replace('-', '+');
			}
			else if (txtZahlenfeld.Text.StartsWith("+"))
			{ }
			else { txtZahlenfeld.Text = txtZahlenfeld.Text.Insert(0, "+"); }
		}

		protected void btnMinus_Click(object sender, EventArgs e)
		{
			if(txtZahlenfeld.Text.StartsWith("+"))
			{
				txtZahlenfeld.Text = txtZahlenfeld.Text.Replace('+', '-');
			}
			else if (txtZahlenfeld.Text.StartsWith("-"))
			{}
			else { txtZahlenfeld.Text = txtZahlenfeld.Text.Insert(0, "-"); }
		}

		protected void btnPoint_Click(object sender, EventArgs e)
		{
			if(txtZahlenfeld.Text.Contains(','))
			{}
			else{txtZahlenfeld.Text += ',';}
		}

		protected void btnClose_Click(object sender, EventArgs e)
		{
			changeState();
		}

		protected void btnEnter_Click(object sender,EventArgs e)
		{
			try
			{				
				//Value = Math.Round(Convert.ToDecimal(txtZahlenfeld.Text), 2);
				Value = decimal.Round(Convert.ToDecimal(txtZahlenfeld.Text), 2);				
			}
			catch
			{
				Value = 0.00m;
			}			
			EventArgs args = new EventArgs();
			OnCommitEvent(args);

			changeState();
		}

		//protected void ibtnCalculator_Click(object sender, ImageClickEventArgs e)
		//{
		//    changeState();
		//}

		protected void changeState()
		{
			if (Rechner.Visible)
			{
				Rechner.Visible = false;
				txtZahlenfeld.Text = "";
			}
			else 
			{
				Rechner.Visible = true;
			}
		}
	}
}