using System;
using GeneralTools.Models;

namespace GeneralTools.UnitTests.ModelMappings
{
    [Serializable]
    public class TestClass1Initialized
    {
        //
        // !! Diese Initial Werte bitte nicht ändern !!
        //
        private int _intProperty = 1;
        private int? _nullableIntProperty = 1;
        private long _longProperty = 1;
        private long? _nullableLongProperty = 1;
        private bool _boolProperty = true;
        private bool? _nullableBoolProperty = true;
        private DateTime _dateTimeProperty = DateTime.Today;
        private DateTime? _nullableDateTimeProperty = DateTime.Today;
        private string _stringProperty1 = "test";
        private string _stringProperty2 = "test";
        private string _stringProperty3 = "test";
        private string _stringPropertyModelMappingComparisonIgnore = "test";
        private string _stringPropertyModelMappingClearable = "test";
        private string _stringPropertyModelMappingCopyIgnore = "test";
        private string _boolPropertyToConvertBack = "X";
        private bool _boolPropertyToConvert = true;

        public int IntProperty
        {
            get { return _intProperty; }
            set { _intProperty = value; }
        }

        public int? NullableIntProperty
        {
            get { return _nullableIntProperty; }
            set { _nullableIntProperty = value; }
        }


        public long LongProperty
        {
            get { return _longProperty; }
            set { _longProperty = value; }
        }

        public long? NullableLongProperty
        {
            get { return _nullableLongProperty; }
            set { _nullableLongProperty = value; }
        }


        public bool BoolProperty
        {
            get { return _boolProperty; }
            set { _boolProperty = value; }
        }

        public bool? NullableBoolProperty
        {
            get { return _nullableBoolProperty; }
            set { _nullableBoolProperty = value; }
        }

        public bool BoolPropertyToConvert
        {
            get { return _boolPropertyToConvert; }
            set { _boolPropertyToConvert = value; }
        }

        public string BoolPropertyToConvertBack
        {
            get { return _boolPropertyToConvertBack; }
            set { _boolPropertyToConvertBack = value; }
        }


        public DateTime DateTimeProperty
        {
            get { return _dateTimeProperty; }
            set { _dateTimeProperty = value; }
        }

        public DateTime? NullableDateTimeProperty
        {
            get { return _nullableDateTimeProperty; }
            set { _nullableDateTimeProperty = value; }
        }


        public string StringProperty1
        {
            get { return _stringProperty1; }
            set { _stringProperty1 = value; }
        }

        public string StringProperty2
        {
            get { return _stringProperty2; }
            set { _stringProperty2 = value; }
        }

        public string StringProperty3
        {
            get { return _stringProperty3; }
            set { _stringProperty3 = value; }
        }

        [ModelMappingCompareIgnore]
        public string StringPropertyModelMappingCompareIgnore
        {
            get { return _stringPropertyModelMappingComparisonIgnore; }
            set { _stringPropertyModelMappingComparisonIgnore = value; }
        }

        [ModelMappingCopyIgnore]
        public string StringPropertyModelMappingCopyIgnore
        {
            get { return _stringPropertyModelMappingCopyIgnore; }
            set { _stringPropertyModelMappingCopyIgnore = value; }
        }

        [ModelMappingClearable]
        public string StringPropertyModelMappingClearable
        {
            get { return _stringPropertyModelMappingClearable; }
            set { _stringPropertyModelMappingClearable = value; }
        }
    }
}
