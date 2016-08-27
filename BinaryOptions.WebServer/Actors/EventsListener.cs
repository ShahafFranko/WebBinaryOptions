using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Akka.Actor;
using BinaryOption.OptionServer.Contract.DTO;
using BinaryOption.OptionServer.Contract.Events;
using BinaryOptions.WebServer.Hubs;
using Microsoft.AspNet.SignalR;

namespace BinaryOptions.WebServer.Actors
{
    public class EventsListener : ReceiveActor
    {
        private TradingHub m_hub;

        public EventsListener()
        {
            Receive<AccountUpdated>(e => Handle(e));
            Receive<InstrumentUpdated>(e => Handle(e));
        }

        private void Handle(AccountUpdated accountUpdated)
        {
            m_hub.PushAccount(accountUpdated);
        }

        private void Handle(InstrumentUpdated instrumentUpdated)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradingHub>();
            context.Clients.All.onInstrumentUpdated(instrumentUpdated);
        }
    }
}