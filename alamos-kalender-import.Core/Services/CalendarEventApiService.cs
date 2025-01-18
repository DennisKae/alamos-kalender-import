using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.Services.RestServices.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;

namespace DennisKae.alamos_kalender_import.Core.Services
{
    /// <summary>Anbindung an Calendar Event APIs</summary>
    public class CalendarEventApiService : ICalendarEventApiService
    {
        private readonly IApiConnectionService _apiConnectionService;

        /// <summary>Konstruktor</summary>
        public CalendarEventApiService(IApiConnectionService apiConnectionService)
        {
            _apiConnectionService = apiConnectionService;
        }

        /// <summary>Erstellt einen neuen Kalendereintrag</summary>
        public async Task<CalendarEventContainerViewModel> CreateCalendarEvent(CalendarEventContainerViewModel request)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.Null(request.CalendarEvent, nameof(request.CalendarEvent));

            ICalendarEventRestService calendarEventRestService = await _apiConnectionService.GetRestService<ICalendarEventRestService>();
            return await calendarEventRestService.CreateCalendarEvent(request.CalendarEvent.CalendarId, request);
        }

        /// <summary>Löscht einen Kalendereintrag</summary>
        public async Task DeleteCalendarEvent(string calenderId, string calendarEventId)
        {
            Guard.Against.NullOrWhiteSpace(calenderId, nameof(calenderId));
            Guard.Against.NullOrWhiteSpace(calendarEventId, nameof(calendarEventId));

            ICalendarEventRestService calendarEventRestService = await _apiConnectionService.GetRestService<ICalendarEventRestService>();
            await calendarEventRestService.DeleteCalendarEvent(calenderId, calendarEventId);
        }

        /// <summary>Liefert die Kalendereinträge des angegebenen Monats im angegebenen Jahr.</summary>
        public async Task<List<CalendarEventContainerViewModel>> GetCalendarEvents(int year, int month)
        {
            Guard.Against.OutOfRange(year, nameof(year), DateTime.Now.Year - 5, DateTime.Now.Year + 5);
            Guard.Against.OutOfRange(month, nameof(month), 1, 12);

            ICalendarEventRestService calendarEventRestService = await _apiConnectionService.GetRestService<ICalendarEventRestService>();
            return await calendarEventRestService.GetCalendarEvents(year, month);
        }

        /// <summary>Aktualisiert einen Kalendereintrag</summary>
        /// <param name="updatedEvent">Aktualisierter Kalendereintrag</param>
        /// <returns>Aktualisierter Kalendereintrag</returns>
        public async Task<CalendarEventContainerViewModel> UpdateCalendarEvent(CalendarEventContainerViewModel updatedEvent)
        {
            Guard.Against.Null(updatedEvent, nameof(updatedEvent));
            Guard.Against.Null(updatedEvent.CalendarEvent, nameof(updatedEvent.CalendarEvent));
            Guard.Against.NullOrWhiteSpace(updatedEvent.CalendarEvent.Id, nameof(updatedEvent.CalendarEvent.Id));
            Guard.Against.NullOrWhiteSpace(updatedEvent.CalendarEvent.CalendarId, nameof(updatedEvent.CalendarEvent.CalendarId));

            ICalendarEventRestService calendarEventRestService = await _apiConnectionService.GetRestService<ICalendarEventRestService>();
            return await calendarEventRestService.UpdateCalendarEvent(updatedEvent.CalendarEvent.CalendarId, updatedEvent.CalendarEvent.Id, updatedEvent);
        }
    }
}