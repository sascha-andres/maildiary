// MailDiary - MailDiary.Types - MailConnector.cs
// created on 2020/08/23

namespace MailDiary.Types.Mail
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using Configuration;

  public interface IMailConnector : IDisposable
  {
    void                     Start();
    void                     Done();
    void                     Whitelisted( MailMessage message );
    void                     Unwanted( MailMessage    message );
    IEnumerable<MailMessage> GetMails();
    void                     SetConfiguration( MailConfiguration mailConfiguration );
  }
}