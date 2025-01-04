using System;
using System.Text.Json.Serialization;
using DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels;

namespace DennisKae.alamos_kalender_import.Core.ViewModels
{
    public class CalendarEventViewModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }
        
        [JsonPropertyName("calendarName")]
        public string CalendarName { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        /// <summary>Inhalt</summary>
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        /*  "serialAppointmentId": null,
            "serialAppointmentIteration": 0,
            "serialAppointmentDistance": 0,
            "serialAppointmentWeekday": "NONE",
         */
        
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("fullDay")]
        public bool FullDay { get; set; }

        /// <summary>Auch dann, wenn Notifications definiert wurden, ist das false. Sinn?</summary>
        [JsonPropertyName("hasNotifications")]
        public bool HasNotifications { get; set; }

        /// <summary>
        /// Art des Kalendereintrags.
        /// TRAINING = Übungsdienst, EVENT = Termin
        /// Achtung manche Arten unterstützen nicht alle Eigenschaften. TRAINING unterstützt AFAIK die meisten Eigenschaften.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "TRAINING";

        [JsonPropertyName("status")]
        public string Status { get; set; } = "PLANNED";

        /// <summary>Absagegrund</summary>
        [JsonPropertyName("cancelledReason")]
        public string CancelationReason { get; set; }
        
        /// <summary>Ort</summary>
        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>Verantwortlicher</summary>
        [JsonPropertyName("responsiblePerson")]
        public string ResponsiblePerson { get; set; }

        /// <summary>Thema</summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        /// <summary>Mindest-Teilnehmerzahl</summary>
        [JsonPropertyName("minParticipants")]
        public int MinParticipants { get; set; }
        
        /// <summary>Maximale Teilnehmerzahl</summary>
        [JsonPropertyName("maxParticipants")]
        public int MaxParticipants { get; set; }

        [JsonPropertyName("firstNotification")]
        public NotificationRequestViewModel FirstNotification { get; set; }
        
        [JsonPropertyName("secondNotification")]
        public NotificationRequestViewModel SecondNotification { get; set; }

        /// <summary>Benutzer-Rückmeldungen ein-/ausschalten. Default = ein</summary>
        [JsonPropertyName("withFeedback")]
        public bool EnableFeedback { get; set; } = true;

        [JsonPropertyName("fromIcalSync")]
        public bool IsFromICalSync { get; set; }
    }
}