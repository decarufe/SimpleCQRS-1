using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECommDemo.Domain.InventoryContext;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleCqrs.Eventing;

namespace ECommDemo.Domain.Denormalizers
{
    public class InventoryItemDenormalizer : IHandleDomainEvents<InventoryItemCreatedEvent>
    {
        public void Handle(InventoryItemCreatedEvent e)
        {
            var server = MongoServer.Create();
            var db = server.GetDatabase("ecomm");
            var collection = db.GetCollection("inventory");

            var    item = new BsonDocument();
            item.Add("itemid", e.ItemId);
            item.Add("description", e.Description);
            item.Add("_id", e.AggregateRootId);

            collection.Save(item);
        }
    }
}
