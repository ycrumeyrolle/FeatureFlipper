namespace FeatureFlipper
{
    using System.Security.Principal;

    public interface IPrincipalProvider
    {
        IPrincipal Principal { get; }
    }
}
