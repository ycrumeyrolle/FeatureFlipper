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
            container.RegisterType<IMessageBuilder, MessageBuilder>();
            container.RegisterType<IMessageFormatter, MessageFormatter>();
            container.RegisterType<IMessageSender, EmailSender>();

            return container;
        }
    }
}
