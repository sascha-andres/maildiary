// MailDiary - MailDiary.Types - Processing.cs
// created on 2020/08/23

namespace MailDiary.Types
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  /// <summary>
  /// Processing contains options related to process mail
  /// </summary>
  public class Processing
  {
    private const string EmailValidation =
      @"(?<name>\A[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*)@(?<domain>(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\z)";
    
    public List<string> WhitelistedSenders { get; set; }

    public Processing()
    {
      WhitelistedSenders = new List<string>();
    }

    /// <summary>
    /// Validate validates the configuration
    /// </summary>
    /// <exception cref="InvalidConfigurationException">Thrown with message containing error</exception>
    public void Validate()
    {
      var r = new Regex(EmailValidation);
      if ( WhitelistedSenders.Any( mail => !r.IsMatch( mail ) ) ) {
        throw new InvalidConfigurationException( "{mail} is not a valid e-mail address" );
      }
    }
  }
}