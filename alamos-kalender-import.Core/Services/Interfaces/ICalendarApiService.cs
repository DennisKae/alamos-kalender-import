using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    /// <summary>Anbindung an Calendar APIs</summary>
    public interface ICalendarApiService
    {
        /// <summary>Liefert alle verfügbaren Kalender</summary>
        Task<List<CalendarResponseViewModel>> GetCalendars();

        /// <summary>Liefert den Kalender mit der angegebenen Bezeichnung</summary>
        Task<CalendarResponseViewModel> GetCalendarByName(string name);
    }
}