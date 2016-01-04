using System;

namespace CarDocu.Contracts
{
    public interface IAutoCompleteTagCloudConsumer
    {
        void OnRequestProcessTag(string tagText);

        void OnDeleteTag(string tagText, bool isPrivateTag);

        Action AfterDeleteAction { get; set; }
    }
}
