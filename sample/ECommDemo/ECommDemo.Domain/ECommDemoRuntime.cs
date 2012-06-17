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

        public ECommDemoRuntime(IWindsorContainer container, IEventStore eventStore, ISnapshotStore snapshotStore)
        {
            _container = container;
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
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
            return new[]
                       {
                             typeof(NewItemCommand).Assembly,
                             typeof(NewItemCommandHandler).Assembly 
                       };
        }
    }
}
