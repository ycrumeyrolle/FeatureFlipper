namespace FeatureFlipper.Tests
{
    using System;
    using System.Security.Principal;
    using Moq;
    using Xunit;

    public class PerRoleFeatureProviderFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            Assert.Throws<ArgumentNullException>(() => new PerRoleFeatureProvider(null));
        }

        [Fact]
        public void RegisterFeature_GuardClause()
        {
            // Arrange
            var provider = new PerRoleFeatureProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.RegisterFeature(null, "role", true));
            Assert.Throws<ArgumentNullException>(() => provider.RegisterFeature(null, "role"));
            Assert.Throws<ArgumentNullException>(() => provider.RegisterFeature("name", null, true));
            Assert.Throws<ArgumentNullException>(() => provider.RegisterFeature("name", null));
        }

        [Fact]
        public void RegisterFeature_Doublon_ThrowsExceptioon()
        {
            // Arrange
            var provider = new PerRoleFeatureProvider();

            // Act & Assert
            provider.RegisterFeature("name", "role");
            Assert.Throws<InvalidOperationException>(() => provider.RegisterFeature("name", "role"));
        }

        [Theory]
        [InlineData(new[] { "user" }, "admin", false)]
        [InlineData(new[] { "user" }, "user", true)]
        [InlineData(new[] { "user" }, "*", true)]
        [InlineData(new[] { "*" }, "user", false)]
        [InlineData(new[] { "user", "admin", "contributor" }, "admin", true)]
        public void TryIsOn(string[] userRoles, string expectedRole, bool expectedIsOn)
        {
            const string FeatureName = "Custom feature";
            bool isOn;

            Mock<IPrincipalProvider> principalProvider = new Mock<IPrincipalProvider>(MockBehavior.Strict);
            principalProvider
                .Setup(r => r.Principal)
                .Returns(new GenericPrincipal(new GenericIdentity("user"), userRoles));

            var provider = new PerRoleFeatureProvider(principalProvider.Object);
            provider.RegisterFeature(FeatureName, expectedRole);

            bool result = provider.TryIsOn(FeatureName, out isOn);

            Assert.Equal(expectedIsOn, isOn);
            Assert.True(result);
        }

        [Theory]
        [InlineData(new[] { "user" }, "admin", false)]
        [InlineData(new[] { "user" }, "user", false)]
        [InlineData(new[] { "user" }, "*", false)]
        [InlineData(new[] { "*" }, "user", false)]
        [InlineData(new[] { "user", "admin", "contributor" }, "admin", false)]
        public void TryIsOn_DeniedRole(string[] userRoles, string expectedRole, bool expectedIsOn)
        {
            const string FeatureName = "Custom feature";
            bool isOn;

            Mock<IPrincipalProvider> principalProvider = new Mock<IPrincipalProvider>(MockBehavior.Strict);
            principalProvider
                .Setup(r => r.Principal)
                .Returns(new GenericPrincipal(new GenericIdentity("user"), userRoles));

            var provider = new PerRoleFeatureProvider(principalProvider.Object);
            provider.RegisterFeature(FeatureName, expectedRole, true);

            bool result = provider.TryIsOn(FeatureName, out isOn);

            Assert.Equal(expectedIsOn, isOn);
            Assert.True(result);
        }

        [Theory]
        [InlineData(new[] { "user" }, "admin", "contributor", false)]
        [InlineData(new[] { "user" }, "user", "contributor", true)]
        [InlineData(new[] { "user" }, "*", "contributor", true)]
        [InlineData(new[] { "*" }, "user", "contributor", false)]
        [InlineData(new[] { "user", "admin", "contributor" }, "admin", "contributor", false)]
        [InlineData(new[] { "user", "admin", "contributor" }, "admin", "*", false)]
        [InlineData(new[] { "user", "contributor" }, "admin", "*", false)]
        public void TryIsOn_DeniedRoleMixed(string[] userRoles, string expectedRole, string deniedRole, bool expectedIsOn)
        {
            const string FeatureName = "Custom feature";
            bool isOn;

            Mock<IPrincipalProvider> principalProvider = new Mock<IPrincipalProvider>(MockBehavior.Strict);
            principalProvider
                .Setup(r => r.Principal)
                .Returns(new GenericPrincipal(new GenericIdentity("user"), userRoles));

            var provider = new PerRoleFeatureProvider(principalProvider.Object);
            provider.RegisterFeature(FeatureName, expectedRole);
            provider.RegisterFeature(FeatureName, deniedRole, true);

            bool result = provider.TryIsOn(FeatureName, out isOn);

            Assert.Equal(expectedIsOn, isOn);
            Assert.True(result);
        }

        [Fact]
        public void TryIsOn_UnregisteredFeature()
        {
            const string FeatureName = "Custom feature";
            bool isOn;
            var provider = new PerRoleFeatureProvider();

            bool result = provider.TryIsOn(FeatureName, out isOn);

            Assert.False(isOn);
            Assert.False(result);
        }

        [Fact]
        public void TryIsOn_GuardClause()
        {
            bool isOn;
            var provider = new PerRoleFeatureProvider();

            Assert.Throws<ArgumentNullException>(() => provider.TryIsOn(null, out isOn));
        }
    }
}
