using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCqrs.Domain;
using SimpleCqrs.Eventing;

namespace ECommDemo.Domain.InventoryContext
{
    public class ItemCreatedEvent : DomainEvent
    {
        public string ItemId { get; protected set; }
        public string Description { get; protected set; }

        public ItemCreatedEvent(string itemId, string description)
        {
            ItemId = itemId;
            Description = description;
        }
    }

    public class InventoryItem : AggregateRoot
    {
        public string ItemId { get; protected set; }
        public string Description { get; protected set; }

        protected InventoryItem()
        {
        }

        public InventoryItem(string id, string description)
        {
            Apply(new ItemCreatedEvent(id, description)
                      {
                          AggregateRootId = Guid.NewGuid()
                      });
        }


        protected void OnItemCreated(ItemCreatedEvent e)
        {
            this.Id = e.AggregateRootId;
            this.ItemId = e.ItemId;
            this.Description = e.Description;
        }

    }
}
