using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCQRSDemo.FakeDb
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Column(TypeName="Money")]
        public decimal Balance { get; set; }
    }
}