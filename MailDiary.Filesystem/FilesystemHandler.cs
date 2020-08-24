namespace MailDiary.Filesystem
{
  using System.IO;
  using Renderer;
  using Types.Configuration;
  using Types.Mail;

  public class FilesystemHandler
  {
    private const    string        _FolderPattern   = "yyyy/MM/dd";
    private const    string        _FileNamePattern = "HHmmss";
    private readonly Configuration _configuration;
    private readonly Renderer      _renderer;

    /// <summary>
    /// Constructor taking the configuration to handle writes to the storage
    /// </summary>
    /// <param name="config"></param>
    public FilesystemHandler( Configuration config )
    {
      _configuration = config;
      _renderer = new Renderer( _configuration );
    }

    /// <summary>
    /// Save will get the markdown and save it to the disk
    /// </summary>
    /// <param name="message">Message to save as markdown</param>
    public void Save( MailMessage message )
    {
      var subFolder      = message.Data.Received.ToString( _FolderPattern );
      var completeFolder = Path.Combine( _configuration.MarkdownBasePath, subFolder );
      if ( !Directory.Exists( completeFolder ) ) {
        Directory.CreateDirectory( completeFolder );
      }

      var filename = message.Data.Received.ToString( _FileNamePattern ) + ".md";
      File.WriteAllText( Path.Combine( completeFolder, filename ),
                        _renderer.Render( message ) );
    }
  }
}