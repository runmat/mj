namespace GeneralTools.Models
{
    public class SelectItem
    {
        [SelectListKey]
        public string Key { get; set; }

        [SelectListText]
        public string Text { get; set; }

        public string Group { get; set; }

        public SelectItem()
        {
        }

        public SelectItem(string key, string text)
        {
            Key = key;
            Text = text;
        }

        public SelectItem(string key, string text, string group)
        {
            Key = key;
            Text = text;
            Group = group;
        }
    }
}
