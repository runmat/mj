using System.Collections.Generic;
using GeneralTools.Models;

namespace ServicesMvc.Areas.DataConverter.Models
{
    /// <summary>
    /// Properties, die in den unterschiedlichen Partials als Datenquelle verwendet werden können
    /// </summary>
    public class GlobalViewData
    {
        public List<SelectItem> DateTransformations
        {
            get
            {
                var newList = new List<SelectItem>
                {
                    new SelectItem("0", "Originales Datumsformat"),
                    new SelectItem("1", "YYYYMMDD"),
                    new SelectItem("2", "DDMMYYYY"),
                    new SelectItem("3", "MMDDYYYY"),
                    new SelectItem("4", "YYYY*MM*DD"),
                    new SelectItem("5", "DD*MM*YYYY"),
                    new SelectItem("6", "MM*DD*YYYY")
                };

                return newList;  
            }
        }
    }
}