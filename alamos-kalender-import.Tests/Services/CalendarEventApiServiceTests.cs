using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    [Ignore("Integrationstests mit API Requests")]
    [TestClass]
    public class CalendarEventApiServiceTests
    {
        private ICalendarEventApiService _calendarEventApiService;
        private ICalendarApiService _calendarApiService;

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider serviceProvider = Util.GetServiceProvider(true);
            _calendarEventApiService = serviceProvider.GetRequiredService<ICalendarEventApiService>();
            _calendarApiService = serviceProvider.GetRequiredService<ICalendarApiService>();
        }
        
        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task CreateCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _calendarApiService.GetCalendarByName(Util.GetTestCalendarName());
            Assert.IsNotNull(calendar);

            CalendarEventContainerViewModel requestContainer = Mocks.GetCalendarEventContainerViewModel(calendar);

            CalendarEventContainerViewModel createdCalendarEventContainer = await _calendarEventApiService.CreateCalendarEvent(requestContainer);
            Assert.IsNotNull(createdCalendarEventContainer);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent.Id);
            
            await _calendarEventApiService.DeleteCalendarEvent(calendar.Id, createdCalendarEventContainer.CalendarEvent.Id);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetCalendarEventsTest()
        {
            // Setzt voraus, dass im aktuellen Monat Termine angelegt wurden
            List<CalendarEventContainerViewModel> events = await _calendarEventApiService.GetCalendarEvents(DateTime.Now.Year, DateTime.Now.Month);

            Assert.IsTrue(events?.Count > 0);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task CreateGetAndDeleteCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _calendarApiService.GetCalendarByName(Util.GetTestCalendarName());
            Assert.IsNotNull(calendar);

            CalendarEventContainerViewModel requestContainer = Mocks.GetCalendarEventContainerViewModel(calendar);

            int year = requestContainer.CalendarEvent.StartDate.Year;
            int month = requestContainer.CalendarEvent.StartDate.Month;

            CalendarEventContainerViewModel createdCalendarEventContainer = await _calendarEventApiService.CreateCalendarEvent(requestContainer);
            Assert.IsNotNull(createdCalendarEventContainer);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent);
            Assert.IsNotNull(createdCalendarEventContainer.CalendarEvent.Id);

            List<CalendarEventContainerViewModel> allEventsBeforeDelete = await _calendarEventApiService.GetCalendarEvents(year, month);

            Assert.IsTrue(allEventsBeforeDelete?.Count > 0);
            Assert.IsTrue(allEventsBeforeDelete.Any(x => x.CalendarEvent.Id == createdCalendarEventContainer.CalendarEvent.Id));

            await _calendarEventApiService.DeleteCalendarEvent(calendar.Id, createdCalendarEventContainer.CalendarEvent.Id);

            List<CalendarEventContainerViewModel> allEventsAfterDelete = await _calendarEventApiService.GetCalendarEvents(year, month);

            Assert.IsTrue(allEventsAfterDelete?.Count > 0);
            Assert.IsTrue(allEventsAfterDelete.All(x => x.CalendarEvent.Id != createdCalendarEventContainer.CalendarEvent.Id));
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task UpdateCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _calendarApiService.GetCalendarByName(Util.GetTestCalendarName());
            Assert.IsNotNull(calendar);

            CalendarEventContainerViewModel requestContainer = Mocks.GetCalendarEventContainerViewModel(calendar);

            CalendarEventContainerViewModel createdEventContainer = await _calendarEventApiService.CreateCalendarEvent(requestContainer);
            Assert.IsNotNull(createdEventContainer);
            Assert.IsNotNull(createdEventContainer.CalendarEvent);
            Assert.IsNotNull(createdEventContainer.CalendarEvent.Id);

            string updatedEventTitle = createdEventContainer.CalendarEvent.Title + "-Updated";
            createdEventContainer.CalendarEvent.Title = updatedEventTitle;

            CalendarEventContainerViewModel updatedEventContainer = await _calendarEventApiService.UpdateCalendarEvent(createdEventContainer);
            Assert.IsNotNull(updatedEventContainer?.CalendarEvent?.Id);
            Assert.AreEqual(updatedEventTitle, updatedEventContainer.CalendarEvent.Title);

            await _calendarEventApiService.DeleteCalendarEvent(calendar.Id, createdEventContainer.CalendarEvent.Id);
        }
    }
}