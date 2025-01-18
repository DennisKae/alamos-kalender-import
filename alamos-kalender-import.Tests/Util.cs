using System;
using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using DennisKae.alamos_kalender_import.Tests.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DennisKae.alamos_kalender_import.Tests
{
    public static class Util
    {
        /// <summary>Liefert den Wert eines User Secrets. Wirft eine Exception, wenn kein Wert gefunden wurde.</summary>
        public static string GetRequiredUserSecret(string key)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ApiConnectionServiceTests).Assembly, false)
                .Build();

            string result = configurationRoot[key];

            if(string.IsNullOrWhiteSpace(result))
            {
                throw new NotFoundException(key, $"Es konnte kein User Secret mit dem Key {key} gefunden werden.");
            }

            return result;
        }

        public static ServiceProvider GetServiceProvider(bool setLogLevelToDebug)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCoreServices();
            
            serviceCollection.AddLogging(builder =>
                {
                    builder.AddDebug();
                    if(setLogLevelToDebug)
                    {
                        builder.SetMinimumLevel(LogLevel.Debug);
                    }
                }
            );

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IApiConnectionService apiConnectionService = serviceProvider.GetRequiredService<IApiConnectionService>();

            string serverUrl = Util.GetRequiredUserSecret("ServerUrl");
            string username = Util.GetRequiredUserSecret("Username");
            string password = Util.GetRequiredUserSecret("Password");
            apiConnectionService.Initialize(serverUrl, username, password);

            return serviceProvider;
        }
        
        public static string GetTestCalendarName() => GetRequiredUserSecret("TestCalendarName");

        public static void DisableApiLogging() => Environment.SetEnvironmentVariable(SharedConstants.DisableApiLoggingEnvironmentVariableName, "true");
    }
}