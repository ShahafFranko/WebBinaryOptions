using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.Events;

namespace BinaryOptions.OptionServer.Handlers
{
    public class ExpireOptionEventHandler : ReceiveActor
    {
        public ExpireOptionEventHandler(ActorSystem system)
        {
            Receive<OneMinuteElapsed>(m => Handle(m));
        }

        private void Handle(OneMinuteElapsed message)
        {
            // get all accounts
            // if account has open position that should expire now?
            // expire the option 
            // save account & position to DB.
        }
    }
}
