namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    public class ProxyGenerator
    {
        private const string GeneratedAssemblyName = "GeneratedProxies";

        private const string GeneratedAssemblyFileName = GeneratedAssemblyName + ".dll";
    
        private readonly AssemblyBuilder assemblyBuilder;

        private readonly ModuleBuilder moduleBuilder;

        public ProxyGenerator()
        {
            AssemblyName assemblyName = new AssemblyName { Name = GeneratedAssemblyName };

            AppDomain thisDomain = Thread.GetDomain();

            this.assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(this.assemblyBuilder.GetName().Name, false);
        }

        public Type CreateNullObject(Type contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            var builder = new NullObjectBuilder(contract, this.moduleBuilder);

            Type nullObjectType = builder.Build();

            return nullObjectType;
        }
    }
}
