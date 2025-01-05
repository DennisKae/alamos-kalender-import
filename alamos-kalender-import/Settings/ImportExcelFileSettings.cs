using System.ComponentModel;
using DennisKae.alamos_kalender_import.Commands;
using Spectre.Console.Cli;

namespace DennisKae.alamos_kalender_import.Settings
{
    /// <summary>CLI Settings für den <see cref="ImportExcelFileCommand"/></summary>
    public class ImportExcelFileSettings : CommandSettings
    {
        /// <summary>URL zum FE2 Server</summary>
        [Description("Adresse (URL) zum FE2 Server. Muss mit \"http\" oder \"https\" beginnen.")]
        [CommandOption("-s|--server <URL>")]
        public string Server { get; set; }

        /// <summary>Benutzername zum Login</summary>
        [CommandOption("-b|--benutzername <Benutzername>")]
        [Description("Benutzername zum Login am FE2 Server. Admin-Benutzer werden nicht unterstützt.")]
        public string Username { get; set; }

        /// <summary>Passwort zum Login</summary>
        [CommandOption("-p|--passwort <Passwort>")]
        [Description("Passwort zum Login am FE2 Server.")]
        public string Password { get; set; }
        
        /// <summary>Pfad zur Excel Datei</summary>
        [CommandOption("-e|--excel-datei <Dateipfad>")]
        [Description("Pfad zur Excel Datei.")]
        public string ExcelFilePath { get; set; }

        /// <summary>Gibt, z.B. bei Problemen mit der Netzwerkverbindungen, mehr Details aus.</summary>
        [CommandOption("--debug")]
        [Description("Gibt, z.B. bei Problemen mit der Netzwerkverbindungen, mehr Details aus.")]
        public bool? Debug { get; set; }
    }
}