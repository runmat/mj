using System;
using System.Windows.Forms;

namespace BahnCheck
{
    public partial class BahnCheckForm : Form
    {
        public BahnCheckForm()
        {
            InitializeComponent();

            lx.Text = DateTime.Now.ToString("HH:mm:ss        (dd.MM.yy)");
        }
    }
}
