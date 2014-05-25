namespace FeatureFlipper.Unity.Sample
{
    using System;

    [Feature("Sender", Version = "Email")]
    public class EmailSender : IMessageSender
    {
        public void SendMessage(Message message)
        {
            Console.WriteLine("The following message was sent by email :");
            Console.WriteLine(message.Text);
        }
    }
}
