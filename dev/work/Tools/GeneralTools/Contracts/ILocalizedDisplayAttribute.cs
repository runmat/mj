namespace GeneralTools.Contracts
{
    public interface ILocalizedDisplayAttribute
    {
        string ResourceID { get; }
        object Suffix { get; }
    }
}
