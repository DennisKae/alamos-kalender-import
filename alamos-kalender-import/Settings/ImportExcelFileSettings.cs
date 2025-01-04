using DennisKae.alamos_kalender_import.Commands;
using Spectre.Console.Cli;

namespace DennisKae.alamos_kalender_import.Settings
{
    /// <summary>CLI Settings für den <see cref="ImportExcelFileCommand"/></summary>
    public class ImportExcelFileSettings : CommandSettings
    {
        [CommandOption("-s|--server")]
        public string Server { get; set; }

        [CommandOption("-u|--username|--user|-b|--benutzer")]
        public string Username { get; set; }

        [CommandOption("-p|--password|--passwort")]
        public string Password { get; set; }
    }
}