using System.Linq;
using SimpleCqrs.Eventing;
using SimpleCQRSDemo.Events;
using SimpleCQRSDemo.FakeDb;

namespace SimpleCQRSDemo.Denormalizers
{
    public class AccountReportDenormalizer : IHandleDomainEvents<AccountCreatedEvent>,
                                             IHandleDomainEvents<AccountNameSetEvent>
    {
        private readonly SqlBankContext dbContext;

        public AccountReportDenormalizer(SqlBankContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Handle(AccountCreatedEvent domainEvent)
        {
            dbContext.Accounts.Add(new Account { Id = domainEvent.AggregateRootId });
            dbContext.SaveChanges();
        }

        public void Handle(AccountNameSetEvent domainEvent)
        {
            var account = dbContext.Accounts.SingleOrDefault(x => x.Id == domainEvent.AggregateRootId);
            account.Name = domainEvent.FirstName + " " + domainEvent.LastName;
            dbContext.SaveChanges();
        }
    }
}