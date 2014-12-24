using Contracts.CalendarService;

namespace AwsAbfuhrkalender.Contracts
{
    public interface IAwsExceImportService
    {
        ICalendarService CalendarService { get; set; }

        void Import(string excelFile, int year);
    }
}
