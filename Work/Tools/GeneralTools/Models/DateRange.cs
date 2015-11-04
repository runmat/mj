using System;
using System.Linq;
using System.Xml.Serialization;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class DateRange : INullable
    {
        private DateRangeType _rangeType;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private DateRangePresets _dateRangePresets;

        public bool IsSelected { get; set; }

        [XmlIgnore]
        DateRangePresets DateRangePresets
        {
            get { return (_dateRangePresets ?? (_dateRangePresets = new DateRangePresets())); }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _rangeType = DateRangeType.None;
                _startDate = value;
                TrySetDateRangeFromIndividualRange(_startDate.GetValueOrDefault(), _endDate.GetValueOrDefault());
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _rangeType = DateRangeType.None;
                _endDate = value;
                TrySetDateRangeFromIndividualRange(_startDate.GetValueOrDefault(), _endDate.GetValueOrDefault());
            }
        }

        public DateRangeType RangeType
        {
            get { return _rangeType; }
            set { SetRangeType(value); }
        }


        public DateRange()
        {
        }

        public DateRange(DateRangeType dateRangeType, bool isSelected = false)
        {
            RangeType = dateRangeType;
            IsSelected = isSelected;
        }

        public DateRange(DateTime startDate, DateTime endDate, bool isSelected = false)
        {
            RangeType = DateRangeType.None;
            StartDate = startDate;
            EndDate = endDate;
            IsSelected = isSelected;
        }

        private void SetRangeType(DateRangeType dateRangeType)
        {
            _rangeType = dateRangeType;

            TrySetIndividualRangeFromActualPresets(dateRangeType);
        }

        public bool IsNull()
        {
            return !IsSelected;
        }

        public bool MoreDaysThan(int days)
        {
            return (IsSelected && (EndDate.GetValueOrDefault() - StartDate.GetValueOrDefault()).TotalDays > days);
        }

        void TrySetIndividualRangeFromActualPresets(DateRangeType dateRangeType)
        {
            if (dateRangeType == DateRangeType.None)
                return;

            if (!DateRangePresets.Presets.ContainsKey(dateRangeType))
                return;

            var preset = DateRangePresets.Presets[dateRangeType];
            _rangeType = dateRangeType;
            _startDate = preset.StartDate;
            _endDate = preset.EndDate;
        }

        void TrySetDateRangeFromIndividualRange(DateTime startDate, DateTime endDate)
        {
            var presets = DateRangePresets.Presets.Where(p => p.Value.StartDate == startDate && p.Value.EndDate == endDate);
            if (presets.None())
                return;

            var preset = presets.First();
            _rangeType = preset.Key;
        }
    }
}


