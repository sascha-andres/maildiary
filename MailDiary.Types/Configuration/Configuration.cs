namespace MailDiary.Types.Configuration
{
  using System;
  using System.IO;
  using AutoMapper;
  using YamlDotNet.Serialization;

  /// <summary>
  /// Base configuration object
  /// </summary>
  public class Configuration : IConfiguration
  {
    [YamlMember( Alias = "mail", ApplyNamingConventions = false )]
    public MailConfiguration Mail { get; set; }

    [YamlMember( Alias = "processing", ApplyNamingConventions = false )]
    public Processing Processing { get; set; }

    [YamlMember( Alias = "markdown-base-path", ApplyNamingConventions = false )]
    public string MarkdownBasePath { get; set; }

    /// <summary>
    /// Configure mapper
    /// </summary>
    /// <returns>and return a new mapper</returns>
    private IMapper getMapper()
    {
      var config = new MapperConfiguration( cfg => {
                                              cfg.CreateMap<IConfiguration, Configuration>();
                                              cfg.CreateMap<MailConfiguration, MailConfiguration>();
                                              cfg.CreateMap<Processing, Processing>();
                                            } );
      return config.CreateMapper();
    }

    /// <summary>
    /// Read configuration from a YAML file
    /// </summary>
    /// <param name="path">Path to yaml file</param>
    /// <returns>Configuration</returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void FromYamlFile( string path )
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

        var mapper = getMapper();
        mapper.Map( config, this );
      } catch ( Exception ex ) {
        throw new InvalidConfigurationException( "could not deserialize", ex );
      }

      Validate();
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