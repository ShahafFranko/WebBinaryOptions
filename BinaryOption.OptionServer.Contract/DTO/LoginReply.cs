using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOption.OptionServer.Contract.DTO
{
    public class LoginReply : IReply
    {
        public LoginReply(bool successfulLogin)
        {
            SuccessfulLogin = successfulLogin;
        }

        public bool SuccessfulLogin { get; private set; }
    }
}
