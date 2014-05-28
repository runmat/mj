namespace CKG.Components.Controls
{
    public class DocumentListGroup : DocumentListLayout
    {
        public int Index { get; private set; }
        public string GroupName { get; internal set; }
        public int GroupCount { get; internal set; }

        internal DocumentListGroup(int index)
        {
            this.Index = index;
        }
    }
}