namespace GeneralTools.Contracts
{
    public interface ITranslationFormatService
    {
        string GetTranslation(string resource);
        string GetTranslationKurz(string resource);
        string GetFormat(string resource);
    }
}
