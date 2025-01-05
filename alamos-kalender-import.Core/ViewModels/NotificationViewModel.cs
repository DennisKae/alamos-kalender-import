using System.Text.Json.Serialization;

namespace DennisKae.alamos_kalender_import.Core.ViewModels
{
    public class NotificationViewModel
    {
        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; } = 1;

        [JsonPropertyName("asPush")]
        public bool IsPushNotification { get; set; } = true;

        [JsonPropertyName("asEmail")]
        public bool IsEmailNotification { get; set; }

        /// <summary>Zeit-Einheit (HOURS oder DAYS)</summary>
        [JsonPropertyName("timeUnit")]
        public string TimeUnit { get; set; } = SharedConstants.NotificationTimeUnitHours;

        /// <summary>Fälligkeitsdatum TODO: Besser als DateTime definieren?</summary>
        [JsonPropertyName("dueDate")]
        public string DueDate { get; set; }
    }
}