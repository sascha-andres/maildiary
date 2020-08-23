// MailDiary - MailDiary.Types - MailConfiguration.cs
// created on 2020/08/23

namespace MailDiary.Types
{
  public class MailConfiguration
  {
    public string Server   { get; set; }
    public int    Port     { get; set; }
    public string User     { get; set; }
    public string Password { get; set; }
  }
}