using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Windsor;
using ECommDemo.Commanding.Commands;
using ECommDemo.Domain.CommandHandlers;
using SimpleCqrs;
using SimpleCqrs.EventStore.MongoDb;
using SimpleCqrs.Eventing;
using SimpleCqrs.Windsor;

namespace ECommDemo.Domain
{
    public class ECommDemoRuntime : SimpleCqrs.SimpleCqrsRuntime<WindsorServiceLocator>
    {
        private IWindsorContainer _container;
        private IEventStore _eventStore;
        private ISnapshotStore _snapshotStore;
        private IEnumerable<Assembly> _assembliesToScan;

        public ECommDemoRuntime(IWindsorContainer container, IEventStore eventStore, ISnapshotStore snapshotStore, IEnumerable<Assembly> assemblies)
        {
            _container = container;
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
            _assembliesToScan = assemblies;
        }

        protected override WindsorServiceLocator GetServiceLocator()
        {
            return new WindsorServiceLocator(_container);
        }

        protected override IEventStore GetEventStore(IServiceLocator serviceLocator)
        {
            return _eventStore;
        }

        protected override ISnapshotStore GetSnapshotStore(IServiceLocator serviceLocator)
        {
            return _snapshotStore;
        }

        protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> GetAssembliesToScan(IServiceLocator serviceLocator)
        {
            return _assembliesToScan;
        }
    }
}
