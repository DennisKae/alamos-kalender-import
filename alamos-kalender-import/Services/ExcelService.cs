using System;
using System.Collections.Generic;
using alamos_kalender_import.Models;
using Ardalis.GuardClauses;

namespace alamos_kalender_import.Services
{
    public class ExcelService
    {
        public List<CalendarEntry> GetCalendarEntries(string filepath)
        {
            Guard.Against.NullOrWhiteSpace(filepath, nameof(filepath));

            if(!filepath.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Das Dateiformat wird nicht unterstüßtzt. Es werden nur .xlsx Dateien unterstützt.");
            }

            return null;
        }
    }
}