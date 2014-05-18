namespace FeatureFlipper.WebApi
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using FeatureFlipper.WebApi.Properties;

    /// <summary>
    /// Selects the actions only if the feature is enabled.
    /// </summary>
    public class FeatureActionSelector : IHttpActionSelector
    {
        private readonly IHttpActionSelector inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureActionSelector"/> class.
        /// </summary>
        /// <param name="inner">The inner <see cref="IHttpActionSelector"/>.</param> 
        public FeatureActionSelector(IHttpActionSelector inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            this.inner = inner;
        }

        /// <inheritsdoc/>
        public ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
        {
            return this.inner.GetActionMapping(controllerDescriptor);
        }

        /// <summary>
        /// Selects the action for the controller. The action will be selected only if the corresponding feature is enabled.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Encapsulated int he HttpResponseException.")]
        public HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            var action = this.inner.SelectAction(controllerContext);
            var featureAttributes = action.GetCustomAttributes<FeatureAttribute>();
            if (featureAttributes.Count != 0)
            {
                var featureAttribute = (FeatureAttribute)featureAttributes[0];
                if (!Features.Flipper.IsOn(featureAttribute.Name))
                {
                    throw new HttpResponseException(Create410Response(controllerContext));
                }
            }

            return action;
        }
        
        // Create a 410 error response with proper message string. 
        private static HttpResponseMessage Create410Response(HttpControllerContext controllerContext)
        {
            return controllerContext.Request.CreateErrorResponse(HttpStatusCode.Gone, Resources.Gone);
        }
    }
}
