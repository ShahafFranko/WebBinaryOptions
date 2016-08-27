using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class CreateAccountRequest : IRequest
    {
        public CreateAccountRequest(string username, string password, int balance)
        {
            Username = username;
            Password = password;
            Balance = balance;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
    }
}
