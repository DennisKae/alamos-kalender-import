using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    [Ignore("Integrationstests mit API Requests")]
    [TestClass]
    public class AlamosApiServiceTests
    {
        private IAlamosApiService _alamosApiService;
        private string _testCalendarName;

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider serviceProvider = Util.GetServiceProvider();
            _alamosApiService = serviceProvider.GetService<IAlamosApiService>();
            _testCalendarName = Util.GetRequiredUserSecret("TestCalendarName");
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetApiTokenTest()
        {
            string apiToken = await _alamosApiService.GetApiToken();
            Assert.IsNotNull(apiToken);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetApiTokenWithCacheTest()
        {
            string apiToken = await _alamosApiService.GetApiTokenWithCache();
            Assert.IsNotNull(apiToken);

            string anotherApiToken = await _alamosApiService.GetApiTokenWithCache();
            Assert.IsNotNull(anotherApiToken);

            Assert.AreEqual(apiToken, anotherApiToken);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetCalendarsTest()
        {
            List<CalendarResponseViewModel> calendars = await _alamosApiService.GetCalendars();
            Assert.IsTrue(calendars?.Count > 0);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetCalendarByNameTest()
        {
            CalendarResponseViewModel calendar = await _alamosApiService.GetCalendarByName(_testCalendarName);
            Assert.IsNotNull(calendar);
            Assert.AreEqual(_testCalendarName, calendar.Name);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task CreateCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _alamosApiService.GetCalendarByName(_testCalendarName);
            Assert.IsNotNull(calendar);

            CalendarEventContainerViewModel requestContainer = GetCalendarEventContainerViewModel(calendar);

            CalendarEventContainerViewModel createdCalendarEventContainer = await _alamosApiService.CreateCalendarEvent(requestContainer);
            Assert.IsNotNull(createdCalendarEventContainer);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent.Id);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetCalendarEventsTest()
        {
            // Setzt voraus, dass im aktuellen Monat Termine angelegt wurden
            List<CalendarEventContainerViewModel> events = await _alamosApiService.GetCalendarEvents(DateTime.Now.Year, DateTime.Now.Month);

            Assert.IsTrue(events?.Count > 0);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task CreateGetAndDeleteCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _alamosApiService.GetCalendarByName(_testCalendarName);
            Assert.IsNotNull(calendar);

            CalendarEventContainerViewModel requestContainer = GetCalendarEventContainerViewModel(calendar);

            int year = requestContainer.CalendarEvent.StartDate.Year;
            int month = requestContainer.CalendarEvent.StartDate.Month;

            CalendarEventContainerViewModel createdCalendarEventContainer = await _alamosApiService.CreateCalendarEvent(requestContainer);
            Assert.IsNotNull(createdCalendarEventContainer);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent.Id);

            List<CalendarEventContainerViewModel> allEventsBeforeDelete = await _alamosApiService.GetCalendarEvents(year, month);

            Assert.IsTrue(allEventsBeforeDelete?.Count > 0);
            Assert.IsTrue(allEventsBeforeDelete.Any(x => x.CalendarEvent.Id == createdCalendarEventContainer.CalendarEvent.Id));

            await _alamosApiService.DeleteCalendarEvent(calendar.Id, createdCalendarEventContainer.CalendarEvent.Id);

            List<CalendarEventContainerViewModel> allEventsAfterDelete = await _alamosApiService.GetCalendarEvents(year, month);

            Assert.IsTrue(allEventsAfterDelete?.Count > 0);
            Assert.IsTrue(allEventsAfterDelete.All(x => x.CalendarEvent.Id != createdCalendarEventContainer.CalendarEvent.Id));
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task UpdateCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _alamosApiService.GetCalendarByName(_testCalendarName);
            Assert.IsNotNull(calendar);

            CalendarEventContainerViewModel requestContainer = GetCalendarEventContainerViewModel(calendar);

            CalendarEventContainerViewModel createdEventContainer = await _alamosApiService.CreateCalendarEvent(requestContainer);
            Assert.IsNotNull(createdEventContainer);
            Assert.IsNotNull(createdEventContainer.CalendarEvent);
            Assert.IsNotNull(createdEventContainer.CalendarEvent.Id);

            string updatedEventTitle = createdEventContainer.CalendarEvent.Title + "-Updated";
            createdEventContainer.CalendarEvent.Title = updatedEventTitle;
            
            CalendarEventContainerViewModel updatedEventContainer = await _alamosApiService.UpdateCalendarEvent(createdEventContainer);
            Assert.IsNotNull(updatedEventContainer?.CalendarEvent?.Id);
            Assert.AreEqual(updatedEventTitle, updatedEventContainer.CalendarEvent.Title);
            
            await _alamosApiService.DeleteCalendarEvent(calendar.Id, createdEventContainer.CalendarEvent.Id);
        }

        private CalendarEventContainerViewModel GetCalendarEventContainerViewModel(CalendarResponseViewModel calendar)
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