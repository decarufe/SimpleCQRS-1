using System.Collections.Generic;
using System.Linq;
using SimpleCQRSDemo.FakeDb;

namespace SimpleCQRSDemo.ReadModel
{
    public class AccountReportReadService
    {
        //private FakeAccountTable fakeAccountDb;
        //public AccountReportReadService(FakeAccountTable fakeAccountDb)
        //{
        //    this.fakeAccountDb = fakeAccountDb;
        //}

        //public IEnumerable<AccountReadModel> GetAccounts()
        //{
        //    return from a in fakeAccountDb
        //           select new AccountReadModel { Id = a.Id, Name = a.Name };
        //}

        #region With DBContext (Not Working?)
        private SqlBankContext dbContext;

        //public AccountReportReadService()
        //    : this(new SqlBankContext("Server=.;Database=test_event_store;Trusted_Connection=True;"))
        //{
        //}
        public AccountReportReadService(SqlBankContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<AccountReadModel> GetAccounts()
        {
            return from a in dbContext.Accounts
                   select new AccountReadModel { Id = a.Id, Name = a.Name };
        }
        #endregion
    }
}