using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.Models;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using DennisKae.alamos_kalender_import.Settings;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using CalendarEvent = DennisKae.alamos_kalender_import.Core.Models.CalendarEvent;

namespace DennisKae.alamos_kalender_import.Commands
{
    /// <summary>Befehl zum Import aus einer Excel Datei</summary>
    [Description("Importiert Termine aus einer Excel Datei und lädt sie in einen Alamos FE2 Server hoch.\n" +
                 "Es werden keine offiziellen Schnittstellen des Alamos FE2 Servers genutzt. Diese Anwendung kann also jederzeit durch ein FE2 Update kaputt gehen.\n" +
                 "Zuletzt getestete FE2 Version: 2.34.118-STABLE")]
    public class ImportExcelFileCommand : AsyncCommand<ImportExcelFileSettings>
    {
        private readonly IExcelService _excelService;
        private readonly IAlamosApiService _alamosApiService;
        private readonly IUserPromptService _userPromptService;
        private readonly ILogger<ImportExcelFileCommand> _logger;

        /// <summary>Konstruktor</summary>
        public ImportExcelFileCommand(
            IExcelService excelService,
            IAlamosApiService alamosApiService,
            IUserPromptService userPromptService,
            ILogger<ImportExcelFileCommand> logger)
        {
            _excelService = excelService;
            _alamosApiService = alamosApiService;
            _userPromptService = userPromptService;
            _logger = logger;
        }

        /// <summary>Führt den Befehl aus.</summary>
        public override async Task<int> ExecuteAsync(CommandContext context, ImportExcelFileSettings settings)
        {
            ValidateExcelFileSetting(settings);

            List<CalendarEvent> calendarEvents = _excelService.GetCalendarEvents(settings.ExcelFilePath);
            if(calendarEvents?.Any() != true)
            {
                _logger.LogInformation("Es wurden keine Kalendereinträge in der Excel Datei gefunden. \nAusgewertete Excel-Datei: " +
                                       settings.ExcelFilePath);
                return 0;
            }

            _logger.LogInformation("Die folgenden Termine wurden eingelesen:");
            LogCalendarEvents(calendarEvents);
            if(!_userPromptService.GetConfirmation("Fortfahren?"))
            {
                return 0;
            }

            await ValidateAlamosSettings(settings);

            List<CalendarResponseViewModel> allCalendars = await _alamosApiService.GetCalendars();
            if(allCalendars?.Any() != true)
            {
                _logger.LogError("Es konnten keine Kalender vom FE2 Server abgerufen werden.");
                return -1;
            }

            List<CalendarEvent> eventsWithoutCalendar = GetCalendarEventsWithoutCalendar(calendarEvents, allCalendars);
            if(eventsWithoutCalendar.Any())
            {
                _logger.LogError("Für die folgenden Kalendereinträge konnte keine Kalender gefunden werden:");
                LogCalendarEvents(eventsWithoutCalendar);

                _logger.LogInformation("Die folgenden Kalender konnten vom FE2 Server abgerufen werden:");
                LogCalendars(allCalendars);

                _logger.LogInformation("Es wurden keine Kalendereinträge hinzugefügt.");
                return -1;
            }

            _logger.LogInformation("Die Kalendereinträge werden hochgeladen...");
            Table progressTable = GetEmptyCalendarEventTable();
            await AnsiConsole.Live(progressTable)
                .StartAsync(async ctx => 
                {
                    for (int index = 0; index < calendarEvents.Count; index++)
                    {
                        AddCalendarEventToTable(calendarEvents[index], index, progressTable);
                        ctx.Refresh();
                        
                        await CreateCalendarEvent(calendarEvents[index], allCalendars);
                    }
                });
            
            _logger.LogInformation("Upload abgeschlossen.");
            
            return 0;
        }

        private async Task CreateCalendarEvent(CalendarEvent calendarEvent, List<CalendarResponseViewModel> allCalendars)
        {
            CalendarResponseViewModel calendar = allCalendars.FirstOrDefault(x=>x.Name?.Equals(calendarEvent.CalendarName, StringComparison.CurrentCultureIgnoreCase) == true);
            CalendarEventViewModel viewModel = calendarEvent.ToCalendarEventViewModel(calendar);
            
            var request = new CreateCalendarEventViewModel
            {
                CalendarEvent = viewModel
            };
            
            await _alamosApiService.CreateCalendarEvent(request);
        }

