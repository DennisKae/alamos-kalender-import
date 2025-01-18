using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.Models;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;

namespace DennisKae.alamos_kalender_import.Core.Services
{
    /// <summary>Anbindung an Calendar APIs</summary>
    public class CalendarApiService : ICalendarApiService
    {
        private readonly IApiConnectionService _apiConnectionService;

        /// <summary>Konstruktor</summary>
        public CalendarApiService(IApiConnectionService apiConnectionService)
        {
            _apiConnectionService = apiConnectionService;
        }
        
        /// <summary>Liefert alle verfügbaren Kalender</summary>
        public async Task<List<CalendarResponseViewModel>> GetCalendars()
        {
            IAlamosRestApiService alamosRestApiService = await _apiConnectionService.GetRestService<IAlamosRestApiService>();
            return await alamosRestApiService.GetCalendars();
        }

        /// <summary>Liefert den Kalender mit der angegebenen Bezeichnung</summary>
        public async Task<CalendarResponseViewModel> GetCalendarByName(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            List<CalendarResponseViewModel> allCalendars = await GetCalendars();

            return allCalendars?.FirstOrDefault(x => x.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false);
        }
    }
}