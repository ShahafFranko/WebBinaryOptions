using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class AccountReply : IReply
    {
        public AccountReply(string username, string password, int balance)
        {
            Username = username;
            Password = password;
            Balance = balance;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Balance { get; private set; }
    }
}
