using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Core.ViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services
{
    /// <summary>Service zum Aufruf der Alamos API</summary>
    public class AlamosApiService : IAlamosApiService
    {
        private static readonly MemoryCache _apiTokenCache = new(new MemoryCacheOptions());

        private readonly ILogger<AlamosApiService> _logger;
        private bool _isInitialized;
        private string _serverUrl;
        private string _username;
        private string _password;

        /// <summary>Konstruktor</summary>
        public AlamosApiService(ILogger<AlamosApiService> logger)
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

        /// <summary>Liefert den API-Token zum angegebenen Benutzernamen und Passwort</summary>
        public async Task<string> GetApiToken()
        {
            EnsureInitialization();

            IAlamosRestApiService alamosRestApiService = await GetRestApiService(false);
            var loginRequest = new LoginRequestViewModel { Username = _username, Password = _password };

            LoginResponseViewModel loginResponse = await alamosRestApiService.Login(loginRequest);

            return loginResponse.ApiToken;
        }
        
        /// <summary>
        /// Liefert den API-Token zum angegebenen Benutzernamen und Passwort und cached den Token bis zum Ende seiner Gültigkeit.
        /// </summary>
        public async Task<string> GetApiTokenWithCache()
        {
            EnsureInitialization();

            string apiToken = await _apiTokenCache.GetOrCreateAsync("ApiToken", async cacheEntry =>
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

        /// <summary>Liefert alle verfügbaren Kalender</summary>
        public async Task<List<CalendarResponseViewModel>> GetCalendars()
        {
            EnsureInitialization();

            IAlamosRestApiService alamosRestApiService = await GetRestApiService();
            return await alamosRestApiService.GetCalendars();
        }
        
        /// <summary>Liefert den Kalender mit der angegebenen Bezeichnung</summary>
        public async Task<CalendarResponseViewModel> GetCalendarByName(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            List<CalendarResponseViewModel> allCalendars = await GetCalendars();

            return allCalendars?.FirstOrDefault(x=>x.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false);
        }
        
        /// <summary>Erstellt einen neuen Kalendereintrag</summary>
        public async Task<CreateCalendarEventViewModel> CreateCalendarEvent(CreateCalendarEventViewModel request)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.Null(request.CalendarEvent, nameof(request.CalendarEvent));
            
            EnsureInitialization();

            IAlamosRestApiService alamosRestApiService = await GetRestApiService();
            return await alamosRestApiService.CreateCalendarEvent(request.CalendarEvent.CalendarId, request);
        }        
        
        /// <summary>Löscht einen Kalendereintrag</summary>
        public async Task DeleteCalendarEvent(string calenderId, string calendarEventId)
        {
            Guard.Against.NullOrWhiteSpace(calenderId, nameof(calenderId));
            Guard.Against.NullOrWhiteSpace(calendarEventId, nameof(calendarEventId));
            
            EnsureInitialization();

            IAlamosRestApiService alamosRestApiService = await GetRestApiService();
            await alamosRestApiService.DeleteCalendarEvent(calenderId, calendarEventId);
        }

        private async Task<IAlamosRestApiService> GetRestApiService(bool authorize = true)
        {
            string apiToken = authorize ? await GetApiToken() : null;
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
            
            return RestService.For<IAlamosRestApiService>(httpClient, refitSettings);
        }

        private void EnsureInitialization()
        {
            if(!_isInitialized)
            {
                throw new InvalidOperationException($"Der ${nameof(AlamosApiService)} wurde nicht initialisiert.");
            }
        }

        private DateTime? GetExpirationFromApiToken(string apiToken)
        {
            // Der JWT Token endhält keine gültige base64 payload
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