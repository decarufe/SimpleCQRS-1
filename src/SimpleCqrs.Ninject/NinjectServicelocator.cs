using System;
using System.Collections.Generic;
using Ninject;

namespace SimpleCqrs.Ninject
{
    public class NinjectServiceLocator : IServiceLocator
    {
        private readonly StandardKernel _kernel;

        public NinjectServiceLocator() : this(new StandardKernel())
        {
        }

        public NinjectServiceLocator(StandardKernel kernel)
        {
            _kernel = kernel;
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }

        public T Resolve<T>() where T : class
        {
            try
            {
                return _kernel.Get<T>();
            }
            catch (Exception ex)
            {
                throw new ServiceResolutionException(typeof(T), ex);
            }
        }

        public T Resolve<T>(string key) where T : class
        {
            try
            {
                return _kernel.Get<T>(key);
            }
            catch (Exception ex)
            {
                
                throw new ServiceResolutionException(typeof(T), ex);
            }
        }

        public object Resolve(Type type)
        {
            try
            {
                return _kernel.Get(type);
            }
            catch (Exception ex)
            {
                throw new ServiceResolutionException(type, ex);
            }
        }

        public IList<T> ResolveServices<T>() where T : class
        {
            var services = _kernel.GetAll<T>();
            return new List<T>(services);
        }

        public void Register<TInterface>(Type implType) where TInterface : class
        {
            _kernel.Bind<TInterface>().To(implType);
        }

        public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            _kernel.Bind<TInterface>().To<TImplementation>();
        }

        public void Register<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            _kernel.Bind<TInterface>().To<TImplementation>().Named(key);
        }

        public void Register(string key, Type type)
        {
            _kernel.Bind(type).ToSelf().Named(key);
        }

        public void Register(Type serviceType, Type implType)
        {
            _kernel.Bind(serviceType).To(implType);
        }

        public void Register<TInterface>(TInterface instance) where TInterface : class
        {
            _kernel.Bind<TInterface>().ToConstant(instance);
        }

        public void Release(object instance)
        {
            if (instance == null) return;
            _kernel.Release(instance);
        }

        public void Reset()
        {
            Dispose();
        }

        public TService Inject<TService>(TService instance) where TService : class
        {
            throw new NotImplementedException();
        }

        public void TearDown<TService>(TService instance) where TService : class
        {
            if (instance == null) return;
            _kernel.Release(instance);
        }

        public void Register<TInterface>(Func<TInterface> factoryMethod) where TInterface : class
        {
            _kernel.Bind<TInterface>().ToMethod(ctx => factoryMethod());
        }
    }
}
