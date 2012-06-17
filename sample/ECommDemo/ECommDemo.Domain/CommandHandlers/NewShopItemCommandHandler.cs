using ECommDemo.Commanding.Commands;
using ECommDemo.Domain.ShopContext;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;

namespace ECommDemo.Domain.CommandHandlers
{
    public class NewShopItemCommandHandler : CommandHandler<NewShopItemCommand>
    {
        private readonly IDomainRepository _repository;

        public NewShopItemCommandHandler(IDomainRepository repository)
        {
            _repository = repository;
        }

        public override void Handle(NewShopItemCommand command)
        {
            var item = new ShopItem(command.ItemId, command.Description);

            _repository.Save(item);
        }
    }
}