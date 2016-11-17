using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCQRSDemo.Specs.TestData.Builders;

namespace SimpleCQRSDemo.Specs.TestData.ObjectMothers
{
    static partial class CommandMothers
    {
        public static class CreateAccount
        {
            public static CommandBuilder.CreateAccountBuilder StewartDent
            { get { return new CommandBuilder.CreateAccountBuilder(); } }
        }
    }
}
