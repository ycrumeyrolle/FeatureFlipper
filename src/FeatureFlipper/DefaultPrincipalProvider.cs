namespace FeatureFlipper
{
    using System.Security.Principal;
    using System.Threading;

    public class DefaultPrincipalProvider : IPrincipalProvider
    {
        public IPrincipal Principal
        {
            get
            {
                return Thread.CurrentPrincipal;
            }
        }
    }
}
