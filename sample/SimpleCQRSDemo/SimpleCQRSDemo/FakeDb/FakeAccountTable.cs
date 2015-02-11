using System.Collections.Generic;
using System.Data.Entity;
using System;

namespace SimpleCQRSDemo.FakeDb
{
    [System.Obsolete("Replace with a real DB", false)]
    public class FakeAccountTable : List<FakeAccountTableRow>
    { }

    public class SqlBankContext : DbContext
    {
        public SqlBankContext(string connectionString) 
            : base(connectionString)
        {
        }
        public DbSet<Account> Accounts { get; set; }
    }
}