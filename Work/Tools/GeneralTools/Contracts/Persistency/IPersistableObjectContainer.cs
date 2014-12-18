namespace GeneralTools.Contracts
{
    public interface IPersistableObjectContainer
    {
        string ObjectKey { get; }

        string ObjectData { get; set; }
    }
}
