using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.CalendarService;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace GoogleCalendar.Services
{
    public class GoogleCalendarService : ICalendarService
    {
        private CalendarService _calendarService;
        private CalendarService CalendarService
        {
            get
            {
                if (_calendarService != null) return _calendarService;

                // Create the service. This will automatically call the previously registered authenticator.
                _calendarService = new CalendarService("Matzi");
                _calendarService.setUserCredentials("jenzenm@googlemail.com", "");

                return _calendarService;
            }
        }

        private CalendarEntry _abfallKalender = null;
        private CalendarEntry AbfallKalender
        {
            get
            {
                if (_abfallKalender != null) return _abfallKalender;

                const string googleCalenderUrl = "https://www.google.com/calendar/feeds/default/owncalendars/full";
                var query = new CalendarQuery { Uri = new Uri(googleCalenderUrl) };
                var resultFeed = CalendarService.Query(query);

                _abfallKalender = (CalendarEntry)resultFeed.Entries.Where(e => e.Title.Text == "Abfalltermine").FirstOrDefault();

                return _abfallKalender;
            }
        }

        private Uri _postUri;
        private Uri PostUri
        {
            get
            {
                if (_postUri != null) return _postUri;

                _postUri = new Uri("https://www.google.com/calendar/feeds/" + AbfallKalender.SelfUri.ToString().Substring(AbfallKalender.SelfUri.ToString().LastIndexOf("/") + 1) + "/private/full");

                return _postUri;
            }
        }

        public AtomFeed Events
        {
            get
            {
                var query = new EventQuery { Uri = PostUri };
                return CalendarService.Query(query);
            }
        }

        public bool CreateEvent(string eventName, DateTime eventStartTime, int eventDurationHours, int eventReminderHours)
        {
            var eventEndTime = eventStartTime.AddHours(eventDurationHours);
            var entry = new EventEntry
            {
                Title = { Text = eventName },
                Content = { Content = eventName },
                EventVisibility = new EventEntry.Visibility("event.public")
            };

            var eventTime = new When(Convert.ToDateTime(eventStartTime), Convert.ToDateTime(eventEndTime));
            entry.Times.Add(eventTime);
            entry.Reminder = new Reminder { Hours = eventReminderHours };

            var requestFactory = (GDataGAuthRequestFactory)CalendarService.RequestFactory;
            requestFactory.CreateRequest(GDataRequestType.Insert, PostUri);

            // Send the request and receive the response:
            var insertedEntry = CalendarService.Insert(PostUri, entry);
            if (insertedEntry == null) return false;

            return true;
        }

        public void DeleteAllEvents()
        {
            var retries = 0;
            while (Events.Entries.Count > 0 && retries++ < 999)
                DeleteAllEventsCore();
        }

        private void DeleteAllEventsCore()
        {
            var eventEntries = Events;
            var batchFeed = new AtomFeed(eventEntries);

            foreach (var entry in eventEntries.Entries)
            {
                entry.Id = new AtomId(entry.EditUri.ToString());
                entry.BatchData = new GDataBatchEntryData(GDataBatchOperationType.delete);

                batchFeed.Entries.Add(entry);
            }

            var batchResultFeed = (EventFeed)CalendarService.Batch(batchFeed, new Uri(eventEntries.Batch));

            //check the return values of the batch operations to make sure they all worked.
            //the insert operation should return a 201 and the rest should return 200
            //var failedEntries = batchResultFeed.Entries.Cast<EventEntry>().Where(entry => entry.BatchData.Status.Code != 200 && entry.BatchData.Status.Code != 201);
        }

        public void ImportCalendarItems(List<CalendarItem> items)
        {
            items.ForEach(item => ImportDayAction(item.Date.Year, item.Date.Month, item.Date.Day, item.ItemType));
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
    }
}
