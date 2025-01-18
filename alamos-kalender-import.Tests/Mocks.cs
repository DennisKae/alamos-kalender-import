using System;
using DennisKae.alamos_kalender_import.Core;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests
{
    public static class Mocks
    {
        public static CalendarEventContainerViewModel GetCalendarEventContainerViewModel(CalendarResponseViewModel calendar)
        {
            Assert.IsNotNull(calendar);

            DateTime now = DateTime.Now;
            DateTime startDate = new(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
            DateTime endDate = startDate.AddHours(1);

            var calendarEvent = new CalendarEventViewModel
            {
                CalendarId = calendar.Id,
                CalendarName = calendar.Name,
                Color = calendar.Color,
                Title = "Test Title",
                StartDate = startDate,
                EndDate = endDate,
                FirstNotification = new NotificationViewModel
                {
                    Duration = 1,
                    TimeUnit = SharedConstants.NotificationTimeUnitHours
                },
                SecondNotification = new NotificationViewModel
                {
                    Duration = 1,
                    TimeUnit = SharedConstants.NotificationTimeUnitDays
                }
            };

            return new CalendarEventContainerViewModel { CalendarEvent = calendarEvent };
        }
    }
}