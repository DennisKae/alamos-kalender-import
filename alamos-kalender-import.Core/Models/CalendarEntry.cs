using CsvHelper.Configuration.Attributes;

namespace DennisKae.alamos_kalender_import.Core.Models
{
    /// <summary>Kalendereintrag</summary>
    public class CalendarEntry
    {
        [Name("Datum")]
        public string Day { get; set; }

        [Name("Start")]
        public string StartTime { get; set; }

        [Name("Ende")]
        public string EndTime { get; set; }

        [Name("Kalender")]
        public string CalendarName { get; set; }

        [Name("Titel")]
        public string Title { get; set; }
    }
}