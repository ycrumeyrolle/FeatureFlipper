namespace FeatureFlipper
{
    using System.Security.Principal;
    using System.Threading;

    /// <summary>
    /// Default implementation of <see cref="IPrincipalProvider"/>.
    /// It gets the <see cref="IPrincipal"/> from <see cref="Thread.CurrentPrincipal"/>.
    /// </summary>
    public class DefaultPrincipalProvider : IPrincipalProvider
    {
        /// <inheritsdoc/>
        public IPrincipal Principal
        {
            get
            {
                return Thread.CurrentPrincipal;
            }
        }
    }
}
