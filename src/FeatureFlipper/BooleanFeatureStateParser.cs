namespace FeatureFlipper
{
    public class BooleanFeatureStateParser : IFeatureStateParser
    {
        public bool TryParse(string value, out bool isOn)
        {
            return bool.TryParse(value, out isOn);
        }
    }
}
