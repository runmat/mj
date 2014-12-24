using Contracts.CalendarService;

namespace GoogleCalendar.Services
{
    public class GoogleCalendarServiceFactory 
    {
        public static ICalendarService Create()
        {
            return new GoogleCalendarService();
        }
    }
}
