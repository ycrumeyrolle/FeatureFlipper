namespace FeatureFlipping
{
    using System;

    public sealed class FeatureProvider : IFeatureProvider
    {
        public FeatureProvider(Func<string, bool?> isOnFunc)
        {
            if (isOnFunc == null)
            {
                throw new ArgumentNullException("isOnFunc");
            }

            this.IsOnFunc = isOnFunc;
        }

        public Func<string, bool?> IsOnFunc { get; set; }

        public bool? IsOn(string feature)
        {
            return IsOnFunc(feature);
        }
    }
}
