using System;

namespace MvcTools.Models
{
    public class SimpleFormDateRangePickerModel : FormControlModel
    {
        public bool UseDateRangeValue { get; set; }
        
        public string UseDateRangeProperty { get; set; }

        
        public DateTime? DateRangeStart { get; set; }

        public DateTime? DateRangeEnd { get; set; }
    }
}
