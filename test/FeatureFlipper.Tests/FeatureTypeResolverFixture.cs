namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Moq;
    using Xunit;

    public class FeatureTypeResolverFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureTypeResolver(null));
        }

        [Theory]
        [MemberData("GetAssembliesResolver")]
        public void GetTypes_NoType_ReturnNull(IAssembliesResolver assembyResolver, int expectedTypes)
        {
            FeatureTypeResolver resolver = new FeatureTypeResolver(assembyResolver);

            // Act
            var result = resolver.GetTypes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTypes, result.Count);
        }

        public static IEnumerable<object[]> GetAssembliesResolver()
        {
            yield return new object[] { GetFullAssemblyResolver(), 3 };
            yield return new object[] { GetEmptyAssemblyResolver(), 0 };
        }

        private static IAssembliesResolver GetFullAssemblyResolver()
        {
            // Arrange
            Mock<XAssembly> assembly0 = new Mock<XAssembly>();
            assembly0
                .Setup(a => a.IsDynamic)
                .Returns(true);

            Mock<Type> type1 = new Mock<Type>();
            type1
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("X") });
            type1.Setup(t => t.Name).Returns("type1");
            Mock<XAssembly> assembly1 = new Mock<XAssembly>();
            assembly1
                .Setup(a => a.GetTypes())
                .Returns(new[] { type1.Object });

            Mock<Type> type2 = new Mock<Type>();
            type2
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("Y") });
            type2.Setup(t => t.Name).Returns("type2");
            Mock<XAssembly> assembly2 = new Mock<XAssembly>();
            assembly2
                .Setup(a => a.GetTypes())
                .Throws(new ReflectionTypeLoadException(new[] { type2.Object }, null));

            Mock<Type> type3 = new Mock<Type>();
            type3
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("Z") });
            type3.Setup(t => t.Name).Returns("type3");
            Mock<XAssembly> assembly3 = new Mock<XAssembly>();
            assembly3
                .Setup(a => a.GetTypes())
                .Returns(new[] { type3.Object });

            Mock<XAssembly> assembly4 = new Mock<XAssembly>();
            assembly4
                .Setup(a => a.GetTypes())
                .Throws(new Exception());

            Mock<XAssembly> assembly5 = new Mock<XAssembly>();
            assembly5
                .Setup(a => a.GetTypes())
                .Returns(new Type[] { null });

            Mock<Type> type6 = new Mock<Type>();
            type6
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new Attribute[0]);
            type2.Setup(t => t.Name).Returns("type6");
            Mock<XAssembly> assembly6 = new Mock<XAssembly>();
            assembly6
                .Setup(a => a.GetTypes())
                .Returns(new[] { type6.Object });

            Mock<IAssembliesResolver> assembliesResolver = new Mock<IAssembliesResolver>(MockBehavior.Strict);
            assembliesResolver
                .Setup(r => r.GetAssemblies())
                .Returns(new[] 
                { 
                    null,
                    assembly0.Object, 
                    assembly1.Object,
                    assembly2.Object,
                    assembly3.Object,
                    assembly4.Object,
                    assembly5.Object,
                    assembly6.Object
                });

            return assembliesResolver.Object;
        }

        private static IAssembliesResolver GetEmptyAssemblyResolver()
        {
            // Arrange      
            Mock<IAssembliesResolver> assembliesResolver = new Mock<IAssembliesResolver>(MockBehavior.Strict);
            assembliesResolver
                .Setup(r => r.GetAssemblies())
                .Returns(new Assembly[] 
                { 
                    null
                });

            return assembliesResolver.Object;
        }
    }
}
