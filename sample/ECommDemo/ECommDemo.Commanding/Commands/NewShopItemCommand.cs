using SimpleCqrs.Commanding;

namespace ECommDemo.Commanding.Commands
{
    public class NewShopItemCommand : ICommand
    {
        public string ItemId { get; protected set; }
        public string Description { get; protected set; }

        public NewShopItemCommand(string itemId, string description)
        {
            ItemId = itemId;
            Description = description;
        }
    }
}