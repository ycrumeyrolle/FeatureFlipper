namespace FeatureFlipper.Unity.Sample
{
    using System.Text;

    [Feature("MessageBuilder")]
    public class MessageBuilder : IMessageBuilder
    {
        public string BuildMessage(string[] text)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0;i < text.Length;i++)
            {
                sb.Append(text[i]);
            }

            return sb.ToString();
        }
    }
}
