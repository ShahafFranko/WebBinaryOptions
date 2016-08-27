using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryOptions.WebServer.Models
{
    public class CreateAccountModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
    }
}