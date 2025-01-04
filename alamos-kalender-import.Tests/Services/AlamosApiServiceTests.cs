using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.Services;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    [Ignore("Integrationstest")]
    [TestClass]
    public class AlamosApiServiceTests
    {
        private IAlamosApiService _alamosApiService;

        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider serviceProvider = Util.GetServiceProvider();
            _alamosApiService = serviceProvider.GetService<IAlamosApiService>();
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
    }
}