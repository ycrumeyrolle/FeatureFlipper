namespace FeatureFlipper
{
    using System.Collections.Generic;

    public interface IFeatureFlipper
    {
        ICollection<IFeatureProvider> Providers { get; }

        /// <summary>
        /// Whether the feature is <c>On</c> or <c>Off</c>.
        /// </summary>
        /// <typeparam name="TContext">The type of the feature.</typeparam>
        /// <param name="contexts">The feature.</param>
        /// <returns>Whether the feature is <c>On</c> or <c>Off</c>.</returns>   
        bool TryIsOn(string feature, out bool isOn);
    }
}
