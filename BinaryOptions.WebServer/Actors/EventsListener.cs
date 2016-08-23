using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.Events;

namespace BinaryOptions.WebServer.Actors
{
    public class EventsListener : ReceiveActor
    {
        public EventsListener()
        {
            Receive<AccountUpdated>(e => Handle(e));
            Receive<InstrumentUpdated>(e => Handle(e));
        }

        private void Handle(AccountUpdated accountUpdated)
        {
            throw new NotImplementedException();
        }

        private void Handle(InstrumentUpdated instrumentUpdated)
        {
            throw new NotImplementedException();
        }
    }
}