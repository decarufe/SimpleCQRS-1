using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleCQRSDemo.FakeDb
{
    [System.Obsolete("Replace with a real DB table entity", false)]
    public class FakeAccountTableRow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}