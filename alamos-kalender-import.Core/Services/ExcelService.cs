using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.Models;

namespace DennisKae.alamos_kalender_import.Core.Services
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