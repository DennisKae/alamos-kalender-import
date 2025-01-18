using System;
using System.Linq;
using DennisKae.alamos_kalender_import.Commands;
using DennisKae.alamos_kalender_import.Core;
using DennisKae.alamos_kalender_import.Core.Services;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DennisKae.alamos_kalender_import
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            AnsiConsole.Write(new Rule("[yellow]Alamos Kalender Import[/]").LeftJustified());

            TypeRegistrar typeRegistrar = GetTypeRegistrar(args);
            CommandApp<ImportExcelFileCommand> app = new(typeRegistrar);

            app.Configure(config =>
            {
                config.PropagateExceptions();
#if DEBUG
                config.ValidateExamples();
#endif
            });

            try
            {
                return app.Run(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return -99;
            }
        }

        private static TypeRegistrar GetTypeRegistrar(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(
                builder =>
                {
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "HH:mm:ss ";
                    });
                    
                    if(args?.Any(x => x.Contains("--debug", StringComparison.InvariantCultureIgnoreCase)) ?? false)
                    {
                        builder.SetMinimumLevel(LogLevel.Debug);
                    }
                });
            serviceCollection.AddSingleton<IUserPromptService, UserPromptService>();
            serviceCollection.AddCoreServices();

            return new TypeRegistrar(serviceCollection);
        }
    }
}