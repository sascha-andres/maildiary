﻿// MailDiary - MailDiary - Validate.cs
// created on 2020/08/23

namespace MailDiary.Commands
{
  using System;
  using Microsoft.Extensions.CommandLineUtils;
  using Microsoft.Extensions.DependencyInjection;
  using Types;
  using Types.Configuration;

  public static class Validate
  {
    /// <summary>
    /// Create the command and attach it to the command line application
    /// </summary>
    /// <param name="cmdApp">Command line application</param>
    /// <param name="configOption">Global configuration option</param>
    public static void RegisterValidate( this CommandLineApplication cmdApp, CommandOption configOption, ServiceProvider serviceProvider )
    {
      cmdApp.Command( "validate", c => {
                                    c.Description =
                                      "Validate configuration";
                                    c.OnExecute(
                                                () => RunCommand( configOption, serviceProvider )
                                               );
                                  } );
    }

    private static int RunCommand( CommandOption configOption, ServiceProvider serviceProvider )
    {
      var cfg = configOption.Value();
      if ( string.IsNullOrEmpty( cfg ) ) {
        Console.WriteLine( "No configuration path provided" );
        return 1;
      }

      Console.WriteLine( $"Using {cfg}" );
      var config = serviceProvider.GetService<IConfiguration>();
      config.FromYamlFile( cfg );
      
      try {
        config.Validate();
        Console.WriteLine( "configuration validated successfully" );
      } catch ( Exception ex ) {
        Console.WriteLine( $"Error in configuration: {ex.Message}\n{ex.InnerException.Message}" );
        return 1;
      }

      return 0;
    }
  }
}