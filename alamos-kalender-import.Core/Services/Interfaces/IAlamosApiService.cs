using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    public interface IAlamosApiService
    {
        /// <summary>Liefert den API Token zum angegebenen Benutzernamen und Passwort</summary>
        Task<string> GetApiToken();

        /// <summary>Liefert die verfügbaren Kalender</summary>
        Task<List<CalendarResponseViewModel>> GetCalendars();

        /// <summary>Initialisiert den Service. Muss einmalig vor dem ersten API Aufruf ausgeführt werden.</summary>
        void Initialize(string serverUrl, string username, string password);

        /// <summary>
        /// Liefert den API-Token zum angegebenen Benutzernamen und Passwort und cached den Token bis zum Ende seiner Gültigkeit.
        /// </summary>
        Task<string> GetApiTokenWithCache();

        /// <summary>Liefert den Kalender mit der angegebenen Bezeichnung</summary>
        Task<CalendarResponseViewModel> GetCalendarByName(string name);

        /// <summary>Erstellt einen neuen Kalendereintrag</summary>
        Task<CreateCalendarEventViewModel> CreateCalendarEvent(CreateCalendarEventViewModel request);

        /// <summary>Löscht einen Kalendereintrag</summary>
        Task DeleteCalendarEvent(string calenderId, string calendarEventId);
    }
}