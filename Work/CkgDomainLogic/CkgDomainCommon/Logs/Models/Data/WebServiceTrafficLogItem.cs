using System;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Logs.Models
{
    public class WebServiceTrafficLogItem
    {
        [GridHidden]
        public int Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string Type { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime Time_Stamp { get; set; }

        [LocalizedDisplay(LocalizeConstants.Content)]
        public string AllXml { get; set; }

        [NotMapped]
        [LocalizedDisplay(LocalizeConstants.Content)]
        public string AllXmlPreview
        {
            get
            {
                var strTemp = AllXml;

                if (String.IsNullOrEmpty(strTemp))
                    return "";

                var ixStart = strTemp.IndexOf("<soap:Body>");

                if (ixStart > -1)
                {
                    strTemp = strTemp.Substring(ixStart + 11);

                    var ixEnd = strTemp.LastIndexOf("</soap:Body>");

                    if (ixEnd > -1)
                    {
                        strTemp = strTemp.Substring(0, ixEnd);
                    }
                }
                return strTemp.Substring(0, (Math.Min(100, strTemp.Length)));
            }
        }
    }
}
