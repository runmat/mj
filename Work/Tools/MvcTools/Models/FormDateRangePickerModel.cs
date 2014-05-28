using System.Web.Mvc;

namespace MvcTools.Models
{
    public class FormDateRangePickerModel : FormControlModel
    {
        public string DateRangeProperty { get; set; }

        public MvcHtmlString ControlHtmlStartDate { get; set; }

        public MvcHtmlString ControlHtmlEndDate { get; set; }
    }
}
