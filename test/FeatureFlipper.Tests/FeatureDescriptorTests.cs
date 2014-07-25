namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public class FeatureDescriptorTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureDescriptor(null, new Dictionary<string, FeatureMetadata>()));
            Assert.Throws<ArgumentNullException>(() => new FeatureDescriptor("X", null));
        }
        
        [Theory]
        [MemberData("GetFeatures")]
        public void Ctor(IDictionary<string, FeatureMetadata> features, int expectedCount)
        {
            // Act
            FeatureDescriptor descriptor = new FeatureDescriptor("myFeature", features);

            // Assert
            Assert.Equal("myFeature", descriptor.Name);
            Assert.NotNull(descriptor.Versions);
            Assert.Equal(expectedCount, descriptor.Versions.Count);
        }

        public static IEnumerable<object[]> GetFeatures()
        {  
            Dictionary<string, FeatureMetadata> features = new Dictionary<string, FeatureMetadata>();
            features.Add("V1", new FeatureMetadata("X", null, typeof(object), "V1", null));
            features.Add("V2", new FeatureMetadata("X", null, typeof(object), "V2", null));
            features.Add("V3", new FeatureMetadata("X", null, typeof(object), "V3", null));
            
            yield return new object[] { features, 3 };
            yield return new object[] { new Dictionary<string, FeatureMetadata>(), 0 };
        }
    }
}
