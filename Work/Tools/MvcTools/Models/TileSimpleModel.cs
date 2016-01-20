using System.Collections.Generic;
using GeneralTools.Models;

namespace MvcTools.Models
{
    public class TileSimpleModel : GeneralEntity
    {
        public string CssClass { get; set; }

        public string IconCssClass { get; set; }

        public string Href { get; set; }

        public IDictionary<string, object> ControlHtmlAttributes { get; set; }

        public string BodyCssClass { get; set; }

        public string TitleCssClass { get; set; }
    }
}
