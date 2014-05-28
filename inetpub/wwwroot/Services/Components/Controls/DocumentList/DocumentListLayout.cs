namespace CKG.Components.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class DocumentListLayout : Control, INamingContainer
    {
        public string SelectedCountry { get; internal set; }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (args is CommandEventArgs)
            {
                base.RaiseBubbleEvent(source, args);
                return true;
            }

            return false;
        }
    }
}
