using System.Collections.Generic;
using System.Data.Entity;
using System;

namespace SimpleCQRSDemo.FakeDb
{
    public class SqlBankContext : DbContext
    {
        public SqlBankContext(string connectionString)
            : base(connectionString)
        {
        }
        public DbSet<Account> Accounts { get; set; }
    }
}