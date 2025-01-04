using System;
using System.Reflection;
using DennisKae.alamos_kalender_import.Core.Services;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DennisKae.alamos_kalender_import
{
    public class Program
    {
        public static int Main(string[] args)
        {
            AnsiConsole.Write(new Rule("[yellow]Alamos Kalender Import[/]").LeftJustified());
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole());
            serviceCollection.AddSingleton<IUserPromptService, UserPromptService>();
            serviceCollection.AddSingleton<IAlamosApiService, AlamosApiService>();
            
            var typeRegistrar = new TypeRegistrar(serviceCollection);
            var app = new CommandApp(typeRegistrar);
            
            app.Configure(config =>
            {
#if DEBUG
                // config.PropagateExceptions();
                // config.ValidateExamples();
#endif
                
                // TODO: https://spectreconsole.net/cli/getting-started
                // TODO: continue...
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

        private static int OldProgram(IServiceCollection serviceCollection, string[] args)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var userPromptService = serviceProvider.GetRequiredService<IUserPromptService>();

            string message = args.Length > 0 ? string.Join(' ', args) : null;

            // message ??= userPromptService.SimpleTextPrompt("What is your message?");
            message ??= userPromptService.PasswordPrompt("What is your password?");
            // message ??= AnsiConsole.Prompt(
            //     new TextPrompt<string>("Enter password:")
            //         .Secret());

            if(string.IsNullOrWhiteSpace(message))
            {
                string versionString = Assembly.GetEntryAssembly()?
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;

                Console.WriteLine($"botsay v{versionString}");
                Console.WriteLine("-------------");
                Console.WriteLine("\nUsage:");
                Console.WriteLine("  botsay <message>");
                return -99;
            }

            return ShowBot(message);
        }

        private static int ShowBot(string message)
        {
            string bot = $"\n        {message}";
            bot += @"
    __________________
                      \
                       \
                          ....
                          ....'
                           ....
                        ..........
                    .............'..'..
                 ................'..'.....
               .......'..........'..'..'....
              ........'..........'..'..'.....
             .'....'..'..........'..'.......'.
             .'..................'...   ......
             .  ......'.........         .....
             .    _            __        ......
            ..    #            ##        ......
           ....       .                 .......
           ......  .......          ............
            ................  ......................
            ........................'................
           ......................'..'......    .......
        .........................'..'.....       .......
     ........    ..'.............'..'....      ..........
   ..'..'...      ...............'.......      ..........
  ...'......     ...... ..........  ......         .......
 ...........   .......              ........        ......
.......        '...'.'.              '.'.'.'         ....
.......       .....'..               ..'.....
   ..       ..........               ..'........
          ............               ..............
         .............               '..............
        ...........'..              .'.'............
       ...............              .'.'.............
      .............'..               ..'..'...........
      ...............                 .'..............
       .........                        ..............
        .....
";
            Console.WriteLine(bot);

            return 1;
        }
    }
}