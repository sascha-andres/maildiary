// MailDiary - MailDiaryTypes.Tests - MailConfigurationTests.cs
// created on 2020/08/23

namespace MailDiaryTypes.Tests
{
  using System;
  using MailDiary.Types;
  using Xunit;
  using Xunit.Abstractions;

  public class MailConfigurationTests
  {
    private readonly ITestOutputHelper _testOutputHelper;

    public MailConfigurationTests( ITestOutputHelper testOutputHelper )
    {
      _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData( "",             0,    "",          "",                      true )]
    [InlineData( "imap.test.de", 0,    "",          "",                      true )]
    [InlineData( "imap.test.de", 1234, "",          "",                      false )]
    [InlineData( "imap.test.de", 1234, "user@imap", "",                      true )]
    [InlineData( "imap.test.de", 1234, "user@imap", "super-secure-password", false )]
    public void TestServer( string server, int port, string user, string password, bool throwException )
    {
      var cfg = new Configuration {
                                    MarkdownBasePath = "/",
                                    Processing       = new Processing(),
                                    Mail = new MailConfiguration
                                           { Server = server, Port = port, User = user, Password = password }
                                  };
      var hasException = false;
      try {
        cfg.Validate();
      } catch ( Exception ex ) {
        _testOutputHelper.WriteLine( ex.Message );
        hasException = true;
      }

      Assert.Equal( throwException, hasException );
    }
  }
}