using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleCqrs.Commanding;
using SimpleCQRSDemo.Commands;
using SimpleCQRSDemo.FakeDb;
using SimpleCQRSDemo.ReadModel;
using SimpleCQRSDemo.Denormalizers;
using SimpleCQRSDemo.Events;

namespace SimpleCQRSDemo
{
    class Program : IDisposable
    {
        // TODO: 0) Make sure to build the src\SimpleCQRS.sln before running this project. Otherwise, the referenced DLLs won't be available.
        // TODO: 1) Create a database called [test_event_store]
        // TODO: 2) Modify the server name as needed (e.g.: .\\SQL_EXPRESS instead of .)
        private const string CONNECTION_STRING = "Server=.;Database=test_event_store;Trusted_Connection=True;";
        static void Main(string[] args)
        {

            try
            {
                var app = new Program();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                Console.WriteLine(inner.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        SampleRunTime runtime;
        ICommandBus commandBus;

        private void Run()
        {
            runtime = new SampleRunTime(CONNECTION_STRING);
            runtime.Start();

            // Configure dependencies
            System.Data.Entity.Database.SetInitializer<SqlBankContext>(new System.Data.Entity.DropCreateDatabaseIfModelChanges<SqlBankContext>());
            var db = new SqlBankContext(CONNECTION_STRING);
            runtime.ServiceLocator.Register(db);

            // Get the Command Bus
            commandBus = runtime.ServiceLocator.Resolve<ICommandBus>();
            // Create and send a couple of command
            var accountReportReadModel = runtime.ServiceLocator.Resolve<AccountReportReadService>();
            var accounts = accountReportReadModel.GetAccounts();
            if (accounts.Count() == 0)
            {
                Console.WriteLine("Adding initial data...\n\n\n");
                var cmdMarcus = new CreateAccountCommand { FirstName = "Marcus", LastName = "Hammarberg" };
                var cmdDarren = new CreateAccountCommand { FirstName = "Darren", LastName = "Cauthon" };
                var cmdTyrone = new CreateAccountCommand { FirstName = "Tyrone", LastName = "Groves" };
                commandBus.Send(cmdMarcus);
                commandBus.Send(cmdDarren);
                commandBus.Send(cmdTyrone);
            }

            ProcessMenu();

            runtime.Shutdown();

            Console.ReadLine();
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Menu:");
            Console.WriteLine("1) List accounts");
            Console.WriteLine("2) Show account transactions");
            Console.WriteLine("3) Add account");
            Console.WriteLine("4) Deposit");
            Console.WriteLine("5) Withdrawal");
            Console.WriteLine("0) Exit");
            Console.WriteLine();
        }

        private void ProcessMenu()
        {
            int menuChoice;
            do
            {
                ShowMenu();
                Console.Write("\tSelect an item: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out menuChoice))
                {
                    switch (menuChoice)
                    {
                        case 0:
                            break;
                        case 1: // List accounts
                            ListAccounts();
                            break;
                        case 2: // Show account transactions
                            Console.WriteLine("... tba ...");
                            break;
                        case 3: // Add account
                            AddAccount();
                            break;
                        case 4: // Deposit
                            {
                                Console.Write("Index of account: ");
                                int index = int.Parse(Console.ReadLine());
                                var accountReportReadModel = runtime.ServiceLocator.Resolve<AccountReportReadService>();
                                IEnumerable<AccountReadModel> accounts = accountReportReadModel.GetAccounts();
                                Console.Write("Amount to deposit: ");
                                decimal amount = decimal.Parse(Console.ReadLine());
                                var cmd = new DepositCommand { Amount = amount, Id = accounts.ToArray()[index].Id };
                                commandBus.Send(cmd);
                            }
                            break;
                        case 5: // Withdrawal
                            {
                                Console.Write("Index of account: ");
                                int index = int.Parse(Console.ReadLine());
                                var accountReportReadModel = runtime.ServiceLocator.Resolve<AccountReportReadService>();
                                IEnumerable<AccountReadModel> accounts = accountReportReadModel.GetAccounts();
                                Console.Write("Amount to withdraw: ");
                                decimal amount = decimal.Parse(Console.ReadLine());
                                var cmd = new WithdrawalCommand { Amount = amount, Id = accounts.ToArray()[index].Id };
                                commandBus.Send(cmd);
                            }
                            break;
                        default:
                            Console.WriteLine("~ Invalid entry ~");
                            break;
                    }
                }
            } while (menuChoice != 0);
        }

        private void ListAccounts()
        {
            // Get the denormalized version of the data back from the read model
            var accountReportReadModel = runtime.ServiceLocator.Resolve<AccountReportReadService>();
            IEnumerable<AccountReadModel> accounts = accountReportReadModel.GetAccounts();
            int count = accounts.Count();
            if (count > 0)
            {
                Console.WriteLine("Accounts in database: " + count);
                Console.WriteLine("#########################");
                int index = 0;
                foreach (var account in accounts)
                {
                    Console.WriteLine("{2}) Id: {0} Name: {1}", account.Id, account.Name, index);
                    index++;
                }
            }
            else
            {
                Console.WriteLine("## No accounts in database read store ##");
            }
        }

        private void AddAccount()
        {
            string first, last;
            Console.Write("First Name: ");
            first = Console.ReadLine();
            Console.Write("Last Name: ");
            last = Console.ReadLine();

            var cmd = new CreateAccountCommand { FirstName = first, LastName = last };
            commandBus.Send(cmd);
        }

        public void Dispose()
        {
            if (runtime != null)
            {
                runtime.Dispose();
                runtime = null;
            }
        }
    }
}
