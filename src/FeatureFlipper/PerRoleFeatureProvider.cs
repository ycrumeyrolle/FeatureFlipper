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

        private readonly IDictionary<string, IDictionary<string, bool>> roleMatrix = new Dictionary<string, IDictionary<string, bool>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFeatureProvider"/> class.
        /// </summary>
        public PerRoleFeatureProvider(IPrincipalProvider principalProvider)
        {
            if (principalProvider == null)
            {
                throw new ArgumentNullException("principalProvider");
            }

            this.principalProvider = principalProvider;
        }

        /// <inheritsdoc />
        public bool TryIsOn(string feature, out bool isOn)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            IDictionary<string, bool> permissions;
            if (!this.roleMatrix.TryGetValue(feature, out permissions))
            {
                isOn = false;
                return false;
            }

            var principal = this.principalProvider.Principal;
            bool allowed = true;
            bool permissionFound = false;
            foreach (var item in permissions)
            {
                if (item.Key == "*")
                {
                    allowed &= !item.Value;
                    permissionFound = true;
                }
                else
                {
                    if (principal.IsInRole(item.Key))
                    {
                        allowed &= !item.Value;
                        permissionFound = true;
                    }
                }
            }

            isOn = permissionFound && allowed;
            return true;
        }

        /// <summary>
        /// Registers a feature. It map a feature name with a role.
        /// </summary>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="role">The role that is allowed to access to the <paramref name="feature"/>.</param>
        public void RegisterFeature(string feature, string role)
        {
            this.RegisterFeature(feature, role, denied: false);
        }

        /// <summary>
        /// Registers a feature. It map a feature name with a role.
        /// </summary>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="role">The role that is allowed or denied to access to the <paramref name="feature"/>.</param>
        /// <param name="denied">Explicitly deny a feature to the <paramref name="role"/>.</param>
        public void RegisterFeature(string feature, string role, bool denied)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            IDictionary<string, bool> permissions;
            if (!this.roleMatrix.TryGetValue(feature, out permissions))
            {
                permissions = new Dictionary<string, bool>();
                this.roleMatrix.Add(feature, permissions);
            }

            if (permissions.ContainsKey(role))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The feature '{0}' is already registered with the role {1}.", feature, role));
            }

            permissions.Add(role, denied);
        }
    }
}
