using SimpleCqrs.Eventing;
using SimpleCQRSDemo.Domain;

namespace SimpleCQRSDemo.Events
{
    public class DepositEvent : DomainEvent
    {
        public SimpleMoney Value { get; set; }
    }
}
