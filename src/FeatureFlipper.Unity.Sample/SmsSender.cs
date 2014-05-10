namespace FeatureFlipper.Unity.Sample
{
    using System;

    public class SmsSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("The following message was sent by SMS :");
            Console.WriteLine(message);
        }
    }
}
