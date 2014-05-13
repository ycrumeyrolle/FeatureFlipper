namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// This implementation of the <see cref="IFeatureProvider"/> tries to get the state of the feature according to the role membership of the current user.
    /// </summary>
    public sealed class PerRoleFeatureProvider : IFeatureProvider
    {
        private readonly IPrincipalProvider principalProvider;

        private readonly IRoleMatrixProvider roleManager;

         /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFeatureProvider"/> class.
        /// </summary>
        public PerRoleFeatureProvider(IRoleMatrixProvider roleManager, IPrincipalProvider principalProvider)
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
        public bool TryIsOn(string feature, out bool isOn)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            var roles = this.roleManager.GetRoleMatrix(feature);
            if (roles == null)
            {
                isOn = false;
                return false;
            }

            var principal = this.principalProvider.Principal;
            foreach (string role in roles)
            {
                if (role == "*")
                {
                    isOn = true;
                    return true;
                }
                else
                {
                    if (principal.IsInRole(role))
                    {
                        isOn = true;
                        return true;
                    }
                }
            }

            isOn = false;
            return true;
        }        
    }
}
