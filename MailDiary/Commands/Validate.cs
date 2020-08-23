// MailDiary - MailDiary - Validate.cs
// created on 2020/08/23

namespace MailDiary.Commands
{
  using System;
  using Microsoft.Extensions.CommandLineUtils;
  using Types;

  public static class Validate
  {
    public static int RunCommand( CommandLineApplication app, CommandOption configOption, CommandLineApplication validate )
    {
      var cfg = configOption.Value();
      if ( string.IsNullOrEmpty( cfg ) ) {
        Console.WriteLine("No configuration path provided");
        return 1;
      }
      
      Console.WriteLine($"Using {cfg}");
      var config = Configuration.FromYamlFile( cfg );
      try {
        config.Validate();
        Console.WriteLine("configuration validated successfully");
      } catch ( Exception ex ) {
        Console.WriteLine($"Error in configuration: {ex.Message}\n{ex.InnerException.Message}");
        return 1;
      }
      return 0;
    }
  }
}