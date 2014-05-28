namespace CKG.Components.Controls
{
    using System.Collections;

    public interface IDocumentGroup
    {
        string GroupName { get; }
        IList Documents { get; }
    }
}
