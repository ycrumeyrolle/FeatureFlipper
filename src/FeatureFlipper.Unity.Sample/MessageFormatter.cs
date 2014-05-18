namespace FeatureFlipper.Unity.Sample
{
    using System;

    [Feature("MessageFormatter")]
    public class MessageFormatter : IMessageFormatter
    {
        public void FormatMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.Text != null)
            {
                message.Text = message.Text.ToUpper();                
            }
        }
    }
}
