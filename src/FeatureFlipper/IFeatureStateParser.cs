namespace FeatureFlipper
{    
    public interface IFeatureStateParser
    {
        bool TryParse(string value, out bool isOn);
    }
}
