using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCqrs;
using SimpleCqrs.Domain;
using SimpleCqrs.EventStore.MongoDb;
using SimpleCqrs.Eventing;

namespace SimpleCrqs.EventStore.MongoDb.Tests
{
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

    [Serializable]
    public class FooNameChangedEvent : DomainEvent
    {
        public string Name { get; set; }
    }
    
    public class FooRoot : AggregateRoot
    {
        public string Name { get; private set; }

        public void CreateMe(Guid id)
        {
            Apply(new FooCreatedEvent{AggregateRootId = id, Bar = "foobar"});
        }

        public void ChangeName(string name)
        {
            Apply(new FooNameChangedEvent { AggregateRootId = this.Id, Name = name });
        }
        
        public void OnFooCreated(FooCreatedEvent domainEvent)
        {
            Id = domainEvent.AggregateRootId;
        }

        public void OnFooNameChanged(FooNameChangedEvent domainEvent)
        {
            this.Name = domainEvent.Name;
        }
    }

    [TestClass]
    public class MongoDBEventStoreTests
    {
        [TestMethod]
        public void can_save_and_load_events_from_eventstore()
        {
            var runtime = new MongoDBRuntime();

            runtime.Start();
            
            var id = Guid.NewGuid();

            var root = new FooRoot();
            root.CreateMe(id);
            root.ChangeName("Andrea Balducci");

            var repo = runtime.ServiceLocator.Resolve<IDomainRepository>();
            repo.Save(root);

            var newRoot = repo.GetById<FooRoot>(id);
            Assert.AreEqual(root.Id, newRoot.Id);
            Assert.AreEqual("Andrea Balducci", newRoot.Name);

            runtime.Shutdown();
        }
    }
}
