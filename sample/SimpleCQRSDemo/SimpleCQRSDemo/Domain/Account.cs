using System;
using SimpleCqrs.Domain;
using SimpleCQRSDemo.Events;
using System.Runtime.Serialization;

namespace SimpleCQRSDemo.Domain
{
    public class Account : AggregateRoot
    {
        public SimpleMoney Balance { get; private set; }
        public  Account()
        {
            Balance = new SimpleMoney(0);
        }

        public Account(Guid id)
        {
            Apply(new AccountCreatedEvent { AggregateRootId = id });
        }

        public void SetName(string firstName, string lastName)
        {
            Apply(new AccountNameSetEvent{ FirstName = firstName, LastName = lastName});
        }

        public void Deposit(SimpleMoney value)
        {
            if (value == null) throw new ArgumentNullException("value", "Value is required to perform a deposit");
            if (value.Amount <= 0) throw new ArgumentException("Money amounts must be greater than zero");
            Apply(new DepositEvent { AggregateRootId = Id, Value = value });
        }

        public void Withdrawal(SimpleMoney value)
        {
            if (value == null) throw new ArgumentNullException("value", "Value is required to perform a deposit");
            if (Balance.Amount < value.Amount) throw new NsfException("Insufficient Funds");
            Apply(new WithdrawalEvent { AggregateRootId = Id, Value = value });
        }

        public void OnAccountCreated(AccountCreatedEvent evt)
        {
            Id = evt.AggregateRootId;
        }

        public void OnDeposit(DepositEvent evt)
        {
            Balance = new SimpleMoney(Balance.Amount + evt.Value.Amount);
        }

        public void OnWithdrawal(WithdrawalEvent evt)
        {
            Balance = new SimpleMoney(Balance.Amount - evt.Value.Amount);
        }
    }
}