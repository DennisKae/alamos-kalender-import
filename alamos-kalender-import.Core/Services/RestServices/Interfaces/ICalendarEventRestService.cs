using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services.RestServices.Interfaces
{
    /// <summary>Service mit den REST API Methoden für Calendar Events</summary>
    public interface ICalendarEventRestService
    {
        [Post("/rest/eventPlanning/calendars/{calendarId}/events?notify=false")]
        Task<CalendarEventContainerViewModel> CreateCalendarEvent([Query]string calendarId, CalendarEventContainerViewModel model);
        
        [Get("/rest/eventPlanning/events/user/{year}/{month}?surrounding=false")]
        Task<List<CalendarEventContainerViewModel>> GetCalendarEvents([Query]int year, [Query]int month);
        
        [Put("/rest/eventPlanning/calendars/{calendarId}/events/{eventId}?notify=false&upcoming=false")]
        Task<CalendarEventContainerViewModel> UpdateCalendarEvent([Query]string calendarId, [Query]string eventId, CalendarEventContainerViewModel updatedEvent);
        
        [Delete("/rest/eventPlanning/calendars/{calendarId}/events/{calendarEventId}?deleteSeries=false")]
        Task DeleteCalendarEvent([Query]string calendarId, [Query]string calendarEventId);
    }
}