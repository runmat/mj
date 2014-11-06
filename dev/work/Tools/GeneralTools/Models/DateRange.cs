using System;

namespace GeneralTools.Models
{
    public enum DateRangeType { LastYear, Last3Months, LastMonth, CurrentMonth, Last30Days, Last7Days, Today, Yesterday }

    public class DateRange 
    {
        public bool IsSelected { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateRange()
        {
        }

        public DateRange(DateRangeType dateRangeType)
        {
            switch (dateRangeType)
            {
                case DateRangeType.LastYear:
                    StartDate = new DateTime(DateTime.Today.Year - 1, 1, 1);
                    EndDate = new DateTime(DateTime.Today.Year - 1, 12, 31);
                    break;

                case DateRangeType.Last3Months:
                    StartDate = DateTime.Today.AddMonths(-3).MoveToFirstDay();
                    EndDate = DateTime.Today.AddMonths(-1).MoveToLastDay();
                    break;

                case DateRangeType.LastMonth:
                    StartDate = DateTime.Today.AddMonths(-1).MoveToFirstDay();
                    EndDate = DateTime.Today.AddMonths(-1).MoveToLastDay();
                    break;

                case DateRangeType.CurrentMonth:
                    StartDate = DateTime.Today.AddMonths(0).MoveToFirstDay();
                    EndDate = DateTime.Today.AddMonths(0).MoveToLastDay();
                    break;

                case DateRangeType.Last30Days:
                    StartDate = DateTime.Today.AddDays(-30);
                    EndDate = DateTime.Today;
                    break;

                case DateRangeType.Last7Days:
                    StartDate = DateTime.Today.AddDays(-7);
                    EndDate = DateTime.Today.AddDays(0);
                    break;

                case DateRangeType.Today:
                    StartDate = DateTime.Today;
                    EndDate = DateTime.Today;
                    break;

                case DateRangeType.Yesterday:
                    StartDate = DateTime.Today.AddDays(-1);
                    EndDate = DateTime.Today.AddDays(-1);
                    break;
            }
        }
    }
}


