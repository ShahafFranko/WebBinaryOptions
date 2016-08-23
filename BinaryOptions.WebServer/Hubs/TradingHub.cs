using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryOption.OptionServer.Contract.Events;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BinaryOptions.WebServer.Hubs
{
    [HubName("tradingHub")]
    public class TradingHub : Hub
    {
        public void Subscribe(Guid accountId)
        {
            
        }

        public void PushAccount(AccountUpdated @event)
        {
            
        }

        public void PushInstrument(InstrumentUpdated @event)
        {
            Clients.All.onInstrumentUpdated(@event);
        }
    }
}