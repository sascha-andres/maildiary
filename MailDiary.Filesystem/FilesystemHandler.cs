namespace MailDiary.Filesystem
{
  using System.IO;
  using Types.Configuration;
  using Types.Mail;

  public class FilesystemHandler
  {
    private readonly Configuration _configuration;

    public FilesystemHandler( Configuration config )
    {
      _configuration = config;
    }

    public void Save( MailMessage message )
    {
      var subFolder      = message.Data.Received.ToString( "yyyy/MM/dd" );
      var completeFolder = Path.Combine( _configuration.MarkdownBasePath, subFolder );
      if ( !Directory.Exists( completeFolder ) ) {
        Directory.CreateDirectory( completeFolder );
      }
      var filename = message.Data.Received.ToString( "HHmmss" ) + ".md";
      File.WriteAllText( Path.Combine( completeFolder, filename),  message.Data.ToMarkdown() );
    }
  }
}