namespace MvcTools
{
    public static class DecimalExtensions
    {
        public static string ToPrice(this decimal value)
        {
            return string.Format("{0:c}", value);
        }

        public static string ToPrice(this decimal? value)
        {
            if (value == null) return "";
            return value.GetValueOrDefault().ToPrice();
        }
    }
}
