namespace FeatureFlipper
{
    public interface IFeatureProvider
    {
        bool TryIsOn(string feature, out bool isOn);
    }
}
