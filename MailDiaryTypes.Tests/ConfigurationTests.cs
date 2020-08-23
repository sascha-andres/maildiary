namespace MailDiaryTypes.Tests
{
  using System;
  using MailDiary.Types;
  using MailDiary.Types.Configuration;
  using Xunit;
  using Xunit.Abstractions;

  public class ConfigurationTests
  {
    private readonly ITestOutputHelper _testOutputHelper;

    public ConfigurationTests( ITestOutputHelper testOutputHelper )
    {
      _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData( "/does-not-exist", true)]
    [InlineData( "/", false)]
    public void TestConfiguration(string path, bool throwException)
    {
      var cfg = new Configuration {
                                    MarkdownBasePath = path,
                                    Processing       = new Processing(),
                                    Mail             = new MailConfiguration { Server = "imap.test.de", Port = 1234 }
                                  };
      var hasException = false;
      try {
        cfg.Validate();
      } catch (Exception ex){
        _testOutputHelper.WriteLine(ex.Message);
        hasException = true;
      }
      Assert.Equal( throwException, hasException );
    }
  }
}