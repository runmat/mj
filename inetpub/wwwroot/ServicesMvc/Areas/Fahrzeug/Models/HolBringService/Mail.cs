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
    public class Mail 
    {
        public string SendMailResult { get; set; }
        // public string MailReceiver { get { return "Marco.Maihofer@kroschke.de"; } }
        public string MailReceiver { get { return "Hinrich.Braasch@kroschke.de"; } }

        public bool UploadedPdfExists { get; set; }
    }
}
