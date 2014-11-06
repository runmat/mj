using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public static class StringExtensions
    {
        public static SelectList ToSelectList(this string separatedList, char separator = ';')
        {
            var values = separatedList.Split(separator);
            for (var i = 0; i < values.Length; i++)
                values[i] = values[i].Trim();

            return values.Select(item => item.Trim()).ToSelectList();
        }

        public static IHtmlString ToHtml(this string s, string lineSeparator = ";")
        {
            s = s.NotNullOrEmpty();
            if (lineSeparator != null && s.Contains(lineSeparator))
                s = s.Replace(lineSeparator, "<br />");

            return new HtmlString(s);
        }
    }
}
