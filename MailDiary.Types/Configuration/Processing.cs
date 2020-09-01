// MailDiary - MailDiary.Types - Processing.cs
// created on 2020/08/23

namespace MailDiary.Types.Configuration
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;
  using YamlDotNet.Serialization;

  /// <summary>
  /// Processing contains options related to process mail
  /// </summary>
  public class Processing
  {
    private const string EmailValidation =
      @"(?<name>\A[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*)@(?<domain>(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\z)";

    [YamlMember( Alias = "whitelisted-senders", ApplyNamingConventions = false )]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public List<string> WhitelistedSenders { get; set; }

    [YamlMember( Alias = "date-time-format", ApplyNamingConventions = false )]
    public string DateTimeFormat { get; set; }

    [YamlMember( Alias = "template", ApplyNamingConventions = false )]
    public string Template { get; set; }

    public Processing()
    {
      WhitelistedSenders = new List<string>();
      DateTimeFormat     = "dd/MM/yyyy HH:mm:ss";
      Template = @"{{
  personContent = """"
if persons.size > 0
  personContent = ""\nMentioned:""
  for p in persons
    personContent = personContent + ""\n- "" + p
  end 
end
tagContent = """"
if tags.size > 0
  tagContent = ""\nTagged:""
  for t in tags
    tagContent = tagContent + ""\n- "" + t
  end 
end
inbetweenContent = """"
if persons.size > 0 && tags.size > 0
  inbetweenContent = ""\n""
end
}}## ({{received}}) {{subject}}{{personContent}}{{inbetweenContent}}{{tagContent}}

{{content}}";
    }

    /// <summary>
    /// Validate validates the configuration
    /// </summary>
    /// <exception cref="InvalidConfigurationException">Thrown with message containing error</exception>
    public void Validate()
    {
      var r = new Regex( EmailValidation );
      if ( WhitelistedSenders.Any( mail => !r.IsMatch( mail ) ) ) {
        throw new InvalidConfigurationException( "{mail} is not a valid e-mail address" );
      }
    }

    /// <summary>
    /// Check for whitelisting
    /// </summary>
    /// <param name="mail">Is this mail whitelisted?</param>
    /// <returns>true if whitelisted</returns>
    public bool IsWhiteListed( string mail )
    {
      return WhitelistedSenders.Contains( mail.ToLower() );
    }
  }
}