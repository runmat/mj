namespace GeneralTools.Models
{
    public class SelectItem
    {
        [SelectListKey]
        public string Key { get; set; }

        [SelectListText]
        public string Text { get; set; }

        public SelectItem(string key, string text)
        {
            Key = key;
            Text = text;
        }
    }
}
