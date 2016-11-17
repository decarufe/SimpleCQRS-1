using SimpleCqrs.Commanding;
using System;

namespace SimpleCQRSDemo.Commands
{
    public class WithdrawalCommand : ICommand
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
    }
}
