namespace MailDiary.Types
{
  using System;
  using System.IO;
  using System.Net.Http;
  using YamlDotNet.Serialization;

  /// <summary>
  /// Base configuration object
  /// </summary>
  public class Configuration
  {
    [YamlMember( Alias = "mail", ApplyNamingConventions = false )]
    public MailConfiguration Mail       { get; set; }
    [YamlMember( Alias = "processing", ApplyNamingConventions = false )]
    public Processing        Processing { get; set; }

    [YamlMember( Alias = "markdown-base-path", ApplyNamingConventions = false )]
    public string MarkdownBasePath { get; set; }

    /// <summary>
    /// Read configuration from a YAML file
    /// </summary>
    /// <param name="path">Path to yaml file</param>
    /// <returns>Configuration</returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static Configuration FromYamlFile( string path )
    {
      var config       = new Configuration();
      var deserializer = new DeserializerBuilder().Build();

      if ( !File.Exists( path ) ) {
        throw new FileNotFoundException();
      }

      try {
        var fileContent = File.ReadAllText( path );
        config = deserializer.Deserialize<Configuration>( fileContent );
        if ( null == config ) {
          throw new ArgumentException( "configuration could not be deserialized" );
        }
      } catch ( Exception ex ) {
        throw new InvalidConfigurationException( "could not deserialize", ex );
      }

      config.Validate();

      return config;
    }

    /// <summary>
    /// Validate validates the configuration
    /// </summary>
    /// <exception cref="InvalidConfigurationException">Thrown with message containing error</exception>
    public void Validate()
    {
      if ( !Directory.Exists( MarkdownBasePath ) ) {
        throw new InvalidConfigurationException( $"Path for markdown does not exist: {MarkdownBasePath}" );
      }

      if ( null == Processing ) throw new InvalidConfigurationException( "No options for processing set" );
      Processing.Validate();
      if ( null == Mail ) throw new InvalidConfigurationException( "No options for mail set" );
      Mail.Validate();
    }
  }
}