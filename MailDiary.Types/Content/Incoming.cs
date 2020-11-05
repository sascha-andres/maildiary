// MailDiary - MailDiary.Types - Incoming.cs
// created on 2020/08/23

namespace MailDiary.Types.Content {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  public class Incoming {
    private readonly Regex  _regexPerson = new Regex("@(?<person>\\S+)");
    private readonly Regex  _regexTag    = new Regex("#(?<tag>\\S+)");
    private          string _subject;

    public Incoming() {
      Tags    = new List<string>();
      Persons = new List<string>();
    }

    public DateTime Received { get; set; }

    public string Subject {
      get => _subject;
      set {
        _subject = value;
        ReadPersons();
        ReadTags();
      }
    }

    public string        Content { get; set; }
    public IList<string> Tags    { get; }
    public IList<string> Persons { get; }

    private void ReadPersons() {
      if (!_regexPerson.IsMatch(_subject)) return;
      var matches = _regexPerson.Matches(_subject);
      for (var i = 0; i < matches.Count; i++) {
        var value = matches[i].Groups["person"].Value;
        Persons.Add(ProcessCamelCase(value));
      }

      var newSubject = _regexPerson.Replace(_subject, "");
      newSubject = newSubject.Replace("  ", " ");
      newSubject = newSubject.Trim();
      _subject   = newSubject;
    }

    private void ReadTags() {
      if (!_regexTag.IsMatch(_subject)) return;
      var matches = _regexTag.Matches(_subject);
      for (var i = 0; i < matches.Count; i++) {
        var value = matches[i].Groups["tag"].Value;
        Tags.Add(ProcessCamelCase(value));
      }

      var newSubject = _regexTag.Replace(_subject, "");
      newSubject = newSubject.Replace("  ", " ");
      newSubject = newSubject.Trim();
      _subject   = newSubject;
    }

    private static string ProcessCamelCase(string incoming) {
      if (IsAllUpper(incoming))
        return incoming;
      var wordList    = new List<string>();
      var currentWord = -1;
      incoming.All(t => {
        if (char.IsUpper(t) || wordList.Count == 0) { // new word
          currentWord++;
          if (wordList.Count != currentWord + 1) wordList.Add("");
        }

        wordList[currentWord] += t;
        return true;
      });
      return wordList.Aggregate((i, j) => i + " " + j);
    }

    private static bool IsAllUpper(string input) {
      return input.All(t => !char.IsLetter(t) || char.IsUpper(t));
    }
  }
}