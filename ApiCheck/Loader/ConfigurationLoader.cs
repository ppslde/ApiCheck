using ApiCheck.Configuration;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ApiCheck.Loader
{
  public static class ConfigurationLoader
  {
    public static ComparerConfiguration LoadComparerConfiguration(Stream stream)
    {
      if (stream == null || stream.Length == 0)
      {
        return new ComparerConfiguration();
      }

      using (var reader = new StreamReader(stream))
      {
        var deserializer = new DeserializerBuilder()
          .WithNamingConvention(CamelCaseNamingConvention.Instance)
          .Build();

        ComparerConfiguration configuration = deserializer.Deserialize<ComparerConfiguration>(reader);

        return configuration ?? new ComparerConfiguration();
      }
    }
  }
}