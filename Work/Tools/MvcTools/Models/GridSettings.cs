using GeneralTools.Services;

namespace MvcTools.Models
{
    public class GridSettings : Store
    {
        public string Columns { get; set; }

        public string OrderBy { get; set; }
        
        public string FilterBy { get; set; }
        
        public string GroupBy { get; set; }
    }
}
