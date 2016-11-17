using SimpleCqrs.Eventing;

namespace SimpleCQRSDemo.Events
{
    public class AccountNumberSetEvent : DomainEvent
    {
        public int AccountNumber { get; set; }
    }
}
