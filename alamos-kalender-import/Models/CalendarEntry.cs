namespace alamos_kalender_import.Models
{
    /// <summary>Kalendereintrag</summary>
    public class CalendarEntry
    {
        public string Datum { get; set; }

        public string Start { get; set; }

        public string Ende { get; set; }

        public string CalendarName { get; set; }

        public string Title { get; set; }
    }
}