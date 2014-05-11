namespace FeatureFlipper
{
    using System.Security.Principal;

    /// <summary>
    /// Represents a provider of <see cref="IPrincipal"/>.
    /// </summary>
    public interface IPrincipalProvider
    {
        /// <summary>
        /// Gets the current <see cref="IPrincipal"/>?
        /// </summary>
        IPrincipal Principal { get; }
    }
}
