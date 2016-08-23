using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.Requests
{
    public class LoginReply
    {
        public LoginReply(bool successfulLogin)
        {
            SuccessfulLogin = successfulLogin;
        }

        public bool SuccessfulLogin { get; private set; }
    }
}
