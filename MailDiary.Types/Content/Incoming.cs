// MailDiary - MailDiary.Types - Incoming.cs
// created on 2020/08/23

namespace MailDiary.Types.Content
{
  using System;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;

  public class Incoming
  {
    readonly Regex  _regexPerson = new Regex( "@(?<person>\\S+)" );
    readonly Regex  _regexTag    = new Regex( "#(?<tag>\\S+)" );
    private  string _subject;

    public Incoming()
    {
      Tags    = new List<string>();
      Persons = new List<string>();
    }

    public DateTime Received { get; set; }

    public string Subject {
      get => _subject;
      set {
        _subject = value;
        readPersons();
        readTags();
      }
    }

    public string        Content { get; set; }
    public IList<string> Tags    { get; set; }
    public IList<string> Persons { get; set; }

    private void readPersons()
    {
      if ( !_regexPerson.IsMatch( _subject ) ) return;
      var matches = _regexPerson.Matches( _subject );
      for ( int i = 0; i < matches.Count; i++ ) {
        Persons.Add( matches[ i ].Groups[ "person" ].Value );
      }
    }

    private void readTags()
    {
      if ( !_regexTag.IsMatch( _subject ) ) return;
      var matches = _regexTag.Matches( _subject );
      for ( int i = 0; i < matches.Count; i++ ) {
        Tags.Add( matches[ i ].Groups[ "tag" ].Value );
      }
    }
  }
}