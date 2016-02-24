using GeneralTools.Models;

namespace MvcTools.Models
{
    public class TileSimpleModel : GeneralEntity
    {
        public string CssClass { get; set; }

        public string IconCssClass { get; set; }

        public string OnClickAction { get; set; }

        public string BodyCssClass { get; set; }

        public string TitleCssClass { get; set; }
    }
}
