using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    [Ignore("Integrationstests mit API Requests")]
    [TestClass]
    public class CalendarApiServiceTests
    {
        private ICalendarApiService _calendarApiService;

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider serviceProvider = Util.GetServiceProvider(true);
            _calendarApiService = serviceProvider.GetRequiredService<ICalendarApiService>();
        }
        
        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetCalendarsTest()
        {
            List<CalendarResponseViewModel> calendars = await _calendarApiService.GetCalendars();
            Assert.IsTrue(calendars?.Count > 0);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetCalendarByNameTest()
        {
            CalendarResponseViewModel calendar = await _calendarApiService.GetCalendarByName(Util.GetTestCalendarName());
            Assert.IsNotNull(calendar);
            Assert.AreEqual(Util.GetTestCalendarName(), calendar.Name);
        }
    }
}