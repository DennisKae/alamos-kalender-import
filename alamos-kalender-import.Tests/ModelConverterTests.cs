using DennisKae.alamos_kalender_import.Core.Models;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests
{
    [TestClass]
    public class ModelConverterTests
    {
        /// <summary>
        /// Beim Excelimport entstehen eigenartige Zeitformate. Hiermit wird getestet, dass sie korrekt konvertiert werden.
        /// </summary>
        [TestMethod]
        public void ToCalendarEventViewModelDateTest()
        {
            var calendarEvent = new CalendarEvent
            {
                Day = "08.01.2025 00:00:00",
                StartTime = "30.12.1899 20:30:00",
                EndTime = "22:00"
            };
            var calendar = new CalendarResponseViewModel();

            CalendarEventViewModel viewModel = calendarEvent.ToCalendarEventViewModel(calendar);
            Assert.IsNotNull(viewModel);
            
            Assert.AreEqual(viewModel.StartDate.Day, viewModel.EndDate.Day);
            Assert.AreEqual(viewModel.StartDate.Month, viewModel.EndDate.Month);
            Assert.AreEqual(viewModel.StartDate.Year, viewModel.EndDate.Year);
            
            Assert.AreEqual(8, viewModel.StartDate.Day);
            Assert.AreEqual(1, viewModel.StartDate.Month);
            Assert.AreEqual(2025, viewModel.StartDate.Year);
            
            Assert.AreEqual(20, viewModel.StartDate.Hour);
            Assert.AreEqual(30, viewModel.StartDate.Minute);
            
            Assert.AreEqual(22, viewModel.EndDate.Hour);
            Assert.AreEqual(0, viewModel.EndDate.Minute);
        }
    }
}