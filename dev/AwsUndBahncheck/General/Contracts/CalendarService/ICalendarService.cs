using System;

namespace Contracts.CalendarService
{
    public interface ICalendarService
    {
        bool CreateEvent(string eventName, DateTime eventStartTime, int eventDurationHours, int eventReminderHours);
    }
}
