using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    /// <summary>Service mit den konkreten REST API Methoden</summary>
    public interface IAlamosRestApiService
    {
        [Post("/rest/login")]
        Task<LoginResponseViewModel> Login(LoginRequestViewModel model);

        [Get("/rest/eventPlanning/calendars?simplified=true")]
        Task<List<CalendarResponseViewModel>> GetCalendars();
        
        [Post("/rest/eventPlanning/calendars/{calendarId}/events?notify=false")]
        Task<CreateCalendarEventViewModel> CreateCalendarEvent([Query]string calendarId, CreateCalendarEventViewModel model);
    }
}