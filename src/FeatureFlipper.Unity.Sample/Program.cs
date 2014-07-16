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
            engine.Send(args);

            Console.ReadKey();
        }

        private static IUnityContainer CreateContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.AddFeatureFlippingExtension();
            container.AddFeatureVersioningExtension();
            container.RegisterType<IMessageBuilder, MessageBuilder>();
            container.RegisterType<IMessageFormatter, MessageFormatter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageSender, EmailSender>("Email");
            container.RegisterType<IMessageSender, SmsSender>("SMS");

            return container;
        }
    }
}
