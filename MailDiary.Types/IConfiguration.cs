// MailDiary - MailDiary.Types - IConfiguration.cs
// created on 2020/08/25

namespace MailDiary.Types
{
  using Configuration;

  public interface IConfiguration
  {
    MailConfiguration Mail             { get; set; }
    Processing        Processing       { get; set; }
    string            MarkdownBasePath { get; set; }
    void              Validate();
    void              FromYamlFile( string path );
  }
}