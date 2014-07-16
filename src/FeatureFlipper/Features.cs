namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Provides access to features flipping.
    /// </summary>
    public static class Features
    {
        private static readonly ServiceContainer ServiceContainer = new ServiceContainer();
     
        private static IFeatureFlipper flipperInstance;

        private static readonly Lazy<IFeatureFlipper> FlipperInner = new Lazy<IFeatureFlipper>(InitializeFlipper);
      
        /// <summary>
        /// Gets or sets the current <see cref="IFeatureFlipper"/>.
        /// </summary>
        public static IFeatureFlipper Flipper
        {
            get
            {
                return Features.flipperInstance ?? Features.FlipperInner.Value;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                Features.flipperInstance = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ServiceContainer"/> with all references services.
        /// </summary>
        public static ServiceContainer Services
        {
            get
            {
                return ServiceContainer;
            }
        }

        private static IFeatureFlipper InitializeFlipper()
        {
            return ServiceContainer.GetService<IFeatureFlipper>();
        }
    }
}
