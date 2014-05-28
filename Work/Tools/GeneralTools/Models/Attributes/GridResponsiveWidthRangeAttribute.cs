using System;

namespace GeneralTools.Models
{
    public enum GridResponsive { Smartphone = 1, SmartphoneOrWider = 11, Tablet = 2, TabletOrWider = 22, Workstation = 3 }
    
    public class GridResponsiveVisibleAttribute : Attribute  
    {
        public GridResponsive Visibility { get; private set; }

        public string CssClassName { get { return Visibility.ToString("F").ToLowerAndHyphens(); } }

        /// <summary>
        /// Responsive Grid Column visibility based on css media tags
        /// </summary>
        /// <param name="visibility">Smartphone, Tablet or Workstation</param>
        public GridResponsiveVisibleAttribute(GridResponsive visibility)
        {
            Visibility = visibility;
        }
    }
}
