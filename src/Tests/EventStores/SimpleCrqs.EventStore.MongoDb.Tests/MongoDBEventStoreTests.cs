﻿using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SimpleCqrs;
using SimpleCqrs.Domain;
using SimpleCqrs.EventStore.MongoDb;
using SimpleCqrs.Eventing;
using SimpleCqrs.Windsor;

namespace SimpleCrqs.EventStore.MongoDb.Tests
{
    public class MongoDBRuntime : SimpleCqrs.SimpleCqrsRuntime<WindsorServiceLocator>
    {
        protected override IEventStore GetEventStore(IServiceLocator serviceLocator)
        {
            return new MongoEventStore("server=localhost;database=cqrs-events");
        }

        protected override ISnapshotStore GetSnapshotStore(IServiceLocator serviceLocator)
        {
            return new MongoSnapshotStore("server=localhost;database=cqrs-snapshot");
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

    [Serializable]
    public class FooRootSnapshot : Snapshot
    {
        public string Name { get; set; }
    }

    public class FooRoot : AggregateRoot, ISnapshotOriginator
    {
        public string Name { get; private set; }

        public void CreateMe(Guid id)
        {
            Apply(new FooCreatedEvent {AggregateRootId = id, Bar = "foobar"});
        }

        public void ChangeName(string name)
        {
            Apply(new FooNameChangedEvent {AggregateRootId = this.Id, Name = name});
        }

        public void OnFooCreated(FooCreatedEvent domainEvent)
        {
            Id = domainEvent.AggregateRootId;
        }

        public void OnFooNameChanged(FooNameChangedEvent domainEvent)
        {
            this.Name = domainEvent.Name;
        }

        public Snapshot GetSnapshot()
        {
            return new FooRootSnapshot()
            {
                AggregateRootId = this.Id,
                LastEventSequence = this.LastEventSequence,
                Name = this.Name
            };
        }

        public void LoadSnapshot(Snapshot snapshot)
        {
            var s = (FooRootSnapshot) snapshot;
            this.Name = s.Name;
        }

        public bool ShouldTakeSnapshot(Snapshot previousSnapshot)
        {
            if (previousSnapshot == null)
                return true;
            return this.LastEventSequence > previousSnapshot.LastEventSequence;
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

            newRoot.ChangeName("no name");
            repo.Save(newRoot);

            var changed = repo.GetById<FooRoot>(id);
            Assert.AreEqual("no name", changed.Name);

            runtime.Shutdown();
        }
    }
}