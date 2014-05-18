namespace FeatureFlipper.Unity.Sample
{
    public class MessageEngine
    {
        private readonly IMessageBuilder builder;
        private readonly IMessageFormatter formatter;
        private readonly IMessageSender sender;

        public MessageEngine(IMessageBuilder builder, IMessageFormatter formatter, IMessageSender sender)
        {
            this.builder = builder;
            this.formatter = formatter;
            this.sender = sender;
        }

        public void Send(string[] text)
        {
            var message = this.builder.BuildMessage(text);
            this.formatter.FormatMessage(message);
            this.sender.SendMessage(message);
        }
    }
}
