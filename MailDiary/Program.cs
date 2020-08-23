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

      // The first argument is the option template.
      // It starts with a pipe-delimited list of option flags/names to use
      // Optionally, It is then followed by a space and a short description of the value to specify.
      // e.g. here we could also just use "-o|--option"
      var configOption = app.Option( "-c|--configuration <path-to-config>",
                                   "Set path to configuration file",
                                   CommandOptionType.SingleValue );

      // This is a helper/shortcut method to display version info - it is creating a regular Option, with some defaults.
      // The default help text is "Show version Information"
      app.VersionOption( "-v|--version",
                        () =>
                          $"Version {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}" );

      app.Command( "validate", ( command ) => {
                                 command.OnExecute( () => Validate.RunCommand( app, configOption, command ) );
                               } );

      try {
        app.Execute( args );
      } catch ( CommandParsingException ex ) {
        // You'll always want to catch this exception, otherwise it will generate a messy and confusing error for the end user.
        // the message will usually be something like:
        // "Unrecognized command or argument '<invalid-command>'"
        Console.WriteLine( ex.Message );
      } catch ( Exception ex ) {
        Console.WriteLine( "Unable to execute application: {0}", ex.Message );
      }
    }
  }
}