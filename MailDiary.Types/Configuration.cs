﻿namespace MailDiary.Types
{
  using System;

  public class Configuration
  {
    public MailConfiguration Mail             { get; set; }
    public string            MarkdownBasePath { get; set; }

    public Configuration( string path )
    {
      throw new NotImplementedException();
    }
  }
}