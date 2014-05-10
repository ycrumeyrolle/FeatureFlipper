namespace FeatureFlipper.Unity.Sample
{
    using System;

    public class EmailSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("The following message was sent by email :");
            Console.WriteLine(message);
        }
    }
}
