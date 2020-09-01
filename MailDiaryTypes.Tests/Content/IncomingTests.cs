// MailDiary - MailDiaryTypes.Tests - IncomingTests.cs
// created on 2020/08/23

namespace MailDiaryTypes.Tests.Content
{
  using System;
  using System.Globalization;
  using System.Linq;
  using MailDiary.Renderer;
  using MailDiary.Types.Configuration;
  using MailDiary.Types.Content;
  using MailDiary.Types.Mail;
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
    [InlineData( "01/01/2000 01:01:01", "subject",       "content", false, @"## (01/01/2000 01:01:01) subject

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject",       "content", false, @"## (01/12/2000 01:01:01) subject

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject @test", "content", false, @"## (01/12/2000 01:01:01) subject
Mentioned:
- test

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject @test @test2", "content", false, @"## (01/12/2000 01:01:01) subject
Mentioned:
- test
- test2

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject #test", "content", false, @"## (01/12/2000 01:01:01) subject
Tagged:
- test

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject #test #test2", "content", false, @"## (01/12/2000 01:01:01) subject
Tagged:
- test
- test2

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject @test #test2", "content", false, @"## (01/12/2000 01:01:01) subject
Mentioned:
- test
Tagged:
- test2

content" )]
    [InlineData( "01/12/2000 01:01:01", "",              "",        true,  @"## (01/12/2000 01:01:01) subject

content" )]
    [InlineData( "01/12/2000 01:01:01", "subject",       "",        true,  @"## (01/12/2000 01:01:01) subject

content" )]
    public void MarkdownGenerationTests( string received, string subject, string content, bool hasException,
                                         string expectedMarkdown )
    {
      var msg = new MailMessage();
      msg.Data.Content = content;
      msg.Data.Received = DateTime.ParseExact( received, "dd/MM/yyyy HH:mm:ss",
                                              CultureInfo.InvariantCulture );
      msg.Data.Subject = subject;

      var renderer = new Renderer( new Configuration
                                   { Processing = new Processing { DateTimeFormat = "dd/MM/yyyy HH:mm:ss" } } );

      var exceptionThrown = false;

      var markdown = "";
      try {
        markdown = renderer.Render( msg );
      } catch ( Exception ex ) {
        _testOutputHelper.WriteLine( ex.Message );
        exceptionThrown = true;
      }

      Assert.Equal( hasException, exceptionThrown );
      if ( !hasException ) {
        Assert.Equal( expectedMarkdown, markdown );
      }
    }

    [Fact]
    public void TagsNotNull()
    {
      var msg = new MailMessage();
      Assert.NotNull( msg.Data.Tags );
    }

    [Fact]
    public void PeopleNotNull()
    {
      var msg = new MailMessage();
      Assert.NotNull( msg.Data.Tags );
    }
    
    [Theory]
    [InlineData( "01/01/2000 01:01:01", "subject",       "content", false, 0 )]
    [InlineData( "01/12/2000 01:01:01", "subject @test", "content", false, 1 )]
    [InlineData( "01/12/2000 01:01:01", "subject @test @test2", "content", false, 2 )]
    public void PersonsTest( string received, string subject, string content, bool hasException,
                                         int numberOfPersons )
    {
      var msg = new MailMessage();
      msg.Data.Content = content;
      msg.Data.Received = DateTime.ParseExact( received, "dd/MM/yyyy HH:mm:ss",
                                              CultureInfo.InvariantCulture );
      msg.Data.Subject = subject;

      Assert.Equal( numberOfPersons, msg.Data.Persons.Count() );
    }
    
    [Theory]
    [InlineData( "01/01/2000 01:01:01", "subject",              "content", false, 0 )]
    [InlineData( "01/12/2000 01:01:01", "subject #test",        "content", false, 1 )]
    [InlineData( "01/12/2000 01:01:01", "subject #test #test2", "content", false, 2 )]
    public void TagsTest( string received, string subject, string content, bool hasException,
                             int    numberOfTags )
    {
      var msg = new MailMessage();
      msg.Data.Content = content;
      msg.Data.Received = DateTime.ParseExact( received, "dd/MM/yyyy HH:mm:ss",
                                              CultureInfo.InvariantCulture );
      msg.Data.Subject = subject;

      Assert.Equal( numberOfTags, msg.Data.Tags.Count() );
    }
  }
}