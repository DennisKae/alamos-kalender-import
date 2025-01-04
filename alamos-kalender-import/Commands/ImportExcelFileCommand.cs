using System;
using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Settings;
using Spectre.Console.Cli;

namespace DennisKae.alamos_kalender_import.Commands
{
    /// <summary>Befehl zum Import aus einer Excel Datei</summary>
    public class ImportExcelFileCommand : AsyncCommand<ImportExcelFileSettings>
    {
        /// <summary>Executes the command.</summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, ImportExcelFileSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}