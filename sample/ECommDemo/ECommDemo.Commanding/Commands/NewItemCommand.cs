using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCqrs.Commanding;

namespace ECommDemo.Commanding.Commands
{
    public class NewItemCommand : ICommand
    {
        public string ItemId { get; protected set; }
        public string Description { get; protected set; }

        public NewItemCommand(string itemId, string description)
        {
            ItemId = itemId;
            Description = description;
        }
    }
}
