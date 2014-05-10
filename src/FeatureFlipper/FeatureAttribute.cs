namespace FeatureFlipper
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class FeatureAttribute : Attribute
    {
        public FeatureAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
