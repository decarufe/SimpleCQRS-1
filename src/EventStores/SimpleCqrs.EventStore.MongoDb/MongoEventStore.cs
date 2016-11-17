using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SimpleCqrs.Eventing;

namespace SimpleCqrs.EventStore.MongoDb
{
    public class MongoEventStore : IEventStore
    {
        private readonly string _databaseName;
        private readonly MongoServer _server;
        private MongoDatabase _database;

        static MongoEventStore()
        {
            // ignore _id mapping
            BsonClassMap.RegisterClassMap<DomainEvent>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.SetIgnoreExtraElementsIsInherited(true);
            });

        }

        public MongoEventStore(string connectionString)
        {
            var connectionStringBuilder = new MongoConnectionStringBuilder(connectionString);
            _databaseName = connectionStringBuilder.DatabaseName;

            _server = MongoServer.Create(connectionString);
            _server.Connect();
            _database = _server.GetDatabase(_databaseName);

            Collection.EnsureIndex(
                new IndexKeysBuilder()
                    .Ascending("AggregateRootId")
                    .Ascending("Sequence")
            );
        }

        private MongoCollection<DomainEvent> Collection
        {
            get { return _database.GetCollection<DomainEvent>("snapshots"); }
        }

        public IEnumerable<DomainEvent> GetEvents(Guid aggregateRootId, int startSequence)
        {
            var eventsCollection = _database.GetCollection<DomainEvent>("events").AsQueryable();

            return (from domainEvent in eventsCollection
                    where domainEvent.AggregateRootId == aggregateRootId
                    where domainEvent.Sequence > startSequence
                    select domainEvent).ToList();
        }

        public void Insert(IEnumerable<DomainEvent> domainEvents)
        {
            var eventsCollection = _database.GetCollection<DomainEvent>("events");
            eventsCollection.InsertBatch(domainEvents);
        }

        public IEnumerable<DomainEvent> GetEventsByEventTypes(IEnumerable<Type> domainEventTypes)
        {
            IMongoQuery query = Query.In("_t", new BsonArray(domainEventTypes.Select(t => t.Name)));
            return _database.GetCollection<DomainEvent>("events").Find(query).ToList();
        }

        public IEnumerable<DomainEvent> GetEventsByEventTypes(IEnumerable<Type> domainEventTypes, Guid aggregateRootId)
        {
            IMongoQuery query = Query.And(
                Query.In("_t", new BsonArray(domainEventTypes.Select(t => t.Name))),
                Query.In("AggregateRootId", aggregateRootId)
            );

            return _database.GetCollection<DomainEvent>("events").Find(query).ToList();
        }

        public IEnumerable<DomainEvent> GetEventsByEventTypes(IEnumerable<Type> domainEventTypes, DateTime startDate, DateTime endDate)
        {
            IMongoQuery query = Query.And(
                Query.In("_t", new BsonArray(domainEventTypes.Select(t => t.Name))),
                Query.GTE("EventDate", startDate).LTE(endDate)
            );
            return _database.GetCollection<DomainEvent>("events").Find(query).ToList();
        }
    }
}