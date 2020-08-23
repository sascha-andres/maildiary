// MailDiary - MailDiary.Types - MailConfiguration.cs
// created on 2020/08/23

namespace MailDiary.Types
{
  using System;

  /// <summary>
  /// Configuration related to IMAP server
  /// </summary>
  // ReSharper disable once ClassNeverInstantiated.Global
  public class MailConfiguration
  {
    public string Server   { get; set; }
    public int    Port     { get; set; }
    public string User     { get; set; }
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
      if ( User.Trim( ' ', '\t' ).Length > 0 && string.IsNullOrWhiteSpace( Password ) )
        throw new InvalidConfigurationException( "No password for user provided" );
    }
  }
}