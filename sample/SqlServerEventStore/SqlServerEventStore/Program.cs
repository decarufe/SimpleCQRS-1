using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;
using SimpleCqrs.Eventing;
using SimpleCqrs.EventStore.SqlServer;
using SimpleCqrs.EventStore.SqlServer.Serializers;
using System.Data.SqlClient;

namespace SqlServerEventStoreSample
{
    class Program
    {
        // TODO: 0) Make sure to build the src\SimpleCQRS.sln before running this project. Otherwise, the referenced DLLs won't be available.
        // TODO: 1) Create a database called [test_event_store]
        // TODO: 2) Modify the server name as needed (e.g.: .\\SQL_EXPRESS instead of .)
        private const string CONNECTION_STRING = "Server=.;Database=test_event_store;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            var p = new Program();

            try
            {
                p.DoBinarySerializedEvents();

                p.DoJsonSerializedEvents();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void DoBinarySerializedEvents() {
            var json = new BinarySampleRuntime(CONNECTION_STRING);
            json.Start();
            
            var id = Guid.NewGuid();

            var serializer = new BinaryDomainEventSerializer();

            var obj = serializer.Serialize(new FooCreatedEvent());

            

            var root = new FooRoot();
            root.CreateMe(id);

            var repo = json.ServiceLocator.Resolve<IDomainRepository>();

            repo.Save(root);

            var newRoot = repo.GetById<FooRoot>(id);

            Console.WriteLine(String.Format("Id : {0}, Type : {1}", newRoot.Id, newRoot.GetType()));

            json.Shutdown();
        }

        void DoJsonSerializedEvents()
        {
            var json = new JsonSampleRuntime(CONNECTION_STRING);
            json.Start();
         
            var id = Guid.NewGuid();

            var root = new FooRoot();
            root.CreateMe(id);
            var repo = json.ServiceLocator.Resolve<IDomainRepository>();
            
            repo.Save(root);

            var newRoot = repo.GetById<FooRoot>(id);

            Console.WriteLine(String.Format("Id : {0}, Type : {1}", newRoot.Id, newRoot.GetType()));
            json.Shutdown();
        }
    }

    public class BinarySampleRuntime : SimpleCqrs.SimpleCqrsRuntime<SimpleCqrs.Unity.UnityServiceLocator>
    {
        private readonly string CONNECTION_STRING;
        public BinarySampleRuntime(string connectionString)
        {
            CONNECTION_STRING = connectionString;
        }
        protected override IEventStore GetEventStore(SimpleCqrs.IServiceLocator serviceLocator) {
            var configuration = new SqlServerConfiguration(CONNECTION_STRING);//,
                //"dbo", "binary_event_store");
            return new SqlServerEventStore(configuration, new BinaryDomainEventSerializer());
        }
    }

    public class JsonSampleRuntime : SimpleCqrs.SimpleCqrsRuntime<SimpleCqrs.Unity.UnityServiceLocator>
    {
        private readonly string CONNECTION_STRING;
        public JsonSampleRuntime(string connectionString)
        {
            CONNECTION_STRING = connectionString;
        }
        protected override IEventStore GetEventStore(SimpleCqrs.IServiceLocator serviceLocator)
        {
            var configuration = new SqlServerConfiguration(CONNECTION_STRING);//,
                //"dbo", "json_event_store");
            return new SqlServerEventStore(configuration, new JsonDomainEventSerializer());
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
}
