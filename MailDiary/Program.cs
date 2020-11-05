namespace MailDiary {
  using System;
  using System.Reflection;
  using Commands;
  using Filesystem;
  using Microsoft.Extensions.CommandLineUtils;
  using Microsoft.Extensions.DependencyInjection;
  using Types;
  using Types.Configuration;
  using Types.Mail;

  internal static class Program {
    private static void Main(string[] args) {
      var serviceProvider = new ServiceCollection()
                            .AddSingleton<IFilesystemHandler, FilesystemHandler>()
                            .AddSingleton<IMailConnector, ImapConnector.ImapConnector>()
                            .AddSingleton<IRenderer, Renderer.Renderer>()
                            .AddSingleton<IConfiguration, Configuration>()
                            .BuildServiceProvider();

      var app = new CommandLineApplication {
        Name        = "MailDiary",
        Description = "MailDiary is a diary app reading input from a mail account"
      };

      app.HelpOption("-?|-h|--help");

      var configOption = app.AddOption("configuration", "c", "set path to configuration file",
                                       CommandOptionType.SingleValue);

      app.VersionOption("-v|--version",
                        () =>
                          $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");

      app.RegisterProcess(configOption, serviceProvider);
      app.RegisterValidate(configOption, serviceProvider);

      try {
        app.Execute(args);
      }
      catch (CommandParsingException ex) {
        Console.WriteLine(ex.Message);
      }
      catch (Exception ex) {
        Console.WriteLine("Unable to execute application: {0}", ex.Message);
      }
    }
  }
}