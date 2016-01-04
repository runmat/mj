using System;

namespace CarDocu.Contracts
{
    public interface IAutoCompleteTagCloudConsumer
    {
        void OnDropDownTabKey(string tagText);

        void OnDeleteTag(string tagText);

        Action AfterDeleteAction { get; set; }
    }
}
