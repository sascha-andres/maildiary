using System;

namespace MailDiary
{
  using System.Reflection;
  using Commands;
  using Microsoft.Extensions.CommandLineUtils;

  class Program
  {
    static void Main( string[] args )
    {
      var app = new CommandLineApplication();
      app.Name        = "MailDiary";
      app.Description = "MailDiary is a diary app reading input from a mail account";

      app.HelpOption( "-?|-h|--help" );

      var configOption = app.Option( "-c|--configuration <path-to-config>",
                                    "Set path to configuration file",
                                    CommandOptionType.SingleValue );

      app.VersionOption( "-v|--version",
                        () =>
                          $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}" );

      app.Command( "validate",
                  ( command ) => { command.OnExecute( () => Validate.RunCommand( app, configOption, command ) ); } );
      
      app.Command( "process",
                  ( command ) => { command.OnExecute( () => Process.RunCommand( app, configOption, command ) ); } );

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