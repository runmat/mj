using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Upload 
    {
        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string UploadFileName { get; set; }

        public string UploadServerFileName { get; set; }

        public byte[] PdfBytes { get; set; }
    }
}
