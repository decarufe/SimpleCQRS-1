using SimpleCqrs;
using SimpleCqrs.EventStore.SqlServer;
using SimpleCqrs.EventStore.SqlServer.Serializers;
using SimpleCqrs.Unity;

namespace SimpleCQRSDemo
{
    public class SampleRunTime : SimpleCqrsRuntime<UnityServiceLocator>
    {
        private readonly string CONNECTION_STRING;
        public SampleRunTime(string connectionString)
        {
            CONNECTION_STRING = connectionString;
        }
        protected override SimpleCqrs.Eventing.IEventStore GetEventStore(IServiceLocator serviceLocator)
        {
            var configuration = new SqlServerConfiguration(CONNECTION_STRING);//,
                //"dbo", "json_event_store");
            return new SqlServerEventStore(configuration, new JsonDomainEventSerializer());
        }
    }
}