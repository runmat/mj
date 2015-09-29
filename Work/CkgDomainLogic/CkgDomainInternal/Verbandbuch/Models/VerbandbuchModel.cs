using System;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainInternal.Verbandbuch.Models
{
    public class VerbandbuchModel
    {
        [LocalizedDisplay(LocalizeConstants.AccidentNo)]
        public string AccidentNo { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.InjuredPerson)]
        public string InjuredPerson { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.TimeOfAccident)]
        public DateTime? DateOfAccident { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.PlaceOfAccident)]
        public string PlaceOfAccident { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.CourseOfAccident)]
        public string CourseOfAccident { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.WitnessName)]
        public string WitnessName { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Injury)]
        public string Injury { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.TimeOfFirstAid)]
        public DateTime? DateOfFirstAid { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.FirstAidMeasures)]
        public string FirstAidMeasures { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.FirstResponder)]
        public string FirstResponder { get; set; }

        public string TimeOfAccident { get; set; }

        public string TimeOfFirstAid { get; set; }

        public string Vkbur { get; set; }

    }
}