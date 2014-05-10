namespace FeatureFlipper
{
    using System.Configuration;

    public class DefaultConfigurationReader : IConfigurationReader
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
