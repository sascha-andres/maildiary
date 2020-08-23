// MailDiary - MailDiary - Process.cs
// created on 2020/08/23

namespace MailDiary.Commands
{
  using System;
  using Filesystem;
  using ImapConnector;
  using Microsoft.Extensions.CommandLineUtils;
  using Types.Configuration;
  using Types.Mail;

  public static class Process
  {
    public static int RunCommand( CommandLineApplication app, CommandOption configOption,
                                  CommandLineApplication process )
    {
      var cfg = configOption.Value();
      if ( string.IsNullOrEmpty( cfg ) ) {
        Console.WriteLine( "No configuration path provided" );
        return 1;
      }

      Console.WriteLine( $"Using {cfg}" );
      var config = Configuration.FromYamlFile( cfg );

      using IMailConnector mailConnector = new ImapConnector();
      mailConnector.SetConfiguration( config.Mail );
      mailConnector.Start();

      var filesystemHandler = new FilesystemHandler( config );
      foreach ( var mail in mailConnector.GetMails() ) {
        if ( config.Processing.IsWhiteListed( mail.SenderMail ) ) {
          try {
            filesystemHandler.Save( mail );
            mailConnector.Whitelisted( mail );
          } catch ( Exception ex ) {
            Console.WriteLine($"Error while writing mail: {ex.Message}");
          }
        } else {
          mailConnector.Unwanted( mail );
        }
      }

      return 0;
    }
  }
}