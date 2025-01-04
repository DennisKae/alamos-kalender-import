using Ardalis.GuardClauses;
using DennisKae.alamos_kalender_import.Core.Services;
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
                .AddUserSecrets(typeof(AlamosApiServiceTests).Assembly, false)
                .Build();
            
            string result = configurationRoot[key];
            
            if(string.IsNullOrWhiteSpace(result))
            {
                throw new NotFoundException(key,$"Es konnte kein User Secret mit dem Key {key} gefunden werden.");
            }
            
            return result;
        }

        public static ServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                builder.AddDebug()
                    .SetMinimumLevel(LogLevel.Debug)
            );
            
            serviceCollection.AddSingleton<IAlamosApiService, AlamosApiService>();
            serviceCollection.AddSingleton<IExcelService, ExcelService>();
            
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            
            IAlamosApiService alamosApiService = serviceProvider.GetRequiredService<IAlamosApiService>();

            string serverUrl = Util.GetRequiredUserSecret("ServerUrl");
            string username = Util.GetRequiredUserSecret("Username");
            string password = Util.GetRequiredUserSecret("Password");
            alamosApiService.Initialize(serverUrl, username, password);
            
            return serviceProvider;
        }
    }
}