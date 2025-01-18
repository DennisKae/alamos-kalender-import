using DennisKae.alamos_kalender_import.Core.Services;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DennisKae.alamos_kalender_import.Core
{
    /// <summary>Extension Methods, die beim Anwendungsstart benötigt werden.</summary>
    public static class StartupExtensions
    {
        /// <summary>Injected die Dependencies des Core Projektes</summary>
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IApiConnectionService, ApiConnectionService>();
            services.AddSingleton<ICalendarApiService, CalendarApiService>();
            services.AddSingleton<ICalendarEventApiService, CalendarEventApiService>();
            services.AddSingleton<IExcelService, ExcelService>();
            
            return services;
        }
    }
}