namespace FeatureFlipper.Unity.Tests
{
    [Feature("Feature", Version = "V1")]
    public class Feature1 : IFeature
    {
        public string GetString()
        {
            throw new System.NotImplementedException();
        }
    }
}
