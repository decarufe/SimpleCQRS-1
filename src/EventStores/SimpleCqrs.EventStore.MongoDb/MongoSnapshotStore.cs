using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SimpleCqrs.Domain;
using SimpleCqrs.Eventing;

namespace SimpleCqrs.EventStore.MongoDb
{
    public class MongoSnapshotStore : ISnapshotStore
    {
        private readonly MongoDatabase _database;

        static MongoSnapshotStore()
        {
            BsonClassMap.RegisterClassMap<Snapshot>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(s => s.AggregateRootId);
            });
        }

        public MongoSnapshotStore(string connectionString)
        {
            var connectionStringBuilder = new MongoConnectionStringBuilder(connectionString);
            var mongo = MongoServer.Create(connectionString);
            mongo.Connect();

            _database = mongo.GetDatabase(connectionStringBuilder.DatabaseName, SafeMode.True);
            
            // setup 
            Collection.EnsureIndex(
                new IndexKeysBuilder().Ascending("AggregateRootId")
            );
        }

        private MongoCollection<Snapshot> Collection
        {
            get { return _database.GetCollection<Snapshot>("snapshots"); }
        }

        public Snapshot GetSnapshot(Guid aggregateRootId)
        {
            return Collection.FindOneById(aggregateRootId);
        }

        public void SaveSnapshot<TSnapshot>(TSnapshot snapshot) where TSnapshot : Snapshot
        {
            Collection.Save(snapshot);
        }
    }
}