using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.Commands;
using BinaryOptions.DAL;

namespace BinaryOptions.OptionServer.Handlers
{
    public class OpenPositionCommandHandler : ReceiveActor
    {
        public OpenPositionCommandHandler()
        {
            Receive<OpenPositionCommand>(c => Handle(c));
        }

        private void Handle(OpenPositionCommand command)
        {
            // get the account.
            // open position on account
            // save account.
            // save position.
        }
    }
}
