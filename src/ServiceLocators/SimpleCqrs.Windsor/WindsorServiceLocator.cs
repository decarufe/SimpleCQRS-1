using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace SimpleCqrs.Windsor
{
    public class WindsorServiceLocator : IServiceLocator
    {
		private static bool _isDisposing;

		public WindsorServiceLocator() : this(new  WindsorContainer())
		{
		    
		}


        public WindsorServiceLocator(IWindsorContainer container)
		{
			Container = container;
		}

		public IWindsorContainer Container { private set; get; }

		public T Resolve<T>() where T : class
		{
			try
			{
				return Container.Resolve<T>();
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
				return Container.Resolve<T>(key);
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
				return Container.Resolve(type);
			}
			catch (Exception ex)
			{
				throw new ServiceResolutionException(type, ex);
			}
		}

		public IList<T> ResolveServices<T>() where T : class
		{
			return Container.ResolveAll<T>();
		}

		public void Register<TInterface>(Type implType) where TInterface : class
		{
			var key = string.Format("{0}-{1}", typeof(TInterface).Name, implType.FullName);
		    Container.Register(Component.For<TInterface>().ImplementedBy(implType).Named(key).LifeStyle.Transient);

			// Work-around, also register this implementation to service mapping
			// without the generated key above.
		    Container.Register(Component.For<TInterface>().ImplementedBy(implType));
        }

		public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface
		{
            Container.Register(Component.For(typeof(TInterface)).ImplementedBy<TImplementation>().LifeStyle.Transient);
		}

		public void Register<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
		{
            Container.Register(Component.For(typeof(TInterface)).ImplementedBy<TImplementation>().Named(key).LifeStyle.Transient);
		}

		public void Register(string key, Type type)
		{
            Container.Register(Component.For(type).Named(key).LifeStyle.Transient);
		}

		public void Register(Type serviceType, Type implType)
		{
            Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.Transient);
		}

		public void Register<TInterface>(TInterface instance) where TInterface : class
		{
            Container.Register(Component.For<TInterface>().Instance(instance).LifeStyle.Transient);
		}

		public void Register<TInterface>(Func<TInterface> factoryMethod) where TInterface : class
		{
            Container.Register(Component.For<TInterface>().UsingFactoryMethod(factoryMethod).LifeStyle.Transient);
		}

		public void Release(object instance)
		{
            Container.Release(instance);
		}

		public void Reset()
		{
			throw new NotSupportedException("Windsor does not support reset");
		}

		public TService Inject<TService>(TService instance) where TService : class
		{
            throw new NotSupportedException("Windsor does not support inject");

		}

		public void TearDown<TService>(TService instance) where TService : class
		{
            Container.Release(instance);
			//Not needed for StructureMap it doesn't keep references beyond the life cycle that was configured.
		}

		public void Dispose()
		{
			if (_isDisposing) return;
			if (Container == null) return;

			_isDisposing = true;
			Container.Dispose();
			Container = null;
		}
	}
}