namespace FeatureFlipper.Unity.Tests
{
    using System;
    using System.Linq;
    using Xunit;

    public class ProxyGeneratorFixture
    {
        [Fact]
        public void CreateNullObject_GuardClause()
        {
            // Arrange   
            NullObjectGenerator generator = new NullObjectGenerator();

            // Act & ssert
            Assert.Throws<ArgumentNullException>(() => generator.CreateNullObject(null));
        }

        [Fact]
        public void CreateNullObject()
        {
            // Arrange   
            NullObjectGenerator generator = new NullObjectGenerator();

            // Act 
            var result = generator.CreateNullObject(typeof(IService));

            Assert.NotNull(result);

            // Assert
            Assert.NotNull(result);
            var interfaces = result.GetInterfaces();

            Assert.Equal(1, interfaces.Count(i => i == typeof(IService)));
        }
    }
}
