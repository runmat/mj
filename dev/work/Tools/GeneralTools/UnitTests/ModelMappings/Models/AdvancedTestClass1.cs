using System;

namespace GeneralTools.UnitTests.ModelMappings
{
    public class AdvancedTestClass1
    {
        public int? IntProperty { get; set; }

        public bool? BoolProperty { get; set; }

        public string StringProperty { get; set; }

        public DateTime? DateTimeProperty { get; set; }

        public string StringPropertyCopyChangedByAppCode { get; set; }

        public string StringPropertyCopyChangedByUserCode { get; set; }

        public string StringPropertyCopyBackChangedByAppCode { get; set; }

        public string StringPropertyCopyBackChangedByUserCode { get; set; }
    }
}
