using SimpleCqrs.Commanding;
using System;

namespace SimpleCQRSDemo.Commands
{
    public class CreateAccountCommand : ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AccountNumber { get; set; }
    }
}