using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.Commands;
using BinaryOptions.DAL;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Requests;

namespace BinaryOptions.OptionServer.Handlers
{
    public class OpenPositionCommandHandler : ReceiveActor
    {
        public OpenPositionCommandHandler()
        {
            Receive<OpenPositionRequest>(c => Handle(c));
        }

        private void Handle(OpenPositionRequest command)
        {
            // get the account.
            // open position on account
            // save account.
            // save position.
            Sender.Tell(new OpenPositionReply(true));
        }
    }
}
