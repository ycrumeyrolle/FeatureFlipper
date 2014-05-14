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

        protected XAssembly(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
