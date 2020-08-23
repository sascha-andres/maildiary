// MailDiary - MailDiary.Types - Incoming.cs
// created on 2020/08/23

namespace MailDiary.Types.Content
{
  using System;

  public class Incoming
  {
    private const string MarkDownTemplate = @"## (%%RECEIVED%%) %%SUBJECT%%

%%CONTENT%%";

    public DateTime Received { get; set; }
    public string   Subject  { get; set; }
    public string   Content  { get; set; }

    public string ToMarkdown()
    {
      if ( string.IsNullOrEmpty( Subject ) )
        throw new InvalidOperationException( "Subject may not be empty" );
      if ( string.IsNullOrEmpty( Content ) )
        throw new InvalidOperationException( "Content may not be empty" );

      return MarkDownTemplate
             .Replace( "%%RECEIVED%%", Received.ToString("dd/MM/yyyy HH:mm:ss") )
             .Replace( "%%SUBJECT%%",  Subject.ToString() )
             .Replace( "%%CONTENT%%",  Content.ToString() );
    }
  }
}