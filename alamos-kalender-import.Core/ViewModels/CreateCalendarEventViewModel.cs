using System.Text.Json.Serialization;

namespace DennisKae.alamos_kalender_import.Core.ViewModels
{
    public class CreateCalendarEventViewModel
    {
        [JsonPropertyName("event")]
        public CalendarEventViewModel CalendarEvent { get; set; }
        
        // "persons": []
    }
}