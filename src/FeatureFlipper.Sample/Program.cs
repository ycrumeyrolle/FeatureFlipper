namespace FeatureFlipper.Sample
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.SetOut(TextWriter.Null);
           // Features.Services.AddRange(typeof(IFeatureStateParser), new VersionStateParser());

       //     Parallel.For(0, int.MaxValue /100000, _ =>
       //     {
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

                // By version feature flipping
                if (Features.Flipper.IsOn("featureByVersion", "V2"))
                {
                    Console.WriteLine(FeatureByVersion());
                }
           // });
          //  Console.ReadKey();
        }

        private static string FeatureByKey()
        {
            return "FeatureByKey is enabled.";
        }

        private static string FeatureByDate()
        {
            return "FeatureByDate is enabled.";
        }

        private static string FeatureByVersion()
        {
            return "FeatureByVersion is enabled.";
        }
    }
}
