using System;
using SimpleCqrs.Commanding;
using SimpleCqrs.Domain;
using SimpleCQRSDemo.Commands;
using SimpleCQRSDemo.Domain;

namespace SimpleCQRSDemo.CommandHandlers
{
    public class WithdrawalCommandHandler : CommandHandler<WithdrawalCommand>
    {
        private readonly IDomainRepository repository;

        public WithdrawalCommandHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public override void Handle(WithdrawalCommand command)
        {
            SimpleMoney value = new SimpleMoney(command.Amount);
            var account = repository.GetExistingById<Account>(command.Id);
            account.Withdrawal(value);

            repository.Save(account);
        }
    }
}
