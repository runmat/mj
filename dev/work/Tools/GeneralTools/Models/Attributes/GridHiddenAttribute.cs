using System.ComponentModel.DataAnnotations;

namespace GeneralTools.Models
{
    public class GridHiddenAttribute : ScaffoldColumnAttribute
    {
        public GridHiddenAttribute()
            : base(false)
        {
        }
    }
}
