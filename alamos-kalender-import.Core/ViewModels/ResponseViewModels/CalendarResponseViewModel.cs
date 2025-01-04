using System.Text.Json.Serialization;

namespace DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels
{
    /// <summary>Informationen über einen Kalender</summary>
    public class CalendarResponseViewModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
        
        [JsonPropertyName("defaultColor")]
        public string DefaultColor { get; set; }

        [JsonPropertyName("defaultResponsiblePerson")]
        public string DefaultResponsiblePerson { get; set; }
    }
}