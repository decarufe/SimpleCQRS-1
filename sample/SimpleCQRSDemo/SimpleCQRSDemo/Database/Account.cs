using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleCQRSDemo.FakeDb
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}