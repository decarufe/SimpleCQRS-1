using NTestDataBuilder;
using SimpleCqrs.Eventing;
using SimpleCQRSDemo.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRSDemo.Specs.TestData.Builders
{
    static class EventBuilder
    {
        public abstract class DomainBuilder<TObject, TBuilder> : TestDataBuilder<TObject, TBuilder> where TObject : DomainEvent where TBuilder : DomainBuilder<TObject, TBuilder>, ITestDataBuilder<TObject>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DomainBuilder"/> class.
            /// </summary>
            public DomainBuilder()
            {
                Set(x => x.AggregateRootId, Guid.NewGuid());
                Set(x => x.EventDate, DateTime.Now);
                Set(x => x.Sequence, 1);
            }
            public TBuilder WithId(Guid id)
            {
                Set(x => x.AggregateRootId, Guid.NewGuid());
                return this.AsProxy();
            }
            public TBuilder WithDate(DateTime date)
            {
                Set(x => x.EventDate, DateTime.Now);
                return this.AsProxy();
            }
            public TBuilder WithSequence(int sequence)
            {
                Set(x => x.Sequence, 1);
                return this.AsProxy();
            }
            protected override TObject BuildObject()
            {
                var obj = default(TObject);
                obj.AggregateRootId = Get(x => x.AggregateRootId);
                obj.EventDate = Get(x => x.EventDate);
                obj.Sequence = Get(x => x.Sequence);
                return obj;
            }
        }

        public class AccountCreatedBuilder : DomainBuilder<AccountCreatedEvent, AccountCreatedBuilder>
        {
        }

        public class AccountNameSetBuilder : DomainBuilder<AccountNameSetEvent, AccountNameSetBuilder>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AccountNameSetBuilder"/> class.
            /// </summary>
            public AccountNameSetBuilder()
            {
                Set(x => x.FirstName, "Stewart");
                Set(x => x.LastName, "Dent");
            }
            public AccountNameSetBuilder WithFirstName(string firstName)
            {
                Set(x => x.FirstName, firstName);
                return this;
            }
            public AccountNameSetBuilder WithLastName(string lastName)
            {
                Set(x => x.LastName, lastName);
                return this;
            }
            protected override AccountNameSetEvent BuildObject()
            {
                var obj = base.BuildObject();
                obj.FirstName = Get(x=>x.FirstName);
                obj.LastName = Get(x=>x.LastName);
                return obj;
            }
        }
    }
}
