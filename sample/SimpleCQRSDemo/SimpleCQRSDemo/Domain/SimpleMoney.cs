using System;
using SimpleCqrs.Domain;
using SimpleCQRSDemo.Events;

namespace SimpleCQRSDemo.Domain
{
    public class SimpleMoney
    {
        public decimal Amount { get; private set; }

        public SimpleMoney(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Money amounts must be greater than zero");
            Amount = amount;
        }
    }
}
