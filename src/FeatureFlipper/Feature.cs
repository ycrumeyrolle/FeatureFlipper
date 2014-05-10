namespace FeatureFlipping
{
    using System;

    public struct Feature : IEquatable<Feature>
    {
        private readonly string name;

        private readonly bool state;

        public Feature(string name, bool state)
        {
            this.name = name;
            this.state = state;
        }

        public string Name { get { return this.name; } }

        public bool State { get { return this.state; } }

        public override bool Equals(object obj)
        {
            if (!(obj is Feature))
            {
                return false;
            }

            return this.Equals((Feature)obj);
        }

        public bool Equals(Feature other)
        {
            return string.Equals(this.name, other.name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return this.name == null ? 0 : this.name.GetHashCode();
        }

        public static bool operator ==(Feature feature1, Feature feature2)
        {
            return feature1.Equals(feature2);
        }

        public static bool operator !=(Feature feature1, Feature feature2)
        {
            return !feature1.Equals(feature2);
        }
    }
}
