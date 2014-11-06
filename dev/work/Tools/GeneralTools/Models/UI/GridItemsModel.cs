using System.Collections.Generic;

namespace GeneralTools.Models
{
    public class GridItemsModel<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

        public string ControllerName { get; set; }

        public string AjaxSelectAction { get; set; }

        public int? FormID { get; set; }
        public int? ItemID { get; set; }
    }
}
