using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace alamos_kalender_import
{
    /// <summary>
    /// Represents a type registrar.
    /// Source: https://github.com/spectreconsole/examples/blob/main/examples/Cli/Injection/Infrastructure/TypeRegistrar.cs
    /// </summary>
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _builder;

        /// <summary>Konstruktor</summary>
        public TypeRegistrar(IServiceCollection builder)
        {
            _builder = builder;
        }

        /// <summary>Registers the specified service.</summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        public void Register(Type service, Type implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        /// <summary>Registers the specified instance.</summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        public void RegisterInstance(Type service, object implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        /// <summary>Registers the specified instance lazily.</summary>
        /// <param name="service">The service.</param>
        /// <param name="factory">The factory that creates the implementation.</param>
        public void RegisterLazy(Type service, Func<object> factory)
        {
            if(factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            
            _builder.AddSingleton(service, (provider) => factory());
        }

        /// <summary>
        /// Builds the type resolver representing the registrations
        /// specified in the current instance.
        /// </summary>
        /// <returns>A type resolver.</returns>
        public ITypeResolver Build()
        {
            return new TypeResolver(_builder.BuildServiceProvider());
        }
    }
}