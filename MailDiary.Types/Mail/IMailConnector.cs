// MailDiary - MailDiary.Types - MailConnector.cs
// created on 2020/08/23

namespace MailDiary.Types.Mail
{
  using System;
  using System.Collections.Generic;
  using Configuration;

  /// <summary>
  /// Interface implemented by mail connectors
  /// </summary>
  public interface IMailConnector : IDisposable
  {
    /// <summary>
    /// Initialize server connection
    /// </summary>
    void                     Start();
    /// <summary>
    /// Clean up everything used to connect to the server
    /// </summary>
    void                     Done();
    /// <summary>
    /// Move or mark mail as processed (whitelisted)
    /// </summary>
    /// <param name="message">Message to move or to mark</param>
    void                     Whitelisted( MailMessage message );
    /// <summary>
    /// Move or mark mail as processed (unwanted)
    /// </summary>
    /// <param name="message">Message to move or to mark</param>
    void                     Unwanted( MailMessage    message );
    /// <summary>
    /// Get a list of all messages in the mail account
    /// </summary>
    /// <returns>En Ienumerable to iterate over</returns>
    IEnumerable<MailMessage> GetMails();
    void                     SetConfiguration( MailConfiguration mailConfiguration );
  }
}