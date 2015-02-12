using System.Collections.Generic;
using System.Linq;
using SimpleCQRSDemo.FakeDb;

namespace SimpleCQRSDemo.ReadModel
{
    public class AccountReportReadService
    {
        private SqlBankContext dbContext;

        public AccountReportReadService(SqlBankContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<AccountReadModel> GetAccounts()
        {
            return from a in dbContext.Accounts
                   select new AccountReadModel { Id = a.Id, Name = a.Name };
        }
    }
}