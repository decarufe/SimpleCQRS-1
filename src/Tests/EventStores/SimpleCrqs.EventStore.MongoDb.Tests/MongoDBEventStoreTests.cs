using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCqrs;
using SimpleCqrs.Domain;
using SimpleCqrs.EventStore.MongoDb;
using SimpleCqrs.Eventing;

namespace SimpleCrqs.EventStore.MongoDb.Tests
{
    public class FakeServiceLocator : IServiceLocator
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public IList<T> ResolveServices<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Register<TInterface>(Type implType) where TInterface : class
        {
            throw new NotImplementedException();
        }

        public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            throw new NotImplementedException();
        }

        public void Register<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            throw new NotImplementedException();
        }

        public void Register(string key, Type type)
        {
            throw new NotImplementedException();
        }

        public void Register(Type serviceType, Type implType)
        {
            throw new NotImplementedException();
        }

        public void Register<TInterface>(TInterface instance) where TInterface : class
        {
            throw new NotImplementedException();
        }

        public void Release(object instance)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public TService Inject<TService>(TService instance) where TService : class
        {
            throw new NotImplementedException();
        }

        public void TearDown<TService>(TService instance) where TService : class
        {
            throw new NotImplementedException();
        }

        public void Register<TInterface>(Func<TInterface> factoryMethod) where TInterface : class
        {
            throw new NotImplementedException();
        }
    }

    public class MongoDBRuntime : SimpleCqrs.SimpleCqrsRuntime<SimpleCqrs.Unity.UnityServiceLocator>
    {
        protected override IEventStore  GetEventStore(IServiceLocator serviceLocator)
        {
            return new MongoEventStore("server=localhost;database=testcqrs");
        }
    
    }

    [Serializable]
    public class FooCreatedEvent : DomainEvent
    {
        public string Bar { get; set; }
    }

    public class FooRoot : AggregateRoot
    {
        public void CreateMe(Guid id)
        {
            Apply(new FooCreatedEvent{AggregateRootId = id, Bar = "foobar"});
        }

        public void OnFooCreated(FooCreatedEvent domainEvent)
        {
            Id = domainEvent.AggregateRootId;
        }
    }

    [TestClass]
    public class MongoDBEventStoreTests
    {
        [TestMethod]
        public void Test()
        {
            var runtime = new MongoDBRuntime();

            runtime.Start();
            
            var id = Guid.NewGuid();

            var root = new FooRoot();
            root.CreateMe(id);

            var repo = runtime.ServiceLocator.Resolve<IDomainRepository>();
            repo.Save(root);

            runtime.Shutdown();
        }
    }
}
