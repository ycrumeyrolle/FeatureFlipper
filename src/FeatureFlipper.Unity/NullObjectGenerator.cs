namespace FeatureFlipper.Unity
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    /// <summary>
    /// A generator of NullObject.
    /// </summary>
    public sealed class NullObjectGenerator
    {
        private const string GeneratedAssemblyName = "NullObjectsAssembly";

        private readonly AssemblyBuilder assemblyBuilder;

        private readonly ModuleBuilder moduleBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectGenerator"/> class.
        /// </summary>
        public NullObjectGenerator()
        {
            AssemblyName assemblyName = new AssemblyName { Name = GeneratedAssemblyName };

            AppDomain domain = Thread.GetDomain();

            this.assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule(this.assemblyBuilder.GetName().Name, false);
        }

        /// <summary>
        /// Creates a NullObject from a given type.
        /// </summary>
        /// <remarks>The contract must be an interface.</remarks>
        /// <param name="contract">The type contract. It must be an interface.</param>
        /// <returns>A dynamic NullObject type.</returns>
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
