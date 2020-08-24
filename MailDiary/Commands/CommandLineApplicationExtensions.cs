// MailDiary - MailDiary - CommandLineApplicationExtensions.cs
// created on 2020/08/24

namespace MailDiary.Commands
{
  using Microsoft.Extensions.CommandLineUtils;

  internal static class CommandLineApplicationExtensions
  {
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
      }

      return null;
    }
  }
}