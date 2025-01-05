using System.Collections.Generic;
using DennisKae.alamos_kalender_import.Core.Models;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    /// <summary>Service zum Umgang mit Excel Dateien</summary>
    public interface IExcelService
    {
        /// <summary>Liefert die Kalendereinträge aus dem Excel File im dem angegebenen Dateipfad.</summary>
        List<CalendarEvent> GetCalendarEvents(string filepath);
    }
}