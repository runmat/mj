using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    //public enum Berechtigungen { Freigeben,  Sperren, Entsperren }

    public class Treuhandberechtigung
    {        
        //[LocalizedDisplay(LocalizeConstants.Approval)]
        public string Freigeben { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Approval)]
        public string Sperren { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Approval)]
        public string Entsperren { get; set; }

        
    }
}
