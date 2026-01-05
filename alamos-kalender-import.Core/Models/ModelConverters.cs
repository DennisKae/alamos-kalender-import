using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;

namespace DennisKae.alamos_kalender_import.Core.Models
{
    /// <summary>Div. Extension Methods zur Konvertierung zwischen Datenmodellen.</summary>
    public static class ModelConverters
    {
        public static List<CalendarEventViewModel> ToCalendarEventViewModels(this List<CalendarEvent> calendarEvents,
            CalendarResponseViewModel calendar)
        {
            Guard.Against.Null(calendar, nameof(calendar));
            if(calendarEvents?.Any() != true)
            {
                return null;
            }

            List<CalendarEventViewModel> result = calendarEvents
                .Select(x => x.ToCalendarEventViewModel(calendar))
                .ToList();
            
            return result;
        }

        public static CalendarEventViewModel ToCalendarEventViewModel(this CalendarEvent calendarEvent, CalendarResponseViewModel calendar)
        {
            Guard.Against.Null(calendar, nameof(calendar));

            if(calendarEvent == null)
            {
                return null;
            }

            var result = new CalendarEventViewModel
            {
                CalendarId = calendar.Id,
                CalendarName = calendar.Name,
                Color = calendar.Color,
                Title = calendarEvent.Title,
                FirstNotification = new NotificationViewModel
                {
                    IsActive = true,
                    Duration = 1,
                    TimeUnit = SharedConstants.NotificationTimeUnitHours
                },
                SecondNotification = new NotificationViewModel
                {
                    IsActive = true,
                    Duration = 1,
                    TimeUnit = SharedConstants.NotificationTimeUnitDays
                }
            };

            DateTime startTime = DateTime.Parse(calendarEvent.CleanedStartTime);
            DateTime endTime = DateTime.Parse(calendarEvent.CleanedEndTime);
            DateTime day = DateTime.ParseExact(calendarEvent.CleanedDay,"dd.MM.yyyy", CultureInfo.CurrentCulture);

            result.StartDate = new DateTime(day.Year, day.Month, day.Day, startTime.Hour, startTime.Minute, startTime.Second);
            result.EndDate = new DateTime(day.Year, day.Month, day.Day, endTime.Hour, endTime.Minute, endTime.Second);

            if (!string.IsNullOrWhiteSpace(calendarEvent.ResponsiblePerson))
            {
                result.ResponsiblePerson = calendarEvent.ResponsiblePerson;
            }

            if (!string.IsNullOrWhiteSpace(calendarEvent.Location))
            {
                result.Location = calendarEvent.Location;
            }
            
            return result;
        }
    }
}