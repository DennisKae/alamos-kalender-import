using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    /// <summary>Anbindung an Calendar Event APIs</summary>
    public interface ICalendarEventApiService
    {
        /// <summary>Erstellt einen neuen Kalendereintrag</summary>
        Task<CalendarEventContainerViewModel> CreateCalendarEvent(CalendarEventContainerViewModel request);

        /// <summary>Löscht einen Kalendereintrag</summary>
        Task DeleteCalendarEvent(string calenderId, string calendarEventId);

        /// <summary>Liefert die Kalendereinträge des angegebenen Monats im angegebenen Jahr.</summary>
        Task<List<CalendarEventContainerViewModel>> GetCalendarEvents(int year, int month);

        /// <summary>Aktualisiert einen Kalendereintrag</summary>
        /// <param name="updatedEvent">Aktualisierter Kalendereintrag</param>
        /// <returns>Aktualisierter Kalendereintrag</returns>
        Task<CalendarEventContainerViewModel> UpdateCalendarEvent(CalendarEventContainerViewModel updatedEvent);
    }
}