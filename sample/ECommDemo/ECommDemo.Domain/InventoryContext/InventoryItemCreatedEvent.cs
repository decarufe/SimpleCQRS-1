using SimpleCqrs.Eventing;

namespace ECommDemo.Domain.InventoryContext
{
    public class InventoryItemCreatedEvent : DomainEvent
    {
        public string ItemId { get; protected set; }
        public string Description { get; protected set; }

        public InventoryItemCreatedEvent(string itemId, string description)
        {
            ItemId = itemId;
            Description = description;
        }
    }
}