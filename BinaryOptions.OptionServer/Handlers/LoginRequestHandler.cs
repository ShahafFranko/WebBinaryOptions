using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;

namespace BinaryOptions.OptionServer.Handlers
{
    public class LoginRequestHandler : ReceiveActor
    {
        public LoginRequestHandler()
        {
            Receive<LoginRequest>(r => Handle(r));
        }

        public LoginReply Handle(LoginRequest loginRequest)
        {
            // go to db and select account.
            // check if credentials are valid.
            // return:
            //Context.Sender.Tell(new LoginReply(true));
            //Context.Sender.Tell(new LoginReply(false));
            return new LoginReply(true);
        }
    }
}
