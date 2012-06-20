using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ECommDemo.ViewModel.Shop
{
    public class Item
    {
        public Guid Id { get; set; }
        [BsonElement("itemid")]
        public string ItemId { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
    }

    public interface ICatalogReader : IReader
    {
        IQueryable<Item> Items { get; }
    }

    public interface ICatalogWriter
    {
        void Save(Item item);
    }
    
    public class CatalogService : ICatalogReader, ICatalogWriter
    {
        private readonly MongoDatabase _database;
        private readonly MongoCollection<Item> _collection;

        public CatalogService(MongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<Item>("catalog");
        }


        public IQueryable<Item> Items
        {
            get { return _collection.AsQueryable(); }
        }

        public void Save(Item item)
        {
            _collection.Save(item);
        }
    }
}
