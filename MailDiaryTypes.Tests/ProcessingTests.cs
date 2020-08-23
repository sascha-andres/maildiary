// MailDiary - MailDiaryTypes.Tests - ProcessingTests.cs
// created on 2020/08/23

namespace MailDiaryTypes.Tests
{
  using System;
  using System.Security.Cryptography;
  using MailDiary.Types;
  using Xunit;
  using Xunit.Abstractions;

  public class ProcessingTests
  {
    private readonly ITestOutputHelper _testOutputHelper;

    public ProcessingTests( ITestOutputHelper testOutputHelper )
    {
      _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData( "asd",          true)]
    [InlineData( "info@test.de", false)]
    public void TestWhiteList( string mail, bool throwException )
    {
      var cfg = new Configuration {
                                    MarkdownBasePath = "/",
                                    Processing       = new Processing (),
                                    Mail = new MailConfiguration
                                           { Server = "test.server.de", Port = 1234 }
                                  };
      cfg.Processing.WhitelistedSenders.Add( mail );
      var hasException = false;
      try {
        cfg.Validate();
      } catch ( Exception ex ) {
        _testOutputHelper.WriteLine( ex.Message );
        hasException = true;
      }

      Assert.Equal( throwException, hasException );
    }

    [Theory]
    [InlineData( "test@info.de", true)]
    [InlineData( "test@test.de", false)]
    public void TestIsWhiteListed( string mail, bool result )
    {
      var cfg = new Configuration {
                                    MarkdownBasePath = "/",
                                    Processing       = new Processing (),
                                    Mail = new MailConfiguration
                                           { Server = "test.server.de", Port = 1234 }
                                  };
      cfg.Processing.WhitelistedSenders.Add( "test@info.de" );
      Assert.Equal( result, cfg.Processing.IsWhiteListed( mail ) );
    }
  }
}