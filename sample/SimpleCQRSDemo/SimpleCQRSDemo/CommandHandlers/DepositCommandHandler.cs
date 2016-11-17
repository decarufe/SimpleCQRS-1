using System;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;
using SimpleCQRSDemo.Commands;
using SimpleCQRSDemo.Domain;

namespace SimpleCQRSDemo.CommandHandlers
{
    public class DepositCommandHandler : CommandHandler<DepositCommand>
    {
        private readonly IDomainRepository repository;

        public DepositCommandHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public override void Handle(DepositCommand command)
        {
            SimpleMoney value = new SimpleMoney(command.Amount);
            var account = repository.GetExistingById<Account>(command.Id);
            account.Deposit(value);

            repository.Save(account);
        }
    }
}
