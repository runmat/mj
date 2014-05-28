using System;

namespace GeneralTools.UnitTests.ModelMappings
{
    public class AdvancedTestClass2
    {
        public int? IntPropertyDifferentName { get; set; }

        public bool? BoolPropertyDifferentName { get; set; }

        public string StringPropertyDifferentName { get; set; }

        public DateTime? DateTimePropertyDifferentName { get; set; }

        public string StringPropertyCopyChangedByAppCode { get; set; }

        public string StringPropertyCopyChangedByUserCode { get; set; }

        public string StringPropertyCopyBackChangedByAppCode { get; set; }

        public string StringPropertyCopyBackChangedByUserCode { get; set; }
    }
}
