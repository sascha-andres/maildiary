﻿namespace MailDiary.Renderer
{
  using System;
  using System.ComponentModel;
  using Scriban;
  using Types.Configuration;
  using Types.Mail;

  /// <summary>
  /// Used to create a text (markdown) representation of a message
  /// </summary>
  public class Renderer
  {
    private readonly Template      _template;
    private readonly Configuration _configuration;

    /// <summary>
    /// Constructor used to set things up
    /// </summary>
    /// <param name="configuration"></param>
    public Renderer( Configuration configuration )
    {
      _configuration = configuration;
      _template      = Template.Parse( configuration.Processing.Template );
    }

    /// <summary>
    /// Render one message
    /// </summary>
    /// <param name="message">Generate string from content of this message</param>
    /// <returns>Rendered text</returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public string Render( MailMessage message )
    {
      if ( null == message ) throw new InvalidEnumArgumentException( "message may not be null" );
      if ( string.IsNullOrEmpty( message.Data.Subject ) )
        throw new InvalidOperationException( "Subject may not be empty" );
      if ( string.IsNullOrEmpty( message.Data.Content ) )
        throw new InvalidOperationException( "Content may not be empty" );
      return _template.Render( new {
                                     message.Data.Subject,
                                     message.Data.Content,
                                     Received =
                                       message.Data.Received.ToString( _configuration.Processing.DateTimeFormat )
                                   } );
    }
  }
}