        private async Task ValidateAlamosSettings(ImportExcelFileSettings settings)
        {
            bool connectionTestWasSuccessful = false;
            bool showDefaultValues = false;

            while (!connectionTestWasSuccessful)
            {
                if(string.IsNullOrWhiteSpace(settings.Server) || showDefaultValues)
                {
                    settings.Server = _userPromptService.SimpleTextPrompt("FE2 Server-Adresse:", showDefaultValues ? settings.Server : null);
                }

                if(!settings.Server.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                {
                    _logger.LogWarning("Die Server-Adresse muss mit \"http\" oder \"https\" beginnen.");
                    showDefaultValues = true;
                    continue;
                }

                if(string.IsNullOrWhiteSpace(settings.Username) || showDefaultValues)
                {
                    settings.Username = _userPromptService.SimpleTextPrompt("Benutzername:", showDefaultValues ? settings.Username : null);
                }

                if(string.IsNullOrWhiteSpace(settings.Password))
                {
                    settings.Password = AnsiConsole.Prompt(new TextPrompt<string>("Passwort:").Secret());
                }

                _alamosApiService.Initialize(settings.Server, settings.Username, settings.Password);

                try
                {
                    string apiKey = await _alamosApiService.GetApiTokenWithCache();
                    if(!string.IsNullOrWhiteSpace(apiKey))
                    {
                        connectionTestWasSuccessful = true;
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError("Login fehlgeschlagen.");
                    AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
                    _logger.LogInformation("Verbindungs- und Zugangsdaten neu eingeben:");
                    showDefaultValues = true;
                    settings.Password = null;
                }

                if(connectionTestWasSuccessful)
                {
                    _logger.LogInformation("Login erfolgreich.");
                }
            }
        }

        private void ValidateExcelFileSetting(ImportExcelFileSettings settings)
        {
            bool validationSuccessful = false;
            while (!validationSuccessful)
            {
                if(string.IsNullOrWhiteSpace(settings.ExcelFilePath))
                {
                    settings.ExcelFilePath = _userPromptService.SimpleTextPrompt("Excel-Dateiname / Dateipfad:");
                }

                if(settings.ExcelFilePath?.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase) != true)
                {
                    _logger.LogError("Es werden nur Excel Dateien mit der Dateiendung .xlsx unterstützt.");
                    settings.ExcelFilePath = null;
                    continue;
                }

                if(!File.Exists(settings.ExcelFilePath))
                {
                    _logger.LogError("Die Excel Datei wurde nicht gefunden.");
                    settings.ExcelFilePath = null;
                    continue;
                }

                validationSuccessful = true;
            }
        }

        /// <summary>Liefert die Kalendereinträge, für die kein Kalender gefunden werden konnte.</summary>
        private List<CalendarEvent> GetCalendarEventsWithoutCalendar(List<CalendarEvent> calendarEvents,
            List<CalendarResponseViewModel> allCalendars)
        {
            List<CalendarEvent> result = calendarEvents
                .Where(x => allCalendars.All(calendar => calendar.Name?.Equals(x.CalendarName, StringComparison.CurrentCultureIgnoreCase) != true))
                .ToList();
            return result;
        }

        private void LogCalendars(List<CalendarResponseViewModel> calendars)
        {
            var table = new Table()
                .HideHeaders()
                .AddColumn("Name");
            
            calendars.ForEach(x => table.AddRow(x?.Name ?? string.Empty));
            
            AnsiConsole.Write(table);
        }

        private Table GetEmptyCalendarEventTable()
        {
            return new Table()
                .AddColumn("Laufende Nr.")
                .AddColumn("Datum")
                .AddColumn("Start")
                .AddColumn("Ende")
                .AddColumn("Kalender")
                .AddColumn("Titel");
        }

        private void AddCalendarEventToTable(CalendarEvent calendarEvent, int index, Table table)
        {
            var columns = new List<string>()
            {
                (index + 1).ToString(),
                calendarEvent.CleanedDay ?? string.Empty,
                calendarEvent.CleanedStartTime ?? string.Empty,
                calendarEvent.CleanedEndTime ?? string.Empty,
                calendarEvent.CalendarName ?? string.Empty,
                calendarEvent.Title,
            };

            table.AddRow(columns.ToArray());
        }

        private void LogCalendarEvents(List<CalendarEvent> calendarEvents)
        {
            Table table = GetEmptyCalendarEventTable();

            for (int index = 0; index < calendarEvents.Count; index++)
            {
                CalendarEvent calendarEvent = calendarEvents[index];
                AddCalendarEventToTable(calendarEvent, index, table);
            }

            AnsiConsole.Write(table);
        }
    }
}