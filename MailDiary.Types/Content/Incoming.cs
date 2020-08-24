// MailDiary - MailDiary.Types - Incoming.cs
// created on 2020/08/23

namespace MailDiary.Types.Content
{
  using System;

  public class Incoming
  {
    public DateTime Received { get; set; }
    public string   Subject  { get; set; }
    public string   Content  { get; set; }
  }
}