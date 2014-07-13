namespace FeatureFlipper.Unity.Tests
{
    [Feature("Feature", Version = "V2")]
    public class Feature2 : IFeature
    {
        public string GetString()
        {
            throw new System.NotImplementedException();
        }
    }
}
