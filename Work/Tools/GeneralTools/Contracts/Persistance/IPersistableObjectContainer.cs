namespace GeneralTools.Contracts
{
    public interface IPersistableObjectContainer : IPersistableObject
    {
        string OwnerKey { get; set; }

        string GroupKey { get; set; }

        string ObjectType { get; set; }

        string ObjectData { get; set; }

        object Object { get; set; }
    }
}
