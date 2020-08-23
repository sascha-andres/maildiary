// MailDiary - MailDiary.Types - MailConfiguration.cs
// created on 2020/08/23

namespace MailDiary.Types.Configuration
{
  using YamlDotNet.Serialization;

  /// <summary>
  /// Configuration related to IMAP server
  /// </summary>
  // ReSharper disable once ClassNeverInstantiated.Global
  public class MailConfiguration
  {
    [YamlMember( Alias = "server", ApplyNamingConventions = false )]
    public string Server { get; set; }

    [YamlMember( Alias = "port", ApplyNamingConventions = false )]
    public int Port { get; set; }

    [YamlMember( Alias = "user", ApplyNamingConventions = false )]
    public string User { get; set; }

    [YamlMember( Alias = "password", ApplyNamingConventions = false )]
    public string Password { get; set; }

    /// <summary>
    /// Validate validates the configuration
    /// </summary>
    /// <exception cref="InvalidConfigurationException">Thrown with message containing error</exception>
    public void Validate()
    {
      if ( string.IsNullOrWhiteSpace( Server ) )
        throw new InvalidConfigurationException( "{Server} is not a valid server" );
      if ( Port <= 0 || Port > 65535 )
        throw new InvalidConfigurationException( "{Port} is not a valid port definition" );
      // ReSharper disable once InvertIf
      if ( !string.IsNullOrEmpty( User ) && User.Trim( ' ', '\t' ).Length > 0 ) {
        if ( string.IsNullOrWhiteSpace( Password ) )
          throw new InvalidConfigurationException( "No password for user provided" );
      }
    }
  }
}