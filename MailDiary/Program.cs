﻿using System;

namespace MailDiary
{
  using System.Reflection;
  using System.Runtime.CompilerServices;
  using Commands;
  using Filesystem;
  using Microsoft.Extensions.CommandLineUtils;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.DependencyInjection.Extensions;
  using Types;
  using Types.Configuration;
  using Types.Mail;

  internal static class Program
  {
    private static void Main( string[] args )
    {
      ServiceProvider serviceProvider = new ServiceCollection()
                                        .AddSingleton<IFilesystemHandler, FilesystemHandler>()
                                        .AddSingleton<IMailConnector, ImapConnector.ImapConnector>()
                                        .AddSingleton<IRenderer, Renderer.Renderer>() // I need a Configuration, but the config file path is passed using an option :(
                                        .BuildServiceProvider();
      
      var app = new CommandLineApplication {
                                             Name        = "MailDiary",
                                             Description = "MailDiary is a diary app reading input from a mail account"
                                           };

      app.HelpOption( "-?|-h|--help" );

      var configOption = app.AddOption( "configuration", "c", "set path to configuration file",
                                       CommandOptionType.SingleValue );

      app.VersionOption( "-v|--version",
                        () =>
                          $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}" );

      app.RegisterProcess( configOption, serviceProvider );
      app.RegisterValidate( configOption );

      try {
        app.Execute( args );
      } catch ( CommandParsingException ex ) {
        Console.WriteLine( ex.Message );
      } catch ( Exception ex ) {
        Console.WriteLine( "Unable to execute application: {0}", ex.Message );
      }
    }
  }
}