using System;

namespace CarDocu.Contracts
{
    public interface IAutoCompleteTagCloudConsumer
    {
        void OnRequestProcessTag(string tagText);

        void OnDeleteTag(string tagText);

        Action AfterDeleteAction { get; set; }
    }
}
