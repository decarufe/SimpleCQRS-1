using SimpleCqrs.Eventing;
using SimpleCQRSDemo.Domain;

namespace SimpleCQRSDemo.Events
{
    public class WithdrawalEvent : DomainEvent
    {
        public SimpleMoney Value { get; set; }
    }
}
