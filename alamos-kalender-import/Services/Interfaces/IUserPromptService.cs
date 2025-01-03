namespace alamos_kalender_import.Services
{
    /// <summary>Service für Benutzerdialoge</summary>
    public interface IUserPromptService
    {
        /// <summary>Fragt einen einfache Benutzereingabe mit der angegebenen Beschreibung ab.</summary>
        string SimpleTextPrompt(string description);

        /// <summary>Fragt eine Passworteingabe mit der angegebenen Beschreibung ab.</summary>
        string PasswordPrompt(string description);
    }
}