using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class AccountDepositRequest
    {
        public AccountDepositRequest(string username, double amount)
        {
            Username = username;
            Amount = amount;
        }

        public string Username { get; private set; }
        public double Amount { get; private set; }
    }
}
