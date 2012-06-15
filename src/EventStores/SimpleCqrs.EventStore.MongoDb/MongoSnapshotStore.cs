using System;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SimpleCqrs.Domain;
using SimpleCqrs.Eventing;

namespace SimpleCqrs.EventStore.MongoDb
{
    public class MongoSnapshotStore : ISnapshotStore
    {
        private readonly MongoDatabase _database;

        public MongoSnapshotStore(string connectionString)
        {
            var mongo = MongoServer.Create(connectionString) ;
            mongo.Connect();

            _database = mongo.GetDatabase("snapshotstore");
        }

        public Snapshot GetSnapshot(Guid aggregateRootId)
        {
            var snapshotsCollection = _database.GetCollection<Snapshot>("snapshots").AsQueryable();
            return (from snapshot in snapshotsCollection
                    where snapshot.AggregateRootId == aggregateRootId
                    select snapshot).SingleOrDefault();
        }

        public void SaveSnapshot<TSnapshot>(TSnapshot snapshot) where TSnapshot : Snapshot
        {
            var io = new MongoInsertOptions();
            io.SafeMode = SafeMode.True;
            var snapshotsCollection = _database.GetCollection<TSnapshot>("snapshots");
            snapshotsCollection.Save(snapshot, io);
        }
    }
}