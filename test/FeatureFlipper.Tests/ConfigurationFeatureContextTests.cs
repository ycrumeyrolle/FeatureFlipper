﻿namespace FeatureFlipper.Tests
{
    using System;
    using Xunit;

    public class ConfigurationFeatureContextTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConfigurationFeatureContext(null));
        }

        [Fact]
        public void Ctor()
        {
            // Act 
            const string key = "123";
            var context = new ConfigurationFeatureContext(key);

            // Assert
            Assert.NotNull(context.Key);
            Assert.Equal(key, context.Key);
        }
    }
}
