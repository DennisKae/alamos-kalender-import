using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Nachverarbeitungen
{
    /// <summary>
    /// Nachverarbeitung: Aktiviert die Notifications in den Terminen der nächsten 12 Monate (inkl. des laufenden Monats)
    /// </summary>
    [Ignore("Führt API Requests aus.")]
    [TestClass]
    public class EnableFutureNotifications
    {
        /// <summary>Bezeichnungen von Kalendereinträgen, die nicht berücksichtigt werden sollen.</summary>
        private readonly List<string> _calendarEventTitlesToIgnore = ["Arbeits- & Gerätedienst"];

        private ILogger<EnableFutureNotifications> _logger;
        private ICalendarEventApiService _calendarEventApiService;

        [TestInitialize]
        public void Initialize()
        {
            Util.DisableApiLogging();
            ServiceProvider serviceProvider = Util.GetServiceProvider(true);
            _logger = serviceProvider.GetRequiredService<ILogger<EnableFutureNotifications>>();
            _calendarEventApiService = serviceProvider.GetService<ICalendarEventApiService>();
        }

        [TestMethod]
        public async Task Run()
        {
            int monthCounter = 0;
            int totalCount = 0;
            DateTime today = DateTime.Today;

            do
            {
                DateTime dateInMonthToEnable = today.AddMonths(monthCounter);

                int count = await ActivateNotificationsInEventsOfMonth(dateInMonthToEnable.Year, dateInMonthToEnable.Month);
                _logger.LogInformation($"{dateInMonthToEnable:yyyy-MM}: {count} Einträge nachverarbeitet.");

                totalCount += count;
                monthCounter++;
            } while (monthCounter < 12);

            _logger.LogInformation($"Zusammenfassung: {totalCount} Einträge nachverarbeitet.");
        }

        private async Task<int> ActivateNotificationsInEventsOfMonth(int year, int month)
        {
            _logger.LogInformation($"Verarbeite {year}-{month}...");

            int resultCounter = 0;

            List<CalendarEventContainerViewModel> eventContainers = await _calendarEventApiService.GetCalendarEvents(year, month);

            if(eventContainers?.Any() != true)
            {
                return resultCounter;
            }

            eventContainers.RemoveAll(x => x?.CalendarEvent == null);
            eventContainers.RemoveAll(x => x.CalendarEvent.FirstNotification == null && x.CalendarEvent.SecondNotification == null);
            eventContainers.RemoveAll(x => x.CalendarEvent.FirstNotification.IsActive && x.CalendarEvent.SecondNotification.IsActive);
            eventContainers.RemoveAll(x =>
                _calendarEventTitlesToIgnore.Contains(x.CalendarEvent.Title.Trim(), StringComparer.CurrentCultureIgnoreCase));

            if(year == DateTime.Today.Year && month == DateTime.Today.Month)
            {
                eventContainers.RemoveAll(x => x.CalendarEvent.StartDate < DateTime.Now);
            }

            foreach (CalendarEventContainerViewModel eventContainer in eventContainers)
            {
                if(eventContainer.CalendarEvent.FirstNotification != null)
                {
                    eventContainer.CalendarEvent.FirstNotification.IsActive = true;
                }

                if(eventContainer.CalendarEvent.SecondNotification != null)
                {
                    eventContainer.CalendarEvent.SecondNotification.IsActive = true;
                }

                await _calendarEventApiService.UpdateCalendarEvent(eventContainer);
                _logger.LogInformation($"{eventContainer.CalendarEvent.StartDate:yyyy-MM-dd HH:mm} - {eventContainer.CalendarEvent.Title} - aktiviert");

                resultCounter++;
            }

            return resultCounter;
        }
    }
}