// MailDiary - MailDiary.Types - IRenderer.cs
// created on 2020/08/24

namespace MailDiary.Types
{
  using Mail;

  public interface IRenderer
  {
    string Render( MailMessage message );
  }
}