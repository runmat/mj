namespace GeneralTools.Contracts
{
    public interface IKeyValueStore<T>
    {
        T GetValue(string key);

        void SetValue(string key, T value);
    }
}
