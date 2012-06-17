using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECommDemo.Commanding.Commands;
using ECommDemo.Domain.InventoryContext;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;

namespace ECommDemo.Domain.CommandHandlers
{
    public class NewItemCommandHandler : CommandHandler<NewItemCommand>
    {
        private readonly IDomainRepository _repository;

        public NewItemCommandHandler(IDomainRepository repository)
        {
            _repository = repository;
        }

        public override void Handle(NewItemCommand command)
        {
            var item = new InventoryItem(command.ItemId, command.Description);

            _repository.Save(item);
        }
    }
}
