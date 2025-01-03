using Spectre.Console;

namespace alamos_kalender_import.Services
{
    /// <summary>Service für Benutzerdialoge</summary>
    public class UserPromptService : IUserPromptService
    {
        /// <summary>Fragt einen einfache Benutzereingabe mit der angegebenen Beschreibung ab.</summary>
        public string SimpleTextPrompt(string description) => AnsiConsole.Prompt(new TextPrompt<string>(description));
        
        /// <summary>Fragt eine Passworteingabe mit der angegebenen Beschreibung ab.</summary>
        public string PasswordPrompt(string description) => AnsiConsole.Prompt(new TextPrompt<string>(description).Secret());
    }
}