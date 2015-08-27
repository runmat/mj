using System.Collections.Generic;
using System.Web;

namespace GeneralTools.Models
{
    public class GeneralSummary
    {
        public bool FullWidthRows { get; set; }

        public string Header { get; set; }
        public IHtmlString HeaderAsHtml { get { return new HtmlString(string.IsNullOrEmpty(Header) ? "" : Header); } }

        public string Footer { get; set; }
        public IHtmlString FooterAsHtml { get { return new HtmlString(string.IsNullOrEmpty(Footer) ? "" : Footer); } }

        public List<GeneralEntity> Items { get; set; }

        public bool IsMassenzulassung { get; set; } // MMA
    }
}
