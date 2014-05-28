namespace CKG.Components.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class DocumentListDataItem : DocumentListLayout, IDataItemContainer
    {
        public int Index { get; private set; }
        public object Document { get; internal set; }

        internal DocumentListDataItem(int index)
        {
            this.Index = index;
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (args is CommandEventArgs)
            {
                base.RaiseBubbleEvent(source, new DocumentListItemCommandEventArgs((CommandEventArgs)args) { CommandSource = source, Item = this});
                return true;
            }

            return false;
        }

        object IDataItemContainer.DataItem { get { return this.Document; } }
        int IDataItemContainer.DataItemIndex { get { return this.Index; } }
        int IDataItemContainer.DisplayIndex { get { return this.Index; } }
    }
}