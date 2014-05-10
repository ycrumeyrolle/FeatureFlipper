namespace FeatureFlipper.Sample
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {            
            // By type feature flipping
            if (Features.Flipper.IsOn<FeatureByType>())
            {
                FeatureByType feature1 = new FeatureByType("FeatureByType is enabled.");
                Console.WriteLine(feature1.GetValue());
            }

            // By key feature flipping
            if (Features.Flipper.IsOn("featureByKey"))
            {
                Console.WriteLine(FeatureByKey());
            }

            // By date feature flipping
            if (Features.Flipper.IsOn("featureByDate"))
            {
                Console.WriteLine(FeatureByDate());
            }

            Console.ReadKey();
        }

        private static string FeatureByKey()
        {
            return "FeatureByKey is enabled.";
        }

        private static string FeatureByDate()
        {
            return "FeatureByDate is enabled.";
        }
    }
}
