namespace FeatureFlipper.Unity.Sample
{
    using System.Text;

    [Feature("MessageBuilder")]
    public class MessageBuilder : IMessageBuilder
    {
        public Message BuildMessage(string[] text)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0;i < text.Length;i++)
            {
                sb.Append(text[i]);
                if (i < text.Length - 1)
                {
                    sb.Append(' ');
                }
            }

            return new Message { Text = sb.ToString() };
        }
    }
}
