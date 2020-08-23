using System;

namespace MailDiary.ImapConnector
{
  using System.Collections.Generic;
  using System.Linq;
  using MailKit;
  using MailKit.Net.Imap;
  using MailKit.Search;
  using MailKit.Security;
  using Types.Configuration;
  using Types.Mail;

  public class ImapConnector : IMailConnector
  {
    private const string            ProcessedFolderName = "Processed";
    private const string            UnwantedFolderName  = "Unwanted";
    private       MailConfiguration _mailConfiguration;
    private       ImapClient        _client;
    private       IMailFolder       _whiteListed;
    private       IMailFolder       _unwanted;

    /// <summary>
    /// Connect to Imap server and prepare everything
    /// </summary>
    public void Start()
    {
      _client = new ImapClient();
      _client.Connect( _mailConfiguration.Server, _mailConfiguration.Port, SecureSocketOptions.SslOnConnect );
      _client.Authenticate( _mailConfiguration.User, _mailConfiguration.Password );
      _client.Inbox.Open( FolderAccess.ReadWrite );
      
      foreach ( var folder in _client.Inbox.GetSubfolders() ) {
        if ( null == _whiteListed && folder.Name == ProcessedFolderName )
          _whiteListed = folder;
        if ( null == _unwanted && folder.Name == UnwantedFolderName )
          _unwanted = folder;
      }
      GenerateFolders();
    }

    /// <summary>
    /// If missing generate folders to move mails to
    /// </summary>
    private void GenerateFolders()
    {
      _whiteListed ??= _client.Inbox.Create( ProcessedFolderName, true );
      _unwanted    ??= _client.Inbox.Create( UnwantedFolderName,  true );
    }

    /// <summary>
    /// Disconnect and cleanup
    /// </summary>
    public void Done()
    {
      _client.Disconnect( true );
      _client = null;
    }

    /// <summary>
    /// Mail was whitelisted and work has finished. Remove from INBOX
    /// </summary>
    /// <param name="message">Message worked on</param>
    public void Whitelisted( MailMessage message )
    {
      _client.Inbox.MoveTo( UniqueId.Parse( message.ServerIdentity ), _whiteListed );
    }

    /// <summary>
    /// Mail is unwanted. Move to unwanted folder
    /// </summary>
    /// <param name="message">Mail worked on</param>
    public void Unwanted( MailMessage message )
    {
      _client.Inbox.MoveTo( UniqueId.Parse( message.ServerIdentity ), _unwanted );
    }

    /// <summary>
    /// Returns data from mail server
    /// </summary>
    /// <returns>IEnumerable of <see cref="MailMessage"/></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<MailMessage> GetMails()
    {
      var uids = _client.Inbox.Search( SearchQuery.All );
      foreach ( var uid in uids ) {
        var message = _client.Inbox.GetMessage( uid );
        yield return new MailMessage {
                                       SenderMail     = message.From.Mailboxes.First().Address,
                                       ServerIdentity = uid.ToString(),
                                       Data = {
                                                Content  = message.TextBody,
                                                Received = message.Date.LocalDateTime,
                                                Subject  = message.Subject
                                              }
                                     };
      }
    }

    /// <summary>
    /// Used to pass in configuration settings
    /// </summary>
    /// <param name="mailConfiguration">Object containing config data</param>
    public void SetConfiguration( MailConfiguration mailConfiguration )
    {
      _mailConfiguration = mailConfiguration;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
      if ( null != _client ) Done();
    }
  }
}