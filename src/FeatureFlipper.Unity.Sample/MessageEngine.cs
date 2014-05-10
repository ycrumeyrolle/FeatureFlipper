namespace FeatureFlipper.Unity.Sample
{
    public class MessageEngine
    {
        private readonly IMessageSender messageSender;

        public MessageEngine(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }

        public void Send()
        {
            this.messageSender.SendMessage("The feature 'IMessageSender' is enabled.");
        }
    }
}
