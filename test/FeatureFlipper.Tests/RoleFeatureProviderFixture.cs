﻿namespace FeatureFlipper.Tests
{
    using System;
    using System.Security.Principal;
    using Moq;
    using Xunit;

    public class RoleFeatureProviderFixture
    {
        private readonly Mock<IRoleMatrixProvider> roleMatrixProvider;

        public RoleFeatureProviderFixture()
        {
            this.roleMatrixProvider = new Mock<IRoleMatrixProvider>(MockBehavior.Strict);
        }

        [Fact]
        public void Ctor_GuardClause()
        {
            Assert.Throws<ArgumentNullException>(() => new RoleFeatureProvider(null, null));
            Assert.Throws<ArgumentNullException>(() => new RoleFeatureProvider(this.roleMatrixProvider.Object, null));
        }

        [Theory]
        [InlineData(new[] { "user" }, "admin", false)]
        [InlineData(new[] { "user" }, "user", true)]
        [InlineData(new[] { "user" }, "*", true)]
        [InlineData(new[] { "*" }, "user", false)]
        [InlineData(new[] { "user", "admin", "contributor" }, "admin", true)]
        public void TryIsOn_KnowRole_ReturnsIsOn(string[] userRoles, string expectedRole, bool expectedIsOn)
        {
            // Arrange
            const string FeatureName = "Custom feature";
            bool isOn;

            Mock<IPrincipalProvider> principalProvider = new Mock<IPrincipalProvider>(MockBehavior.Strict);
            principalProvider
                .Setup(r => r.Principal)
                .Returns(new GenericPrincipal(new GenericIdentity("user"), userRoles));

            this.roleMatrixProvider
                .Setup(p => p.GetRoleMatrix(FeatureName))
                .Returns(new[] { expectedRole });
            var provider = new RoleFeatureProvider(this.roleMatrixProvider.Object, principalProvider.Object);

            // Act
            bool result = provider.TryIsOn(FeatureName, out isOn);

            // Assert
            Assert.Equal(expectedIsOn, isOn);
            Assert.True(result);
        }
        
        [Fact]
        public void TryIsOn_UnregisteredFeature()
        {
            const string FeatureName = "Custom feature";
            bool isOn;
            this.roleMatrixProvider
                .Setup(p => p.GetRoleMatrix(FeatureName))
                .Returns((string[])null);
            var provider = new RoleFeatureProvider(this.roleMatrixProvider.Object, new DefaultPrincipalProvider());

            bool result = provider.TryIsOn(FeatureName, out isOn);

            Assert.False(isOn);
            Assert.False(result);
        }

        [Fact]
        public void TryIsOn_GuardClause()
        {
            bool isOn;
            var provider = new RoleFeatureProvider(this.roleMatrixProvider.Object, new DefaultPrincipalProvider());

            Assert.Throws<ArgumentNullException>(() => provider.TryIsOn(null, out isOn));
        }
    }
}