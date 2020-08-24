// MailDiary - MailDiary - CommandLineApplicationExtensions.cs
// created on 2020/08/24

namespace MailDiary.Commands
{
  using System;
  using Microsoft.Extensions.CommandLineUtils;

  internal static class CommandLineApplicationExtensions
  {
    /// <summary>
    /// Shortcut to create commands
    /// </summary>
    /// <param name="app">Command line application object</param>
    /// <param name="optionName">Name for option</param>
    /// <param name="shortName">Short name for option</param>
    /// <param name="description">Description for help</param>
    /// <param name="optionType">Type of option</param>
    /// <returns></returns>
    internal static CommandOption AddOption( this CommandLineApplication app, string optionName, string shortName,
                                             string                      description, CommandOptionType optionType )
    {
      var argSuffix = optionType == CommandOptionType.MultipleValue ? "..." : null;
      var argString = optionType == CommandOptionType.SingleValue ? null : $" <arg>{argSuffix}";

      switch ( optionType ) {
        case CommandOptionType.MultipleValue:
        case CommandOptionType.SingleValue:
          return
            app.Option( string.IsNullOrWhiteSpace( shortName ) ? $"-{shortName}|--{optionName}{argString}" : $"--{optionName}{argString}",
                       description, optionType );
        case CommandOptionType.NoValue:
          return
            app.Option( string.IsNullOrWhiteSpace( shortName ) ? $"-{shortName}|--{optionName}" : $"--{optionName}",
                       description, optionType );
        default:
          return null;
      }
    }
  }
}