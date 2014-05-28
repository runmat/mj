using System.ComponentModel.DataAnnotations;

namespace GeneralTools.Models
{
    public class DisplayFormatDateAttribute : DisplayFormatAttribute
    {
        public DisplayFormatDateAttribute()
        {
            DataFormatString = "{0:d}";
        }
    }
}
