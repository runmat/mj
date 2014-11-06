using System;
using GeneralTools.Models;

namespace GeneralTools.UnitTests.ModelMappings
{
    [Serializable]
    class TestClass2
    {
        public int IntProperty { get; set; }

        public int? NullableIntProperty { get; set; }

        
        public long LongProperty { get; set; }

        public long? NullableLongProperty { get; set; }


        public bool BoolProperty { get; set; }

        public bool? NullableBoolProperty { get; set; }

        public string BoolPropertyToConvert { get; set; }

        public bool BoolPropertyToConvertBack { get; set; }


        public DateTime DateTimeProperty { get; set; }

        public DateTime? NullableDateTimeProperty { get; set; }


        public string StringProperty1 { get; set; }

        public string StringProperty2 { get; set; }

        public string StringProperty3 { get; set; }

        [ModelMappingCompareIgnore]
        public string StringPropertyModelMappingCompareIgnore { get; set; }

        [ModelMappingCopyIgnore]
        public string StringPropertyModelMappingCopyIgnore { get; set; }

        [ModelMappingClearable]
        public string StringPropertyModelMappingClearable { get; set; }
    }
}
