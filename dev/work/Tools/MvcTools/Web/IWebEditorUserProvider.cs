namespace MvcTools.Web
{
    public interface IWebEditorUserProvider
    {
        bool CurrentUserIsEditor { get; }
    }
}
