using System;
using System.Collections.Generic;

namespace DevEn.IoC.Core
{
    public interface IDependencyContainer
    {
        IDictionary<string, object> Configuration { get; }

        IDependencyContainer RegisterConfigurationParameter(string key, object value);

        IDependencyContainer Register<TContract, TImplementation>()
            where TImplementation : class, TContract
            where TContract : class;

        IDependencyContainer Register<TImplementation>()
            where TImplementation : class;

        IDependencyContainer Register(Type implementation);

        IDependencyContainer Register<TContract>(Type implementation)
            where TContract : class;

        IDependencyContainer Register(Type contract, Type implementation);

        IDependencyContainer Register<TImplementation>(Func<TImplementation> resolver)
            where TImplementation : class;

        IDependencyContainer Register<TContract, TImplementation>(Func<TImplementation> resolver)
            where TImplementation : class, TContract
            where TContract : class;

        T Resolve<T>()
            where T : class;

        T Resolve<T>(Type type)
            where T : class;
    }
}
