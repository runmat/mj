using System.Web;

namespace GeneralTools.Models
{
    public class GeneralEntity
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public IHtmlString TitleAsHtml { get { return new HtmlString(string.IsNullOrEmpty(Title) ? "" : Title); } }

        public IHtmlString BodyAsHtml { get { return new HtmlString(string.IsNullOrEmpty(Body) ? "" : Body); } }

        public string Tag { get; set; }
    }
}
