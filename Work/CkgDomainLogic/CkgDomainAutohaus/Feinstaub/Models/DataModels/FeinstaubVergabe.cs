﻿using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class FeinstaubVergabe
    {
        [Required]
        [StringLength(20)]
        [LocalizedDisplay("Kennzeichen")]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [Required]
        [LocalizedDisplay("Plakettenart")]
        public string Plakettenart { get; set; }
    }
}
