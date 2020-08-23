// MailDiary - MailDiary.Types - InvalidConfigurationException.cs
// created on 2020/08/23

namespace MailDiary.Types
{
  using System;

  /// <summary>
  /// Thrown when the configuration is invalid
  /// </summary>
  public class InvalidConfigurationException : Exception
  {
    public InvalidConfigurationException( string message ) : base( message )
    {
    }
    
    public InvalidConfigurationException( string message, Exception innerException ) : base( message, innerException )
    {
    }
  }
}