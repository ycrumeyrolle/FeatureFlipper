namespace FeatureFlipper.WebApi
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Provides extensions methods to the <see cref="HttpConfiguration"/>.
    /// </summary>
    public static class FlipperHttpConfigurationExtensions
    {
        /// <summary>
        /// Enables feature flipping.
        /// </summary>
        /// <param name="configuration">The <see cref="HttpConfiguration"/>.</param>
        public static void EnableFeatureFlipping(this HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            var actionSelector = configuration.Services.GetActionSelector();
            actionSelector = new FeatureActionSelector(actionSelector);
            configuration.Services.Replace(typeof(IHttpActionSelector), actionSelector);
        }
    }
}
