using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core;
using DennisKae.alamos_kalender_import.Core.Services;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    // [Ignore("Integrationstest")]
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

            var request = new CreateCalendarEventViewModel
            {
                CalendarEvent = GetCalendarEventViewModel(calendar)
            };

            CreateCalendarEventViewModel createdCalendarEvent = await _alamosApiService.CreateCalendarEvent(request);
            Assert.IsNotNull(createdCalendarEvent);
            Assert.IsNotNull(createdCalendarEvent.CalendarEvent);
            Assert.IsNotNull(createdCalendarEvent.CalendarEvent.Id);
        }
        
        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task DeleteCalendarEventTest()
        {
            CalendarResponseViewModel calendar = await _alamosApiService.GetCalendarByName(_testCalendarName);
            Assert.IsNotNull(calendar);

            var request = new CreateCalendarEventViewModel
            {
                CalendarEvent = GetCalendarEventViewModel(calendar)
            };

            CreateCalendarEventViewModel createdCalendarEvent = await _alamosApiService.CreateCalendarEvent(request);
            Assert.IsNotNull(createdCalendarEvent);
            Assert.IsNotNull(createdCalendarEvent.CalendarEvent);
            Assert.IsNotNull(createdCalendarEvent.CalendarEvent.Id);
            
            await _alamosApiService.DeleteCalendarEvent(calendar.Id, createdCalendarEvent.CalendarEvent.Id);
            
            // TODO: Prüfen ob der Eintrag auch wirklich gelöscht wurde.
        }

        private CalendarEventViewModel GetCalendarEventViewModel(CalendarResponseViewModel calendar)
        {
            Assert.IsNotNull(calendar);

            DateTime now = DateTime.Now;
            DateTime startDate = new(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
            DateTime endDate = startDate.AddHours(1);

            var result = new CalendarEventViewModel
            {
                CalendarId = calendar.Id,
                CalendarName = calendar.Name,
                Color = calendar.Color,
                Title = "Test Title",
                StartDate = startDate,
                EndDate = endDate,
                FirstNotification = new NotificationRequestViewModel
                {
                    Duration = 1,
                    TimeUnit = SharedConstants.NotificationTimeUnitHours
                },
                SecondNotification = new NotificationRequestViewModel
                {
                    Duration = 1,
                    TimeUnit = SharedConstants.NotificationTimeUnitDays
                }
            };

            return result;
        }
    }
}