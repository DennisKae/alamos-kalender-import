using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services.RestServices.Interfaces
{
    /// <summary>Service mit den REST API Methoden für Calendars</summary>
    public interface ICalendarRestService
    {
        [Get("/rest/eventPlanning/calendars?simplified=true")]
        Task<List<CalendarResponseViewModel>> GetCalendars();
    }
}