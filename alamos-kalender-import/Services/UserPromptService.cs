using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using Spectre.Console;

namespace DennisKae.alamos_kalender_import.Core.Services
{
    /// <summary>Service für Benutzerdialoge</summary>
    public class UserPromptService : IUserPromptService
    {
        /// <summary>Fragt eine einfache Benutzereingabe mit der angegebenen Beschreibung ab.</summary>
        public string SimpleTextPrompt(string description) => AnsiConsole.Prompt(new TextPrompt<string>(description));

        /// <summary>Fragt eine einfache Benutzereingabe mit der angegebenen Beschreibung ab.</summary>
        public string SimpleTextPrompt(string description, string defaultValue)
        {
            if(string.IsNullOrWhiteSpace(defaultValue))
            {
                return SimpleTextPrompt(description);
            }

            return AnsiConsole.Prompt(new TextPrompt<string>(description).DefaultValue(defaultValue));
        }

        /// <summary>Holt eine Benutzerbestätigung ein.</summary>
        public bool GetConfirmation(string description, bool defaultValue = true) =>
            AnsiConsole.Prompt(
                new TextPrompt<bool>(description)
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(defaultValue)
                    .WithConverter(choice => choice ? "ja" : "nein"));

        /// <summary>Fragt eine Passworteingabe mit der angegebenen Beschreibung ab.</summary>
        public string PasswordPrompt(string description) => AnsiConsole.Prompt(new TextPrompt<string>(description).Secret());
    }
}