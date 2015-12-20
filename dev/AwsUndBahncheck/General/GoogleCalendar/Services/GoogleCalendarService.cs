using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts.CalendarService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleCalendar.Services
{
    public class GoogleCalendarService : ICalendarService
    {
        private const string ClientId = "1011254458932-nkqvlaicfiev9t7qmmtkk2ov09d3tr06.apps.googleusercontent.com";
        private const string ClientSecret = "2-tw_L9f6k1b0OjUEN6TtkLI";
        private const string ApplicationName = "AwsAbfuhrkalender";
        private string _abfallCalendarId = "";

        private CalendarService _calendarService;
        private CalendarService CalendarService
        {
            get
            {
                if (_calendarService != null) return _calendarService;

                var userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
                    new[] { CalendarService.Scope.Calendar }, 
                    "user", CancellationToken.None, 
                    new FileDataStore("Drive.Auth.Store")).Result;

                // Create the service. This will automatically call the previously registered authenticator.
                _calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = ApplicationName,
                });

                var calList = CalendarService.CalendarList.List().Execute();
                var cals = calList.Items;
                var abfallCalendar = cals.FirstOrDefault(cal => cal.Summary.ToLower().StartsWith("abfalltermine"));
                if (abfallCalendar != null)
                    _abfallCalendarId = abfallCalendar.Id;

                return _calendarService;
            }
        }

        public bool CreateEvent(string eventName, DateTime eventStartTime, int eventDurationHours, int eventReminderHours)
        {
            var eventEndTime = eventStartTime.AddHours(eventDurationHours);
            var newEvent = new Event
            {
                Start = new EventDateTime { DateTime = eventStartTime },
                End = new EventDateTime { DateTime = eventEndTime },
                Description = eventName,
                Summary = eventName,
                Reminders = new Event.RemindersData
                {
                    UseDefault = false,
                    Overrides = new List<EventReminder>
                    {
                        new EventReminder
                        {
                            Method = "popup",
                            Minutes = 780,
                        }
                    }
                }
            };

            var request = CalendarService.Events.Insert(newEvent, _abfallCalendarId);
            request.Execute();

            return true;
        }

        void ImportDayAction(int year, int month, int day, string action)
        {
            var actionFullNames = new Dictionary<string, string>
                                      {
                                          {"r", "Müll"},
                                          {"g", "Gelber Sack"},
                                          {"b", "Bio-Tonne"},
                                          {"p", "Papier-Tonne"},
                                          {"w", "Weihnachtsbaum-Abfuhr"},
                                      };
            string actionFullName;
            if (!actionFullNames.TryGetValue(action, out actionFullName)) return;

            //System.Diagnostics.Trace.TraceError("{0} {1} {2} {3}", year, month, day, actionFullName);

            const int eventStartHour = 9;
            const int eventDurationHours = 1;
            const int eventReminderHours = 13;
            var eventStartTime = new DateTime(year, month, day, eventStartHour, 0, 0);
            CreateEvent(actionFullName, eventStartTime, eventDurationHours, eventReminderHours);
        }

        void DeleteAllItems(int year)
        {
            var list = CalendarService.Events.List(_abfallCalendarId);
            list.MaxResults = 999999;
            var events = list.Execute().Items.ToList();
            events.ForEach(e =>
            {
                if (e.Start.DateTime.GetValueOrDefault().Year == year)
                    CalendarService.Events.Delete(_abfallCalendarId, e.Id).Execute();
            });
        }

        public void ImportCalendarItems(List<CalendarItem> items)
        {
            if (items.Any())
                DeleteAllItems(items.First().Date.Year);

            items.ForEach(item => ImportDayAction(item.Date.Year, item.Date.Month, item.Date.Day, item.ItemType));
        }
    }
}
