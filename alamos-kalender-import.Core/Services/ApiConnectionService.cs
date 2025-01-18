using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.Models;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services
{
    /// <summary>Kümmert sich um die API Verbindung</summary>
    public class ApiConnectionService : IApiConnectionService
    {
        private static readonly MemoryCache ApiTokenCache = new(new MemoryCacheOptions());

        private readonly ILogger<ApiConnectionService> _logger;
        
        private bool _isInitialized;
        
        /// <summary>URL zum FE2 Server</summary>
        private string _serverUrl;

        /// <summary>Benutzername zum Login am FE2 Server</summary>
        private string _username;

        /// <summary>Passwort zum Login am FE2 Server</summary>
        private string _password;

        /// <summary>Konstruktor</summary>
        public ApiConnectionService(ILogger<ApiConnectionService> logger)
        {
            _logger = logger;
        }
        
        /// <summary>Initialisiert den Service. Muss einmalig vor dem ersten API Aufruf ausgeführt werden.</summary>
        public void Initialize(string serverUrl, string username, string password)
        {
            _serverUrl = Guard.Against.NullOrWhiteSpace(serverUrl, nameof(serverUrl));
            _username = Guard.Against.NullOrWhiteSpace(username, nameof(username));
            _password = Guard.Against.NullOrWhiteSpace(password, nameof(password));
            _isInitialized = true;
        }

        /// <summary>Liefert einen REST API Service mit Benutzerauthentifizierung</summary>
        public async Task<T> GetRestService<T>() => await GetRestService<T>(true);

        /// <summary>Liefert einen REST API Service</summary>
        public async Task<T> GetRestService<T>(bool authorize)
        {
            EnsureInitialization();

            string apiToken = authorize ? await GetApiTokenWithCache() : null;
            var httpClient = new HttpClient(new AuthHeaderHandler(_logger, apiToken))
            {
                BaseAddress = new Uri(_serverUrl)
            };

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new DateTimeConverter());

            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(serializerOptions)
            };

            return RestService.For<T>(httpClient, refitSettings);
        }

        /// <summary>Liefert den API-Token zum angegebenen Benutzernamen und Passwort</summary>
        public async Task<string> GetApiToken()
        {
            IAlamosRestApiService alamosRestApiService = await GetRestService<IAlamosRestApiService>(false);
            var loginRequest = new LoginRequestViewModel { Username = _username, Password = _password };

            LoginResponseViewModel loginResponse = await alamosRestApiService.Login(loginRequest);

            return loginResponse.ApiToken;
        }

        /// <summary>
        /// Liefert den API-Token zum angegebenen Benutzernamen und Passwort und cached den Token bis zum Ende seiner Gültigkeit.
        /// </summary>
        public async Task<string> GetApiTokenWithCache()
        {
            string apiToken = await ApiTokenCache.GetOrCreateAsync("ApiToken", async cacheEntry =>
            {
                string apiToken = await GetApiToken();

                if(string.IsNullOrWhiteSpace(apiToken))
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(0);
                    return apiToken;
                }

                DateTime? expiration = GetExpirationFromApiToken(apiToken);
                if(expiration.HasValue)
                {
                    cacheEntry.SlidingExpiration = expiration.Value.Subtract(DateTime.Now);
                }
                else
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(1);
                }

                return apiToken;
            });

            return apiToken;
        }

        private void EnsureInitialization()
        {
            if(!_isInitialized)
            {
                throw new InvalidOperationException($"Der ${nameof(ApiConnectionService)} wurde nicht initialisiert.");
            }
        }

        private DateTime? GetExpirationFromApiToken(string apiToken)
        {
            // Der JWT Token enthält keine gültige base64 payload. Somit kann seine Gültigkeitsdauer nicht ausgelesen werden.
            // Möglicherweise wird hier eine unübliche Kodierung genutzt.
            // TODO: Weiter analysieren und fixen.
            return DateTime.Now.AddSeconds(30);

            // if(string.IsNullOrWhiteSpace(apiToken))
            // {
            //     return null;
            // }

            // try
            // {
            //     var handler = new JsonWebTokenHandler();
            //     SecurityToken securityToken = handler.ReadToken(apiToken);
            //     _logger.LogWarning($"Der JWT Token ist abgelaufen ({securityToken?.ValidTo:dd.MM.yyyy_HH:HH:mm:ss}). \nJWT: {apiToken}");
            //     if(securityToken == null || securityToken.ValidTo <= DateTime.Now)
            //     {
            //         _logger.LogWarning($"Der JWT Token ist abgelaufen ({securityToken?.ValidTo:dd.MM.yyyy_HH:HH:mm:ss}). \nJWT: {apiToken}");
            //         return null;
            //     }
            //     
            //     return securityToken.ValidTo > DateTime.Now ? securityToken.ValidTo : null;
            // }
            // catch (Exception exception)
            // {
            //     _logger.LogError(exception, $"Fehler beim Einlesen des JWT Tokens. \nJWT Token: {apiToken}");
            //     return null;
            // }
        }
    }
}