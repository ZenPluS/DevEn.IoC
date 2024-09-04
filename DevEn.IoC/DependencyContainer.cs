using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevEn.IoC.Core;

namespace DevEn.IoC
{
    public sealed class DependencyContainer : IDependencyContainer
    {
        private readonly IDictionary<Type, (Type Type, Func<object> Resolver)> _resolvers = new Dictionary<Type, (Type Type, Func<object> Resolver)>();
        public IDictionary<string, object> Configuration { get; } = new Dictionary<string, object>();

        public IDependencyContainer RegisterConfigurationParameter(string key, object value)
        {
            Configuration[key] = value;
            return this;
        }

        public IDependencyContainer Register<TContract, TImplementation>()
            where TImplementation : class, TContract
            where TContract : class
        {
            _resolvers[typeof(TContract)] = (
                typeof(TImplementation),
                () => Resolve(typeof(TImplementation))
                );
            Register<TImplementation>();
            return this;
        }

        public IDependencyContainer Register<TImplementation>()
            where TImplementation : class
        {
            _resolvers[typeof(TImplementation)] = (
                typeof(TImplementation),
                () => Resolve(typeof(TImplementation))
                );
            return this;
        }

        public IDependencyContainer Register(Type implementation)
        {
            _resolvers[implementation] = (
                implementation,
                () => Resolve(implementation)
            );
            return this;
        }

        public IDependencyContainer Register<TContract>(Type implementation)
            where TContract : class
        {
            _resolvers[typeof(TContract)] = (
                implementation,
                () => Resolve(implementation)
            );
            Register(implementation);
            return this;
        }

        public IDependencyContainer Register<TImplementation>(Func<TImplementation> resolver)
            where TImplementation : class
        {
            _resolvers[typeof(TImplementation)] = (
                typeof(TImplementation),
                resolver
                );
            return this;
        }

        public IDependencyContainer Register<TContract, TImplementation>(Func<TImplementation> resolver)
            where TImplementation : class, TContract
            where TContract : class
        {
            _resolvers[typeof(TContract)] = (
                typeof(TImplementation),
                resolver
                );
            Register(resolver);
            return this;
        }

        public T Resolve<T>() where T : class
        {
            var isResolved = _resolvers.TryGetValue(typeof(T), out var impl);
            if (isResolved)
                return (T)impl.Resolver();

            return null;
        }

        public T Resolve<T>(Type type)
            where T : class
            => (T)Resolve(type);

        private object Resolve(Type contract)
        {
            var typeFound = _resolvers.TryGetValue(contract, out var objectResolver);
            if (!typeFound)
                return null;

            var constructors = objectResolver.Type.GetConstructors();
            if (constructors.Length == 0)
                constructors = objectResolver.Type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            var constructor = constructors.First();
            var constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
                return Activator.CreateInstance(objectResolver.Type);

            var ctorParamLength = constructorParameters.Length;
            var resolvedParams = constructorParameters.Select(ParameterResolver)
                .Where(x => x != null)
                .ToArray();

            return ctorParamLength > resolvedParams.Length
                ? null
                : constructor.Invoke(resolvedParams);
        }

        private object ParameterResolver(ParameterInfo paramInfo)
        {
            var parameterTypeFound = _resolvers.TryGetValue(paramInfo.ParameterType, out var implementation);
            var parameterFound = Configuration.TryGetValue(paramInfo.Name, out var value);

            switch (true)
            {
                case true when parameterTypeFound && !parameterFound:
                    return implementation.Resolver();
                case true when !parameterTypeFound && parameterFound:
                    return value;
                default:
                    return null;
            }
        }
    }
}
