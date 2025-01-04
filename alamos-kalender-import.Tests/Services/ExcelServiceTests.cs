using System.Collections.Generic;
using DennisKae.alamos_kalender_import.Core.Models;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    [TestClass]
    public class ExcelServiceTests
    {
        private IExcelService _excelService;
        
        [TestInitialize]
        public void InitializeExcelService()
        {
            _excelService = Util.GetServiceProvider().GetRequiredService<IExcelService>();
        }

        [TestMethod]
        public void GetCalendarEntriesTest()
        {
            var fileName = "Kalender.xlsx";
            
            List<CalendarEntry> result = _excelService.GetCalendarEntries(fileName);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 3);
        }
    }
}