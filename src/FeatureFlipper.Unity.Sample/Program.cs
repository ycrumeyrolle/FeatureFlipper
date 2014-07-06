namespace FeatureFlipper.Unity.Sample
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Practices.Unity;

    public class Program
    {
        public static void Main(string[] args)
        {
            IUnityContainer container = CreateContainer();

          //  Console.SetOut(TextWriter.Null);

          //  Parallel.For(0, int.MaxValue / 1000, _ =>
           // {
                MessageEngine engine = container.Resolve<MessageEngine>();
                engine.Send(args);
          //  });
            //Console.ReadKey();
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
