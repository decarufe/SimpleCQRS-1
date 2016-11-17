using ECommDemo.Domain.InventoryContext;
using SimpleCqrs.Eventing;

namespace ECommDemo.ViewModel.Inventory
{
    public class InventoryItemDenormalizer : IHandleDomainEvents<InventoryItemCreatedEvent>
    {
        private readonly IInventoryWriter _inventoryWriter;

        public InventoryItemDenormalizer(IInventoryWriter inventoryWriter)
        {
            _inventoryWriter = inventoryWriter;
        }

        public void Handle(InventoryItemCreatedEvent e)
        {
            _inventoryWriter.Save(new Item()
                                      {
                                          Id = e.AggregateRootId,
                                          ItemId = e.ItemId,
                                          Description = e.Description,
                                          InStock = 0
                                      });
        }
    }
}
