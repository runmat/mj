namespace CKG.Components.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class DocumentListItemCommandEventArgs : CommandEventArgs
    {
        public DocumentListItemCommandEventArgs(CommandEventArgs e)
            : base(e)
        { }

        public DocumentListDataItem Item { get; internal set; }
        public object CommandSource { get; internal set; }
    }
}