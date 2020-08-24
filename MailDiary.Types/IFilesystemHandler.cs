// MailDiary - MailDiary.Types - IFilesystemHandler.cs
// created on 2020/08/24

namespace MailDiary.Types
{
  using Mail;

  public interface IFilesystemHandler
  {
    void Save( MailMessage message );
  }
}