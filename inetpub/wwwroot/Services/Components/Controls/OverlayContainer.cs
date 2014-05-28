using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CKG.Components.Controls
{
    public class OverlayContainer : Control, INamingContainer 
    {
        private string closeScript;

        public string CloseScript
        {
            get { return closeScript; }
            set { closeScript = value; }
        }
    }
}
