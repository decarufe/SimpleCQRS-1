using ECommDemo.Domain.InventoryContext;
using ECommDemo.Domain.ShopContext;
using ECommDemo.ViewModel.Inventory;
using SimpleCqrs.Eventing;

namespace ECommDemo.ViewModel.Shop
{
    public class ShopItemDenormalizer : IHandleDomainEvents<ShopItemCreatedEvent>
    {
        private readonly ICatalogWriter _writer;

        public ShopItemDenormalizer(ICatalogWriter writer)
        {
            _writer = writer;
        }

        public void Handle(ShopItemCreatedEvent e)
        {
            _writer.Save(new Shop.Item()
                        {
                            Id = e.AggregateRootId,
                            ItemId = e.ItemId,
                            Description = e.Description,
                        });
        }
    }
}
