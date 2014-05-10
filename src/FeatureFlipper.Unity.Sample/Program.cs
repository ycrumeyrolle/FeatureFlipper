namespace FeatureFlipper.Unity.Sample
{
    using System;
    using Microsoft.Practices.Unity;

    public class Program
    {
        public static void Main(string[] args)
        {
            IUnityContainer container = CreateContainer();

            MessageEngine engine = container.Resolve<MessageEngine>();
            engine.Send();

            Console.ReadKey();
        }

        private static IUnityContainer CreateContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.AddFeatureFlippingExtension();
            container.RegisterType<IMessageSender, EmailSender>();

            return container;
        }
    }
}
