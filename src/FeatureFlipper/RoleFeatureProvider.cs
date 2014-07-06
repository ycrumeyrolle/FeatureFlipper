namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// This implementation of the <see cref="IFeatureProvider"/> tries to get the state of the feature according to the role membership of the current user.
    /// </summary>
    public sealed class RoleFeatureProvider : IFeatureProvider
    {
        private readonly IPrincipalProvider principalProvider;

        private readonly IRoleMatrixProvider roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFeatureProvider"/> class.
        /// </summary>
        public RoleFeatureProvider(IRoleMatrixProvider roleManager, IPrincipalProvider principalProvider)
        {
            if (roleManager == null)
            {
                throw new ArgumentNullException("roleManager");
            }

            if (principalProvider == null)
            {
                throw new ArgumentNullException("principalProvider");
            }

            this.roleManager = roleManager;
            this.principalProvider = principalProvider;
        }

        /// <inheritsdoc />
        public bool TryIsOn(FeatureMetadata metadata, out bool isOn)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            var roles = this.roleManager.GetRoleMatrix(metadata);
            if (roles == null)
            {
                isOn = false;
                return false;
            }

            var principal = this.principalProvider.Principal;
            foreach (string role in roles)
            {
                if (role == "*" || principal.IsInRole(role))
                {
                    isOn = true;
                    return true;
                }
            }

            isOn = false;
            return true;
        }
    }
}
