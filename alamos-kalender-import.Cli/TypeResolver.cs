using System;
using Spectre.Console.Cli;

namespace DennisKae.alamos_kalender_import.Cli
{
    /// <summary>
    /// Represents a type resolver.
    /// Source: https://github.com/spectreconsole/examples/blob/main/examples/Cli/Injection/Infrastructure/TypeResolver.cs
    /// </summary>
    public sealed class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        /// <summary>Konstruktor</summary>
        public TypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        
        /// <summary>Resolves an instance of the specified type.</summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type, or <c>null</c> if no registration for the specified type exists.</returns>
        public object Resolve(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return _provider.GetService(type);
        }
        
        public void Dispose()
        {
            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}