using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryOption.OptionServer.Contract.DTO;

namespace BinaryOptions.WebServer.Models
{
    public class AccountModel
    {
        public string Username { get; set; }
        public double Balance { get; set; }

        public static AccountModel FromDto(AccountReply accountDto)
        {
            return new AccountModel()
            {
                Username =  accountDto.Username,
                Balance = accountDto.Balance
            };
        }
    }
}