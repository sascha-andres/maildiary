// MailDiary - MailDiaryTypes.Tests - IncomingTests.cs
// created on 2020/08/23

namespace MailDiaryTypes.Tests.Content
{
  using System;
  using System.Globalization;
  using MailDiary.Types.Content;
  using Xunit;
  using Xunit.Abstractions;

  public class IncomingTests
  {
    private readonly ITestOutputHelper _testOutputHelper;

    public IncomingTests( ITestOutputHelper testOutputHelper )
    {
      _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData( "01/01/2000 01:01:01", "subject", "content", false, @"## (01/01/2000 01:01:01) subject

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject", "content", false, @"## (01/12/2000 01:01:01) subject

content" )]
    [InlineData( "01/12/2000 01:01:01", "",        "",        true,  @"## (01/12/2000 01:01:01) subject

content" )]
    public void MarkdownGenerationTests( string received, string subject, string content, bool hasException,
                                         string expectedMarkdown )
    {
      var incoming = new Incoming {
                                    Content = content, Received = DateTime.ParseExact( received, "dd/MM/yyyy HH:mm:ss",
                                     CultureInfo.InvariantCulture ),
                                    Subject = subject
                                  };
      var exceptionThrown = false;
      var markdown        = "";
      try {
        markdown = incoming.ToMarkdown();
      } catch ( Exception ex ) {
        _testOutputHelper.WriteLine( ex.Message );
        exceptionThrown = true;
      }

      Assert.Equal( hasException, exceptionThrown );
      if ( !hasException ) {
        Assert.Equal( expectedMarkdown, markdown );
      }
    }
  }
}