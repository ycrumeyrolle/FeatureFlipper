namespace FeatureFlipper.Unity.Sample
{
    [Feature("MessageFormatter")]
    public class MessageFormatter : IMessageFormatter
    {
        public string FormatMessage(string message)
        {
            return message.ToUpper();
        }
    }
}
