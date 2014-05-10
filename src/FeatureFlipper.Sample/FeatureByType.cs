namespace FeatureFlipper.Sample
{
    public class FeatureByType
    {
        private readonly string value;

        public FeatureByType(string value)
        {
            this.value = value;
        }

        public string GetValue()
        {
            return this.value;
        }
    }
}
