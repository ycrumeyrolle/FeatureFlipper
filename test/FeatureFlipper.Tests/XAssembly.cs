namespace FeatureFlipper.Tests
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class XAssembly : Assembly
    {
        public XAssembly()
        {               
        }

        public XAssembly(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
