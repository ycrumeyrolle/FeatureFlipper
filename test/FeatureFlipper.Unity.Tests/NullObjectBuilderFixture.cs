namespace FeatureFlipper.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;
    using Xunit;

    public class NullObjectBuilderFixture
    {
        private static ModuleBuilder CreateModuleBuilder()
        {
            AssemblyName assemblyName = new AssemblyName("GeneratedProxies");

            AppDomain thisDomain = Thread.GetDomain();

            var assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            return assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, false); 
        }

        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new NullObjectBuilder(null, null));
            Assert.Throws<ArgumentNullException>(() => new NullObjectBuilder(typeof(IFeature1), null));
            Assert.Throws<ArgumentException>(() => new NullObjectBuilder(typeof(Feature1), CreateModuleBuilder()));
        }

        [Fact]
        public void PreBuildUp_GuardClause()
        {
            // Arrange   
            var moduleBuilder = CreateModuleBuilder();
 
            NullObjectBuilder builder = new NullObjectBuilder(typeof(IService), moduleBuilder);

            // Act 
            var result = builder.Build();

            Assert.NotNull(result);
         
            // Assert
            Assert.NotNull(result);
            var interfaces = result.GetInterfaces();

            Assert.Equal(1, interfaces.Count(i => i == typeof(IService)));
        }
    }
}
