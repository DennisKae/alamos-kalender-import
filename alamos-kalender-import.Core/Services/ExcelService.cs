using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ardalis.GuardClauses;
using CsvHelper;
using CsvHelper.Excel;
using DennisKae.alamos_kalender_import.Core.Models;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;

namespace DennisKae.alamos_kalender_import.Core.Services
{
    /// <summary>Service zum Umgang mit Excel Dateien</summary>
    public class ExcelService : IExcelService
    {
        /// <summary>Liefert die Kalendereinträge aus dem Excel File im dem angegebenen Dateipfad.</summary>
        public List<CalendarEvent> GetCalendarEvents(string filepath)
        {
            Guard.Against.NullOrWhiteSpace(filepath, nameof(filepath));

            if(!filepath.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Das Dateiformat wird nicht unterstützt. Es werden nur .xlsx Dateien unterstützt.");
            }

            if(!File.Exists(filepath))
            {
                throw new FileNotFoundException("Die Excel Datei wurde nicht gefunden.", filepath);
            }
            
            using var parser = new ExcelParser(filepath);
            using var reader = new CsvReader(parser);

            List<CalendarEvent> calendarEntries = reader.GetRecords<CalendarEvent>().ToList();
            
            // Leere Zeilen entfernen
            calendarEntries.RemoveAll(x =>
                string.IsNullOrWhiteSpace(x?.Day) && string.IsNullOrWhiteSpace(x?.StartTime) && string.IsNullOrWhiteSpace(x?.EndTime) &&
                string.IsNullOrWhiteSpace(x?.CalendarName) && string.IsNullOrWhiteSpace(x?.Title));
            
            return calendarEntries;
        }
    }
}