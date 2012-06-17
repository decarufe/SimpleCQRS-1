using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECommDemo.Commanding.Commands;
using ECommDemo.Domain.InventoryContext;
using ECommDemo.Domain.ShopContext;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;
using SimpleCqrs.Eventing;

namespace ECommDemo.Domain.EventHandlers
{
    public class InventoryItemCreatedEventHanlder : IHandleDomainEvents<InventoryItemCreatedEvent>
    {
        private ICommandBus _bus;
        public InventoryItemCreatedEventHanlder(ICommandBus bus)
        {
            _bus = bus;
        }

        public void Handle(InventoryItemCreatedEvent e)
        {
            _bus.Send(new NewShopItemCommand(e.ItemId, "shop " + e.Description));
        }
    }
}
