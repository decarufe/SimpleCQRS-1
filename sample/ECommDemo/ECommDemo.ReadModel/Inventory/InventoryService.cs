using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ECommDemo.ViewModel.Inventory
{
    public class Item
    {
        public Guid Id { get; set; }
        
        public string ItemId { get; set; }
        public string Description { get; set; }
        public decimal InStock { get; set; }
    }

    public interface IInventoryReader : IReader
    {
        IQueryable<Item> Items { get; }
    }

    public interface IInventoryWriter : IWriter
    {
        void Save(Item item);
    }
    
    public class InventoryService : IInventoryReader, IInventoryWriter
    {
        private readonly MongoDatabase _database;
        private readonly MongoCollection<Item> _collection;

        public InventoryService(MongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<Item>("inventory");
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
