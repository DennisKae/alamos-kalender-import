using System.Collections.Generic;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    public interface IRestApiService
    {
        [Post("/rest/login")]
        Task<LoginResponseViewModel> Login(LoginRequestViewModel model);

        [Get("/rest/eventPlanning/calendars?simplified=true")]
        Task<List<CalendarResponseViewModel>> GetCalendars();
    }
}