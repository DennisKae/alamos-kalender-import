using System;
using CsvHelper.Configuration.Attributes;

namespace DennisKae.alamos_kalender_import.Core.Models
{
    /// <summary>Kalendereintrag</summary>
    public class CalendarEvent
    {
        [Name("Datum")]
        public string Day { get; set; }
        public string CleanedDay => DateTime.TryParse(Day, out DateTime dateTime) ? dateTime.ToString("dd.MM.yyyy") : null;

        [Name("Start")]
        public string StartTime { get; set; }
        
        public string CleanedStartTime => DateTime.TryParse(StartTime, out DateTime dateTime) ? dateTime.ToString("HH:mm") : null;

        [Name("Ende")]
        public string EndTime { get; set; }
        
        public string CleanedEndTime => DateTime.TryParse(EndTime, out DateTime dateTime) ? dateTime.ToString("HH:mm") : null;

        [Name("Kalender")]
        public string CalendarName { get; set; }

        [Name("Titel")]
        public string Title { get; set; }
    }
}