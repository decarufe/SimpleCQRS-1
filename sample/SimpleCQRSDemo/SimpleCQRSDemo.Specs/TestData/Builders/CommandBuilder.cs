﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTestDataBuilder;
using SimpleCQRSDemo.Commands;

namespace SimpleCQRSDemo.Specs.TestData.Builders
{
    static class CommandBuilder
    {
        public class CreateAccountBuilder : TestDataBuilder<CreateAccountCommand, CreateAccountBuilder>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CreateAccount"/> class.
            /// </summary>
            public CreateAccountBuilder()
            {
                Set(x => x.AccountNumber, 99);
                Set(x => x.FirstName, "Stewart");
                Set(x => x.LastName, "Dent");
            }
            public CreateAccountBuilder WithFirstName(string firstName)
            {
                Set(x => x.FirstName, firstName);
                return this;
            }
            public CreateAccountBuilder WithLastName(string lastName)
            {
                Set(x => x.LastName, lastName);
                return this;
            }
            public CreateAccountBuilder WithAccountNumber(int accountNumber)
            {
                Set(x => x.AccountNumber, accountNumber);
                return this;
            }
            protected override CreateAccountCommand BuildObject()
            {
                return new CreateAccountCommand { AccountNumber = Get(x => x.AccountNumber), FirstName = Get(x => x.FirstName), LastName = Get(x => x.LastName) };
            }
        }
        
        public class DepositBuilder : TestDataBuilder<DepositCommand, DepositBuilder>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DepositBuilder"/> class.
            /// </summary>
            public DepositBuilder()
            {
                Set(x => x.Amount, 50m);
            }
            public DepositBuilder WithAmount(decimal amount)
            {
                Set(x => x.Amount, amount);
                return this;
            }
            public DepositBuilder WithId(Guid id)
            {
                Set(x => x.Id, id);
                return this;
            }
            protected override DepositCommand BuildObject()
            {
                return new DepositCommand { Id = Get(x => x.Id), Amount = Get(x => x.Amount) };
            }
        }
    }
}
