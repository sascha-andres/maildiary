// MailDiary - MailDiary.Types - MailMessage.cs
// created on 2020/08/23

namespace MailDiary.Types.Mail
{
  using Content;

  public class MailMessage
  {
    public string   SenderMail     { get; set; }
    public string   ServerIdentity { get; set; }
    public Incoming Data           { get; }

    public MailMessage()
    {
      Data = new Incoming();
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
      return $"Mail from [{SenderMail}] with subject [{Data.Subject}] on [{Data.Received}]";
    }
  }
}