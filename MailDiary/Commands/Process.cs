// MailDiary - MailDiary - Process.cs
// created on 2020/08/23

namespace MailDiary.Commands
{
  using System;
  using Filesystem;
  using ImapConnector;
  using Microsoft.Extensions.CommandLineUtils;
  using Microsoft.Extensions.DependencyInjection;
  using Types;
  using Types.Configuration;
  using Types.Mail;

  public static class Process
  {
    /// <summary>
    /// Create the command and attach it to the command line application
    /// </summary>
    /// <param name="cmdApp">Command line application</param>
    /// <param name="configOption">Global configuration option</param>
    public static void RegisterProcess( this CommandLineApplication cmdApp, CommandOption configOption,
                                        ServiceProvider           serviceProvider )
    {
      cmdApp.Command( "process", c => {
                                   c.Description =
                                     "Get mails and generate markdown file(s)";
                                   var preserveMails = c.AddOption( "preserve-mails", "p",
                                                                   "do not move mails to folders",
                                                                   CommandOptionType.NoValue );
                                   c.OnExecute(
                                               () => RunCommand( configOption, preserveMails, serviceProvider )
                                              );
                                 } );
    }

    private static int RunCommand( CommandOption   configOption, CommandOption preserveMails,
                                   ServiceProvider serviceProvider )
    {
      var cfg = configOption.Value();
      if ( string.IsNullOrEmpty( cfg ) ) {
        Console.WriteLine( "No configuration path provided" );
        return 1;
      }

      Console.WriteLine( $"Using {cfg}" );
      var config = serviceProvider.GetService<IConfiguration>();
      config.FromYamlFile( cfg );

      using var mailConnector = serviceProvider.GetService<IMailConnector>();
      mailConnector.Start();

      var filesystemHandler = serviceProvider.GetService<IFilesystemHandler>();
      foreach ( var mail in mailConnector.GetMails() ) {
        if ( config.Processing.IsWhiteListed( mail.SenderMail ) ) {
          try {
            filesystemHandler.Save( mail );
            if ( !preserveMails.HasValue() )
              mailConnector.Whitelisted( mail );
          } catch ( Exception ex ) {
            Console.WriteLine( $"Error while writing mail: {ex.Message}" );
          }
        } else {
          if ( !preserveMails.HasValue() )
            mailConnector.Unwanted( mail );
        }
      }

      return 0;
    }
  }
}