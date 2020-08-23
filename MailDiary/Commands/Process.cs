// MailDiary - MailDiary - Process.cs
// created on 2020/08/23

namespace MailDiary.Commands
{
  using System;
  using System.Threading;
  using ImapConnector;
  using Microsoft.Extensions.CommandLineUtils;
  using Types.Configuration;

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

      using var mailConnector = new ImapConnector();
      mailConnector.SetConfiguration( config.Mail );
      mailConnector.Start();

      foreach ( var mail in mailConnector.GetMails() ) {
        if ( config.Processing.IsWhiteListed( mail.SenderMail ) ) {
          Console.WriteLine( mail.ToString() );
        } else {
          mailConnector.Unwanted( mail );
        }
      }

      return 0;
    }
  }
}