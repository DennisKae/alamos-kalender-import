using System.Threading.Tasks;

namespace DennisKae.alamos_kalender_import.Core.Services.Interfaces
{
    /// <summary>Kümmert sich um die API Verbindung</summary>
    public interface IApiConnectionService
    {
        /// <summary>Initialisiert den Service. Muss einmalig vor dem ersten API Aufruf ausgeführt werden.</summary>
        void Initialize(string serverUrl, string username, string password);

        /// <summary>Liefert einen REST API Service mit Benutzerauthentifizierung</summary>
        Task<T> GetRestService<T>();

        /// <summary>Liefert einen REST API Service</summary>
        Task<T> GetRestService<T>(bool authorize);

        /// <summary>Liefert den API-Token zum angegebenen Benutzernamen und Passwort</summary>
        Task<string> GetApiToken();

        /// <summary>
        /// Liefert den API-Token zum angegebenen Benutzernamen und Passwort und cached den Token bis zum Ende seiner Gültigkeit.
        /// </summary>
        Task<string> GetApiTokenWithCache();
    }
}